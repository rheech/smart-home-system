namespace DeviceAirConditioner
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
            this.btnAirConditioner = new System.Windows.Forms.Button();
            this.lblHeatOnOff = new System.Windows.Forms.Label();
            this.tabSystemStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabSystemStatus
            // 
            this.tabSystemStatus.Controls.Add(this.lblHeatOnOff);
            this.tabSystemStatus.Controls.Add(this.btnAirConditioner);
            // 
            // btnAirConditioner
            // 
            this.btnAirConditioner.Location = new System.Drawing.Point(293, 211);
            this.btnAirConditioner.Name = "btnAirConditioner";
            this.btnAirConditioner.Size = new System.Drawing.Size(75, 23);
            this.btnAirConditioner.TabIndex = 0;
            this.btnAirConditioner.Text = "Cool";
            this.btnAirConditioner.UseVisualStyleBackColor = true;
            // 
            // lblHeatOnOff
            // 
            this.lblHeatOnOff.AutoSize = true;
            this.lblHeatOnOff.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeatOnOff.Location = new System.Drawing.Point(164, 57);
            this.lblHeatOnOff.Name = "lblHeatOnOff";
            this.lblHeatOnOff.Size = new System.Drawing.Size(54, 31);
            this.lblHeatOnOff.TabIndex = 1;
            this.lblHeatOnOff.Text = "Off";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 312);
            this.Name = "frmMain";
            this.Text = "Air Conditioner";
            this.tabSystemStatus.ResumeLayout(false);
            this.tabSystemStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAirConditioner;
        private System.Windows.Forms.Label lblHeatOnOff;
    }
}

