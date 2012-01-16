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
