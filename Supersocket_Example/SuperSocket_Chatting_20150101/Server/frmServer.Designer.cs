namespace Server
{
	partial class frmServer
	{
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		/// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form 디자이너에서 생성한 코드

		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnStop = new System.Windows.Forms.Button();
			this.listUser = new System.Windows.Forms.ListBox();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.btnStart = new System.Windows.Forms.Button();
			this.listMsg = new System.Windows.Forms.ListBox();
			this.lvLog = new System.Windows.Forms.ListView();
			this.colIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colIcon = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colLog = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SuspendLayout();
			// 
			// btnStop
			// 
			this.btnStop.Location = new System.Drawing.Point(193, 433);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(75, 23);
			this.btnStop.TabIndex = 14;
			this.btnStop.Text = "서버 중지";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// listUser
			// 
			this.listUser.FormattingEnabled = true;
			this.listUser.ItemHeight = 12;
			this.listUser.Location = new System.Drawing.Point(311, 183);
			this.listUser.Name = "listUser";
			this.listUser.Size = new System.Drawing.Size(162, 244);
			this.listUser.TabIndex = 13;
			// 
			// txtPort
			// 
			this.txtPort.Location = new System.Drawing.Point(5, 435);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(100, 21);
			this.txtPort.TabIndex = 11;
			this.txtPort.Text = "8000";
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(111, 433);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(75, 23);
			this.btnStart.TabIndex = 10;
			this.btnStart.Text = "서버 시작";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// listMsg
			// 
			this.listMsg.FormattingEnabled = true;
			this.listMsg.ItemHeight = 12;
			this.listMsg.Location = new System.Drawing.Point(3, 183);
			this.listMsg.Name = "listMsg";
			this.listMsg.Size = new System.Drawing.Size(302, 244);
			this.listMsg.TabIndex = 12;
			// 
			// lvLog
			// 
			this.lvLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colIndex,
            this.colIcon,
            this.colLog});
			this.lvLog.Location = new System.Drawing.Point(3, 2);
			this.lvLog.Name = "lvLog";
			this.lvLog.Size = new System.Drawing.Size(470, 175);
			this.lvLog.TabIndex = 15;
			this.lvLog.UseCompatibleStateImageBehavior = false;
			this.lvLog.View = System.Windows.Forms.View.Details;
			// 
			// colIndex
			// 
			this.colIndex.Text = "";
			// 
			// colIcon
			// 
			this.colIcon.Text = "";
			// 
			// colLog
			// 
			this.colLog.Text = "Log";
			this.colLog.Width = 340;
			// 
			// frmServer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(479, 463);
			this.Controls.Add(this.lvLog);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.listUser);
			this.Controls.Add(this.listMsg);
			this.Controls.Add(this.txtPort);
			this.Controls.Add(this.btnStart);
			this.Name = "frmServer";
			this.Text = "Server_SuperSocket";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.ListBox listUser;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.ListBox listMsg;
		private System.Windows.Forms.ListView lvLog;
		private System.Windows.Forms.ColumnHeader colIndex;
		private System.Windows.Forms.ColumnHeader colIcon;
		private System.Windows.Forms.ColumnHeader colLog;
	}
}

