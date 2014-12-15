using System;

namespace CYS.IO.Socket.Core.Common
{
    public class EventArgCollection
    {
        /// <summary>
        /// ConnectionEventArgs
        /// </summary>
        public class ConnectionEventArgs : EventArgs
        {
            public bool IsConnected { get; set; }
        }

        /// <summary>
        /// SocketEventArgs
        /// </summary>
        public class SocketEventArgs : EventArgs
        {
            /// <summary>
            /// ClientName
            /// </summary>
            public string ClientName { get; set; }

            /// <summary>
            /// IsConnect
            /// </summary>
            public bool IsConnect { get; set; }

            /// <summary>
            /// Message
            /// </summary>
            public string Msg { get; set; }
        }

        /// <summary>
        /// ErrorEventArgs
        /// </summary>
        public class SocketErrorEventArgs : EventArgs
        {
            /// <summary>
            /// Exception
            /// </summary>
            public Exception Exp { get; set; }
            /// <summary>
            /// Message
            /// </summary>
            public string Msg { get; set; }
        }
    }
}
