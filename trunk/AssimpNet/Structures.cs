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

using System.Runtime.InteropServices;

namespace Assimp {
    /// <summary>
    /// Represents the memory requirements for the different components of an imported
    /// scene. All sizes in in bytes.
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct MemoryInfo {

        /// <summary>
        /// Size of the storage allocated for texture data, in bytes.
        /// </summary>
        public uint Textures;

        /// <summary>
        /// Size of the storage allocated for material data, in bytes.
        /// </summary>
        public uint Materials;

        /// <summary>
        /// Size of the storage allocated for mesh data, in bytes.
        /// </summary>
        public uint Meshes;

        /// <summary>
        /// Size of the storage allocated for node data, in bytes.
        /// </summary>
        public uint Nodes;

        /// <summary>
        /// Size of the storage allocated for animation data, in bytes.
        /// </summary>
        public uint Animations;

        /// <summary>
        /// Size of the storage allocated for camera data, in bytes.
        /// </summary>
        public uint Cameras;

        /// <summary>
        /// Size of the storage allocated for light data, in bytes.
        /// </summary>
        public uint Lights;

        /// <summary>
        /// Total storage allocated for the imported scene, in bytes.
        /// </summary>
        public uint Total;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct VectorKey {
        public double Time;

        public Vector3D value;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct QuaternionKey {
        public double Time;

        public Quaternion Value;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct Meshkey {
        public double Time;

        public uint Value;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct UVTransform {
        public Vector2D Translation;

        public Vector2D Scaling;

        public float Rotation;
    }

    /// <summary>
    /// Represents a texel in ARGB8888 format.
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct Texel {
        public byte B;

        public byte G;

        public byte R;

        public byte A;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct VertexWeight {
        public uint VertexID;

        public float Weight;
    }
}
