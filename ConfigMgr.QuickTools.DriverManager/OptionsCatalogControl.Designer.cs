namespace ConfigMgr.QuickTools.DriverManager
{
    partial class OptionsCatalogControl
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
            this.textBoxDellCatalogUri = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.browseFolderControlDownload = new Microsoft.ConfigurationManagement.AdminConsole.OsdCommon.BrowseFolderControl();
            this.textBoxHPCatalogUri = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxDellCatalogUri
            // 
            this.textBoxDellCatalogUri.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDellCatalogUri.Location = new System.Drawing.Point(10, 41);
            this.textBoxDellCatalogUri.Name = "textBoxDellCatalogUri";
            this.textBoxDellCatalogUri.Size = new System.Drawing.Size(360, 20);
            this.textBoxDellCatalogUri.TabIndex = 17;
            this.textBoxDellCatalogUri.TextChanged += new System.EventHandler(this.Control_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(155, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Dell DriverPackCatalog.cab url:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 241);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Specify a temporary package download path:";
            // 
            // browseFolderControlDownload
            // 
            this.browseFolderControlDownload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.browseFolderControlDownload.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.browseFolderControlDownload.Description = "";
            this.browseFolderControlDownload.EditBoxAccessibleName = null;
            this.browseFolderControlDownload.FolderTextReadOnly = false;
            this.browseFolderControlDownload.IsLocalFolder = false;
            this.browseFolderControlDownload.LableDescriptionWidth = 0;
            this.browseFolderControlDownload.Location = new System.Drawing.Point(10, 257);
            this.browseFolderControlDownload.MinimumSize = new System.Drawing.Size(150, 25);
            this.browseFolderControlDownload.Name = "browseFolderControlDownload";
            this.browseFolderControlDownload.Size = new System.Drawing.Size(360, 25);
            this.browseFolderControlDownload.TabIndex = 19;
            this.browseFolderControlDownload.FolderTextChanged += new System.EventHandler(this.Control_TextChanged);
            // 
            // textBoxHPCatalogUri
            // 
            this.textBoxHPCatalogUri.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxHPCatalogUri.Location = new System.Drawing.Point(10, 86);
            this.textBoxHPCatalogUri.Name = "textBoxHPCatalogUri";
            this.textBoxHPCatalogUri.Size = new System.Drawing.Size(360, 20);
            this.textBoxHPCatalogUri.TabIndex = 21;
            this.textBoxHPCatalogUri.TextChanged += new System.EventHandler(this.Control_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(193, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "HP HPClientDriverPackCatalog.cab url:";
            // 
            // OptionsCatalogControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxHPCatalogUri);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.browseFolderControlDownload);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxDellCatalogUri);
            this.Controls.Add(this.label2);
            this.Name = "OptionsCatalogControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxDellCatalogUri;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private Microsoft.ConfigurationManagement.AdminConsole.OsdCommon.BrowseFolderControl browseFolderControlDownload;
        private System.Windows.Forms.TextBox textBoxHPCatalogUri;
        private System.Windows.Forms.Label label3;
    }
}
