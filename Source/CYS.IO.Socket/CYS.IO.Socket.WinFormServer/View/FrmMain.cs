using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CYS.IO.Socket.WinFormServer.Common;
using System.Threading;
using CYS.IO.Socket.Core.Common;

namespace CYS.IO.Socket.WinFormServer.View
{
    public partial class FrmMain : Form
    {
        #region Properties
        private bool IsListen = false;
        private ServerContract contractMgmt;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public FrmMain()
        {
            InitializeComponent();
        }
        #endregion

        #region Public Method

        #endregion

        #region Event Method
        /// <summary>
        /// 最小化事件,显示到托盘 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Visible = false;
                this.notifyIcon1.ShowBalloonTip(500, "Tips", "Auto Hide", ToolTipIcon.None);
            }
        }
        /// <summary>
        /// 托盘图标单击显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel = true;
            //this.Visible = false;
        }
        /// <summary>
        /// Window Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Load(object sender, EventArgs e)
        {
            contractMgmt = new ServerContract(this);
            AsyncStartListen();
        }
        /// <summary>
        /// MIT_Close_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MIT_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// MIT_Config_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MIT_Config_Click(object sender, EventArgs e)
        {
            (new View.FrmConfig()).ShowDialog();
        }
        /// <summary>
        /// Connect to server, start listenning.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_ConnectToSvr_Click(object sender, EventArgs e)
        {
            AsyncStartListen();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Txt_OutPut_TextChanged(object sender, EventArgs e)
        {
            var ctrl = sender as TextBox;
            ctrl.SelectionStart = ctrl.Text.Length;
            ctrl.ScrollToCaret();

            if (ctrl.Text.Length > 2 * 1024 * 1024)
            {
                ctrl.Text = string.Empty;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void AsyncStartListen()
        {
            if (!IsListen)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
                {
                    contractMgmt.StartListen();
                }));
            }
        }
        #endregion

        #region Public Method
        /// <summary>
        /// MessageCallBackMethod
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MessageCallBackMethod(object sender, EventArgs e)
        {
            string info = sender as string;
            Txt_OutPut.Invoke(new Action(() =>
            {
                Txt_OutPut.Text += System.Environment.NewLine + info;
            }));
        }
        /// <summary>
        /// ServersCallBackMethod
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ServersCallBackMethod(EventArgCollection.SocketEventArgs e)
        {
            Tev_Clients.Invoke(new Action(() =>
            {
                Tev_Clients.BeginUpdate();
                if (e.IsConnect)
                    Tev_Clients.Nodes.Add(e.ClientName, e.ClientName);
                else
                    Tev_Clients.Nodes.RemoveByKey(e.ClientName);
                Tev_Clients.EndUpdate();
            }));
        }
        #endregion

        #region Private Method

        #endregion
    }
}
