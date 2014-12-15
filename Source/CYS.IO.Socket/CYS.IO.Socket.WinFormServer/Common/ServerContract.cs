using System;
using System.Net;
using CYS.IO.Socket.BLL.Common;
using CYS.IO.Socket.Core;
using CYS.IO.Socket.Core.Common;
using CYS.IO.Socket.Core.Interface;
using CYS.IO.Socket.WinFormServer.Properties;

namespace CYS.IO.Socket.WinFormServer.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class ServerContract : ContractAdapter
    {
        #region Public Properties

        #endregion Public Properties

        #region Private Properties
        private ISocketServer listener;
        private View.FrmMain window;

        #endregion Private Properties

        #region Constructor
        public ServerContract()
        {
            // IPEndPoint point = new IPEndPoint(IPAddress.Parse(Settings.Default.SYS_ServerIP), Settings.Default.SYS_ServerPort);
            IPEndPoint point = new IPEndPoint(IPAddress.Any, Settings.Default.SYS_ServerPort);

            //string locHostName = Dns.GetHostName();
            //IPHostEntry ipHostEntry = Dns.GetHostEntry(locHostName);
            //IPEndPoint point = new IPEndPoint(ipHostEntry.AddressList[1], Settings.Default.SYS_ServerPort);

            listener = new HSSocketServer(point);

            listener.OnSocketEventRaised += new EventHandler<EventArgCollection.SocketEventArgs>(listener_OnSocketEventRaised);
            listener.OnSocketErrorRaised += new EventHandler<EventArgCollection.SocketErrorEventArgs>(listener_OnErrorRaised);
            listener.ExecuteProtocolCommand += new DelegateCollection.ExecuteCommandEventHandler(base.ExecuteProtocolCommand);
        }

        public ServerContract(View.FrmMain main)
            : this()
        {
            this.window = main;
        }

        #endregion Constructor

        #region Public Method
        /// <summary>
        /// StartListen
        /// </summary>
        public void StartListen()
        {
            listener.AsyncConnect();
        }

        #endregion Public Method

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
                SocketState state = func.Execute(p, s, listener);

                if (this.window != null)
                    this.window.MessageCallBackMethod(state.OutPutMsg, null);
            }
            catch (Exception Ex)
            {
                Logger.Write(LogType.Error, Ex.Message, Ex);

                if (this.window != null)
                    this.window.MessageCallBackMethod(Ex.ToString(), null);
            }
            finally
            {
                listener.ContinueAsyncReceive(s);
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
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listener_OnErrorRaised(object sender, EventArgCollection.SocketErrorEventArgs e)
        {
            Logger.Write(LogType.Error, e.Msg, e.Exp);

            if (this.window != null)
                this.window.MessageCallBackMethod(e.Msg, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listener_OnSocketEventRaised(object sender, EventArgCollection.SocketEventArgs e)
        {
            if (this.window != null)
            {
                this.window.MessageCallBackMethod(e.Msg, null);
                if (!string.IsNullOrEmpty(e.ClientName))
                    this.window.ServersCallBackMethod(e);
            }
        }
        #endregion Private Method
    }
}
