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
using Assimp.Unmanaged;

namespace Assimp {
    public class Light {
        private String _name;
        private LightSourceType _lightType;
        private float _angleInnerCone;
        private float _angleOuterCone;
        private float _attConstant;
        private float _attLinear;
        private float _attQuadratic;
        private Vector3D _position;
        private Vector3D _direction;
        private Color3D _diffuse;
        private Color3D _specular;
        private Color3D _ambient;

        public String Name {
            get {
                return _name;
            }
        }

        public LightSourceType LightType {
            get {
                return _lightType;
            }
        }

        public float AngleInnerCone {
            get {
                return _angleInnerCone;
            }
        }

        public float AngleOuterCone {
            get {
                return _angleOuterCone;
            }
        }

        public float AttenuationConstant {
            get {
                return _attConstant;
            }
        }

        public float AttenuationLinear {
            get {
                return _attLinear;
            }
        }

        public float AttenuationQuadratic {
            get {
                return _attQuadratic;
            }
        }

        public Vector3D Position {
            get {
                return _position;
            }
        }

        public Vector3D Direction {
            get {
                return _direction;
            }
        }

        public Color3D ColorDiffuse {
            get {
                return _diffuse;
            }
        }

        public Color3D ColorSpecular {
            get {
                return _specular;
            }
        }

        public Color3D ColorAmbient {
            get {
                return _ambient;
            }
        }

        internal Light(AiLight light) {
            _name = light.Name.Data;
            _lightType = light.Type;
            _angleInnerCone = light.AngleInnerCone;
            _angleOuterCone = light.AngleOuterCone;
            _attConstant = light.AttenuationConstant;
            _attLinear = light.AttenuationLinear;
            _attQuadratic = light.AttenuationQuadratic;
            _position = light.Position;
            _direction = light.Direction;
            _diffuse = light.ColorDiffuse;
            _specular = light.ColorSpecular;
            _ambient = light.ColorAmbient;
        }
    }
}
