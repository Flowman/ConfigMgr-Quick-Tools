namespace ConfigMgr.QuickTools.DriverManager
{
    partial class OptionsLegacyControl
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
            this.label3 = new System.Windows.Forms.Label();
            this.browseFolderControlLegacyPackage = new Microsoft.ConfigurationManagement.AdminConsole.OsdCommon.BrowseFolderControl();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxConsoleFolder = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxZipContent = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(206, 13);
            this.label3.TabIndex = 30;
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
            this.browseFolderControlLegacyPackage.Location = new System.Drawing.Point(10, 48);
            this.browseFolderControlLegacyPackage.MinimumSize = new System.Drawing.Size(150, 25);
            this.browseFolderControlLegacyPackage.Name = "browseFolderControlLegacyPackage";
            this.browseFolderControlLegacyPackage.Size = new System.Drawing.Size(361, 25);
            this.browseFolderControlLegacyPackage.TabIndex = 29;
            this.browseFolderControlLegacyPackage.FolderTextChanged += new System.EventHandler(this.Control_Changed);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(273, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "Specify a network path (UNC) to store legacy packages:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 101);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(204, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "Console Folder name to add packages to:";
            // 
            // textBoxConsoleFolder
            // 
            this.textBoxConsoleFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxConsoleFolder.Location = new System.Drawing.Point(10, 120);
            this.textBoxConsoleFolder.Name = "textBoxConsoleFolder";
            this.textBoxConsoleFolder.Size = new System.Drawing.Size(253, 20);
            this.textBoxConsoleFolder.TabIndex = 24;
            this.textBoxConsoleFolder.TextChanged += new System.EventHandler(this.Control_Changed);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 175);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(217, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Use this option for better transfer size. LINK?";
            // 
            // checkBoxZipContent
            // 
            this.checkBoxZipContent.AutoSize = true;
            this.checkBoxZipContent.Location = new System.Drawing.Point(10, 155);
            this.checkBoxZipContent.Name = "checkBoxZipContent";
            this.checkBoxZipContent.Size = new System.Drawing.Size(80, 17);
            this.checkBoxZipContent.TabIndex = 0;
            this.checkBoxZipContent.Text = "Zip content";
            this.checkBoxZipContent.UseVisualStyleBackColor = true;
            this.checkBoxZipContent.CheckedChanged += new System.EventHandler(this.Control_Changed);
            // 
            // OptionsLegacyControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxConsoleFolder);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.browseFolderControlLegacyPackage);
            this.Controls.Add(this.checkBoxZipContent);
            this.Controls.Add(this.label4);
            this.Name = "OptionsLegacyControl";
            this.Size = new System.Drawing.Size(380, 420);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private Microsoft.ConfigurationManagement.AdminConsole.OsdCommon.BrowseFolderControl browseFolderControlLegacyPackage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxConsoleFolder;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBoxZipContent;
    }
}
