namespace DeviceThermometer
{
    partial class frmMain
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
            this.lblTitleCurrentTemp = new System.Windows.Forms.Label();
            this.lblTemperature = new System.Windows.Forms.Label();
            this.lblAverageTemp = new System.Windows.Forms.Label();
            this.tabSystemStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabSystemStatus
            // 
            this.tabSystemStatus.Controls.Add(this.lblAverageTemp);
            this.tabSystemStatus.Controls.Add(this.lblTemperature);
            this.tabSystemStatus.Controls.Add(this.lblTitleCurrentTemp);
            // 
            // lblTitleCurrentTemp
            // 
            this.lblTitleCurrentTemp.AutoSize = true;
            this.lblTitleCurrentTemp.Location = new System.Drawing.Point(8, 27);
            this.lblTitleCurrentTemp.Name = "lblTitleCurrentTemp";
            this.lblTitleCurrentTemp.Size = new System.Drawing.Size(107, 13);
            this.lblTitleCurrentTemp.TabIndex = 0;
            this.lblTitleCurrentTemp.Text = "Current Temperature:";
            // 
            // lblTemperature
            // 
            this.lblTemperature.AutoSize = true;
            this.lblTemperature.Location = new System.Drawing.Point(121, 27);
            this.lblTemperature.Name = "lblTemperature";
            this.lblTemperature.Size = new System.Drawing.Size(37, 13);
            this.lblTemperature.TabIndex = 1;
            this.lblTemperature.Text = "25 ℃ ";
            // 
            // lblAverageTemp
            // 
            this.lblAverageTemp.AutoSize = true;
            this.lblAverageTemp.Location = new System.Drawing.Point(8, 52);
            this.lblAverageTemp.Name = "lblAverageTemp";
            this.lblAverageTemp.Size = new System.Drawing.Size(113, 13);
            this.lblAverageTemp.TabIndex = 2;
            this.lblAverageTemp.Text = "Average Temperature:";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 312);
            this.Name = "frmMain";
            this.Text = "Thermometer";
            this.tabSystemStatus.ResumeLayout(false);
            this.tabSystemStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitleCurrentTemp;
        private System.Windows.Forms.Label lblTemperature;
        private System.Windows.Forms.Label lblAverageTemp;
    }
}

