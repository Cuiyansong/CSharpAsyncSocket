using System;
using System.Net;
using System.Net.Sockets;
using CYS.IO.Socket.Core.Base;
using CYS.IO.Socket.Core.Common;
using CYS.IO.Socket.Core.Interface;

namespace CYS.IO.Socket.Core
{
    /// <summary>
    /// Socket Server
    /// </summary>
    public class HSSocketServer : HSSocketBase, ISocketServer
    {
        #region Public Properties
        /// <summary>
        /// ExecuteProtocolCommand
        /// </summary>
        public event CYS.IO.Socket.Core.Common.DelegateCollection.ExecuteCommandEventHandler ExecuteProtocolCommand;
        /// <summary>
        /// OnSocketEventRaised
        /// </summary>
        public event EventHandler<EventArgCollection.SocketEventArgs> OnSocketEventRaised;
        /// <summary>
        /// OnErrorRaised
        /// </summary>
        public event EventHandler<EventArgCollection.SocketErrorEventArgs> OnSocketErrorRaised;
        #endregion Public Properties

        #region Private Properties
        /// <summary>
        /// max number of clients
        /// </summary>
        private int maxClients = 100;
        /// <summary>
        /// SendTimeOutMS
        /// </summary>
        private int SendTimeOutMS = 60000;
        /// <summary>
        /// ReceiveTimeOutMS
        /// </summary>
        private int ReceiveTimeOutMS = 60000;
        /// <summary>
        /// locendpoint
        /// </summary>
        private System.Net.IPEndPoint locEndpoint;
        /// <summary>
        /// all done event
        /// </summary>
        public static System.Threading.ManualResetEvent connectDone = new System.Threading.ManualResetEvent(false);
        #endregion Private Properties

        #region Constructor
        public HSSocketServer(System.Net.IPEndPoint ep)
        {
            locEndpoint = ep;
            maxClients = 100;
        }
        public HSSocketServer(System.Net.IPEndPoint ep, int clients)
        {
            locEndpoint = ep;
            maxClients = clients;
        }

        #endregion Constructor

        #region Public Method

        #endregion Public Method

        #region Private Method
        /// <summary>
        /// 异步接收连接
        /// </summary>
        /// <param name="ar"></param>
        private void AsyncAcceptConnectCallback(IAsyncResult ar)
        {
            SocketState state = new SocketState();
            try
            {
                System.Net.Sockets.Socket listener = (System.Net.Sockets.Socket)ar.AsyncState;
                System.Net.Sockets.Socket handler = listener.EndAccept(ar);
                //int keepAlive = -1744830460; // SIO_KEEPALIVE_VALS
                //byte[] inValue = new byte[] { 1, 0, 0, 0, 0x20, 0x4e, 0, 0, 0xd0, 0x07, 0, 0 }; //True, 20 秒, 2 秒
                //handler.IOControl(keepAlive, inValue, null);


                //handler.SendTimeout = SendTimeOutMS;
                //handler.ReceiveTimeout = ReceiveTimeOutMS;

                state = new SocketState();
                state.workSocket = handler;
                base.SetHeartBeat(state);

                connectDone.Set();

                state.IsReceiveThreadAlive = true;
                SetConnection(true, state);
                handler.BeginReceive(state.buffer, 0, 2, 0, new AsyncCallback(AsyncReceivedCallback), state);
            }
            catch (System.Net.Sockets.SocketException SktEx)
            {
                WriteLog(SktEx, SktEx.ErrorCode.ToString());
                SetConnection(false, state);
            }
            catch (Exception Ex)
            {
                WriteLog(Ex, null);
                SetConnection(false, state);
            }
        }
        #endregion Private Method

        #region IListener 成员
        /// <summary>
        /// AsyncConnect
        /// </summary>
        public void AsyncConnect()
        {
            System.Net.Sockets.Socket listener = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(locEndpoint);
                listener.Listen(maxClients);
                while (true)
                {
                    connectDone.Reset();
                    Mark("接收到客户端连接，开始接收");
                    listener.BeginAccept(new AsyncCallback(AsyncAcceptConnectCallback), listener);
                    connectDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                WriteLog(e, null);
            }
        }
        #endregion

        #region IDisposable 成员

        public void Dispose()
        {

        }

        #endregion

        #region Base 成员
        /// <summary>
        /// Write Log
        /// </summary>
        /// <param name="exp"></param>
        public override void WriteLog(Exception exp, string msg)
        {
            if (OnSocketErrorRaised != null)
                OnSocketErrorRaised(this, new EventArgCollection.SocketErrorEventArgs() { Exp = exp, Msg = msg });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public override void Mark(string s)
        {
            if (OnSocketEventRaised != null)
                OnSocketEventRaised(null, new EventArgCollection.SocketEventArgs() { Msg = s });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public override void SetConnection(bool IsConnected, SocketState state)
        {
            if (OnSocketEventRaised != null)
                OnSocketEventRaised(this, new EventArgCollection.SocketEventArgs()
                {
                    ClientName = IPAddress.Parse(((IPEndPoint)state.workSocket.RemoteEndPoint).Address.ToString()) + " : " + ((IPEndPoint)state.workSocket.RemoteEndPoint).Port.ToString(),
                    IsConnect = IsConnected,
                    Msg = IsConnected ? "连接成功" : "中断连接"
                });

            if (!IsConnected && state.workSocket != null)
            {
                state.IsReceiveThreadAlive = false;

                try
                {
                    state.workSocket.Shutdown(SocketShutdown.Both);
                    state.workSocket.Close();
                }
                catch (Exception Ex)
                {
                    WriteLog(Ex, "When release socket connection.");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="state"></param>
        public override void InvokeExecuteProtocolCommand(CommunicateProtocol protocol, SocketState state)
        {
            if (ExecuteProtocolCommand != null)
                ExecuteProtocolCommand(protocol, state);
        }
        #endregion
    }
}
