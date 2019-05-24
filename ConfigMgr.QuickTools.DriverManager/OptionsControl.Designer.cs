namespace ConfigMgr.QuickTools.DriverManager
{
    partial class OptionsControl
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelExample = new System.Windows.Forms.Label();
            this.checkBoxLegacyFolder = new System.Windows.Forms.CheckBox();
            this.browseFolderControlPackage = new Microsoft.ConfigurationManagement.AdminConsole.OsdCommon.BrowseFolderControl();
            this.browseFolderControlSource = new Microsoft.ConfigurationManagement.AdminConsole.OsdCommon.BrowseFolderControl();
            this.labelPackage = new System.Windows.Forms.Label();
            this.labelSource = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.browseFolderControlLegacyPackage = new Microsoft.ConfigurationManagement.AdminConsole.OsdCommon.BrowseFolderControl();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxZipContent = new System.Windows.Forms.CheckBox();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 356);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(310, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Use this options if using the legacy driver source folder structure.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 145);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(206, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Example: \\\\sharename\\Drivers\\Packages";
            // 
            // labelExample
            // 
            this.labelExample.AutoSize = true;
            this.labelExample.Location = new System.Drawing.Point(7, 76);
            this.labelExample.Name = "labelExample";
            this.labelExample.Size = new System.Drawing.Size(192, 13);
            this.labelExample.TabIndex = 20;
            this.labelExample.Text = "Example: \\\\sharename\\Drivers\\Source";
            // 
            // checkBoxLegacyFolder
            // 
            this.checkBoxLegacyFolder.AutoSize = true;
            this.checkBoxLegacyFolder.Location = new System.Drawing.Point(10, 336);
            this.checkBoxLegacyFolder.Name = "checkBoxLegacyFolder";
            this.checkBoxLegacyFolder.Size = new System.Drawing.Size(139, 17);
            this.checkBoxLegacyFolder.TabIndex = 19;
            this.checkBoxLegacyFolder.Text = "Legacy Folder Structure";
            this.checkBoxLegacyFolder.UseVisualStyleBackColor = true;
            this.checkBoxLegacyFolder.CheckedChanged += new System.EventHandler(this.CheckBoxLegacyFolder_CheckedChanged);
            // 
            // browseFolderControlPackage
            // 
            this.browseFolderControlPackage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.browseFolderControlPackage.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.browseFolderControlPackage.Description = "";
            this.browseFolderControlPackage.EditBoxAccessibleName = null;
            this.browseFolderControlPackage.FolderTextReadOnly = false;
            this.browseFolderControlPackage.IsLocalFolder = false;
            this.browseFolderControlPackage.LableDescriptionWidth = 0;
            this.browseFolderControlPackage.Location = new System.Drawing.Point(10, 117);
            this.browseFolderControlPackage.MinimumSize = new System.Drawing.Size(150, 25);
            this.browseFolderControlPackage.Name = "browseFolderControlPackage";
            this.browseFolderControlPackage.Size = new System.Drawing.Size(360, 25);
            this.browseFolderControlPackage.TabIndex = 18;
            this.browseFolderControlPackage.FolderTextChanged += new System.EventHandler(this.BrowseFolderControlPackage_FolderTextChanged);
            // 
            // browseFolderControlSource
            // 
            this.browseFolderControlSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.browseFolderControlSource.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.browseFolderControlSource.Description = "";
            this.browseFolderControlSource.EditBoxAccessibleName = null;
            this.browseFolderControlSource.FolderTextReadOnly = false;
            this.browseFolderControlSource.IsLocalFolder = false;
            this.browseFolderControlSource.LableDescriptionWidth = 0;
            this.browseFolderControlSource.Location = new System.Drawing.Point(10, 48);
            this.browseFolderControlSource.MinimumSize = new System.Drawing.Size(150, 25);
            this.browseFolderControlSource.Name = "browseFolderControlSource";
            this.browseFolderControlSource.Size = new System.Drawing.Size(360, 25);
            this.browseFolderControlSource.TabIndex = 17;
            this.browseFolderControlSource.FolderTextChanged += new System.EventHandler(this.BrowseFolderControlSource_FolderTextChanged);
            // 
            // labelPackage
            // 
            this.labelPackage.AutoSize = true;
            this.labelPackage.Location = new System.Drawing.Point(7, 101);
            this.labelPackage.Name = "labelPackage";
            this.labelPackage.Size = new System.Drawing.Size(268, 13);
            this.labelPackage.TabIndex = 16;
            this.labelPackage.Text = "Specify a network path (UNC) to store driver packages:";
            // 
            // labelSource
            // 
            this.labelSource.AutoSize = true;
            this.labelSource.Location = new System.Drawing.Point(7, 32);
            this.labelSource.Name = "labelSource";
            this.labelSource.Size = new System.Drawing.Size(294, 13);
            this.labelSource.TabIndex = 15;
            this.labelSource.Text = "Specify a network path (UNC) to the drivers source structure:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 247);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(206, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "Example: \\\\sharename\\Packages\\Drivers";
            // 
            // browseFolderControlLegacyPackage
            // 
            this.browseFolderControlLegacyPackage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.browseFolderControlLegacyPackage.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.browseFolderControlLegacyPackage.Description = "";
            this.browseFolderControlLegacyPackage.EditBoxAccessibleName = null;
            this.browseFolderControlLegacyPackage.FolderTextReadOnly = false;
            this.browseFolderControlLegacyPackage.IsLocalFolder = false;
            this.browseFolderControlLegacyPackage.LableDescriptionWidth = 0;
            this.browseFolderControlLegacyPackage.Location = new System.Drawing.Point(10, 219);
            this.browseFolderControlLegacyPackage.MinimumSize = new System.Drawing.Size(150, 25);
            this.browseFolderControlLegacyPackage.Name = "browseFolderControlLegacyPackage";
            this.browseFolderControlLegacyPackage.Size = new System.Drawing.Size(360, 25);
            this.browseFolderControlLegacyPackage.TabIndex = 24;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 203);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(273, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Specify a network path (UNC) to store legacy packages:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(3, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(372, 161);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Driver Package Manager";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.checkBoxZipContent);
            this.groupBox2.Location = new System.Drawing.Point(3, 181);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(372, 131);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Legacy Package Manager";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(174, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Use this options better transfer size.";
            // 
            // checkBoxZipContent
            // 
            this.checkBoxZipContent.AutoSize = true;
            this.checkBoxZipContent.Location = new System.Drawing.Point(7, 92);
            this.checkBoxZipContent.Name = "checkBoxZipContent";
            this.checkBoxZipContent.Size = new System.Drawing.Size(80, 17);
            this.checkBoxZipContent.TabIndex = 0;
            this.checkBoxZipContent.Text = "Zip content";
            this.checkBoxZipContent.UseVisualStyleBackColor = true;
            // 
            // OptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.browseFolderControlLegacyPackage);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelExample);
            this.Controls.Add(this.checkBoxLegacyFolder);
            this.Controls.Add(this.browseFolderControlPackage);
            this.Controls.Add(this.browseFolderControlSource);
            this.Controls.Add(this.labelPackage);
            this.Controls.Add(this.labelSource);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "OptionsControl";
            this.Size = new System.Drawing.Size(378, 402);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelExample;
        private System.Windows.Forms.CheckBox checkBoxLegacyFolder;
        private Microsoft.ConfigurationManagement.AdminConsole.OsdCommon.BrowseFolderControl browseFolderControlPackage;
        private Microsoft.ConfigurationManagement.AdminConsole.OsdCommon.BrowseFolderControl browseFolderControlSource;
        private System.Windows.Forms.Label labelPackage;
        private System.Windows.Forms.Label labelSource;
        private System.Windows.Forms.Label label3;
        private Microsoft.ConfigurationManagement.AdminConsole.OsdCommon.BrowseFolderControl browseFolderControlLegacyPackage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBoxZipContent;
    }
}
