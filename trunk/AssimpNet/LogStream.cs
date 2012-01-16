using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Assimp {

    /// <summary>
    /// Callback delegate for Assimp's LogStream.
    /// </summary>
    /// <param name="msg">Log message</param>
    /// <param name="userData">User data that is passed to the callback</param>
    public delegate void LogStreamCallback([InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] String msg, IntPtr userData);

    /// <summary>
    /// Represents a log stream, which receives all log messages and
    /// streams them somewhere.
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct LogStream {
        private LogStreamCallback _callback;

        [MarshalAs(UnmanagedType.LPStr)]
        private String _userData;

        /// <summary>
        /// Callback that is called when a message is logged.
        /// </summary>
        public LogStreamCallback Callback {
            get {
                return _callback;
            }
        }

        /// <summary>
        /// User data to be passed to the callback.
        /// </summary>
        public String UserData {
            get {
                return _userData;
            }
        }

        public LogStream(LogStreamCallback callback) {
            _callback = callback;
            _userData = null;
        }

        public LogStream(LogStreamCallback callback, String userData) {
            _callback = callback;
            _userData = userData;
        }
    }
}
