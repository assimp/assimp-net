using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assimp.Unmanaged;

namespace Assimp {
    /// <summary>
    /// Describes a right-handed camera in the scene. An important aspect is that
    /// the camera itself is also part of the scenegraph, meaning any values such
    /// as the direction vector are not *absolute*, they can be relative to the coordinate
    /// system defined by the node which corresponds to the camera. This allows for camera
    /// animations.
    /// </summary>
    public class Camera {
        private String _name;
        private Vector3D _position;
        private Vector3D _up;
        private Vector3D _direction;
        private float _fieldOfView;
        private float _clipPlaneNear;
        private float _clipPlaneFar;
        private float _aspectRatio;

        /// <summary>
        /// Gets the name of the camera. This corresponds to a node in the
        /// scenegraph with the same name. This node specifies the position of the
        /// camera in the scene hierarchy and can be animated.
        /// </summary>
        public String Name {
            get {
                return _name;
            }
        }

        /// <summary>
        /// Gets the position of the camera relative to the coordinate space defined by
        /// the corresponding node. THe default value is 0|0|0.
        /// </summary>
        public Vector3D Position {
            get {
                return _position;
            }
        }

        /// <summary>
        /// Gets the 'up' vector of the camera, relative to the coordinate space defined by the
        /// corresponding node. The 'right' vector of the camera is the cross product of the up
        /// and direction vectors. The default value is 0|1|0.
        /// </summary>
        public Vector3D Up {
            get {
                return _up;
            }
        }

        /// <summary>
        /// Gets the viewing direction of the camera, relative to the coordiante space defined by the corresponding node.
        /// The default value is 0|0|1.
        /// </summary>
        public Vector3D Direction {
            get {
                return _direction;
            }
        }

        /// <summary>
        /// Gets the half horizontal field of view angle, in radians. The FoV angle is
        /// the angle between the center line of the screen and the left or right border. The default
        /// value is 1/4PI.
        /// </summary>
        public float FieldOfview {
            get {
                return _fieldOfView;
            }
        }

        /// <summary>
        /// Gets the distance of the near clipping plane from the camera. The value may not
        /// be 0.0f for arithmetic reasons to prevent a division through zero. The default value is 0.1f;
        /// </summary>
        public float ClipPlaneNear {
            get {
                return _clipPlaneNear;
            }
        }

        /// <summary>
        /// Gets the distance of the far clipping plane from the camera. The far clippling plane must
        /// be further than the near clippling plane. The default value is 1000.0f. The ratio between
        /// the near and far plane should not be too large (between 1000 - 10000 should be ok) to avoid
        /// floating-point inaccuracies which can lead to z-fighting.
        /// </summary>
        public float ClipPlaneFar {
            get {
                return _clipPlaneFar;
            }
        }

        /// <summary>
        /// Gets the screen aspect ratio. This is the ratio between the width and height of the screen. Typical
        /// values are 4/3, 1/2, or 1/1. This value is 0 if the aspect ratio is not defined in the source file.
        /// The default value is zero.
        /// </summary>
        public float AspectRatio {
            get {
                return _aspectRatio;
            }
        }

        /// <summary>
        /// Gets a right-handed view matrix.
        /// </summary>
        public Matrix4x4 ViewMatrix {
            get {
                Vector3D zAxis = _direction;
                zAxis.Normalize();
                Vector3D yAxis = _up;
                yAxis.Normalize();
                Vector3D xAxis = Vector3D.Cross(_up, _direction);
                zAxis.Normalize();

                //Assimp docs *say* they deal with Row major matrices,
                //but aiCamera.h has this calc done with translation in the 4th column
                Matrix4x4 mat;
                mat.M11 = xAxis.X;
                mat.M12 = xAxis.Y;
                mat.M13 = xAxis.Z;
                mat.M14 = 0;

                mat.M21 = yAxis.X;
                mat.M22 = yAxis.Y;
                mat.M23 = yAxis.Z;
                mat.M24 = 0;

                mat.M31 = zAxis.X;
                mat.M32 = zAxis.Y;
                mat.M33 = zAxis.Z;
                mat.M34 = 0;

                mat.M41 = -(Vector3D.Dot(xAxis, _position));
                mat.M42 = -(Vector3D.Dot(yAxis, _position));
                mat.M43 = -(Vector3D.Dot(zAxis, _position));
                mat.M44 = 1.0f;

                return mat;
            }
        }

        /// <summary>
        /// Constructs a new Camera.
        /// </summary>
        /// <param name="camera">Unmanaged aiCamera</param>
        internal Camera(AiCamera camera) {
            _name = camera.Name.Data;
            _position = camera.Position;
            _direction = camera.LookAt;
            _up = camera.Up;
            _fieldOfView = camera.HorizontalFOV;
            _clipPlaneFar = camera.ClipPlaneFar;
            _clipPlaneNear = camera.ClipPlaneNear;
            _aspectRatio = camera.Aspect;
        }
    }
}
