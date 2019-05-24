namespace ConfigMgr.QuickTools.DriverManager
{
    partial class LegacyPackageGeneralPage
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
            this.labelInformation = new System.Windows.Forms.Label();
            this.labelOptions = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonOptions = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelInformation
            // 
            this.labelInformation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelInformation.Location = new System.Drawing.Point(18, 24);
            this.labelInformation.Name = "labelInformation";
            this.labelInformation.Size = new System.Drawing.Size(337, 134);
            this.labelInformation.TabIndex = 32;
            this.labelInformation.Text = " ";
            // 
            // labelOptions
            // 
            this.labelOptions.AutoSize = true;
            this.labelOptions.Location = new System.Drawing.Point(59, 259);
            this.labelOptions.Name = "labelOptions";
            this.labelOptions.Size = new System.Drawing.Size(0, 13);
            this.labelOptions.TabIndex = 31;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(47, 241);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 13);
            this.label4.TabIndex = 30;
            this.label4.Text = "Driver Manager Options";
            // 
            // buttonOptions
            // 
            this.buttonOptions.Location = new System.Drawing.Point(21, 235);
            this.buttonOptions.Name = "buttonOptions";
            this.buttonOptions.Size = new System.Drawing.Size(24, 24);
            this.buttonOptions.TabIndex = 29;
            this.buttonOptions.UseVisualStyleBackColor = true;
            this.buttonOptions.Click += new System.EventHandler(this.ButtonOptions_Click);
            // 
            // LegacyPackageGeneralPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelInformation);
            this.Controls.Add(this.labelOptions);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonOptions);
            this.Name = "LegacyPackageGeneralPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelInformation;
        private System.Windows.Forms.Label labelOptions;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonOptions;
    }
}
