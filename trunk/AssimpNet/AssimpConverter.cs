using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Assimp.Unmanaged;
using Assimp.Configs;
using System.Runtime.InteropServices;

namespace Assimp {
    public class AssimpConverter : IDisposable {
        private bool m_isDisposed;
        private bool m_verboseEnabled;
        private Dictionary<String, PropertyConfig> m_importConfigs;
        private Dictionary<String, PropertyConfig> m_exportConfigs;
        private List<LogStream> m_logStreams;
        private Object m_sync = new Object();

        private ExportFormatDescription[] m_exportFormats;
        private String[] m_importFormats;

        private float m_scale = 1.0f;
        private float m_xAxisRotation = 0.0f;
        private float m_yAxisRotation = 0.0f;
        private float m_zAxisRotation = 0.0f;
        private bool m_buildMatrix = false;
        private Matrix4x4 m_scaleRot = Matrix4x4.Identity;

        /// <summary>
        /// Gets if the importer has been disposed.
        /// </summary>
        public bool IsDisposed {
            get {
                return m_isDisposed;
            }
        }

        /// <summary>
        /// Gets or sets the uniform scale for the model. This is multiplied
        /// with the existing root node's transform.
        /// </summary>
        public float Scale {
            get {
                return m_scale;
            }
            set {
                if(m_scale != value) {
                    m_scale = value;
                    m_buildMatrix = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the model's rotation about the X-Axis, in degrees. This is multiplied
        /// with the existing root node's transform.
        /// </summary>
        public float XAxisRotation {
            get {
                return m_xAxisRotation;
            }
            set {
                if(m_xAxisRotation != value) {
                    m_xAxisRotation = value;
                    m_buildMatrix = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the model's rotation abut the Y-Axis, in degrees. This is multiplied
        /// with the existing root node's transform.
        /// </summary>
        public float YAxisRotation {
            get {
                return m_yAxisRotation;
            }
            set {
                if(m_yAxisRotation != value) {
                    m_yAxisRotation = value;
                    m_buildMatrix = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the model's rotation about the Z-Axis, in degrees. This is multiplied
        /// with the existing root node's transform.
        /// </summary>
        public float ZAxisRotation {
            get {
                return m_zAxisRotation;
            }
            set {
                if(m_zAxisRotation != value) {
                    m_zAxisRotation = value;
                    m_buildMatrix = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets if verbose logging should be enabled.
        /// </summary>
        public bool VerboseLoggingEnabled {
            get {
                return m_verboseEnabled;
            }
            set {
                m_verboseEnabled = value;
            }
        }

        /// <summary>
        /// Gets the import property configurations set to this converter.
        /// </summary>
        public Dictionary<String, PropertyConfig> ImportPropertyConfigurations {
            get {
                return m_importConfigs;
            }
        }

        /// <summary>
        /// Gets the export property configurations set to this converter.
        /// </summary>
        public Dictionary<String, PropertyConfig> ExportPropertyConfigurations {
            get {
                return m_exportConfigs;
            }
        }

        /// <summary>
        /// Gets the logstreams attached to this converter.
        /// </summary>
        public List<LogStream> LogStreams {
            get {
                return m_logStreams;
            }
        }

        public AssimpConverter() {
            m_exportConfigs = new Dictionary<String, PropertyConfig>();
            m_importConfigs = new Dictionary<String, PropertyConfig>();
            m_logStreams = new List<LogStream>();
        }

        ~AssimpConverter() {
            Dispose(false);
        }

        #region ConvertFromFile

        #region File to File

        public void ConvertFromFileToFile(String inputFilename, String outputFilename, String exportFormatId) {
            ConvertFromFileToFile(inputFilename, PostProcessSteps.None, outputFilename, exportFormatId, PostProcessSteps.None);
        }

        public void ConvertFromFileToFile(String inputFilename, String outputFilename, String exportFormatId, PostProcessSteps exportProcessSteps) {
            ConvertFromFileToFile(inputFilename, PostProcessSteps.None, outputFilename, exportFormatId, exportProcessSteps);
        }

        public void ConvertFromFileToFile(String inputFilename, PostProcessSteps importProcessSteps, String outputFilename, String exportFormatId, PostProcessSteps exportProcessSteps) {
            lock(m_sync) {
                if(m_isDisposed) {
                    throw new ObjectDisposedException("Importer has been disposed.");
                }

                if(String.IsNullOrEmpty(inputFilename) || !File.Exists(inputFilename)) {
                    throw new FileNotFoundException("Filename was null or could not be found", inputFilename);
                }

                IntPtr ptr = IntPtr.Zero;
                PrepareImport();

                try {
                    ptr = AssimpMethods.ImportFile(inputFilename, PostProcessSteps.None);

                    if(ptr == IntPtr.Zero)
                        throw new AssimpException("Error importing file: " + AssimpMethods.GetErrorString());

                    TransformScene(ptr);

                    ptr = AssimpMethods.ApplyPostProcessing(ptr, importProcessSteps);
                    ValidateScene(ptr);
                } finally {
                    ReleaseImportConfigs();
                }

                PrepareExport();

                try {
                    AssimpMethods.ExportScene(ptr, exportFormatId, outputFilename, exportProcessSteps);
                } finally {
                    Cleanup();
                    AssimpMethods.ReleaseImport(ptr);
                }
            }
        }

        #endregion

        #region File to Blob

        public ExportDataBlob ConvertFromFileToBlob(String inputFilename, String exportFormatId) {
            return ConvertFromFileToBlob(inputFilename, PostProcessSteps.None, exportFormatId, PostProcessSteps.None);
        }

        public ExportDataBlob ConvertFromFileToBlob(String inputFilename, String exportFormatId, PostProcessSteps exportProcessSteps) {
            return ConvertFromFileToBlob(inputFilename, PostProcessSteps.None, exportFormatId, exportProcessSteps);
        }

        public ExportDataBlob ConvertFromFileToBlob(String inputFilename, PostProcessSteps importProcessSteps, String exportFormatId, PostProcessSteps exportProcessSteps) {
            lock(m_sync) {
                if(m_isDisposed) {
                    throw new ObjectDisposedException("Importer has been disposed.");
                }

                if(String.IsNullOrEmpty(inputFilename) || !File.Exists(inputFilename)) {
                    throw new FileNotFoundException("Filename was null or could not be found", inputFilename);
                }

                IntPtr ptr = IntPtr.Zero;
                PrepareImport();

                try {
                    ptr = AssimpMethods.ImportFile(inputFilename, PostProcessSteps.None);

                    if(ptr == IntPtr.Zero)
                        throw new AssimpException("Error importing file: " + AssimpMethods.GetErrorString());

                    TransformScene(ptr);
                    ptr = AssimpMethods.ApplyPostProcessing(ptr, importProcessSteps);
                    ValidateScene(ptr);
                } finally {
                    ReleaseImportConfigs();
                }

                PrepareExport();

                try {
                    return AssimpMethods.ExportSceneToBlob(ptr, exportFormatId, exportProcessSteps);
                } finally {
                    Cleanup();
                    AssimpMethods.ReleaseImport(ptr);
                }
            }
        }

        #endregion

        #endregion

        #region ConvertFromStream

        #region Stream to File

        public void ConvertFromStreamToFile(Stream inputStream, String importFormatHint, String outputFilename, String exportFormatId) {
            ConvertFromStreamToFile(inputStream, importFormatHint, PostProcessSteps.None, outputFilename, exportFormatId, PostProcessSteps.None);
        }

        public void ConvertFromStreamToFile(Stream inputStream, String importFormatHint, String outputFilename, String exportFormatId, PostProcessSteps exportProcessSteps) {
            ConvertFromStreamToFile(inputStream, importFormatHint, PostProcessSteps.None, outputFilename, exportFormatId, exportProcessSteps);
        }

        public void ConvertFromStreamToFile(Stream inputStream, String importFormatHint, PostProcessSteps importProcessSteps, String outputFilename, String exportFormatId, PostProcessSteps exportProcessSteps) {
            lock(m_sync) {
                if(m_isDisposed) {
                    throw new ObjectDisposedException("Importer has been disposed.");
                }

                if(inputStream == null || inputStream.CanRead != true) {
                    throw new AssimpException("stream", "Can't read from the stream it's null or write-only");
                }

                if(String.IsNullOrEmpty(importFormatHint)) {
                    throw new AssimpException("formatHint", "Format hint is null or empty");
                }

                IntPtr ptr = IntPtr.Zero;
                PrepareImport();

                try {
                    ptr = AssimpMethods.ImportFileFromStream(inputStream, importProcessSteps, importFormatHint);

                    if(ptr == IntPtr.Zero)
                        throw new AssimpException("Error importing file: " + AssimpMethods.GetErrorString());

                    TransformScene(ptr);

                    ptr = AssimpMethods.ApplyPostProcessing(ptr, importProcessSteps);
                    ValidateScene(ptr);
                } finally {
                    ReleaseImportConfigs();
                }

                PrepareExport();

                try {
                    AssimpMethods.ExportScene(ptr, exportFormatId, outputFilename, exportProcessSteps);
                } finally {
                    Cleanup();
                    AssimpMethods.ReleaseImport(ptr);
                }
            }
        }

        #endregion

        #region Stream to Blob

        public ExportDataBlob ConvertFromStreamToBlob(Stream inputStream, String importFormatHint, String exportFormatId) {
            return ConvertFromStreamToBlob(inputStream, importFormatHint, PostProcessSteps.None, exportFormatId, PostProcessSteps.None);
        }

        public ExportDataBlob ConvertFromStreamToBlob(Stream inputStream, String importFormatHint, String exportFormatId, PostProcessSteps exportProcessSteps) {
            return ConvertFromStreamToBlob(inputStream, importFormatHint, PostProcessSteps.None, exportFormatId, exportProcessSteps);
        }

        public ExportDataBlob ConvertFromStreamToBlob(Stream inputStream, String importFormatHint, PostProcessSteps importProcessSteps, String exportFormatId, PostProcessSteps exportProcessSteps) {
            lock(m_sync) {
                if(m_isDisposed) {
                    throw new ObjectDisposedException("Importer has been disposed.");
                }

                if(inputStream == null || inputStream.CanRead != true) {
                    throw new AssimpException("stream", "Can't read from the stream it's null or write-only");
                }

                if(String.IsNullOrEmpty(importFormatHint)) {
                    throw new AssimpException("formatHint", "Format hint is null or empty");
                }

                IntPtr ptr = IntPtr.Zero;
                PrepareImport();

                try {
                    ptr = AssimpMethods.ImportFileFromStream(inputStream, importProcessSteps, importFormatHint);

                    if(ptr == IntPtr.Zero)
                        throw new AssimpException("Error importing file: " + AssimpMethods.GetErrorString());

                    TransformScene(ptr);

                    ptr = AssimpMethods.ApplyPostProcessing(ptr, importProcessSteps);
                    ValidateScene(ptr);
                } finally {
                    ReleaseImportConfigs();
                }

                PrepareExport();

                try {
                    return AssimpMethods.ExportSceneToBlob(ptr, exportFormatId, exportProcessSteps);
                } finally {
                    Cleanup();
                    AssimpMethods.ReleaseImport(ptr);
                }
            }
        }

        #endregion

        #endregion

        #region Format support

        public ExportFormatDescription[] GetSupportedExportFormats() {
            if(m_exportFormats == null)
                m_exportFormats = AssimpMethods.GetExportFormatDescriptions();

            return (ExportFormatDescription[]) m_exportFormats.Clone();
        }

        public String[] GetSupportedImportFormats() {
            if(m_importFormats == null)
                m_importFormats = AssimpMethods.GetExtensionList();

            return (String[]) m_importFormats.Clone();
        }

        public bool IsImportFormatSupported(String format) {
            return AssimpMethods.IsExtensionSupported(format);
        }

        public bool IsExportFormatSupported(String format) {
            ExportFormatDescription[] exportFormats = GetSupportedExportFormats();

            foreach(ExportFormatDescription desc in exportFormats) {
                if(String.Equals(desc.FormatId, format))
                    return true;
            }

            return false;
        }

        #endregion

        #region Import/Export configs

        public void SetImportConfig(PropertyConfig config) {
            if(config == null)
                return;

            String name = config.Name;

            m_importConfigs[name] = config;
        }

        public void RemoveImportConfig(String configName) {
            if(String.IsNullOrEmpty(configName))
                return;

            PropertyConfig oldConfig;
            if(!m_importConfigs.TryGetValue(configName, out oldConfig))
                m_importConfigs.Remove(configName);
        }

        public void RemoveImportConfigs() {
            m_importConfigs.Clear();
        }

        public bool ContainsImportConfig(String configName) {
            if(String.IsNullOrEmpty(configName))
                return false;

            return m_importConfigs.ContainsKey(configName);
        }

        public void SetExportConfig(PropertyConfig config) {
            if(config == null)
                return;

            String name = config.Name;

            m_exportConfigs[name] = config;
        }

        public void RemoveExportConfig(String configName) {
            if(String.IsNullOrEmpty(configName))
                return;

            PropertyConfig oldConfig;
            if(!m_exportConfigs.TryGetValue(configName, out oldConfig))
                m_exportConfigs.Remove(configName);
        }

        public void RemoveExportConfigs() {
            m_exportConfigs.Clear();
        }

        public bool ContainsExportConfig(String configName) {
            if(String.IsNullOrEmpty(configName))
                return false;

            return m_exportConfigs.ContainsKey(configName);
        }

        #endregion

        #region Logstreams

        public void AttachLogStream(LogStream logstream) {
            if(logstream == null || m_logStreams.Contains(logstream))
                return;

            m_logStreams.Add(logstream);
        }

        public void DetatchLogStream(LogStream logstream) {
            if(logstream == null)
                return;

            m_logStreams.Remove(logstream);
        }

        public void DetatchLogStreams() {
            m_logStreams.Clear();
        }

        #endregion

        #region Dispose

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
            if(!m_isDisposed) {
                if(disposing) {
                    //Dispose of managed resources
                }
                m_isDisposed = true;
            }
        }

        #endregion

        #region Private methods

        //Build import transformation matrix
        private void BuildMatrix() {

            if(m_buildMatrix) {
                Matrix4x4 scale = Matrix4x4.FromScaling(new Vector3D(m_scale, m_scale, m_scale));
                Matrix4x4 xRot = Matrix4x4.FromRotationX(m_xAxisRotation * (float) (180.0d / Math.PI));
                Matrix4x4 yRot = Matrix4x4.FromRotationY(m_yAxisRotation * (float) (180.0d / Math.PI));
                Matrix4x4 zRot = Matrix4x4.FromRotationZ(m_zAxisRotation * (float) (180.0d / Math.PI));
                m_scaleRot = scale * ((xRot * yRot) * zRot);
            }

            m_buildMatrix = false;
        }

        //Transforms the root node of the scene and writes it back to the native structure
        private unsafe bool TransformScene(IntPtr scene) {
            BuildMatrix();

            try {
                if(!m_scaleRot.IsIdentity) {
                    IntPtr rootNode = Marshal.ReadIntPtr(MemoryHelper.AddIntPtr(scene, sizeof(uint))); //Skip over sceneflags

                    IntPtr matrixPtr = MemoryHelper.AddIntPtr(rootNode, Marshal.SizeOf(typeof(AiString))); //Skip over AiString

                    Matrix4x4 matrix = MemoryHelper.MarshalStructure<Matrix4x4>(matrixPtr); //Get the root transform

                    matrix = matrix * m_scaleRot; //Transform

                    //Save back to unmanaged mem
                    int index = 0;
                    for(int i = 1; i <= 4; i++) {
                        for(int j = 1; j <= 4; j++) {
                            float value = matrix[i, j];
                            byte[] bytes = BitConverter.GetBytes(value);
                            foreach(byte b in bytes) {
                                Marshal.WriteByte(matrixPtr, index, b);
                                index++;
                            }
                        }
                    }
                    return true;
                }
            } catch(Exception) {

            }
            return false;
        }

        //Attachs all logstreams to Assimp
        private void AttachLogs() {
            foreach(LogStream log in m_logStreams) {
                log.Attach();
            }
        }

        //Detatches all logstreams from Assimp
        private void DetatachLogs() {
            foreach(LogStream log in m_logStreams) {
                log.Detach();
            }
        }

        //Creates all import property stores and sets their values
        private void CreateImportConfigs() {
            foreach(KeyValuePair<String, PropertyConfig> config in m_importConfigs) {
                config.Value.CreatePropertyStore();
            }
        }

        //Destroys all import property stores
        private void ReleaseImportConfigs() {
            foreach(KeyValuePair<String, PropertyConfig> config in m_importConfigs) {
                config.Value.ReleasePropertyStore();
            }
        }

        //Creates all export property stores and sets their values
        private void CreateExportConfigs() {
            foreach(KeyValuePair<String, PropertyConfig> config in m_exportConfigs) {
                config.Value.CreatePropertyStore();
            }
        }

        //Destroys all export property stores
        private void ReleaseExportConfigs() {
            foreach(KeyValuePair<String, PropertyConfig> config in m_exportConfigs) {
                config.Value.ReleasePropertyStore();
            }
        }
        
        //Doese all the necessary prep work before we import.
        private void PrepareImport() {
            AssimpMethods.EnableVerboseLogging(m_verboseEnabled);
            AttachLogs();
            CreateImportConfigs();
        }

        //Does all the necessary prep work before we export.
        private void PrepareExport() {
            ReleaseImportConfigs();
            CreateExportConfigs();
        }

        //Does all the necessary cleanup after we export.
        private void Cleanup() {
            ReleaseExportConfigs();
            DetatachLogs();
        }

        //Validate the imported scene to ensure its complete and load the return scene
        private void ValidateScene(IntPtr ptr) {
            AiScene scene = MemoryHelper.MarshalStructure<AiScene>(ptr);
            if((scene.Flags & SceneFlags.Incomplete) == SceneFlags.Incomplete) {
                throw new AssimpException("Error importing file: Imported scene is incomplete. " + AssimpMethods.GetErrorString());
            }
        }

        #endregion
    }
}
