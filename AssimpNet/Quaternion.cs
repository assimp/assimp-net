using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Globalization;

namespace Assimp {
    /// <summary>
    /// A 4D vector that represents a rotation.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Quaternion {

        public float W;

        public float X;

        public float Y;

        public float Z;
    }
}
