using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assimp.Unmanaged;
using System.IO;

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
