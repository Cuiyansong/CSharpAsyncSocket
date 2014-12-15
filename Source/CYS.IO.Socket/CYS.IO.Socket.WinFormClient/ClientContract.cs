using System;
using System.Net;
using System.Threading;
using CYS.IO.Socket.BLL.Common;
using CYS.IO.Socket.Core;
using CYS.IO.Socket.Core.Common;
using CYS.IO.Socket.Core.Interface;
using CYS.IO.Socket.WinFormClient.Properties;

namespace CYS.IO.Socket.WinFormClient
{
    public class ClientContractMgmt : ContractAdapter
    {
        #region Properties
        private ISocketClient client;
        private FrmMain window;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public ClientContractMgmt(FrmMain win)
        {
            window = win as FrmMain;

            IPEndPoint point = new IPEndPoint(IPAddress.Parse(Settings.Default.SYS_ServerIP), Settings.Default.SYS_ServerPort);
            client = new HSSocketClient(point);

            client.OnSocketEventRaised += new EventHandler<EventArgCollection.SocketEventArgs>(OnSocketEventRaised);
            client.OnSocketErrorRaised += new EventHandler<EventArgCollection.SocketErrorEventArgs>(OnErrorRaised);
            client.ExecuteProtocolCommand += new DelegateCollection.ExecuteCommandEventHandler(ExecuteProtocolCommand);
            client.OnConnectionChanged += new EventHandler<EventArgCollection.ConnectionEventArgs>(OnConnectionChanged);
        }
        #endregion

        #region Public Method
        /// <summary>
        /// 同步连接
        /// </summary>
        public bool SyncConncetToServer()
        {
            return client.SyncConnect();
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public void ShutDown()
        {
            client.ShutDown();
        }
        /// <summary>
        /// 异步上传
        /// </summary>
        /// <param name="path"></param>
        public void AsyncExecuteUpLoad(string fullpath)
        {
            var p = ProtocolMgmt.InitProtocolHeader(1, null, null);

            var state = client.GetState();
            if (state == null)
                throw new ArgumentNullException("Socket null");

            ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
            {
                base.ExecuteProtocolCommand(p, state);
            }));
        }
        /// <summary>
        /// 异步下载
        /// </summary> 
        public void AsyncExecuteDownLoad()
        {
            var p = ProtocolMgmt.InitProtocolHeader(2, null, null);

            var state = client.GetState();
            if (state == null)
                throw new ArgumentNullException("Socket null");

            ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
            {
                base.ExecuteProtocolCommand(p, state);
            }));
        }
        #endregion

        #region Private Method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        /// <param name="p"></param>
        /// <param name="s"></param>
        protected override void CommandWrapper(ICommandFunc func, CommunicateProtocol p, SocketState s)
        {
            try
            {
                func.profile = this.GetProfile(Settings.Default);
                SocketState state = func.Execute(p, s, client);

                OnSocketEventRaised(this, new EventArgCollection.SocketEventArgs() { Msg = state.OutPutMsg });
            }
            catch (Exception Ex)
            {
                OnErrorRaised(this, new EventArgCollection.SocketErrorEventArgs() { Exp = Ex });
            }
            finally
            {
                client.ContinueAsyncReceive(s);
                GC.WaitForFullGCComplete();
                GC.Collect();
            }
        }
        /// <summary>
        /// Get System Setting
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        private CommandProfile GetProfile(Settings settings)
        {
            return new CommandProfile()
            {
                UpLoadPath = settings.SYS_UpLoadPath,
                DownLoadPath = settings.SYS_DownLoadPath,
                UpLoadFName = settings.SYS_UpLoadFName,
                DownLoadFName = settings.SYS_DownLoadFName,
                PID = settings.SYS_PID,
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnErrorRaised(object sender, EventArgCollection.SocketErrorEventArgs e)
        {
            //Logger.Write(LogType.Error, e.Msg, e.Exp);

            Console.WriteLine(e.Msg + System.Environment.NewLine + e.Exp.ToString());

            if (this.window != null)
                this.window.OnMessageReceived(e.Exp.Message.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSocketEventRaised(object sender, EventArgCollection.SocketEventArgs e)
        {
            Console.WriteLine(e.Msg);

            if (this.window != null)
                this.window.OnMessageReceived(e.Msg);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConnectionChanged(object sender, EventArgCollection.ConnectionEventArgs e)
        {
            if (this.window != null)
                this.window.UpdateControlState(e.IsConnected);
        }
        #endregion
    }
}
