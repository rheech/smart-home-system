﻿namespace libdeviceui
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
            this.tabDevList = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lvDeviceList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnTest = new System.Windows.Forms.Button();
            this.btnDevList = new System.Windows.Forms.Button();
            this.tabLog = new System.Windows.Forms.TabPage();
            this.txtLogConsole = new libdeviceui.cptConsoleTextBox(this.components);
            this.tabMain.SuspendLayout();
            this.tabDevList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabSystemStatus);
            this.tabMain.Controls.Add(this.tabDevList);
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
            // tabDevList
            // 
            this.tabDevList.Controls.Add(this.splitContainer1);
            this.tabDevList.Location = new System.Drawing.Point(4, 22);
            this.tabDevList.Name = "tabDevList";
            this.tabDevList.Padding = new System.Windows.Forms.Padding(3);
            this.tabDevList.Size = new System.Drawing.Size(376, 240);
            this.tabDevList.TabIndex = 2;
            this.tabDevList.Text = "Device List";
            this.tabDevList.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lvDeviceList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnTest);
            this.splitContainer1.Panel2.Controls.Add(this.btnDevList);
            this.splitContainer1.Size = new System.Drawing.Size(370, 234);
            this.splitContainer1.SplitterDistance = 198;
            this.splitContainer1.TabIndex = 4;
            // 
            // lvDeviceList
            // 
            this.lvDeviceList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvDeviceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvDeviceList.Location = new System.Drawing.Point(0, 0);
            this.lvDeviceList.Name = "lvDeviceList";
            this.lvDeviceList.Size = new System.Drawing.Size(370, 198);
            this.lvDeviceList.TabIndex = 3;
            this.lvDeviceList.UseCompatibleStateImageBehavior = false;
            this.lvDeviceList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Device ID";
            this.columnHeader1.Width = 74;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 94;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "TCP Address";
            this.columnHeader3.Width = 150;
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTest.Location = new System.Drawing.Point(214, 3);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 2;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnDevList
            // 
            this.btnDevList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDevList.Location = new System.Drawing.Point(295, 3);
            this.btnDevList.Name = "btnDevList";
            this.btnDevList.Size = new System.Drawing.Size(75, 23);
            this.btnDevList.TabIndex = 1;
            this.btnDevList.Text = "&Update";
            this.btnDevList.UseVisualStyleBackColor = true;
            this.btnDevList.Click += new System.EventHandler(this.btnDevList_Click);
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
            this.tabDevList.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabLog.ResumeLayout(false);
            this.tabLog.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tabLog;
        private cptConsoleTextBox txtLogConsole;
        protected System.Windows.Forms.TabPage tabSystemStatus;
        private System.Windows.Forms.TabPage tabDevList;
        private System.Windows.Forms.Button btnDevList;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.ListView lvDeviceList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        protected System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}