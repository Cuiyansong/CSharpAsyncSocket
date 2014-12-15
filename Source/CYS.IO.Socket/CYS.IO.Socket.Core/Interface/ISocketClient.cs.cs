using System;
using CYS.IO.Socket.Core.Common;

namespace CYS.IO.Socket.Core.Interface
{
    public interface ISocketClient : IHSSocket, IDisposable
    {
        SocketState GetState();

        bool SyncConnect();

        void ShutDown();

        event EventHandler<EventArgCollection.ConnectionEventArgs> OnConnectionChanged;

        event DelegateCollection.ExecuteCommandEventHandler ExecuteProtocolCommand;

        event EventHandler<EventArgCollection.SocketEventArgs> OnSocketEventRaised;

        event EventHandler<EventArgCollection.SocketErrorEventArgs> OnSocketErrorRaised;
    }
}
