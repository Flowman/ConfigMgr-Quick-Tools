namespace ConfigMgr.QuickTools.Device
{
    partial class ClientCacheDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientCacheDialog));
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.trackBarWithoutFocus1 = new ConfigMgr.QuickTools.Device.TrackBarWithoutFocus();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelLocation = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelCacheSize = new System.Windows.Forms.Label();
            this.labelSpaceToUse = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarWithoutFocus1)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(347, 145);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(101, 20);
            this.numericUpDown1.TabIndex = 1;
            // 
            // trackBarWithoutFocus1
            // 
            this.trackBarWithoutFocus1.Location = new System.Drawing.Point(46, 145);
            this.trackBarWithoutFocus1.Name = "trackBarWithoutFocus1";
            this.trackBarWithoutFocus1.Size = new System.Drawing.Size(295, 45);
            this.trackBarWithoutFocus1.TabIndex = 2;
            this.trackBarWithoutFocus1.ValueChanged += new System.EventHandler(this.TrackBar1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Amount of disk space to use:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Location: ";
            // 
            // labelLocation
            // 
            this.labelLocation.AutoSize = true;
            this.labelLocation.Location = new System.Drawing.Point(100, 39);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(0, 13);
            this.labelLocation.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(46, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Current cache size:";
            // 
            // labelCacheSize
            // 
            this.labelCacheSize.AutoSize = true;
            this.labelCacheSize.Location = new System.Drawing.Point(147, 61);
            this.labelCacheSize.Name = "labelCacheSize";
            this.labelCacheSize.Size = new System.Drawing.Size(0, 13);
            this.labelCacheSize.TabIndex = 7;
            // 
            // labelSpaceToUse
            // 
            this.labelSpaceToUse.AutoSize = true;
            this.labelSpaceToUse.Location = new System.Drawing.Point(190, 126);
            this.labelSpaceToUse.Name = "labelSpaceToUse";
            this.labelSpaceToUse.Size = new System.Drawing.Size(0, 13);
            this.labelSpaceToUse.TabIndex = 8;
            // 
            // ClientCacheDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 330);
            this.Controls.Add(this.labelSpaceToUse);
            this.Controls.Add(this.labelCacheSize);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelLocation);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackBarWithoutFocus1);
            this.Controls.Add(this.numericUpDown1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ClientCacheDialog";
            this.Load += new System.EventHandler(this.ClientCacheDialog_Load);
            this.Shown += new System.EventHandler(this.ClientCacheDialog_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarWithoutFocus1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private TrackBarWithoutFocus trackBarWithoutFocus1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelLocation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelCacheSize;
        private System.Windows.Forms.Label labelSpaceToUse;
    }
}