namespace Zetta.ConfigMgr.IntegrationKit
{
    partial class SUDeploymentGeneralPage
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
            this.listViewListCollections = new System.Windows.Forms.ListView();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderCollectionId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.labelDescription = new System.Windows.Forms.Label();
            this.buttonSelectCollections = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listViewListCollections
            // 
            this.listViewListCollections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderCollectionId});
            this.listViewListCollections.HideSelection = false;
            this.listViewListCollections.Location = new System.Drawing.Point(13, 16);
            this.listViewListCollections.Name = "listViewListCollections";
            this.listViewListCollections.Size = new System.Drawing.Size(563, 300);
            this.listViewListCollections.TabIndex = 0;
            this.listViewListCollections.UseCompatibleStateImageBehavior = false;
            this.listViewListCollections.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            this.columnHeaderName.Width = 450;
            // 
            // columnHeaderCollectionId
            // 
            this.columnHeaderCollectionId.Text = "Collection ID";
            this.columnHeaderCollectionId.Width = 90;
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(10, 0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(177, 13);
            this.labelDescription.TabIndex = 4;
            this.labelDescription.Text = "Selected collections for deployment:";
            // 
            // buttonSelectCollections
            // 
            this.buttonSelectCollections.Location = new System.Drawing.Point(388, 322);
            this.buttonSelectCollections.Name = "buttonSelectCollections";
            this.buttonSelectCollections.Size = new System.Drawing.Size(91, 23);
            this.buttonSelectCollections.TabIndex = 5;
            this.buttonSelectCollections.Text = "Browse";
            this.buttonSelectCollections.UseVisualStyleBackColor = true;
            this.buttonSelectCollections.Click += new System.EventHandler(this.buttonSelectCollections_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(485, 322);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(91, 23);
            this.buttonClear.TabIndex = 6;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // SUDeploymentGeneralPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.buttonSelectCollections);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.listViewListCollections);
            this.MinimumSize = new System.Drawing.Size(600, 360);
            this.Name = "SUDeploymentGeneralPage";
            this.Size = new System.Drawing.Size(600, 360);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewListCollections;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Button buttonSelectCollections;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderCollectionId;
        private System.Windows.Forms.Button buttonClear;
    }
}
