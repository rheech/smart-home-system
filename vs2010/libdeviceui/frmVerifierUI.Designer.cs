namespace libdeviceui
{
    partial class frmVerifierUI : frmAbstractUI
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
            this.txtLogConsole = new libdeviceui.cptConsoleTextBox(this.components);
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabPageLog = new System.Windows.Forms.TabPage();
            this.tabPageVerification = new System.Windows.Forms.TabPage();
            this.btnInsert = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lstOverview = new System.Windows.Forms.ListBox();
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.btnCheck = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabMain.SuspendLayout();
            this.tabPageLog.SuspendLayout();
            this.tabPageVerification.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtLogConsole
            // 
            this.txtLogConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLogConsole.Location = new System.Drawing.Point(3, 3);
            this.txtLogConsole.Multiline = true;
            this.txtLogConsole.Name = "txtLogConsole";
            this.txtLogConsole.ReadOnly = true;
            this.txtLogConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLogConsole.Size = new System.Drawing.Size(534, 376);
            this.txtLogConsole.TabIndex = 3;
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabPageLog);
            this.tabMain.Controls.Add(this.tabPageVerification);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 24);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(548, 408);
            this.tabMain.TabIndex = 4;
            // 
            // tabPageLog
            // 
            this.tabPageLog.Controls.Add(this.txtLogConsole);
            this.tabPageLog.Location = new System.Drawing.Point(4, 22);
            this.tabPageLog.Name = "tabPageLog";
            this.tabPageLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLog.Size = new System.Drawing.Size(540, 382);
            this.tabPageLog.TabIndex = 0;
            this.tabPageLog.Text = "Log";
            this.tabPageLog.UseVisualStyleBackColor = true;
            // 
            // tabPageVerification
            // 
            this.tabPageVerification.Controls.Add(this.btnInsert);
            this.tabPageVerification.Controls.Add(this.label3);
            this.tabPageVerification.Controls.Add(this.lstOverview);
            this.tabPageVerification.Controls.Add(this.txtQuery);
            this.tabPageVerification.Controls.Add(this.txtComment);
            this.tabPageVerification.Controls.Add(this.btnCheck);
            this.tabPageVerification.Controls.Add(this.label2);
            this.tabPageVerification.Controls.Add(this.label1);
            this.tabPageVerification.Location = new System.Drawing.Point(4, 22);
            this.tabPageVerification.Name = "tabPageVerification";
            this.tabPageVerification.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageVerification.Size = new System.Drawing.Size(540, 382);
            this.tabPageVerification.TabIndex = 1;
            this.tabPageVerification.Text = "Verifier";
            this.tabPageVerification.UseVisualStyleBackColor = true;
            // 
            // btnInsert
            // 
            this.btnInsert.Location = new System.Drawing.Point(459, 19);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(75, 23);
            this.btnInsert.TabIndex = 12;
            this.btnInsert.Text = "&Insert";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Overview";
            // 
            // lstOverview
            // 
            this.lstOverview.FormattingEnabled = true;
            this.lstOverview.Items.AddRange(new object[] {
            "A[] Heater.Heating <> Airconditioner.Cooling",
            "E<> not Heater.Heating"});
            this.lstOverview.Location = new System.Drawing.Point(3, 19);
            this.lstOverview.Name = "lstOverview";
            this.lstOverview.Size = new System.Drawing.Size(445, 108);
            this.lstOverview.TabIndex = 10;
            // 
            // txtQuery
            // 
            this.txtQuery.Location = new System.Drawing.Point(3, 158);
            this.txtQuery.Multiline = true;
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Size = new System.Drawing.Size(445, 91);
            this.txtQuery.TabIndex = 9;
            // 
            // txtComment
            // 
            this.txtComment.Location = new System.Drawing.Point(3, 268);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.ReadOnly = true;
            this.txtComment.Size = new System.Drawing.Size(445, 97);
            this.txtComment.TabIndex = 8;
            this.txtComment.Text = "Verifier Beta Started. (Not implemented yet).";
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(459, 48);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(75, 23);
            this.btnCheck.TabIndex = 7;
            this.btnCheck.Text = "&Check";
            this.btnCheck.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 252);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Status";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 142);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Query";
            // 
            // frmVerifierUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 454);
            this.Controls.Add(this.tabMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Name = "frmVerifierUI";
            this.Text = "frmVerificationServer";
            this.Load += new System.EventHandler(this.frmVerifierUI_Load);
            this.Controls.SetChildIndex(this.tabMain, 0);
            this.tabMain.ResumeLayout(false);
            this.tabPageLog.ResumeLayout(false);
            this.tabPageLog.PerformLayout();
            this.tabPageVerification.ResumeLayout(false);
            this.tabPageVerification.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private cptConsoleTextBox txtLogConsole;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabPageLog;
        private System.Windows.Forms.TabPage tabPageVerification;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lstOverview;
        private System.Windows.Forms.TextBox txtQuery;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnInsert;
    }
}