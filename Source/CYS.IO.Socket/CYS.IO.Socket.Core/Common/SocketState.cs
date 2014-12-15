using System.Text;

namespace CYS.IO.Socket.Core.Common
{
    public class SocketState
    {
        /// <summary>
        /// isReceiveThreadAlive
        /// </summary>
        private bool isReceiveThreadAlive = false;

        /// <summary>
        /// Client socket. 
        /// </summary>
        public System.Net.Sockets.Socket workSocket = null;
        /// <summary>
        /// Size of receive buffer. 
        /// </summary>
        public const int BufferSize = 1024;
        /// <summary>
        /// Receive buffer.   
        /// </summary>
        public byte[] buffer = new byte[BufferSize];
        /// <summary>
        /// Received data string.
        /// </summary>
        public StringBuilder sb = new StringBuilder();
        /// <summary>
        /// Is socket begin receive data.
        /// </summary>
        public bool IsReceiveThreadAlive
        {
            get
            {
                lock (this)
                {
                    return isReceiveThreadAlive;
                }
            }
            set
            {
                lock (this)
                {
                    isReceiveThreadAlive = value;
                }
            }
        }
        /// <summary>
        /// OutPutMsg
        /// </summary>
        public string OutPutMsg { get; set; }
    }
}
