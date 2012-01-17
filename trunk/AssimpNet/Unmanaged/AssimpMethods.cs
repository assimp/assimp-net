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

        [DllImport(AssimpDLL, EntryPoint = "aiReleaseImport", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ReleaseImport(IntPtr pScene);

        [DllImport(AssimpDLL, EntryPoint = "aiApplyPostProcessing", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr aiApplyPostProcessing(IntPtr scene, uint Flags);

        public static IntPtr ApplyPostProcessing(IntPtr scene, PostProcessSteps flags) {
            return aiApplyPostProcessing(scene, (uint) flags);
        }

        #endregion

        #region Logging Methods

        [DllImport(AssimpDLL, EntryPoint = "aiAttachLogStream", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttachLogStream(ref LogStream stream);

        [DllImport(AssimpDLL, EntryPoint = "aiEnableVerboseLogging", CallingConvention = CallingConvention.Cdecl)]
        public static extern void EnableVerboseLogging([InAttribute()] [MarshalAs(UnmanagedType.Bool)] bool enable);

        [DllImport(AssimpDLL, EntryPoint = "aiDetachLogStream", CallingConvention = CallingConvention.Cdecl)]
        public static extern ReturnCode DetachLogStream(ref LogStream stream);

        [DllImport(AssimpDLL, EntryPoint = "aiDetachAllLogStreams", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DetachAllLogStreams();

        #endregion

        #region Error and Info methods

        [DllImport(AssimpDLL, EntryPoint = "aiGetErrorString", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr aiGetErrorString();

        public static String GetErrorString() {
            return Marshal.PtrToStringAnsi(aiGetErrorString());
        }

        [DllImport(AssimpDLL, EntryPoint = "aiIsExtensionSupported", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsExtensionSupported([InAttribute()] [MarshalAs(UnmanagedType.LPStr)] String extension);

        [DllImport(AssimpDLL, EntryPoint = "aiGetExtensionList", CallingConvention = CallingConvention.Cdecl)]
        private static extern void aiGetExtensionList(ref AiString extensionsOut);

        public static String[] GetExtensionList() {
            AiString aiString = new AiString();
            aiGetExtensionList(ref aiString);
            return aiString.Data.Split(new String[] { "*", ";*" }, StringSplitOptions.RemoveEmptyEntries);
        }

        [DllImport(AssimpDLL, EntryPoint = "aiGetMemoryRequirements", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GetMemoryRequirements(IntPtr scene, ref MemoryInfo memoryInfo);

        public static MemoryInfo GetMemoryRequirements(IntPtr scene) {
            MemoryInfo info = new MemoryInfo();
            if(scene != IntPtr.Zero) {
                GetMemoryRequirements(scene, ref info);
            }
            return info;
        }

        #endregion

        #region Import Properties setters

        [DllImportAttribute(AssimpDLL, EntryPoint = "aiSetImportPropertyInteger", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetImportPropertyInteger([InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] String name, int value);

        [DllImportAttribute(AssimpDLL, EntryPoint = "aiSetImportPropertyFloat", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetImportPropertyFloat([InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] String name, float value);

        [DllImportAttribute(AssimpDLL, EntryPoint = "aiSetImportPropertyString", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetImportPropertyString([InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] String name, ref AiString value);

        #endregion

        #region Material getters

        [DllImportAttribute(AssimpDLL, EntryPoint = "aiGetMaterialProperty", CallingConvention = CallingConvention.Cdecl)]
        private static extern ReturnCode aiGetMaterialProperty(ref AiMaterial mat, [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] String key, uint type, uint index, ref IntPtr propertyOut);

        public static AiMaterialProperty GetMaterialProperty(ref AiMaterial mat, String key, uint type, uint index) {
            IntPtr ptr = new IntPtr();
            aiGetMaterialProperty(ref mat, key, type, index, ref ptr);
            return MemoryHelper.MarshalStructure<AiMaterialProperty>(Marshal.ReadIntPtr(ptr));
        }


        #endregion

        #region Math methods

        [DllImportAttribute(AssimpDLL, EntryPoint="aiCreateQuaternionFromMatrix", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CreateQuaternionFromMatrix(ref Quaternion quat, ref Matrix3x3 mat);

        [DllImportAttribute(AssimpDLL, EntryPoint="aiDecomposeMatrix", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DecomposeMatrix(ref Matrix4x4 mat, ref Vector3D scaling, ref Quaternion rotation, ref Vector3D position);

        [DllImportAttribute(AssimpDLL, EntryPoint="aiTransposeMatrix4", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TransposeMatrix4(ref Matrix4x4 mat);

        [DllImportAttribute(AssimpDLL, EntryPoint="aiTransposeMatrix3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TransposeMatrix3(ref Matrix3x3 mat);

        [DllImportAttribute(AssimpDLL, EntryPoint="aiTransformVecByMatrix3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TransformVecByMatrix3(ref Vector3D vec, ref Matrix3x3 mat);

        [DllImportAttribute(AssimpDLL, EntryPoint="aiTransformVecByMatrix4", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TransformVecByMatrix4(ref Vector3D vec, ref Matrix4x4 mat);

        [DllImportAttribute(AssimpDLL, EntryPoint="aiMultiplyMatrix4", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MultiplyMatrix4(ref Matrix4x4 dst, ref Matrix4x4 src);

        [DllImportAttribute(AssimpDLL, EntryPoint="aiMultiplyMatrix3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MultiplyMatrix3(ref Matrix3x3 dst, ref Matrix3x3 src);

        [DllImportAttribute(AssimpDLL, EntryPoint="aiIdentityMatrix3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IdentityMatrix3(ref Matrix3x3 mat);

        [DllImportAttribute(AssimpDLL, EntryPoint="aiIdentityMatrix4", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IdentityMatrix4(ref Matrix4x4 mat);

        #endregion

        #region Version info

        [DllImport(AssimpDLL, EntryPoint="aiGetLegalString", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr aiGetLegalString();

        public static String GetLegalString() {
            return Marshal.PtrToStringAnsi(aiGetLegalString());
        }

        [DllImport(AssimpDLL, EntryPoint="aiGetVersionMinor", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint GetVersionMinor();

        [DllImport(AssimpDLL, EntryPoint="aiGetVersionMajor", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint GetVersionMajor();

        [DllImport(AssimpDLL, EntryPoint="aiGetVersionRevision", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint GetVersionRevision();

        [DllImport(AssimpDLL, EntryPoint="aiGetCompileFlags", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint aiGetCompileFlags();

        public static CompileFlags GetCompileFlags() {
            return (CompileFlags) aiGetCompileFlags();
        }

        #endregion
    }
}
