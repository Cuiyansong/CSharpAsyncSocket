using System;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using CYS.IO.Socket.Core.Base;
using CYS.IO.Socket.Core.Common;
using CYS.IO.Socket.Core.Interface;
using System.Threading;
using System.Collections.Generic;

namespace CYS.IO.Socket.Core
{
    /// <summary>
    /// Socket Client
    /// </summary>
    public class HSSocketClient : HSSocketBase, ISocketClient
    {
        #region Properties
        /// <summary>
        /// Socket state list
        /// </summary>
        private List<SocketState> statelst = new List<SocketState>();
        /// <summary>
        /// SendTimeOutMS
        /// </summary>
        private int SendTimeOutMS = 30000;
        /// <summary>
        /// ReceiveTimeOutMS
        /// </summary>
        private int ReceiveTimeOutMS = 30000;
        /// <summary>
        /// locendpoint
        /// </summary>
        private IPEndPoint locEndpoint;
        /// <summary>
        /// all done event
        /// </summary>
        public ManualResetEvent connectDone = new ManualResetEvent(false);
        /// <summary>
        /// sendDone event
        /// </summary>
        public ManualResetEvent sendDone = new ManualResetEvent(false);
        /// <summary>
        /// receiveDone event
        /// </summary>
        public ManualResetEvent receiveDone = new ManualResetEvent(false);

        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ep"></param>
        public HSSocketClient(IPEndPoint ep)
        {
            locEndpoint = ep;
        }
        #endregion

        #region Private Method
        /// <summary>
        /// ConnectToServer
        /// </summary>
        /// <returns></returns>
        private bool SyncConnectToServer()
        {
            System.Net.Sockets.Socket socketClient = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connectDone.Reset();
            socketClient.BeginConnect(locEndpoint, new AsyncCallback(AsyncConnectCallback), socketClient);
            connectDone.WaitOne();

            return socketClient == null ? false : socketClient.Connected;
        }
        /// <summary>
        /// AsyncConnectCallback
        /// </summary>
        /// <param name="ar"></param>
        private void AsyncConnectCallback(IAsyncResult ar)
        {
            SocketState state = new SocketState();
            try
            {
                System.Net.Sockets.Socket handler = (System.Net.Sockets.Socket)ar.AsyncState;
                handler.EndConnect(ar);
                state = new SocketState();
                state.workSocket = handler;
                //handler.SendTimeout = SendTimeOutMS;
                //handler.ReceiveTimeout = ReceiveTimeOutMS;

                Mark(string.Format("连接成功"));
                state.IsReceiveThreadAlive = true;
                statelst.Add(state);
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
                WriteLog(Ex, Ex.Message);
                SetConnection(false, state);
            }
            finally
            {
                connectDone.Set();
            }
        }
        #endregion

        #region ISocketClient 成员
        /// <summary>
        /// Get current state
        /// </summary>
        /// <returns></returns>
        public SocketState GetState()
        {
            if (statelst != null && statelst.Count > 0)
                return statelst.FirstOrDefault();
            else
                return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SyncConnect()
        {
            Mark("正在建立连接");

            return SyncConnectToServer();
        }
        /// <summary>
        /// 
        /// </summary>
        public void ShutDown()
        {
            Mark("正在关闭连接");

            this.Dispose();
        }


        public event EventHandler<EventArgCollection.ConnectionEventArgs> OnConnectionChanged;

        public event DelegateCollection.ExecuteCommandEventHandler ExecuteProtocolCommand;

        public event EventHandler<EventArgCollection.SocketEventArgs> OnSocketEventRaised;

        public event EventHandler<EventArgCollection.SocketErrorEventArgs> OnSocketErrorRaised;

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            if (statelst != null && statelst.Count > 0)
            {
                try
                {
                    foreach (var item in statelst)
                    {
                        item.workSocket.Shutdown(SocketShutdown.Both);
                        item.workSocket.Close();
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        #endregion

        #region Base Functnion
        /// <summary>
        /// Write Logger 
        /// </summary>
        /// <param name="exp"></param>
        public override void WriteLog(Exception exp, string msg)
        {
            if (OnSocketErrorRaised != null)
                OnSocketErrorRaised(exp.Message, new EventArgCollection.SocketErrorEventArgs() { Exp = exp, Msg = msg });
        }
        /// <summary>
        /// Remark the step of operation
        /// </summary>
        /// <param name="s"></param>
        public override void Mark(string s)
        {
            var Id = System.Threading.Thread.CurrentThread.ManagedThreadId;

            if (OnSocketEventRaised != null)
                OnSocketEventRaised(null, new EventArgCollection.SocketEventArgs() { Msg = string.Format("[线程ID:{0}] {1}", Id, s) });
        }
        /// <summary>
        /// SetConnection
        /// </summary>
        /// <param name="IsConnected"></param>
        public override void SetConnection(bool IsConnected, SocketState state)
        {
            if (!IsConnected && state.workSocket != null)
            {
                state.IsReceiveThreadAlive = false;

                try
                {
                    if (statelst.Contains(state))
                        statelst.Remove(state);

                    state.workSocket.Shutdown(SocketShutdown.Both);
                    state.workSocket.Close();
                }
                catch (Exception Ex)
                {
                    WriteLog(Ex, "When release socket connection.");
                }
            }

            if (OnConnectionChanged != null)
                OnConnectionChanged(this, new EventArgCollection.ConnectionEventArgs() { IsConnected = IsConnected });
        }
        /// <summary>
        /// ExecuteProtocolCommand
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
