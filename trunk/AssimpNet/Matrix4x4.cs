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
    /// Represents a 4x4 matrix. Assimp docs say their matrices are always row-major,
    /// and it looks like they're only describing the memory layout. Matrices are treated
    /// as column vectors however (X base in the first column, Y base the second, Z base the third,
    /// and fourth column contains the translation).
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix4x4 {
        /// <summary>
        /// Value at row 1, column 1 of the matrix
        /// </summary>
        public float A1;

        /// <summary>
        /// Value at row 1, column 2 of the matrix
        /// </summary>
        public float A2;

        /// <summary>
        /// Value at row 1, column 3 of the matrix
        /// </summary>
        public float A3;

        /// <summary>
        /// Value at row 1, column 4 of the matrix
        /// </summary>
        public float A4;

        /// <summary>
        /// Value at row 2, column 1 of the matrix
        /// </summary>
        public float B1;

        /// <summary>
        /// Value at row 2, column 2 of the matrix
        /// </summary>
        public float B2;

        /// <summary>
        /// Value at row 2, column 3 of the matrix
        /// </summary>
        public float B3;

        /// <summary>
        /// Value at row 2, column 4 of the matrix
        /// </summary>
        public float B4;

        /// <summary>
        /// Value at row 3, column 1 of the matrix
        /// </summary>
        public float C1;

        /// <summary>
        /// Value at row 3, column 2 of the matrix
        /// </summary>
        public float C2;

        /// <summary>
        /// Value at row 3, column 3 of the matrix
        /// </summary>
        public float C3;

        /// <summary>
        /// Value at row 3, column 4 of the matrix
        /// </summary>
        public float C4;

        /// <summary>
        /// Value at row 4, column 1 of the matrix
        /// </summary>
        public float D1;

        /// <summary>
        /// Value at row 4, column 2 of the matrix
        /// </summary>
        public float D2;

        /// <summary>
        /// Value at row 4, column 3 of the matrix
        /// </summary>
        public float D3;

        /// <summary>
        /// Value at row 4, column 4 of the matrix
        /// </summary>
        public float D4;

        /// <summary>
        /// Gets or sets the value at the specific one-based row, column
        /// index. E.g. i = 1, j = 2 gets the value in row 1, column 2 (MA2). Indices
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
                                return A1;
                            case 2:
                                return A2;
                            case 3:
                                return A3;
                            case 4:
                                return A4;
                            default:
                                return 0;
                        }
                    case 2:
                        switch(j) {
                            case 1:
                                return B1;
                            case 2:
                                return B2;
                            case 3:
                                return B3;
                            case 4:
                                return B4;
                            default:
                                return 0;
                        }
                    case 3:
                        switch(j) {
                            case 1:
                                return C1;
                            case 2:
                                return C2;
                            case 3:
                                return C3;
                            case 4:
                                return C4;
                            default:
                                return 0;
                        }
                    case 4:
                        switch(j) {
                            case 1:
                                return D1;
                            case 2:
                                return D2;
                            case 3:
                                return D3;
                            case 4:
                                return D4;
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
                                A1 = value;
                                break;
                            case 2:
                                A2 = value;
                                break;
                            case 3:
                                A3 = value;
                                break;
                            case 4:
                                A4 = value;
                                break;
                        }
                        break;
                    case 2:
                        switch(j) {
                            case 1:
                                B1 = value;
                                break;
                            case 2:
                                B2 = value;
                                break;
                            case 3:
                                B3 = value;
                                break;
                            case 4:
                                B4 = value;
                                break;
                        }
                        break;
                    case 3:
                        switch(j) {
                            case 1:
                                C1 = value;
                                break;
                            case 2:
                                C2 = value;
                                break;
                            case 3:
                                C3 = value;
                                break;
                            case 4:
                                C4 = value;
                                break;
                        }
                        break;
                    case 4:
                        switch(j) {
                            case 1:
                                D1 = value;
                                break;
                            case 2:
                                D2 = value;
                                break;
                            case 3:
                                D3 = value;
                                break;
                            case 4:
                                D4 = value;
                                break;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Constructs a new Matrix4x4.
        /// </summary>
        /// <param name="m11">Element at row 1, column 1</param>
        /// <param name="m12">Element at row 1, column 2</param>
        /// <param name="m13">Element at row 1, column 3</param>
        /// <param name="m14">Element at row 1, column 4</param>
        /// <param name="m21">Element at row 2, column 1</param>
        /// <param name="m22">Element at row 2, column 2</param>
        /// <param name="m23">Element at row 2, column 3</param>
        /// <param name="m24">Element at row 2, column 4</param>
        /// <param name="m31">Element at row 3, column 1</param>
        /// <param name="m32">Element at row 3, column 2</param>
        /// <param name="m33">Element at row 3, column 3</param>
        /// <param name="m34">Element at row 3, column 4</param>
        /// <param name="m41">Element at row 4, column 1</param>
        /// <param name="m42">Element at row 4, column 2</param>
        /// <param name="m43">Element at row 4, column 3</param>
        /// <param name="m44">Element at row 4, column 4</param>
        public Matrix4x4(float m11, float m12, float m13, float m14, float m21, float m22, float m23, float m24,
            float m31, float m32, float m33, float m34, float m41, float m42, float m43, float m44) {
                this.A1 = m11;
                this.A2 = m12;
                this.A3 = m13;
                this.A4 = m14;
                this.B1 = m21;
                this.B2 = m22;
                this.B3 = m23;
                this.B4 = m24;
                this.C1 = m31;
                this.C2 = m32;
                this.C3 = m33;
                this.C4 = m34;
                this.D1 = m41;
                this.D2 = m42;
                this.D3 = m43;
                this.D4 = m44;
        }
    }
}
