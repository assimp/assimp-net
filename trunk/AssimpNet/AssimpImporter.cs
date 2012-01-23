/*
* Copyright (c) 2012 Nicholas Woodfield
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.IO;
using Assimp.Configs;
using System.Runtime.InteropServices;
using Assimp.Unmanaged;

namespace Assimp {
    /// <summary>
    /// Assimp importer that will use Assimp to load a model into managed memory.
    /// </summary>
    public class AssimpImporter : IDisposable {
        private bool _isDisposed;
        private bool _verboseEnabled;
        private Dictionary<String, IPropertyConfig> _configs;
        private List<LogStream> _logStreams;
        private Object sync = new Object();

        /// <summary>
        /// Gets if the importer has been disposed.
        /// </summary>
        public bool IsDisposed {
            get {
                return _isDisposed;
            }
        }

        /// <summary>
        /// Gets or sets if verbose logging should be enabled.
        /// </summary>
        public bool VerboseLoggingEnabled {
            get {
                return _verboseEnabled;
            }
            set {
                _verboseEnabled = value;
                AssimpMethods.EnableVerboseLogging(value);
            }
        }

        /// <summary>
        /// Gets the property configurations set to this importer.
        /// </summary>
        public Dictionary<String, IPropertyConfig> PropertyConfigurations {
            get {
                return _configs;
            }
        }

        /// <summary>
        /// Gets the logstreams attached to this importer.
        /// </summary>
        public List<LogStream> LogStreams {
            get {
                return _logStreams;
            }
        }

        /// <summary>
        /// Constructs a new AssimpImporter.
        /// </summary>
        public AssimpImporter() {
            _configs = new Dictionary<String, IPropertyConfig>();
            _logStreams = new List<LogStream>();
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="AssimpImporter"/> is reclaimed by garbage collection.
        /// </summary>
        ~AssimpImporter() {
            Dispose(false);
        }

        /// <summary>
        /// Importers a model from the specified file without running any post-process steps. The importer sets configurations
        /// and loads the model into managed memory, releasing the unmanaged memory used by Assimp.
        /// </summary>
        /// <param name="file">Full path to the file</param>
        /// <returns>The imported scene</returns>
        /// <exception cref="AssimpException">Thrown if the file is valid or there was a general error in importing the model.</exception>
        /// <exception cref="System.ObjectDisposedException">Thrown if attempting to import a model if the importer has been disposed of</exception>
        public Scene ImportFile(String file) {
            return ImportFile(file, PostProcessSteps.None);
        }

        /// <summary>
        /// Importers a model from the specified file. The importer sets configurations
        /// and loads the model into managed memory, releasing the unmanaged memory used by Assimp.
        /// </summary>
        /// <param name="file">Full path to the file</param>
        /// <param name="postProcessFlags">Post processing flags, if any</param>
        /// <returns>The imported scene</returns>
        /// <exception cref="AssimpException">Thrown if the file is valid or there was a general error in importing the model.</exception>
        /// <exception cref="System.ObjectDisposedException">Thrown if attempting to import a model if the importer has been disposed of</exception>
        public Scene ImportFile(String file, PostProcessSteps postProcessFlags) {
            lock(sync) {
                if(_isDisposed) {
                    throw new ObjectDisposedException("Importer has been disposed.");
                }
                if(String.IsNullOrEmpty(file) || !File.Exists(file)) {
                    throw new AssimpException("file", "Filename is null or not valid.");
                }

                IntPtr ptr = IntPtr.Zero;
                try {

                    AttachLogs();
                    ApplyConfigs();
                    
                    ptr = AssimpMethods.ImportFile(file, postProcessFlags);
                    
                    ApplyConfigsDefault();
                    DetatachLogs();

                    if(ptr == IntPtr.Zero) {
                        throw new AssimpException("Error importing file: " + AssimpMethods.GetErrorString());
                    }

                    AiScene scene = MemoryHelper.MarshalStructure<AiScene>(ptr);
                    if((scene.Flags & SceneFlags.Incomplete) == SceneFlags.Incomplete) {
                        throw new AssimpException("Error importing file: Imported scene is incomplete. " + AssimpMethods.GetErrorString());
                    }

                    return new Scene(scene);
                } finally {
                    if(ptr != IntPtr.Zero) {
                        AssimpMethods.ReleaseImport(ptr);
                    }
                }
            }
        }

        /// <summary>
        /// Attaches a logging stream to the importer.
        /// </summary>
        /// <param name="logstream"></param>
        public void AttachLogStream(LogStream logstream) {
            if(logstream == null || _logStreams.Contains(logstream)) {
                return;
            }
            _logStreams.Add(logstream);
        }

        /// <summary>
        /// Detaches a logging stream from the importer.
        /// </summary>
        /// <param name="logStream"></param>
        public void DetachLogStream(LogStream logStream) {
            if(logStream == null) {
                return;
            }
            _logStreams.Remove(logStream);
        }

        /// <summary>
        /// Detaches all logging streams that are currently attached to the importer.
        /// </summary>
        public void DetachLogStreams() {
            foreach(LogStream stream in _logStreams) {
                stream.Detach();
            }
        }

        /// <summary>
        /// Gets the model formats that are supported by Assimp. Each
        /// format should follow this example: ".3ds"
        /// </summary>
        /// <returns>The format extensions that are supported</returns>
        public String[] GetSupportedFormats() {
            return AssimpMethods.GetExtensionList();
        }

        /// <summary>
        /// Checks of the format extension is supported. Example: ".3ds"
        /// </summary>
        /// <param name="formatExtension">Format extension</param>
        /// <returns></returns>
        public bool IsFormatSupported(String formatExtension) {
            return AssimpMethods.IsExtensionSupported(formatExtension);
        }

        /// <summary>
        /// Sets a configuration property to the importer.
        /// </summary>
        /// <param name="config">Config to set</param>
        public void SetConfig(IPropertyConfig config) {
            if(config == null) {
                return;
            }
            String name = config.Name;
            if(!_configs.ContainsKey(name)) {
                _configs[name] = config;
            } else {
                _configs.Add(name, config);
            }
        }

        /// <summary>
        /// Removes a set configuration property by name.
        /// </summary>
        /// <param name="configName">Name of the config property</param>
        public void RemoveConfig(String configName) {
            if(String.IsNullOrEmpty(configName)) {
                return;
            }
            IPropertyConfig oldConfig;
            if(!_configs.TryGetValue(configName, out oldConfig)) {
                _configs.Remove(configName);
            }
        }

        /// <summary>
        /// Checks if the importer has a config set by the specified name.
        /// </summary>
        /// <param name="configName">Name of the config property</param>
        /// <returns>True if the config is present, false otherwise</returns>
        public bool ContainsConfig(String configName) {
            if(String.IsNullOrEmpty(configName)) {
                return false;
            }
            return _configs.ContainsKey(configName);
        }

        //Sets all config properties to Assimp
        private void ApplyConfigs() {
            foreach(KeyValuePair<String, IPropertyConfig> config in _configs) {
                config.Value.ApplyValue();
            }
        }

        //Sets all default config properties to Assimp
        private void ApplyConfigsDefault() {
            foreach(KeyValuePair<String, IPropertyConfig> config in _configs) {
                config.Value.ApplyDefaultValue();
            }
        }

        //Attachs all logstreams to Assimp
        private void AttachLogs() {
            foreach(LogStream log in _logStreams) {
                log.Attach();
            }
        }

        //Detatches all logstreams from Assimp
        private void DetatachLogs() {
            foreach(LogStream log in _logStreams) {
                log.Detach();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected void Dispose(bool disposing) {

            if(!_isDisposed) {
                if(disposing) {
                    //Dispose of managed resources
                }
                _isDisposed = true;
            }
        }
    }
}
