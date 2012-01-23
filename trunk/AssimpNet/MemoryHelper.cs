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
    /// Helper static class containing functions that aid dealing with unmanaged memory to managed memory conversions.
    /// </summary>
    public static class MemoryHelper {

        /// <summary>
        /// Marshals a c-style pointer array to a managed array of structs. This will read
        /// from the start of the IntPtr provided and care should be taken in ensuring that the number
        /// of elements to read is correct.
        /// </summary>
        /// <typeparam name="T">Struct type</typeparam>
        /// <param name="pointer">Pointer to unmanaged memory</param>
        /// <param name="length">Number of elements to marshal</param>
        /// <returns>Managed array, or null if the pointer was not valid</returns>
        public static T[] MarshalArray<T>(IntPtr pointer, int length) where T : struct {
            return MarshalArray<T>(pointer, length, false);
        }

        /// <summary>
        /// Marshals a c-style pointer array to a manged array of structs. Takes in a parameter denoting if the
        /// pointer is a "pointer to a pointer" (void**) which requires some extra care. This will read from the start of
        /// the IntPtr and care should be taken in esnuring that the number of elements to read is correct.
        /// </summary>
        /// <typeparam name="T">Struct type</typeparam>
        /// <param name="pointer">Pointer to unmanaged memory</param>
        /// <param name="length">Number of elements to marshal</param>
        /// <param name="pointerToPointer">True if the unmanaged pointer is void** or not.</param>
        /// <returns>Managed array, or null if the pointer was not valid</returns>
        public static T[] MarshalArray<T>(IntPtr pointer, int length, bool pointerToPointer) where T : struct {
            if(pointer == IntPtr.Zero) {
                return null;
            }

            try {
                Type type = typeof(T);
                //If the pointer is a void** we need to step by the pointer size, otherwise it's just a void* and step by the type size.
                int stride = (pointerToPointer) ? IntPtr.Size : Marshal.SizeOf(typeof(T));
                T[] array = new T[length];

                for(int i = 0; i < length; i++) {
                    IntPtr currPos = pointer + (stride * i);
                    //If pointer is a void**, read the current position to get the proper pointer
                    if(pointerToPointer) {
                        currPos = Marshal.ReadIntPtr(currPos);
                    }
                    array[i] = (T) Marshal.PtrToStructure(currPos, type);
                }
                return array;
            } catch(Exception) {
                return null;
            }
        }

        /// <summary>
        /// Convienence method for marshaling a pointer to a structure.
        /// </summary>
        /// <typeparam name="T">Struct type</typeparam>
        /// <param name="ptr">Pointer to marshal</param>
        /// <returns>Marshaled structure</returns>
        public static T MarshalStructure<T>(IntPtr ptr) where T : struct {
            if(ptr == IntPtr.Zero) {
                return default(T);
            }
            return (T) Marshal.PtrToStructure(ptr, typeof(T));
        }
    }
}
