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
using System.Runtime.InteropServices;

namespace Assimp.Unmanaged {
    /// <summary>
    /// Static class containing the P/Invoke methods exposing the Assimp C-API.
    /// </summary>
    public static class AssimpMethods {

        #region Native DLL Declarations

#if X64
        private const String AssimpDLL = "Assimp64.dll";
#else
        private const String AssimpDLL = "Assimp32.dll";
#endif

        #endregion

        #region Core Import Methods

        [DllImport(AssimpDLL, EntryPoint = "aiImportFile", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr aiImportFile([InAttribute()] [MarshalAs(UnmanagedType.LPStr)] String file, uint flags);

        /// <summary>
        /// Imports a file.
        /// </summary>
        /// <param name="file">Valid filename</param>
        /// <param name="flags">Post process flags specifying what steps are to be run after the import.</param>
        /// <returns>Pointer to the unmanaged data structure.</returns>
        public static IntPtr ImportFile(String file, PostProcessSteps flags) {
            return aiImportFile(file, (uint) flags);
        }

        //TODO : ImportFileEx

        /*
        [DllImport(AssimpDLL, EntryPoint = "aiImportFileFromMemory", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr aiImportFileFromMemory(byte[] buffer, uint length, uint flags, [InAttribute()] [MarshalAs(UnmanagedType.LPStr)] String fileHint);

        public static IntPtr ImportFileFromMemory(byte[] fileContents, PostProcessSteps flags, String fileExtensionHint) {
            return aiImportFileFromMemory(fileContents, (uint) fileContents.Length, (uint) flags, fileExtensionHint);
        }
        */

        /// <summary>
        /// Releases the unmanaged scene data structure.
        /// </summary>
        /// <param name="scene">Pointer to the unmanaged scene data structure.</param>
        [DllImport(AssimpDLL, EntryPoint = "aiReleaseImport", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ReleaseImport(IntPtr scene);

        [DllImport(AssimpDLL, EntryPoint = "aiApplyPostProcessing", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr aiApplyPostProcessing(IntPtr scene, uint Flags);

        /// <summary>
        /// Applies a post-processing step on an already imported scene.
        /// </summary>
        /// <param name="scene">Pointer to the unmanaged scene data structure.</param>
        /// <param name="flags">Post processing steps to run.</param>
        /// <returns>Pointer to the unmanaged scene data structure.</returns>
        public static IntPtr ApplyPostProcessing(IntPtr scene, PostProcessSteps flags) {
            return aiApplyPostProcessing(scene, (uint) flags);
        }

        #endregion

        #region Logging Methods

        /// <summary>
        /// Attaches a log stream callback to catch Assimp messages.
        /// </summary>
        /// <param name="stream">Logstream to attach</param>
        [DllImport(AssimpDLL, EntryPoint = "aiAttachLogStream", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttachLogStream(ref AiLogStream stream);

        /// <summary>
        /// Enables verbose logging.
        /// </summary>
        /// <param name="enable">True if verbose logging is to be enabled or not.</param>
        [DllImport(AssimpDLL, EntryPoint = "aiEnableVerboseLogging", CallingConvention = CallingConvention.Cdecl)]
        public static extern void EnableVerboseLogging([InAttribute()] [MarshalAs(UnmanagedType.Bool)] bool enable);

        /// <summary>
        /// Detaches a logstream callback.
        /// </summary>
        /// <param name="stream">Logstream to detach</param>
        /// <returns>A return code signifying if the function was successful or not.</returns>
        [DllImport(AssimpDLL, EntryPoint = "aiDetachLogStream", CallingConvention = CallingConvention.Cdecl)]
        public static extern ReturnCode DetachLogStream(ref AiLogStream stream);

        /// <summary>
        /// Detaches all logstream callbacks currently attached to Assimp.
        /// </summary>
        [DllImport(AssimpDLL, EntryPoint = "aiDetachAllLogStreams", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DetachAllLogStreams();

        #endregion

        #region Error and Info methods

        [DllImport(AssimpDLL, EntryPoint = "aiGetErrorString", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr aiGetErrorString();

        /// <summary>
        /// Gets the last error logged in Assimp.
        /// </summary>
        /// <returns>The last error message logged.</returns>
        public static String GetErrorString() {
            return Marshal.PtrToStringAnsi(aiGetErrorString());
        }

        /// <summary>
        /// Checks whether the model format extension is supported by Assimp.
        /// </summary>
        /// <param name="extension">Model format extension, e.g. ".3ds"</param>
        /// <returns>True if the format is supported, false otherwise.</returns>
        [DllImport(AssimpDLL, EntryPoint = "aiIsExtensionSupported", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsExtensionSupported([InAttribute()] [MarshalAs(UnmanagedType.LPStr)] String extension);

        [DllImport(AssimpDLL, EntryPoint = "aiGetExtensionList", CallingConvention = CallingConvention.Cdecl)]
        private static extern void aiGetExtensionList(ref AiString extensionsOut);

        /// <summary>
        /// Gets all the model format extensions that are currently supported by Assimp.
        /// </summary>
        /// <returns>Array of supported format extensions</returns>
        public static String[] GetExtensionList() {
            AiString aiString = new AiString();
            aiGetExtensionList(ref aiString);
            return aiString.Data.Split(new String[] { "*", ";*" }, StringSplitOptions.RemoveEmptyEntries);
        }

        [DllImport(AssimpDLL, EntryPoint = "aiGetMemoryRequirements", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GetMemoryRequirements(IntPtr scene, ref AiMemoryInfo memoryInfo);

        /// <summary>
        /// Gets the memory requirements of the scene.
        /// </summary>
        /// <param name="scene">Pointer to the unmanaged scene data structure.</param>
        /// <returns>The memory information about the scene.</returns>
        public static AiMemoryInfo GetMemoryRequirements(IntPtr scene) {
            AiMemoryInfo info = new AiMemoryInfo();
            if(scene != IntPtr.Zero) {
                GetMemoryRequirements(scene, ref info);
            }
            return info;
        }

        #endregion

        #region Import Properties setters

        /// <summary>
        /// Sets an integer property value.
        /// </summary>
        /// <param name="name">Property name</param>
        /// <param name="value">Property value</param>
        [DllImportAttribute(AssimpDLL, EntryPoint = "aiSetImportPropertyInteger", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetImportPropertyInteger([InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] String name, int value);

        /// <summary>
        /// Sets a float property value.
        /// </summary>
        /// <param name="name">Property name</param>
        /// <param name="value">Property value</param>
        [DllImportAttribute(AssimpDLL, EntryPoint = "aiSetImportPropertyFloat", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetImportPropertyFloat([InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] String name, float value);

        [DllImportAttribute(AssimpDLL, EntryPoint = "aiSetImportPropertyString", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SetImportPropertyString([InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] String name, ref AiString value);

        /// <summary>
        /// Sets a string property value.
        /// </summary>
        /// <param name="name">Property name</param>
        /// <param name="value">Property value</param>
        public static void SetImportPropertyString(String name, String value) {
            AiString str = new AiString();
            str.Data = value;
            //Note: aiTypes.h specifies aiString is UTF-8 encoded string.
            str.Length = (uint) System.Text.UTF8Encoding.UTF8.GetByteCount(value);
            SetImportPropertyString(name, ref str);
        }

        #endregion

        #region Material getters

        /*
        [DllImportAttribute(AssimpDLL, EntryPoint = "aiGetMaterialProperty", CallingConvention = CallingConvention.Cdecl)]
        private static extern ReturnCode aiGetMaterialProperty(ref AiMaterial mat, [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] String key, uint type, uint index, ref IntPtr propertyOut);

        public static AiMaterialProperty GetMaterialProperty(ref AiMaterial mat, String key, uint type, uint index) {
            IntPtr ptr = new IntPtr();
            aiGetMaterialProperty(ref mat, key, type, index, ref ptr);
            return MemoryHelper.MarshalStructure<AiMaterialProperty>(Marshal.ReadIntPtr(ptr));
        }
        */
        #endregion

        #region Math methods

        /// <summary>
        /// Creates a quaternion from the 3x3 rotation matrix.
        /// </summary>
        /// <param name="quat">Quaternion struct to fill</param>
        /// <param name="mat">Rotation matrix</param>
        [DllImportAttribute(AssimpDLL, EntryPoint="aiCreateQuaternionFromMatrix", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CreateQuaternionFromMatrix(ref Quaternion quat, ref Matrix3x3 mat);

        /// <summary>
        /// Decomposes a 4x4 matrix into its scaling, rotation, and translation parts.
        /// </summary>
        /// <param name="mat">4x4 Matrix to decompose</param>
        /// <param name="scaling">Scaling vector</param>
        /// <param name="rotation">Quaternion containing the rotation</param>
        /// <param name="position">Translation vector</param>
        [DllImportAttribute(AssimpDLL, EntryPoint="aiDecomposeMatrix", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DecomposeMatrix(ref Matrix4x4 mat, ref Vector3D scaling, ref Quaternion rotation, ref Vector3D position);

        /// <summary>
        /// Transposes the 4x4 matrix.
        /// </summary>
        /// <param name="mat">Matrix to transpose</param>
        [DllImportAttribute(AssimpDLL, EntryPoint="aiTransposeMatrix4", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TransposeMatrix4(ref Matrix4x4 mat);

        /// <summary>
        /// Transposes the 3x3 matrix.
        /// </summary>
        /// <param name="mat">Matrix to transpose</param>
        [DllImportAttribute(AssimpDLL, EntryPoint="aiTransposeMatrix3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TransposeMatrix3(ref Matrix3x3 mat);

        /// <summary>
        /// Transforms the vector by the 3x3 rotation matrix.
        /// </summary>
        /// <param name="vec">Vector to transform</param>
        /// <param name="mat">Rotation matrix</param>
        [DllImportAttribute(AssimpDLL, EntryPoint="aiTransformVecByMatrix3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TransformVecByMatrix3(ref Vector3D vec, ref Matrix3x3 mat);

        /// <summary>
        /// Transforms the vector by the 4x4 matrix.
        /// </summary>
        /// <param name="vec">Vector to transform</param>
        /// <param name="mat">Matrix transformation</param>
        [DllImportAttribute(AssimpDLL, EntryPoint="aiTransformVecByMatrix4", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TransformVecByMatrix4(ref Vector3D vec, ref Matrix4x4 mat);

        /// <summary>
        /// Multiplies two 4x4 matrices. The destination matrix receives the result.
        /// </summary>
        /// <param name="dst">First input matrix and is also the Matrix to receive the result</param>
        /// <param name="src">Second input matrix, to be multiplied with "dst".</param>
        [DllImportAttribute(AssimpDLL, EntryPoint="aiMultiplyMatrix4", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MultiplyMatrix4(ref Matrix4x4 dst, ref Matrix4x4 src);

        /// <summary>
        /// Multiplies two 3x3 matrices. The destination matrix receives the result.
        /// </summary>
        /// <param name="dst">First input matrix and is also the Matrix to receive the result</param>
        /// <param name="src">Second input matrix, to be multiplied with "dst".</param>
        [DllImportAttribute(AssimpDLL, EntryPoint="aiMultiplyMatrix3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MultiplyMatrix3(ref Matrix3x3 dst, ref Matrix3x3 src);

        /// <summary>
        /// Creates a 3x3 identity matrix.
        /// </summary>
        /// <param name="mat">Matrix to hold the identity</param>
        [DllImportAttribute(AssimpDLL, EntryPoint="aiIdentityMatrix3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IdentityMatrix3(ref Matrix3x3 mat);

        /// <summary>
        /// Creates a 4x4 identity matrix.
        /// </summary>
        /// <param name="mat">Matrix to hold the identity</param>
        [DllImportAttribute(AssimpDLL, EntryPoint="aiIdentityMatrix4", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IdentityMatrix4(ref Matrix4x4 mat);

        #endregion

        #region Version info

        [DllImport(AssimpDLL, EntryPoint="aiGetLegalString", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr aiGetLegalString();

        /// <summary>
        /// Gets the Assimp legal info.
        /// </summary>
        /// <returns>String containing Assimp legal info.</returns>
        public static String GetLegalString() {
            return Marshal.PtrToStringAnsi(aiGetLegalString());
        }

        /// <summary>
        /// Gets the native Assimp DLL's minor version number.
        /// </summary>
        /// <returns></returns>
        [DllImport(AssimpDLL, EntryPoint="aiGetVersionMinor", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint GetVersionMinor();

        /// <summary>
        /// Gets the native Assimp DLL's major version number.
        /// </summary>
        /// <returns>Assimp major version number</returns>
        [DllImport(AssimpDLL, EntryPoint="aiGetVersionMajor", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint GetVersionMajor();

        /// <summary>
        /// Gets the native Assimp DLL's revision version number.
        /// </summary>
        /// <returns>Assimp revision version number</returns>
        [DllImport(AssimpDLL, EntryPoint="aiGetVersionRevision", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint GetVersionRevision();

        /// <summary>
        /// Gets the native Assimp DLL's current version number as "major.minor.revision" string. This is the
        /// version of Assimp that this wrapper is currently using.
        /// </summary>
        /// <returns></returns>
        public static String GetVersion() {
            uint major = GetVersionMajor();
            uint minor = GetVersionMinor();
            uint rev = GetVersionRevision();

            return String.Format("{0}.{1}.{2}", major.ToString(), minor.ToString(), rev.ToString());
        }

        [DllImport(AssimpDLL, EntryPoint="aiGetCompileFlags", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint aiGetCompileFlags();

        /// <summary>
        /// Get the compilation flags that describe how the native Assimp DLL was compiled.
        /// </summary>
        /// <returns>Compilation flags</returns>
        public static CompileFlags GetCompileFlags() {
            return (CompileFlags) aiGetCompileFlags();
        }

        #endregion
    }
}
