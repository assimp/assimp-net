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
using System.IO;
using Assimp.Unmanaged;

namespace Assimp {
    public class AssimpImporter : IDisposable {
        private bool _verboseEnabled;
        private bool _isDisposed;

        public bool VerboseLoggingEnabled {
            get {
                return _verboseEnabled;
            }
            set {
                _verboseEnabled = value;
                AssimpMethods.EnableVerboseLogging(value);
            }
        }

        public bool IsDisposed {
            get {
                return _isDisposed;
            }
        }

        public AssimpImporter() {
        }

        ~AssimpImporter() {
            Dispose(false);
        }

        public Scene ImportFile(String file, PostProcessSteps postProcessFlags) {
            if(String.IsNullOrEmpty(file) || !File.Exists(file)) {
                throw new AssimpException("file", "Filename is null or not valid.");
            }

            IntPtr ptr = IntPtr.Zero;
            try {
                ptr = AssimpMethods.ImportFile(file, postProcessFlags);

                if(ptr == IntPtr.Zero) {
                    throw new AssimpException("Error importing file: " + AssimpMethods.GetErrorString());
                }

                AiScene scene = MemoryHelper.MarshalStructure<AiScene>(ptr);
                if((scene.Flags & SceneFlags.Incomplete) == SceneFlags.Incomplete) {
                    throw new AssimpException("Error importing file: Imported scene is incomplete. " + AssimpMethods.GetErrorString());
                }
                MemoryInfo memInfo = AssimpMethods.GetMemoryRequirements(ptr);

                return new Scene(scene, memInfo);
            } finally {
                if(ptr != IntPtr.Zero) {
                    AssimpMethods.ReleaseImport(ptr);
                }
            }
        }

        public void AttachLogStream(ref LogStream logstream) {
            AssimpMethods.AttachLogStream(ref logstream);
        }

        public void DetachLogStream(ref LogStream logStream) {
            AssimpMethods.DetachLogStream(ref logStream);
        }

        public void DetachAllLogStreams() {
            AssimpMethods.DetachAllLogStreams();
        }

        public String[] GetSupportedFormats() {
            return AssimpMethods.GetExtensionList();
        }

        public bool IsFormatSupported(String formatExtension) {
            return AssimpMethods.IsExtensionSupported(formatExtension);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing) {

            if(!_isDisposed) {
                if(disposing) {
                    //Dispose of managed resources
                }
                AssimpMethods.DetachAllLogStreams();
                _isDisposed = true;
            }
        }
    }
}
