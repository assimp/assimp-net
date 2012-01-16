using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Assimp.Unmanaged;

namespace Assimp {
    public class Scene {
        private SceneFlags _flags;
        private MemoryInfo _memInfo;
        private Node _rootNode;
        private Mesh[] _meshes;
        private Light[] _lights;

        //TODO:
        //Cameras
        //Animations
        //Materials
        //Textures

        public SceneFlags SceneFlags {
            get {
                return _flags;
            }
        }

        public MemoryInfo MemoryInfo {
            get {
                return _memInfo;
            }
        }

        public Node RootNode {
            get {
                return _rootNode;
            }
        }

        public int MeshCount {
            get {
                return (_meshes == null) ? 0 : _meshes.Length;
            }
        }

        public bool HasMeshes {
            get {
                return _meshes != null;
            }
        }

        public Mesh[] Meshes {
            get {
                return _meshes;
            }
        }

        public int LightCount {
            get {
                return (_lights == null) ? 0 : _lights.Length;
            }
        }

        public bool HasLights {
            get {
                return _lights != null;
            }
        }

        public Light[] Lights {
            get {
                return _lights;
            }
        }

        internal Scene(AiScene scene, MemoryInfo memInfo) {
            _memInfo = memInfo;
            _flags = scene.Flags;

            if(scene.NumMeshes > 0 && scene.Meshes != IntPtr.Zero) {
                AiMesh[] meshes = MemoryHelper.MarshalArray<AiMesh>(Marshal.ReadIntPtr(scene.Meshes), (int) scene.NumMeshes);
                _meshes = new Mesh[meshes.Length];
                for(int i = 0; i < _meshes.Length; i++) {
                    _meshes[i] = new Mesh(meshes[i]);
                }
            }

            if(scene.RootNode != IntPtr.Zero) {
                _rootNode = new Node(MemoryHelper.MarshalStructure<AiNode>(scene.RootNode), null);
            }

            if(scene.NumLights > 0 && scene.Lights != IntPtr.Zero) {
                AiLight[] lights = MemoryHelper.MarshalArray<AiLight>(Marshal.ReadIntPtr(scene.Lights), (int) scene.NumLights);
                _lights = new Light[lights.Length];
                for(int i = 0; i < _lights.Length; i++) {
                    _lights[i] = new Light(lights[i]);
                }
            }
        }
    }
}
