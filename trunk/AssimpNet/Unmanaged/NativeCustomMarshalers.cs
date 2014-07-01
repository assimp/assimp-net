/*
* Copyright (c) 2012-2014 AssimpNet - Nicholas Woodfield
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
using System.Text;

namespace Assimp.Unmanaged
{
    #region AiMesh

    /// <summary>
    /// Custom marshaler for <see cref="AiMesh"/>.
    /// </summary>
    public sealed class AiMeshMarshaler : INativeCustomMarshaler
    {
        private int m_sizeInBytes;

        /// <summary>
        /// Gets the native data size in bytes.
        /// </summary>
        public int NativeDataSize
        {
            get 
            {
                return m_sizeInBytes;
            }
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="AiMeshMarshaler"/> class.
        /// </summary>
        public AiMeshMarshaler()
        {
            m_sizeInBytes = sizeof(PrimitiveType) + (IntPtr.Size * 7) + 
                (IntPtr.Size * AiDefines.AI_MAX_NUMBER_OF_COLOR_SETS) + 
                (IntPtr.Size * AiDefines.AI_MAX_NUMBER_OF_TEXTURECOORDS) + 
                (sizeof(uint) * 5) + (sizeof(uint) * AiDefines.AI_MAX_NUMBER_OF_TEXTURECOORDS) + 
                MemoryHelper.SizeOf<AiString>();
        }

        /// <summary>
        /// Marshals the managed object to the unmanaged chunk of memory.
        /// </summary>
        /// <param name="managedObj">Managed object to marshal.</param>
        /// <param name="nativeData">Unmanaged chunk of memory to write to.</param>
        public void MarshalManagedToNative(Object managedObj, IntPtr nativeData)
        {
            IntPtr currPos = nativeData;
            AiMesh mesh = (AiMesh)managedObj;

            MemoryHelper.Write<PrimitiveType>(currPos, ref mesh.PrimitiveTypes);
            currPos = MemoryHelper.AddIntPtr(currPos, sizeof(PrimitiveType));

            MemoryHelper.Write<uint>(currPos, ref mesh.NumVertices);
            currPos = MemoryHelper.AddIntPtr(currPos, sizeof(uint));

            MemoryHelper.Write<uint>(currPos, ref mesh.NumFaces);
            currPos = MemoryHelper.AddIntPtr(currPos, sizeof(uint));

            MemoryHelper.Write<IntPtr>(currPos, ref mesh.Vertices);
            currPos = MemoryHelper.AddIntPtr(currPos, IntPtr.Size);

            MemoryHelper.Write<IntPtr>(currPos, ref mesh.Normals);
            currPos = MemoryHelper.AddIntPtr(currPos, IntPtr.Size);

            MemoryHelper.Write<IntPtr>(currPos, ref mesh.Tangents);
            currPos = MemoryHelper.AddIntPtr(currPos, IntPtr.Size);

            MemoryHelper.Write<IntPtr>(currPos, ref mesh.BiTangents);
            currPos = MemoryHelper.AddIntPtr(currPos, IntPtr.Size);

            if(mesh.Colors == null)
                mesh.Colors = new IntPtr[AiDefines.AI_MAX_NUMBER_OF_COLOR_SETS];

            MemoryHelper.Write<IntPtr>(currPos, mesh.Colors, 0, mesh.Colors.Length);
            currPos = MemoryHelper.AddIntPtr(currPos, IntPtr.Size * AiDefines.AI_MAX_NUMBER_OF_COLOR_SETS);

            if (mesh.TextureCoords == null)
                mesh.TextureCoords = new IntPtr[AiDefines.AI_MAX_NUMBER_OF_TEXTURECOORDS];

            MemoryHelper.Write<IntPtr>(currPos, mesh.TextureCoords, 0, mesh.TextureCoords.Length);
            currPos = MemoryHelper.AddIntPtr(currPos, IntPtr.Size * AiDefines.AI_MAX_NUMBER_OF_TEXTURECOORDS);

            if (mesh.NumUVComponents == null)
                mesh.NumUVComponents = new uint[AiDefines.AI_MAX_NUMBER_OF_TEXTURECOORDS];

            MemoryHelper.Write<uint>(currPos, mesh.NumUVComponents, 0, mesh.NumUVComponents.Length);
            currPos = MemoryHelper.AddIntPtr(currPos, sizeof(uint) * AiDefines.AI_MAX_NUMBER_OF_TEXTURECOORDS);

            MemoryHelper.Write<IntPtr>(currPos, ref mesh.Faces);
            currPos = MemoryHelper.AddIntPtr(currPos, IntPtr.Size);

            MemoryHelper.Write<uint>(currPos, ref mesh.NumBones);
            currPos = MemoryHelper.AddIntPtr(currPos, sizeof(uint));

            MemoryHelper.Write<IntPtr>(currPos, ref mesh.Bones);
            currPos = MemoryHelper.AddIntPtr(currPos, IntPtr.Size);

            MemoryHelper.Write<uint>(currPos, ref mesh.MaterialIndex);
            currPos = MemoryHelper.AddIntPtr(currPos, sizeof(uint));

            MemoryHelper.Write<AiString>(currPos, ref mesh.Name);
            currPos = MemoryHelper.AddIntPtr(currPos, MemoryHelper.SizeOf<AiString>());

            MemoryHelper.Write<uint>(currPos, ref mesh.NumAnimMeshes);
            currPos = MemoryHelper.AddIntPtr(currPos, sizeof(uint));

            MemoryHelper.Write<IntPtr>(currPos, ref mesh.AnimMeshes);
        }

        /// <summary>
        /// Marshals the managed object from the unmanaged chunk of memory.
        /// </summary>
        /// <param name="nativeData">Unmanaged chunk of memory to read from.</param>
        /// <returns>Managed object marshaled.</returns>
        public Object MarshalNativeToManaged(IntPtr nativeData)
        {
            IntPtr currPos = nativeData;

            AiMesh mesh;
            mesh.PrimitiveTypes = MemoryHelper.Read<PrimitiveType>(currPos);
            currPos = MemoryHelper.AddIntPtr(currPos, sizeof(PrimitiveType));

            mesh.NumVertices = MemoryHelper.Read<uint>(currPos);
            currPos = MemoryHelper.AddIntPtr(currPos, sizeof(uint));

            mesh.NumFaces = MemoryHelper.Read<uint>(currPos);
            currPos = MemoryHelper.AddIntPtr(currPos, sizeof(uint));

            mesh.Vertices = MemoryHelper.Read<IntPtr>(currPos);
            currPos = MemoryHelper.AddIntPtr(currPos, IntPtr.Size);

            mesh.Normals = MemoryHelper.Read<IntPtr>(currPos);
            currPos = MemoryHelper.AddIntPtr(currPos, IntPtr.Size);

            mesh.Tangents = MemoryHelper.Read<IntPtr>(currPos);
            currPos = MemoryHelper.AddIntPtr(currPos, IntPtr.Size);

            mesh.BiTangents = MemoryHelper.Read<IntPtr>(currPos);
            currPos = MemoryHelper.AddIntPtr(currPos, IntPtr.Size);

            mesh.Colors = new IntPtr[AiDefines.AI_MAX_NUMBER_OF_COLOR_SETS];
            MemoryHelper.Read<IntPtr>(currPos, mesh.Colors, 0, mesh.Colors.Length);
            currPos = MemoryHelper.AddIntPtr(currPos, IntPtr.Size * AiDefines.AI_MAX_NUMBER_OF_COLOR_SETS);

            mesh.TextureCoords = new IntPtr[AiDefines.AI_MAX_NUMBER_OF_TEXTURECOORDS];
            MemoryHelper.Read<IntPtr>(currPos, mesh.TextureCoords, 0, mesh.TextureCoords.Length);
            currPos = MemoryHelper.AddIntPtr(currPos, IntPtr.Size * AiDefines.AI_MAX_NUMBER_OF_TEXTURECOORDS);

            mesh.NumUVComponents = new uint[AiDefines.AI_MAX_NUMBER_OF_TEXTURECOORDS];
            MemoryHelper.Read<uint>(currPos, mesh.NumUVComponents, 0, mesh.NumUVComponents.Length);
            currPos = MemoryHelper.AddIntPtr(currPos, sizeof(uint) * AiDefines.AI_MAX_NUMBER_OF_TEXTURECOORDS);

            mesh.Faces = MemoryHelper.Read<IntPtr>(currPos);
            currPos = MemoryHelper.AddIntPtr(currPos, IntPtr.Size);

            mesh.NumBones = MemoryHelper.Read<uint>(currPos);
            currPos = MemoryHelper.AddIntPtr(currPos, sizeof(uint));

            mesh.Bones = MemoryHelper.Read<IntPtr>(currPos);
            currPos = MemoryHelper.AddIntPtr(currPos, IntPtr.Size);

            mesh.MaterialIndex = MemoryHelper.Read<uint>(currPos);
            currPos = MemoryHelper.AddIntPtr(currPos, sizeof(uint));

            mesh.Name = MemoryHelper.Read<AiString>(currPos);
            currPos = MemoryHelper.AddIntPtr(currPos, MemoryHelper.SizeOf<AiString>());

            mesh.NumAnimMeshes = MemoryHelper.Read<uint>(currPos);
            currPos = MemoryHelper.AddIntPtr(currPos, sizeof(uint));

            mesh.AnimMeshes = MemoryHelper.Read<IntPtr>(currPos);

            return mesh;
        }
    }

    #endregion

    #region AiAnimMesh

    /// <summary>
    /// Custom marshaler for <see cref="AiAnimMesh"/>.
    /// </summary>
    public sealed class AiAnimMeshMarshaler : INativeCustomMarshaler
    {
        private int m_sizeInBytes;

        /// <summary>
        /// Gets the native data size in bytes.
        /// </summary>
        public int NativeDataSize
        {
            get
            {
                return m_sizeInBytes;
            }
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="AiAnimMeshMarshaler"/> class.
        /// </summary>
        public AiAnimMeshMarshaler()
        {
            m_sizeInBytes = sizeof(uint) + (IntPtr.Size * 4) +
                (IntPtr.Size * AiDefines.AI_MAX_NUMBER_OF_COLOR_SETS) + (IntPtr.Size * AiDefines.AI_MAX_NUMBER_OF_TEXTURECOORDS);
        }

        /// <summary>
        /// Marshals the managed object to the unmanaged chunk of memory.
        /// </summary>
        /// <param name="managedObj">Managed object to marshal.</param>
        /// <param name="nativeData">Unmanaged chunk of memory to write to.</param>
        public void MarshalManagedToNative(Object managedObj, IntPtr nativeData)
        {
            IntPtr currPos = nativeData;
            AiAnimMesh animMesh = (AiAnimMesh)managedObj;

            MemoryHelper.Write<IntPtr>(currPos, ref animMesh.Vertices);
            currPos = MemoryHelper.AddIntPtr(currPos, IntPtr.Size);

            MemoryHelper.Write<IntPtr>(currPos, ref animMesh.Normals);
            currPos = MemoryHelper.AddIntPtr(currPos, IntPtr.Size);

            MemoryHelper.Write<IntPtr>(currPos, ref animMesh.Tangents);
            currPos = MemoryHelper.AddIntPtr(currPos, IntPtr.Size);

            MemoryHelper.Write<IntPtr>(currPos, ref animMesh.BiTangents);
            currPos = MemoryHelper.AddIntPtr(currPos, IntPtr.Size);

            if (animMesh.Colors == null)
                animMesh.Colors = new IntPtr[AiDefines.AI_MAX_NUMBER_OF_COLOR_SETS];

            MemoryHelper.Write<IntPtr>(currPos, animMesh.Colors, 0, animMesh.Colors.Length);
            currPos = MemoryHelper.AddIntPtr(currPos, IntPtr.Size * AiDefines.AI_MAX_NUMBER_OF_COLOR_SETS);

            if (animMesh.TextureCoords == null)
                animMesh.TextureCoords = new IntPtr[AiDefines.AI_MAX_NUMBER_OF_TEXTURECOORDS];

            MemoryHelper.Write<IntPtr>(currPos, animMesh.TextureCoords, 0, animMesh.TextureCoords.Length);
            currPos = MemoryHelper.AddIntPtr(currPos, IntPtr.Size * AiDefines.AI_MAX_NUMBER_OF_TEXTURECOORDS);

            MemoryHelper.Write<uint>(currPos, ref animMesh.NumVertices);
        }

        /// <summary>
        /// Marshals the managed object from the unmanaged chunk of memory.
        /// </summary>
        /// <param name="nativeData">Unmanaged chunk of memory to read from.</param>
        /// <returns>Managed object marshaled.</returns>
        public Object MarshalNativeToManaged(IntPtr nativeData)
        {
            IntPtr currPos = nativeData;

            AiAnimMesh animMesh;
            animMesh.Vertices = MemoryHelper.Read<IntPtr>(currPos);
            currPos = MemoryHelper.AddIntPtr(currPos, IntPtr.Size);

            animMesh.Normals = MemoryHelper.Read<IntPtr>(currPos);
            currPos = MemoryHelper.AddIntPtr(currPos, IntPtr.Size);

            animMesh.Tangents = MemoryHelper.Read<IntPtr>(currPos);
            currPos = MemoryHelper.AddIntPtr(currPos, IntPtr.Size);

            animMesh.BiTangents = MemoryHelper.Read<IntPtr>(currPos);
            currPos = MemoryHelper.AddIntPtr(currPos, IntPtr.Size);

            animMesh.Colors = new IntPtr[AiDefines.AI_MAX_NUMBER_OF_COLOR_SETS];
            MemoryHelper.Read<IntPtr>(currPos, animMesh.Colors, 0, animMesh.Colors.Length);
            currPos = MemoryHelper.AddIntPtr(currPos, IntPtr.Size * AiDefines.AI_MAX_NUMBER_OF_COLOR_SETS);

            animMesh.TextureCoords = new IntPtr[AiDefines.AI_MAX_NUMBER_OF_TEXTURECOORDS];
            MemoryHelper.Read<IntPtr>(currPos, animMesh.TextureCoords, 0, animMesh.TextureCoords.Length);
            currPos = MemoryHelper.AddIntPtr(currPos, IntPtr.Size * AiDefines.AI_MAX_NUMBER_OF_TEXTURECOORDS);

            animMesh.NumVertices = MemoryHelper.Read<uint>(currPos);

            return animMesh;
        }
    }

    #endregion
}
