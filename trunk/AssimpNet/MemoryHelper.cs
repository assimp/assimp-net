using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            if(pointer == IntPtr.Zero) {
                return null;
            }

            try {
                Type type = typeof(T);
                int typeSize = Marshal.SizeOf(type);
                T[] array = new T[length];

                for(int i = 0; i < length; i++) {
                    IntPtr currPos = pointer + (typeSize * i); //This will take care of any 32/64 bit size concerns.
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
