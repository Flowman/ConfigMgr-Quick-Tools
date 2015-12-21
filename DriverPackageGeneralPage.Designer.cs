namespace Zetta.ConfigMgr.IntegrationKit
{
    partial class DriverPackageGeneralPage
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
            this.browseFolderControlSource = new Microsoft.ConfigurationManagement.AdminConsole.OsdCommon.BrowseFolderControl();
            this.browseFolderControlPackage = new Microsoft.ConfigurationManagement.AdminConsole.OsdCommon.BrowseFolderControl();
            this.labelSource = new System.Windows.Forms.Label();
            this.labelPackage = new System.Windows.Forms.Label();
            this.labelExample = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxImportRemote = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.radioButtonDomain = new System.Windows.Forms.RadioButton();
            this.radioButtonWorkgroup = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // browseFolderControlSource
            // 
            this.browseFolderControlSource.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.browseFolderControlSource.Description = "";
            this.browseFolderControlSource.FolderTextReadOnly = false;
            this.browseFolderControlSource.IsLocalFolder = false;
            this.browseFolderControlSource.LableDescriptionWidth = 0;
            this.browseFolderControlSource.Location = new System.Drawing.Point(6, 25);
            this.browseFolderControlSource.MinimumSize = new System.Drawing.Size(150, 25);
            this.browseFolderControlSource.Name = "browseFolderControlSource";
            this.browseFolderControlSource.Size = new System.Drawing.Size(476, 25);
            this.browseFolderControlSource.TabIndex = 0;
            this.browseFolderControlSource.FolderTextChanged += new System.EventHandler(this.browseFolderControl_FolderTextChanged);
            // 
            // browseFolderControlPackage
            // 
            this.browseFolderControlPackage.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.browseFolderControlPackage.Description = "";
            this.browseFolderControlPackage.FolderTextReadOnly = false;
            this.browseFolderControlPackage.IsLocalFolder = false;
            this.browseFolderControlPackage.LableDescriptionWidth = 0;
            this.browseFolderControlPackage.Location = new System.Drawing.Point(6, 110);
            this.browseFolderControlPackage.MinimumSize = new System.Drawing.Size(150, 25);
            this.browseFolderControlPackage.Name = "browseFolderControlPackage";
            this.browseFolderControlPackage.Size = new System.Drawing.Size(476, 25);
            this.browseFolderControlPackage.TabIndex = 1;
            this.browseFolderControlPackage.FolderTextChanged += new System.EventHandler(this.browseFolderControl_FolderTextChanged);
            // 
            // labelSource
            // 
            this.labelSource.AutoSize = true;
            this.labelSource.Location = new System.Drawing.Point(3, 9);
            this.labelSource.Name = "labelSource";
            this.labelSource.Size = new System.Drawing.Size(80, 13);
            this.labelSource.TabIndex = 5;
            this.labelSource.Text = "Drivers Source:";
            // 
            // labelPackage
            // 
            this.labelPackage.AutoSize = true;
            this.labelPackage.Location = new System.Drawing.Point(3, 94);
            this.labelPackage.Name = "labelPackage";
            this.labelPackage.Size = new System.Drawing.Size(89, 13);
            this.labelPackage.TabIndex = 6;
            this.labelPackage.Text = "Driver Packages:";
            // 
            // labelExample
            // 
            this.labelExample.AutoSize = true;
            this.labelExample.Location = new System.Drawing.Point(3, 53);
            this.labelExample.Name = "labelExample";
            this.labelExample.Size = new System.Drawing.Size(154, 13);
            this.labelExample.TabIndex = 7;
            this.labelExample.Text = "Example: \\\\sharename\\Source";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 138);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Example: \\\\sharename\\Packages";
            // 
            // checkBoxImportRemote
            // 
            this.checkBoxImportRemote.AutoSize = true;
            this.checkBoxImportRemote.Location = new System.Drawing.Point(7, 196);
            this.checkBoxImportRemote.Name = "checkBoxImportRemote";
            this.checkBoxImportRemote.Size = new System.Drawing.Size(188, 17);
            this.checkBoxImportRemote.TabIndex = 9;
            this.checkBoxImportRemote.Text = "Grab drivers from remote computer";
            this.checkBoxImportRemote.UseVisualStyleBackColor = true;
            this.checkBoxImportRemote.CheckedChanged += new System.EventHandler(this.checkBoxImportRemote_CheckedChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(23, 218);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(354, 32);
            this.label2.TabIndex = 10;
            this.label2.Text = "In order to grab drivers from remote computer, remote registry service needs to b" +
    "e enabled on the target host.";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(43, 330);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(235, 13);
            this.linkLabel1.TabIndex = 11;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "https://support.microsoft.com/en-us/kb/951016";
            this.linkLabel1.Click += new System.EventHandler(this.linkLabel1_Click);
            // 
            // radioButtonDomain
            // 
            this.radioButtonDomain.AutoSize = true;
            this.radioButtonDomain.Enabled = false;
            this.radioButtonDomain.Location = new System.Drawing.Point(26, 253);
            this.radioButtonDomain.Name = "radioButtonDomain";
            this.radioButtonDomain.Size = new System.Drawing.Size(95, 17);
            this.radioButtonDomain.TabIndex = 12;
            this.radioButtonDomain.TabStop = true;
            this.radioButtonDomain.Text = "Domain Joined";
            this.radioButtonDomain.UseVisualStyleBackColor = true;
            this.radioButtonDomain.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButtonWorkgroup
            // 
            this.radioButtonWorkgroup.AutoSize = true;
            this.radioButtonWorkgroup.Enabled = false;
            this.radioButtonWorkgroup.Location = new System.Drawing.Point(26, 276);
            this.radioButtonWorkgroup.Name = "radioButtonWorkgroup";
            this.radioButtonWorkgroup.Size = new System.Drawing.Size(78, 17);
            this.radioButtonWorkgroup.TabIndex = 13;
            this.radioButtonWorkgroup.TabStop = true;
            this.radioButtonWorkgroup.Text = "Workgroup";
            this.radioButtonWorkgroup.UseVisualStyleBackColor = true;
            this.radioButtonWorkgroup.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(43, 301);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(334, 29);
            this.label3.TabIndex = 14;
            this.label3.Text = "For a Workgroup host, the \'LocalAccountTokenFilterPolicy\' registry key needs to b" +
    "e set to 1.";
            // 
            // DriverPackageGeneralPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.radioButtonWorkgroup);
            this.Controls.Add(this.radioButtonDomain);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBoxImportRemote);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelExample);
            this.Controls.Add(this.labelPackage);
            this.Controls.Add(this.labelSource);
            this.Controls.Add(this.browseFolderControlPackage);
            this.Controls.Add(this.browseFolderControlSource);
            this.Name = "DriverPackageGeneralPage";
            this.Size = new System.Drawing.Size(485, 360);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.ConfigurationManagement.AdminConsole.OsdCommon.BrowseFolderControl browseFolderControlSource;
        private Microsoft.ConfigurationManagement.AdminConsole.OsdCommon.BrowseFolderControl browseFolderControlPackage;
        private System.Windows.Forms.Label labelSource;
        private System.Windows.Forms.Label labelPackage;
        private System.Windows.Forms.Label labelExample;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxImportRemote;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.RadioButton radioButtonDomain;
        private System.Windows.Forms.RadioButton radioButtonWorkgroup;
        private System.Windows.Forms.Label label3;
    }
}
