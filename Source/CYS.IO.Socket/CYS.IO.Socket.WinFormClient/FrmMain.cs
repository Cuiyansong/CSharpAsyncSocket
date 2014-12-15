using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CYS.IO.Socket.WinFormClient
{
    public partial class FrmMain : Form
    {
        public delegate void UpdateControlUI(bool obj);

        public delegate void UpdateMessage(string obj);

        ClientContractMgmt clientMgmt;

        public FrmMain()
        {
            InitializeComponent();

            InitControls(false);
        }

        #region Events

        private void FrmMain_Load(object sender, EventArgs e)
        {
            //clientMgmt = new ClientContractMgmt(this);

            //clientMgmt.SyncConncetToServer();
        }

        private void Btn_Connect_Click(object sender, EventArgs e)
        {
            clientMgmt = new ClientContractMgmt(this);

            clientMgmt.SyncConncetToServer();
        }

        private void Btn_DownLoad_Click(object sender, EventArgs e)
        {
            clientMgmt.AsyncExecuteDownLoad();
        }

        private void Btn_Send_Click(object sender, EventArgs e)
        {
            clientMgmt.AsyncExecuteUpLoad(string.Empty);
        }

        private void Btn_ShutDown_Click(object sender, EventArgs e)
        {
            clientMgmt.ShutDown();
        }

        private void InitControls(bool IsShow)
        {
            this.Btn_Connect.Visible = !IsShow;
            this.Btn_DownLoad.Visible = IsShow;
            this.Btn_Send.Visible = IsShow;
        }

        private void UpdateControl(bool IsConnected)
        {
            InitControls(IsConnected);

            if (!IsConnected)
                MessageBox.Show(string.Format(IsConnected ? "连接成功" : "中断连接"));
        }

        private void UpdateOutPutMsg(string msg)
        {
            Txt_OutPut.Text += System.Environment.NewLine + msg;
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
        #endregion

        #region Public Method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IsConnected"></param>
        public void UpdateControlState(bool IsConnected)
        {
            this.BeginInvoke(new UpdateControlUI(UpdateControl), IsConnected);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public void OnMessageReceived(string msg)
        {
            this.BeginInvoke(new UpdateMessage(UpdateOutPutMsg), msg);
        }
        #endregion
    }
}
