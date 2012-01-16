using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
