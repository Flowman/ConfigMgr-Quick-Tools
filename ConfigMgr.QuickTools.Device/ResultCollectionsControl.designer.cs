using Microsoft.ConfigurationManagement.AdminConsole.Common;

namespace ConfigMgr.QuickTools.Device.PropertiesDialog
{
    partial class ResultCollectionsControl
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
            if (disposing)
            {
                if (this.components != null)
                    this.components.Dispose();
                if (this.backgroundWorker != null && this.backgroundWorker.IsBusy)
                {
                    this.backgroundWorker.CancelAsync();
                    this.backgroundWorker.Dispose();
                }
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
            this.listViewListCollections = new Microsoft.ConfigurationManagement.AdminConsole.Common.SmsSearchableListView();
            this.columnHeaderCollection = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderCollectionID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listViewListCollections
            // 
            this.listViewListCollections.Activation = System.Windows.Forms.ItemActivation.Standard;
            this.listViewListCollections.Alignment = System.Windows.Forms.ListViewAlignment.Top;
            this.listViewListCollections.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewListCollections.AutoSort = true;
            this.listViewListCollections.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.listViewListCollections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderCollection,
            this.columnHeaderCollectionID});
            this.listViewListCollections.CustomNoResultsText = null;
            this.listViewListCollections.FullRowSelect = true;
            this.listViewListCollections.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Clickable;
            this.listViewListCollections.HideSelection = false;
            this.listViewListCollections.IsLoading = false;
            this.listViewListCollections.LargeImageList = null;
            this.listViewListCollections.Location = new System.Drawing.Point(14, 34);
            this.listViewListCollections.MultiSelect = true;
            this.listViewListCollections.Name = "listViewListCollections";
            this.listViewListCollections.ShowSearchBar = true;
            this.listViewListCollections.Size = new System.Drawing.Size(354, 304);
            this.listViewListCollections.SmallImageList = null;
            this.listViewListCollections.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewListCollections.StateImageList = null;
            this.listViewListCollections.TabIndex = 0;
            this.listViewListCollections.TileSize = new System.Drawing.Size(0, 0);
            this.listViewListCollections.UseCompatibleStateImageBehavior = false;
            this.listViewListCollections.View = System.Windows.Forms.View.Details;
            this.listViewListCollections.CopyKeyEvent += new System.EventHandler<System.EventArgs>(this.ListViewListCollections_CopyKeyEvent);
            // 
            // columnHeaderCollection
            // 
            this.columnHeaderCollection.Text = "Collection";
            // 
            // columnHeaderCollectionID
            // 
            this.columnHeaderCollectionID.Text = "Collection ID";
            this.columnHeaderCollectionID.Width = 90;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Collection membership for this resource:";
            // 
            // ResultCollectionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listViewListCollections);
            this.Name = "ResultCollectionsControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SmsSearchableListView listViewListCollections;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader columnHeaderCollection;
        private System.Windows.Forms.ColumnHeader columnHeaderCollectionID;
    }
}
