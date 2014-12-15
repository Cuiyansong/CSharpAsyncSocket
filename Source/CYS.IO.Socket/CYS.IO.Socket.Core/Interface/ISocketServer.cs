using System;
using CYS.IO.Socket.Core.Common;

namespace CYS.IO.Socket.Core.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISocketServer : IHSSocket, IDisposable
    {
        void AsyncConnect();

        event DelegateCollection.ExecuteCommandEventHandler ExecuteProtocolCommand;

        event EventHandler<EventArgCollection.SocketEventArgs> OnSocketEventRaised;

        event EventHandler<EventArgCollection.SocketErrorEventArgs> OnSocketErrorRaised;
    }
}
