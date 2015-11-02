namespace libdeviceui
{
    partial class frmDeviceUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.components = new System.ComponentModel.Container();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabSystemStatus = new System.Windows.Forms.TabPage();
            this.tabLog = new System.Windows.Forms.TabPage();
            this.txtLogConsole = new libdeviceui.cptConsoleTextBox(this.components);
            this.tabMain.SuspendLayout();
            this.tabLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabSystemStatus);
            this.tabMain.Controls.Add(this.tabLog);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 24);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(384, 266);
            this.tabMain.TabIndex = 3;
            // 
            // tabSystemStatus
            // 
            this.tabSystemStatus.Location = new System.Drawing.Point(4, 22);
            this.tabSystemStatus.Name = "tabSystemStatus";
            this.tabSystemStatus.Padding = new System.Windows.Forms.Padding(3);
            this.tabSystemStatus.Size = new System.Drawing.Size(376, 240);
            this.tabSystemStatus.TabIndex = 0;
            this.tabSystemStatus.Text = "System Status";
            this.tabSystemStatus.UseVisualStyleBackColor = true;
            // 
            // tabLog
            // 
            this.tabLog.Controls.Add(this.txtLogConsole);
            this.tabLog.Location = new System.Drawing.Point(4, 22);
            this.tabLog.Name = "tabLog";
            this.tabLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabLog.Size = new System.Drawing.Size(376, 240);
            this.tabLog.TabIndex = 1;
            this.tabLog.Text = "Log";
            this.tabLog.UseVisualStyleBackColor = true;
            // 
            // txtLogConsole
            // 
            this.txtLogConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLogConsole.Location = new System.Drawing.Point(3, 3);
            this.txtLogConsole.Multiline = true;
            this.txtLogConsole.Name = "txtLogConsole";
            this.txtLogConsole.ReadOnly = true;
            this.txtLogConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLogConsole.Size = new System.Drawing.Size(370, 234);
            this.txtLogConsole.TabIndex = 0;
            // 
            // frmDeviceUI
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(384, 312);
            this.Controls.Add(this.tabMain);
            this.Name = "frmDeviceUI";
            this.Text = "frmDeviceUI";
            this.Load += new System.EventHandler(this.frmDeviceUI_Load);
            this.Controls.SetChildIndex(this.tabMain, 0);
            this.tabMain.ResumeLayout(false);
            this.tabLog.ResumeLayout(false);
            this.tabLog.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tabLog;
        private cptConsoleTextBox txtLogConsole;
        private System.Windows.Forms.TabControl tabMain;
        protected System.Windows.Forms.TabPage tabSystemStatus;
    }
}