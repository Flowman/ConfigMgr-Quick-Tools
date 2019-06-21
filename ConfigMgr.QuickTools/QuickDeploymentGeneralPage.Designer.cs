namespace ConfigMgr.QuickTools
{
    partial class QuickDeploymentGeneralPage
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
            this.listViewCollections = new Microsoft.ConfigurationManagement.AdminConsole.Common.SmsListView();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderCollectionId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.labelDescription = new System.Windows.Forms.Label();
            this.buttonSelectCollections = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listViewCollections
            // 
            this.listViewCollections.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewCollections.AutoSort = true;
            this.listViewCollections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderCollectionId});
            this.listViewCollections.CustomNoResultsText = null;
            this.listViewCollections.HideSelection = false;
            this.listViewCollections.IsLoading = false;
            this.listViewCollections.Location = new System.Drawing.Point(7, 21);
            this.listViewCollections.Name = "listViewCollections";
            this.listViewCollections.Size = new System.Drawing.Size(454, 300);
            this.listViewCollections.TabIndex = 0;
            this.listViewCollections.UseCompatibleStateImageBehavior = false;
            this.listViewCollections.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            this.columnHeaderName.Width = 250;
            // 
            // columnHeaderCollectionId
            // 
            this.columnHeaderCollectionId.Text = "Collection ID";
            this.columnHeaderCollectionId.Width = 90;
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(3, 4);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(177, 13);
            this.labelDescription.TabIndex = 4;
            this.labelDescription.Text = "Selected collections for deployment:";
            // 
            // buttonSelectCollections
            // 
            this.buttonSelectCollections.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelectCollections.Location = new System.Drawing.Point(274, 327);
            this.buttonSelectCollections.Name = "buttonSelectCollections";
            this.buttonSelectCollections.Size = new System.Drawing.Size(91, 23);
            this.buttonSelectCollections.TabIndex = 5;
            this.buttonSelectCollections.Text = "Browse";
            this.buttonSelectCollections.UseVisualStyleBackColor = true;
            this.buttonSelectCollections.Click += new System.EventHandler(this.ButtonSelectCollections_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClear.Location = new System.Drawing.Point(371, 327);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(91, 23);
            this.buttonClear.TabIndex = 6;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.ButtonClear_Click);
            // 
            // QuickDeploymentGeneralPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.buttonSelectCollections);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.listViewCollections);
            this.Name = "QuickDeploymentGeneralPage";
            this.Size = new System.Drawing.Size(485, 360);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.ConfigurationManagement.AdminConsole.Common.SmsListView listViewCollections;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Button buttonSelectCollections;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderCollectionId;
        private System.Windows.Forms.Button buttonClear;
    }
}
