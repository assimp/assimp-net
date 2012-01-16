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

namespace Assimp {
    /// <summary>
    /// Represents a 3x3 row-major matrix.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix3x3 {
        /// <summary>
        /// Value at row 1, column 1 of the matrix
        /// </summary>
        public float M11;

        /// <summary>
        /// Value at row 1, column 2 of the matrix
        /// </summary>
        public float M12;

        /// <summary>
        /// Value at row 1, column 3 of the matrix
        /// </summary>
        public float M13;

        /// <summary>
        /// Value at row 2, column 1 of the matrix
        /// </summary>
        public float M21;

        /// <summary>
        /// Value at row 2, column 2 of the matrix
        /// </summary>
        public float M22;

        /// <summary>
        /// Value at row 2, column 3 of the matrix
        /// </summary>
        public float M23;

        /// <summary>
        /// Value at row 3, column 1 of the matrix
        /// </summary>
        public float M31;

        /// <summary>
        /// Value at row 3, column 2 of the matrix
        /// </summary>
        public float M32;

        /// <summary>
        /// Value at row 3, column 3 of the matrix
        /// </summary>
        public float M33;

        /// <summary>
        /// Gets or sets the value at the specific one-based row, column
        /// index. E.g. i = 1, j = 2 gets the value in row 1, column 2 (M12). Indices
        /// out of range return a value of zero.
        /// 
        /// </summary>
        /// <param name="i">One-based Row index</param>
        /// <param name="j">One-based Column index</param>
        /// <returns>Matrix value</returns>
        public float this[int i, int j] {
            get {
                switch(i) {
                    case 1:
                        switch(j) {
                            case 1:
                                return M11;
                            case 2:
                                return M12;
                            case 3:
                                return M13;
                            default:
                                return 0;
                        }
                    case 2:
                        switch(j) {
                            case 1:
                                return M21;
                            case 2:
                                return M22;
                            case 3:
                                return M23;
                            default:
                                return 0;
                        }
                    case 3:
                        switch(j) {
                            case 1:
                                return M31;
                            case 2:
                                return M32;
                            case 3:
                                return M33;
                            default:
                                return 0;
                        }
                    default:
                        return 0;
                }
            }
            set {
                switch(i) {
                    case 1:
                        switch(j) {
                            case 1:
                                M11 = value;
                                break;
                            case 2:
                                M12 = value;
                                break;
                            case 3:
                                M13 = value;
                                break;
                        }
                        break;
                    case 2:
                        switch(j) {
                            case 1:
                                M21 = value;
                                break;
                            case 2:
                                M22 = value;
                                break;
                            case 3:
                                M23 = value;
                                break;
                        }
                        break;
                    case 3:
                        switch(j) {
                            case 1:
                                M31 = value;
                                break;
                            case 2:
                                M32 = value;
                                break;
                            case 3:
                                M33 = value;
                                break;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Constructs a new Matrix3x3.
        /// </summary>
        /// <param name="m11">Element at row 1, column 1</param>
        /// <param name="m12">Element at row 1, column 2</param>
        /// <param name="m13">Element at row 1, column 3</param>
        /// <param name="m21">Element at row 2, column 1</param>
        /// <param name="m22">Element at row 2, column 2</param>
        /// <param name="m23">Element at row 2, column 3</param>
        /// <param name="m31">Element at row 3, column 1</param>
        /// <param name="m32">Element at row 3, column 2</param>
        /// <param name="m33">Element at row 3, column 3</param>
        public Matrix3x3(float m11, float m12, float m13, float m21, float m22, float m23, float m31, float m32, float m33) {
                this.M11 = m11;
                this.M12 = m12;
                this.M13 = m13;
                this.M21 = m21;
                this.M22 = m22;
                this.M23 = m23;
                this.M31 = m31;
                this.M32 = m32;
                this.M33 = m33;
        }
    }
}
