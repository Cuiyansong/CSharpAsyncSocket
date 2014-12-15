namespace CYS.IO.Socket.WinFormClient
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Btn_ShutDown = new System.Windows.Forms.Button();
            this.Btn_Send = new System.Windows.Forms.Button();
            this.Btn_DownLoad = new System.Windows.Forms.Button();
            this.Btn_Connect = new System.Windows.Forms.Button();
            this.Txt_OutPut = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Btn_ShutDown
            // 
            this.Btn_ShutDown.Location = new System.Drawing.Point(35, 186);
            this.Btn_ShutDown.Name = "Btn_ShutDown";
            this.Btn_ShutDown.Size = new System.Drawing.Size(100, 23);
            this.Btn_ShutDown.TabIndex = 7;
            this.Btn_ShutDown.Text = "Close";
            this.Btn_ShutDown.UseVisualStyleBackColor = true;
            this.Btn_ShutDown.Click += new System.EventHandler(this.Btn_ShutDown_Click);
            // 
            // Btn_Send
            // 
            this.Btn_Send.Location = new System.Drawing.Point(35, 141);
            this.Btn_Send.Name = "Btn_Send";
            this.Btn_Send.Size = new System.Drawing.Size(100, 23);
            this.Btn_Send.TabIndex = 6;
            this.Btn_Send.Text = "UPLoad";
            this.Btn_Send.UseVisualStyleBackColor = true;
            this.Btn_Send.Click += new System.EventHandler(this.Btn_Send_Click);
            // 
            // Btn_DownLoad
            // 
            this.Btn_DownLoad.Location = new System.Drawing.Point(35, 101);
            this.Btn_DownLoad.Name = "Btn_DownLoad";
            this.Btn_DownLoad.Size = new System.Drawing.Size(100, 23);
            this.Btn_DownLoad.TabIndex = 5;
            this.Btn_DownLoad.Text = "DownLoad";
            this.Btn_DownLoad.UseVisualStyleBackColor = true;
            this.Btn_DownLoad.Click += new System.EventHandler(this.Btn_DownLoad_Click);
            // 
            // Btn_Connect
            // 
            this.Btn_Connect.Location = new System.Drawing.Point(35, 42);
            this.Btn_Connect.Name = "Btn_Connect";
            this.Btn_Connect.Size = new System.Drawing.Size(100, 23);
            this.Btn_Connect.TabIndex = 4;
            this.Btn_Connect.Text = "Connect";
            this.Btn_Connect.UseVisualStyleBackColor = true;
            this.Btn_Connect.Click += new System.EventHandler(this.Btn_Connect_Click);
            // 
            // Txt_OutPut
            // 
            this.Txt_OutPut.BackColor = System.Drawing.SystemColors.Control;
            this.Txt_OutPut.Location = new System.Drawing.Point(151, 15);
            this.Txt_OutPut.Multiline = true;
            this.Txt_OutPut.Name = "Txt_OutPut";
            this.Txt_OutPut.ReadOnly = true;
            this.Txt_OutPut.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Txt_OutPut.Size = new System.Drawing.Size(445, 226);
            this.Txt_OutPut.TabIndex = 8;
            this.Txt_OutPut.TextChanged += new System.EventHandler(this.Txt_OutPut_TextChanged);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 257);
            this.Controls.Add(this.Txt_OutPut);
            this.Controls.Add(this.Btn_ShutDown);
            this.Controls.Add(this.Btn_Send);
            this.Controls.Add(this.Btn_DownLoad);
            this.Controls.Add(this.Btn_Connect);
            this.Name = "FrmMain";
            this.Text = "FrmMain";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Btn_ShutDown;
        private System.Windows.Forms.Button Btn_Send;
        private System.Windows.Forms.Button Btn_DownLoad;
        private System.Windows.Forms.Button Btn_Connect;
        private System.Windows.Forms.TextBox Txt_OutPut;
    }
}

