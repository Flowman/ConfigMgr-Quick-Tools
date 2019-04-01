namespace ConfigMgr.QuickTools.DriverManager
{
    partial class DriverOptions
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelPackage = new System.Windows.Forms.Label();
            this.labelSource = new System.Windows.Forms.Label();
            this.browseFolderControlSource = new Microsoft.ConfigurationManagement.AdminConsole.OsdCommon.BrowseFolderControl();
            this.browseFolderControlPackage = new Microsoft.ConfigurationManagement.AdminConsole.OsdCommon.BrowseFolderControl();
            this.checkBoxLegacyFolder = new System.Windows.Forms.CheckBox();
            this.labelExample = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(425, 203);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(344, 203);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // labelPackage
            // 
            this.labelPackage.AutoSize = true;
            this.labelPackage.Location = new System.Drawing.Point(12, 83);
            this.labelPackage.Name = "labelPackage";
            this.labelPackage.Size = new System.Drawing.Size(268, 13);
            this.labelPackage.TabIndex = 8;
            this.labelPackage.Text = "Specify a network path (UNC) to store driver packages:";
            // 
            // labelSource
            // 
            this.labelSource.AutoSize = true;
            this.labelSource.Location = new System.Drawing.Point(12, 9);
            this.labelSource.Name = "labelSource";
            this.labelSource.Size = new System.Drawing.Size(294, 13);
            this.labelSource.TabIndex = 7;
            this.labelSource.Text = "Specify a network path (UNC) to the drivers source structure:";
            // 
            // browseFolderControlSource
            // 
            this.browseFolderControlSource.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.browseFolderControlSource.Description = "";
            this.browseFolderControlSource.EditBoxAccessibleName = null;
            this.browseFolderControlSource.FolderTextReadOnly = false;
            this.browseFolderControlSource.IsLocalFolder = false;
            this.browseFolderControlSource.LableDescriptionWidth = 0;
            this.browseFolderControlSource.Location = new System.Drawing.Point(15, 25);
            this.browseFolderControlSource.MinimumSize = new System.Drawing.Size(150, 25);
            this.browseFolderControlSource.Name = "browseFolderControlSource";
            this.browseFolderControlSource.Size = new System.Drawing.Size(485, 25);
            this.browseFolderControlSource.TabIndex = 9;
            // 
            // browseFolderControlPackage
            // 
            this.browseFolderControlPackage.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.browseFolderControlPackage.Description = "";
            this.browseFolderControlPackage.EditBoxAccessibleName = null;
            this.browseFolderControlPackage.FolderTextReadOnly = false;
            this.browseFolderControlPackage.IsLocalFolder = false;
            this.browseFolderControlPackage.LableDescriptionWidth = 0;
            this.browseFolderControlPackage.Location = new System.Drawing.Point(15, 99);
            this.browseFolderControlPackage.MinimumSize = new System.Drawing.Size(150, 25);
            this.browseFolderControlPackage.Name = "browseFolderControlPackage";
            this.browseFolderControlPackage.Size = new System.Drawing.Size(485, 25);
            this.browseFolderControlPackage.TabIndex = 10;
            // 
            // checkBoxLegacyFolder
            // 
            this.checkBoxLegacyFolder.AutoSize = true;
            this.checkBoxLegacyFolder.Location = new System.Drawing.Point(15, 157);
            this.checkBoxLegacyFolder.Name = "checkBoxLegacyFolder";
            this.checkBoxLegacyFolder.Size = new System.Drawing.Size(139, 17);
            this.checkBoxLegacyFolder.TabIndex = 11;
            this.checkBoxLegacyFolder.Text = "Legacy Folder Structure";
            this.checkBoxLegacyFolder.UseVisualStyleBackColor = true;
            // 
            // labelExample
            // 
            this.labelExample.AutoSize = true;
            this.labelExample.Location = new System.Drawing.Point(12, 53);
            this.labelExample.Name = "labelExample";
            this.labelExample.Size = new System.Drawing.Size(154, 13);
            this.labelExample.TabIndex = 12;
            this.labelExample.Text = "Example: \\\\sharename\\Source";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 127);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Example: \\\\sharename\\Packages";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 177);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(310, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Use this options if using the legacy driver source folder structure.";
            // 
            // DriverOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 238);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelExample);
            this.Controls.Add(this.checkBoxLegacyFolder);
            this.Controls.Add(this.browseFolderControlPackage);
            this.Controls.Add(this.browseFolderControlSource);
            this.Controls.Add(this.labelPackage);
            this.Controls.Add(this.labelSource);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Name = "DriverOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label labelPackage;
        private System.Windows.Forms.Label labelSource;
        private Microsoft.ConfigurationManagement.AdminConsole.OsdCommon.BrowseFolderControl browseFolderControlSource;
        private Microsoft.ConfigurationManagement.AdminConsole.OsdCommon.BrowseFolderControl browseFolderControlPackage;
        private System.Windows.Forms.CheckBox checkBoxLegacyFolder;
        private System.Windows.Forms.Label labelExample;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}