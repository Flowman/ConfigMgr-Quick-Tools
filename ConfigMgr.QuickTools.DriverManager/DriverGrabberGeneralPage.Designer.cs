namespace ConfigMgr.QuickTools.DriverManager
{
    partial class DriverGrabberGeneralPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelWMIStatus = new System.Windows.Forms.Label();
            this.checkBoxWMI = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxShare = new System.Windows.Forms.CheckBox();
            this.labelShareStatus = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonOptions = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.labelOptions = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelWMIStatus
            // 
            this.labelWMIStatus.Location = new System.Drawing.Point(110, 92);
            this.labelWMIStatus.Name = "labelWMIStatus";
            this.labelWMIStatus.Size = new System.Drawing.Size(341, 35);
            this.labelWMIStatus.TabIndex = 1;
            this.labelWMIStatus.Text = " ";
            // 
            // checkBoxWMI
            // 
            this.checkBoxWMI.AutoCheck = false;
            this.checkBoxWMI.AutoSize = true;
            this.checkBoxWMI.Location = new System.Drawing.Point(19, 90);
            this.checkBoxWMI.Name = "checkBoxWMI";
            this.checkBoxWMI.Size = new System.Drawing.Size(85, 17);
            this.checkBoxWMI.TabIndex = 2;
            this.checkBoxWMI.Text = "WMI Status:";
            this.checkBoxWMI.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(389, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "To grab driver from a remote device, WMI and ADMIN$ share access is required.";
            // 
            // checkBoxShare
            // 
            this.checkBoxShare.AutoCheck = false;
            this.checkBoxShare.AutoSize = true;
            this.checkBoxShare.Location = new System.Drawing.Point(19, 130);
            this.checkBoxShare.Name = "checkBoxShare";
            this.checkBoxShare.Size = new System.Drawing.Size(90, 17);
            this.checkBoxShare.TabIndex = 5;
            this.checkBoxShare.Text = "Share Status:";
            this.checkBoxShare.UseVisualStyleBackColor = true;
            // 
            // labelShareStatus
            // 
            this.labelShareStatus.Location = new System.Drawing.Point(110, 132);
            this.labelShareStatus.Name = "labelShareStatus";
            this.labelShareStatus.Size = new System.Drawing.Size(341, 35);
            this.labelShareStatus.TabIndex = 4;
            this.labelShareStatus.Text = " ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Requirements:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(301, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "- Usually local admininistrator access on the device is required.";
            // 
            // buttonOptions
            // 
            this.buttonOptions.Location = new System.Drawing.Point(19, 174);
            this.buttonOptions.Name = "buttonOptions";
            this.buttonOptions.Size = new System.Drawing.Size(24, 24);
            this.buttonOptions.TabIndex = 19;
            this.buttonOptions.UseVisualStyleBackColor = true;
            this.buttonOptions.Click += new System.EventHandler(this.ButtonOptions_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(45, 180);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Driver Manager Options";
            // 
            // labelOptions
            // 
            this.labelOptions.AutoSize = true;
            this.labelOptions.Location = new System.Drawing.Point(57, 198);
            this.labelOptions.Name = "labelOptions";
            this.labelOptions.Size = new System.Drawing.Size(0, 13);
            this.labelOptions.TabIndex = 21;
            // 
            // DriverGrabberGeneralPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelOptions);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonOptions);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBoxShare);
            this.Controls.Add(this.labelShareStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBoxWMI);
            this.Controls.Add(this.labelWMIStatus);
            this.Name = "DriverGrabberGeneralPage";
            this.Size = new System.Drawing.Size(485, 360);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelWMIStatus;
        private System.Windows.Forms.CheckBox checkBoxWMI;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxShare;
        private System.Windows.Forms.Label labelShareStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonOptions;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelOptions;
    }
}
