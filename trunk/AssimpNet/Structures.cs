using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
}
