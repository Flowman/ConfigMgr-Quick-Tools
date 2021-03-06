﻿namespace ConfigMgr.QuickTools.Device.PropertiesDialog
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
            this.listViewCollections = new Microsoft.ConfigurationManagement.AdminConsole.Common.SmsSearchableListView();
            this.columnHeaderCollection = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderCollectionID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listViewListCollections
            // 
            this.listViewCollections.Activation = System.Windows.Forms.ItemActivation.Standard;
            this.listViewCollections.Alignment = System.Windows.Forms.ListViewAlignment.Top;
            this.listViewCollections.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewCollections.AutoSort = true;
            this.listViewCollections.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.listViewCollections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderCollection,
            this.columnHeaderCollectionID});
            this.listViewCollections.CustomNoResultsText = null;
            this.listViewCollections.FullRowSelect = true;
            this.listViewCollections.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Clickable;
            this.listViewCollections.HideSelection = false;
            this.listViewCollections.IsLoading = false;
            this.listViewCollections.LargeImageList = null;
            this.listViewCollections.Location = new System.Drawing.Point(14, 34);
            this.listViewCollections.MultiSelect = true;
            this.listViewCollections.Name = "listViewListCollections";
            this.listViewCollections.ShowSearchBar = true;
            this.listViewCollections.Size = new System.Drawing.Size(354, 304);
            this.listViewCollections.SmallImageList = null;
            this.listViewCollections.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewCollections.StateImageList = null;
            this.listViewCollections.TabIndex = 0;
            this.listViewCollections.TileSize = new System.Drawing.Size(0, 0);
            this.listViewCollections.UseCompatibleStateImageBehavior = false;
            this.listViewCollections.View = System.Windows.Forms.View.Details;
            this.listViewCollections.CopyKeyEvent += new System.EventHandler<System.EventArgs>(this.ListView_CopyKeyEvent);
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
            this.Controls.Add(this.listViewCollections);
            this.Name = "ResultCollectionsControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.ConfigurationManagement.AdminConsole.Common.SmsSearchableListView listViewCollections;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader columnHeaderCollection;
        private System.Windows.Forms.ColumnHeader columnHeaderCollectionID;
    }
}
