using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Assimp {
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Ray {
        public Vector3D Position;

        public Vector3D Direction;
    }
}
