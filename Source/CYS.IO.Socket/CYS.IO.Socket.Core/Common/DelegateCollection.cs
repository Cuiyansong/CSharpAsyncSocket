
namespace CYS.IO.Socket.Core.Common
{
    public class DelegateCollection
    {
        /// <summary>
        /// ExecuteCommandEventHandler delegate
        /// </summary>
        /// <param name="p"></param>
        /// <param name="s"></param>
        public delegate void ExecuteCommandEventHandler(CommunicateProtocol p, SocketState s);
    }
}
