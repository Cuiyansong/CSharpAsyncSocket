using CYS.IO.Socket.Core.Common;

namespace CYS.IO.Socket.Core.Interface
{
    /// <summary>
    /// HSSocket基类约束接口
    /// </summary>
    public interface IHSSocket
    {
        void SyncBufferSend(SocketState ss, byte[] bytes);

        byte[] SyncBufferReceive(SocketState ss, int len);

        void ContinueAsyncReceive(SocketState ss);
    }
}
