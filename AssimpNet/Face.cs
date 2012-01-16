using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assimp.Unmanaged;

namespace Assimp {
    /// <summary>
    /// A single face in a mesh, referring to multiple vertices. This can be a triangle
    /// if the index count is equal to three, or a polygon if the count is greater than three.
    /// 
    /// Since multiple primitive types can be contained in a single mesh, this approach
    /// allows you to better examine how the mesh is constructed. If you use the <see cref="Assimp.PostProcessSteps.SortByPrimitiveType"/>
    /// post process step flag during import, then each mesh will be homogenous where primitive type is concerned.
    /// </summary>
    public class Face {
        private uint _numIndices;
        private uint[] _indices;
        private int[] _intIndices;

        /// <summary>
        /// Gets the number of indices defined in the face.
        /// </summary>
        public uint IndexCount {
            get {
                return _numIndices;
            }
        }

        /// <summary>
        /// Gets the indices that refer to positions of vertex data in the mesh's vertex 
        /// arrays.
        /// </summary>
        public uint[] Indices {
            get {
                return _indices;
            }
        }

        //Internal use only
        internal int[] IntIndices {
            get {
                return _intIndices;
            }
        }

        /// <summary>
        /// Constructs a new Face.
        /// </summary>
        /// <param name="face">Unmanaged AiFace structure</param>
        internal Face(AiFace face) {
            _numIndices = face.NumIndices;

            if(_numIndices > 0 && face.Indices != IntPtr.Zero) {
                _indices = MemoryHelper.MarshalArray<uint>(face.Indices, (int)_numIndices);
                _intIndices = MemoryHelper.MarshalArray<int>(face.Indices, (int) _numIndices);
            }
        }
    }
}
