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
using System.Globalization;
using System.Runtime.InteropServices;

namespace Assimp {
    /// <summary>
    /// A 4D vector that represents a rotation.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Quaternion : IEquatable<Quaternion> {
        /// <summary>
        /// Rotation component of the quaternion/
        /// </summary>
        public float W;

        /// <summary>
        /// X component of the vector part of the quaternion.
        /// </summary>
        public float X;

        /// <summary>
        /// Y component of the vector part of the quaternion.
        /// </summary>
        public float Y;

        /// <summary>
        /// Z component of the vector part of the quaternion.
        /// </summary>
        public float Z;

        /// <summary>
        /// Constructs a new Quaternion.
        /// </summary>
        /// <param name="w">W component</param>
        /// <param name="x">X component</param>
        /// <param name="y">Y component</param>
        /// <param name="z">Z component</param>
        public Quaternion(float w, float x, float y, float z) {
            W = w;
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Constructs a new Quaternion from a rotation matrix.
        /// </summary>
        /// <param name="matrix">Rotation matrix to create the Quaternion from.</param>
        public Quaternion(Matrix3x3 matrix) {
            float t = 1 + matrix.A1 + matrix.B1 + matrix.C1;

            //Large enough
            if(t > 0.001f) {
                float s = (float)Math.Sqrt(t) * 2.0f;
                X = (matrix.C2 - matrix.B3) / s;
                Y = (matrix.A3 - matrix.C1) / s;
                Z = (matrix.B1 - matrix.A2) / s;
                W = 0.25f * s;
            //Else have to check several cases
            } else if((matrix.A1 > matrix.B2) && (matrix.A1 > matrix.C3)) {
                //Column One:
                float s = (float) Math.Sqrt(((1.0f + matrix.A1) - matrix.B2) - matrix.C3) * 2.0f;
                X = 0.25f * s;
                Y = (matrix.B1 + matrix.A2) / s;
                Z = (matrix.A3 + matrix.C1) / s;
                W = (matrix.C2 - matrix.B3) / s;
            } else if(matrix.B2 > matrix.C3) {
                //Column Two:
                float s = (float) Math.Sqrt(((1.0f + matrix.B2) - matrix.A1) - matrix.C3) * 2.0f;
                X = (matrix.B1 + matrix.A2) / s;
                Y = 0.25f * s;
                Z = (matrix.C2 + matrix.B3) / s;
                W = (matrix.A3 - matrix.C1) / s;
            } else {
                //Column Three:
                float s = (float) Math.Sqrt(((1.0f + matrix.C3) - matrix.A1) - matrix.B2) * 2.0f;
                X = (matrix.A3 + matrix.C1) / s;
                Y = (matrix.C2 + matrix.B3) / s;
                Z = 0.25f * s;
                W = (matrix.B1 - matrix.A2) / s;
            }
        }

        /// <summary>
        /// Constructs a new Quaternion from three euler angles.
        /// </summary>
        /// <param name="pitch">Pitch</param>
        /// <param name="yaw">Yaw</param>
        /// <param name="roll">Roll</param>
        public Quaternion(float pitch, float yaw, float roll) {
            float sinPitch = (float) Math.Sin(pitch * .5f);
            float cosPitch = (float) Math.Cos(pitch * .5f);
            float sinYaw = (float) Math.Sin(yaw * .5f);
            float cosYaw = (float) Math.Cos(yaw * .5f);
            float sinRoll = (float) Math.Sin(roll * .5f);
            float cosRoll = (float) Math.Cos(roll * .5f);
            float cosPitchCosYaw = cosPitch * cosYaw;
            float sinPitchSinYaw = sinPitch * sinYaw;

            X = (sinRoll * cosPitchCosYaw) - (cosRoll * sinPitchSinYaw);
            Y = (cosRoll * sinPitch * cosYaw) + (sinRoll * cosPitch * sinYaw);
            Z = (cosRoll * cosPitch * sinYaw) - (sinRoll * sinPitch * cosYaw);
            W = (cosRoll * cosPitchCosYaw) + (sinRoll * sinPitchSinYaw);
        }

        /// <summary>
        /// Constructs a new Quaternion from an axis-angle.
        /// </summary>
        /// <param name="axis">Axis</param>
        /// <param name="angle">Angle about the axis</param>
        public Quaternion(Vector3D axis, float angle) {
            axis.Normalize();
            float sinAngle = (float) Math.Sin(angle / 2.0f);
            float cosAngle = (float) Math.Cos(angle / 2.0f);
            X = axis.X * sinAngle;
            Y = axis.Y * sinAngle;
            Z = axis.Z * sinAngle;
            W = cosAngle;
        }

        /// <summary>
        /// Normalizes the quaternion.
        /// </summary>
        public void Normalize() {
            float mag = (X * X)+ (Y * Y) + (Z * Z) + (W * W);
            if(mag != 0) {
                X /= mag;
                Y /= mag;
                Z /= mag;
                W /= mag;
            }
        }

        /// <summary>
        /// Transforms this quaternion into its conjugate.
        /// </summary>
        public void Conjugate() {
            X = -X;
            Y = -Y;
            Z = -Z;
        }

        /// <summary>
        /// Returns a matrix representation of the quaternion.
        /// </summary>
        /// <returns></returns>
        public Matrix3x3 GetMatrix() {
            Matrix3x3 mat;
            mat.A1 = 1.0f - 2.0f * (Y * Y + Z * Z);
            mat.A2 = 2.0f * (X * Y - Z * W);
            mat.A3 = 2.0f * (X * Y + Y * W);

            mat.B1 = 2.0f * (X * Y + Z * W);
            mat.B2 = 1.0f - 2.0f * (X * X + Z * Z);
            mat.B3 = 2.0f * (Y * Z - X * W);

            mat.C1 = 2.0f * (X * Z - Y * W);
            mat.C2 = 2.0f * (Y * Z + X * W);
            mat.C3 = 1.0f - 2.0f * (X * X + Y * Y);

            return mat;
        }

        /// <summary>
        /// Spherical interpolation between two quaternions.
        /// </summary>
        /// <param name="start">Start rotation when factor == 0</param>
        /// <param name="end">End rotation when factor == 1</param>
        /// <param name="factor">Interpolation factor between 0 and 1, values beyond this range yield undefined values</param>
        /// <returns></returns>
        public static Quaternion Slerp(Quaternion start, Quaternion end, float factor) {
            //Calc cosine theta
            float cosom = (start.X * end.X) + (start.Y * end.Y) + (start.Z * end.Z) + (start.W * end.W);

            //Reverse signs if needed
            if(cosom < 0.0f) {
                cosom = -cosom;
                end.X = -end.X;
                end.Y = -end.Y;
                end.Z = -end.Z;
                end.W = -end.W;
            }

            //calculate coefficients
            float sclp, sclq;
            //0.0001 -> some episilon
            if((1.0f - cosom) > 0.0001f) {
                //Do a slerp
                float omega, sinom;
                omega = (float) Math.Acos(cosom); //extract theta from the product's cos theta
                sinom = (float) Math.Sin(omega);
                sclp = (float) Math.Sin((1.0f - factor) * omega) / sinom;
                sclq = (float) Math.Sin(factor * omega) / sinom;
            } else {
                //Very close, do a lerp instead since its faster
                sclp = 1.0f - factor;
                sclq = factor;
            }

            Quaternion q;
            q.X = (sclp * start.X) + (sclq * end.X);
            q.Y = (sclp * start.Y) + (sclq * end.Y);
            q.Z = (sclp * start.Z) + (sclq * end.Z);
            q.W = (sclp * start.W) + (sclq * end.W);
            return q;
        }

        /// <summary>
        /// Rotates a point by this quaternion.
        /// </summary>
        /// <param name="vec">Point to rotate</param>
        /// <param name="quat">Quaternion representing the rotation</param>
        /// <returns></returns>
        public static Vector3D Rotate(Vector3D vec, Quaternion quat) {
            float x2 = quat.X + quat.X;
            float y2 = quat.Y + quat.Y;
            float z2 = quat.Z + quat.Z;

            float wx2 = quat.W * x2;
            float wy2 = quat.W * y2;
            float wz2 = quat.W * z2;

            float xx2 = quat.X * x2;
            float xy2 = quat.X * y2;
            float xz2 = quat.X * z2;

            float yy2 = quat.Y * y2;
            float yz2 = quat.Y * z2;

            float zz2 = quat.Z * z2;

            float x = ((vec.X * ((1.0f - yy2) - zz2)) + (vec.Y * (xy2 - wz2))) + (vec.Z * (xz2 + wy2));
            float y = ((vec.X * (xy2 + wz2)) + (vec.Y * ((1.0f - xx2) - zz2))) + (vec.Z * (yz2 - wx2));
            float z = ((vec.X * (xz2 - wy2)) + (vec.Y * (yz2 + wx2))) + (vec.Z * ((1.0f - xx2) - yy2));

            Vector3D v;
            v.X = x;
            v.Y = y;
            v.Z = z;
            return v;
        }

        /// <summary>
        /// Multiplies two quaternions.
        /// </summary>
        /// <param name="a">First quaternion</param>
        /// <param name="b">Second quaternion</param>
        /// <returns>Resulting quaternion</returns>
        public static Quaternion operator*(Quaternion a, Quaternion b) {
            Quaternion q;
            q.W = (a.W * b.W) - (a.X * b.X) - (a.Y * b.Y) - (a.Z * b.Z);
            q.X = (a.W * b.X) + (a.X * b.W) + (a.Y * b.Z) - (a.Z * b.Y);
            q.Y = (a.W * b.Y) + (a.Y * b.W) + (a.Z * b.X) - (a.X * b.Z);
            q.Z = (a.W * b.Z) + (a.Z * b.W) + (a.X * b.Y) - (a.Y * b.X);
            return q;
        }

        /// <summary>
        /// Tests equality between two quaternions.
        /// </summary>
        /// <param name="a">First quaternion</param>
        /// <param name="b">Second quaternion</param>
        /// <returns>True if the quaternions are equal, false otherwise.</returns>
        public static bool operator==(Quaternion a, Quaternion b) {
            return (a.W == b.W) && (a.X == b.X) && (a.Y == b.Y) && (a.Z == b.Z);
        }

        /// <summary>
        /// Tests inequality between two quaternions.
        /// </summary>
        /// <param name="a">First quaternion</param>
        /// <param name="b">Second quaternion</param>
        /// <returns>True if the quaternions are not equal, false otherwise.</returns>
        public static bool operator!=(Quaternion a, Quaternion b) {
            return (a.W != b.W) || (a.X != b.X) || (a.Y != b.Y) || (a.Z != b.Z);
        }

        /// <summary>
        /// Tests equality between two quaternions.
        /// </summary>
        /// <param name="other">Quaternion to compare</param>
        /// <returns>True if the quaternions are equal.</returns>
        public bool Equals(Quaternion other) {
            return (W == other.W) && (X == other.X) && (Y == other.Y) && (Z == other.Z);
        }

        /// <summary>
        /// Tests equality between this color and another object.
        /// </summary>
        /// <param name="obj">Object to test against</param>
        /// <returns>True if the object is a color and the components are equal</returns>
        public override bool Equals(object obj) {
            if(obj is Quaternion) {
                return Equals((Quaternion) obj);
            }
            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return W.GetHashCode() + X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() {
            CultureInfo info = CultureInfo.CurrentCulture;
            return String.Format(info, "{{W:{0} X:{1} Y:{2} Z:{3}}}",
                new Object[] { W.ToString(info), X.ToString(info), Y.ToString(info), Z.ToString(info) });
        }
    }
}
