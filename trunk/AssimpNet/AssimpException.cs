using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assimp {
    public class AssimpException : Exception {

        public AssimpException() : base() {}

        public AssimpException(String msg) : base(msg) {}

        public AssimpException(String paramName, String msg)
            : base("Parameter: " + paramName + " Error: " + msg) { }

        public AssimpException(String msg, Exception innerException) : base(msg, innerException) {}
    }
}
