using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CYS.IO.Socket.WinFormServer.View
{
    public partial class FrmConfig : Form
    {
        CYS.IO.Socket.WinFormServer.Properties.Settings set;

        public FrmConfig()
        {
            InitializeComponent();
        }


        private void Frm_Config_Load(object sender, EventArgs e)
        {
            set = CYS.IO.Socket.WinFormServer.Properties.Settings.Default;

            this.Txt_IP.Text = set.SYS_ServerIP;
            this.Txt_Port.Text = set.SYS_ServerPort.ToString();
        }

        private void Btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                set.SYS_ServerIP = this.Txt_IP.Text;
                set.SYS_ServerPort = int.Parse(this.Txt_Port.Text);
                set.Save();

                Application.Exit();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }
    }
}
