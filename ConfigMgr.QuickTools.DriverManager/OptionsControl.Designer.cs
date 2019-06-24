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
            this.textBoxConsoleFolder = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxQuickMerge = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 389);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(305, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Use this option if using the legacy driver source folder structure.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 141);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(206, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Example: \\\\sharename\\Drivers\\Packages";
            // 
            // labelExample
            // 
            this.labelExample.AutoSize = true;
            this.labelExample.Location = new System.Drawing.Point(13, 76);
            this.labelExample.Name = "labelExample";
            this.labelExample.Size = new System.Drawing.Size(192, 13);
            this.labelExample.TabIndex = 20;
            this.labelExample.Text = "Example: \\\\sharename\\Drivers\\Source";
            // 
            // checkBoxLegacyFolder
            // 
            this.checkBoxLegacyFolder.AutoSize = true;
            this.checkBoxLegacyFolder.Location = new System.Drawing.Point(10, 369);
            this.checkBoxLegacyFolder.Name = "checkBoxLegacyFolder";
            this.checkBoxLegacyFolder.Size = new System.Drawing.Size(139, 17);
            this.checkBoxLegacyFolder.TabIndex = 19;
            this.checkBoxLegacyFolder.Text = "Legacy Folder Structure";
            this.checkBoxLegacyFolder.UseVisualStyleBackColor = true;
            this.checkBoxLegacyFolder.CheckedChanged += new System.EventHandler(this.Control_Changed);
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
            this.browseFolderControlPackage.Location = new System.Drawing.Point(10, 113);
            this.browseFolderControlPackage.MinimumSize = new System.Drawing.Size(150, 25);
            this.browseFolderControlPackage.Name = "browseFolderControlPackage";
            this.browseFolderControlPackage.Size = new System.Drawing.Size(361, 25);
            this.browseFolderControlPackage.TabIndex = 18;
            this.browseFolderControlPackage.FolderTextChanged += new System.EventHandler(this.Control_Changed);
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
            this.browseFolderControlSource.Size = new System.Drawing.Size(361, 25);
            this.browseFolderControlSource.TabIndex = 17;
            this.browseFolderControlSource.FolderTextChanged += new System.EventHandler(this.Control_Changed);
            // 
            // labelPackage
            // 
            this.labelPackage.AutoSize = true;
            this.labelPackage.Location = new System.Drawing.Point(7, 97);
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
            // textBoxConsoleFolder
            // 
            this.textBoxConsoleFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxConsoleFolder.Location = new System.Drawing.Point(10, 183);
            this.textBoxConsoleFolder.Name = "textBoxConsoleFolder";
            this.textBoxConsoleFolder.Size = new System.Drawing.Size(253, 20);
            this.textBoxConsoleFolder.TabIndex = 26;
            this.textBoxConsoleFolder.TextChanged += new System.EventHandler(this.Control_Changed);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 164);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(233, 13);
            this.label6.TabIndex = 27;
            this.label6.Text = "Console Folder name to add driver packages to:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 208);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(159, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "Example: %MANUFACTURER%";
            // 
            // checkBoxQuickMerge
            // 
            this.checkBoxQuickMerge.AutoSize = true;
            this.checkBoxQuickMerge.Location = new System.Drawing.Point(10, 235);
            this.checkBoxQuickMerge.Name = "checkBoxQuickMerge";
            this.checkBoxQuickMerge.Size = new System.Drawing.Size(138, 17);
            this.checkBoxQuickMerge.TabIndex = 29;
            this.checkBoxQuickMerge.Text = "Quick Merge duplicates";
            this.checkBoxQuickMerge.UseVisualStyleBackColor = true;
            this.checkBoxQuickMerge.Visible = false;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Location = new System.Drawing.Point(14, 255);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(357, 42);
            this.label4.TabIndex = 30;
            this.label4.Text = "Use this option only if all the drivers are either x64 or x86. This will merge dr" +
    "ivers based on Model and Version, so if there is a mix with x64 and x86 it might" +
    " merge wrong architecture.";
            this.label4.Visible = false;
            // 
            // OptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.checkBoxQuickMerge);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.labelExample);
            this.Controls.Add(this.textBoxConsoleFolder);
            this.Controls.Add(this.checkBoxLegacyFolder);
            this.Controls.Add(this.browseFolderControlPackage);
            this.Controls.Add(this.browseFolderControlSource);
            this.Controls.Add(this.labelPackage);
            this.Controls.Add(this.labelSource);
            this.Name = "OptionsControl";
            this.Size = new System.Drawing.Size(380, 420);
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
        private System.Windows.Forms.TextBox textBoxConsoleFolder;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBoxQuickMerge;
        private System.Windows.Forms.Label label4;
    }
}
