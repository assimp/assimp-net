using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Globalization;
using Assimp;

namespace Assimp.Unmanaged {
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct AiScene {
        public SceneFlags Flags;

        public IntPtr RootNode;

        public uint NumMeshes;

        public IntPtr Meshes;

        public uint NumMaterials;

        public IntPtr Materials;

        public uint NumAnimations;

        public IntPtr Animations;

        public uint NumTextures;

        public IntPtr Textures;

        public uint NumLights;

        public IntPtr Lights;

        public uint NumCameras;

        public IntPtr Cameras;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct AiNode {
        public AiString Name;

        public Matrix4x4 Transformation;

        public IntPtr parent;

        public uint NumChildren;

        public IntPtr Children;

        public uint NumMeshes;

        public IntPtr Meshes;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct AiMesh {
        public PrimitiveType PrimitiveTypes;

        public uint NumVertices;

        public uint NumFaces;

        public IntPtr Vertices;

        public IntPtr Normals;

        public IntPtr Tangents;

        public IntPtr BiTangents;

        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst=4, ArraySubType = UnmanagedType.SysUInt)]
        public IntPtr[] Colors;

        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst=4, ArraySubType = UnmanagedType.SysUInt)]
        public IntPtr[] TextureCoords;

        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst=4, ArraySubType = UnmanagedType.U4)]
        public uint[] NumUVComponents;

        public IntPtr Faces;

        public uint NumBones;

        public IntPtr Bones;

        public uint MaterialIndex;

        public AiString Name;

        public uint NumAnimMeshes;

        public IntPtr AnimMeshes;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct AiTexture {
        public uint Width;

        public uint Height;

        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst=4)]
        public String FormatHint;

        public IntPtr Data;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct AiFace {
        public uint NumIndices;

        public IntPtr Indices;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct AiVertexWeight {
        public uint NumIndices;

        public IntPtr Indices;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct AiBone {
        public AiString Name;

        public uint NumWeights;

        public IntPtr Weights;

        public Matrix4x4 OffsetMatrix;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct AiAnimMesh {
        public IntPtr Vertices;

        public IntPtr Normals;

        public IntPtr Tangents;

        public IntPtr BiTangents;

        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.SysUInt)]
        public IntPtr[] Colors;

        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.SysUInt)]
        public IntPtr[] TextureCoords;

        public uint NumVertices;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct AiMaterialProperty {
        public AiString Key;

        public uint Semantic;

        public uint Index;

        public uint DataLength;

        public PropertyTypeInfo Type;

        //This seems wrong
        [MarshalAsAttribute(UnmanagedType.LPStr)]
        public String Data;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct AiMaterial {
        public IntPtr Properties;

        public uint NumProperties;

        public uint NumAllocated;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct AiNodeAnim {
        public AiString NodeName;

        public uint NumPositionKeys;

        public IntPtr PositionKeys;

        public uint NumRotationKeys;

        public IntPtr RotationKeys;

        public uint NumScalingKeys;

        public IntPtr ScalingKeys;

        public AnimationBehaviour Prestate;

        public AnimationBehaviour PostState;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct AiMeshAnim {
        public AiString Name;

        public uint NumKeys;

        public IntPtr Keys;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct AiAnimation {
        public AiString Name;

        public double Duration;

        public double TicksPerSecond;

        public uint NumChannels;

        public IntPtr Channels;

        public uint NumMeshChannels;

        public IntPtr MeshChannels;
    }


    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct AiLight {
        public AiString Name;

        public LightSourceType Type;

        public Vector3D Position;

        public Vector3D Direction;

        public float AttenuationConstant;

        public float AttenuationLinear;

        public float AttenuationQuadratic;

        public Color3D ColorDiffuse;

        public Color3D ColorSpecular;

        public Color3D ColorAmbient;

        public float AngleInnerCone;

        public float AngleOuterCone;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct AiCamera {
        public AiString Name;

        public Vector3D Position;

        public Vector3D Up;

        public Vector3D LookAt;

        public float HorizontalFOV;

        public float ClipPlaneNear;

        public float ClipPlaneFar;

        public float Aspect;
    }

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct AiString {
        public uint Length;

        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public String Data;
    }

}
