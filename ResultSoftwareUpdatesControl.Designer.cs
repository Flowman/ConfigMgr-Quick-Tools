namespace Zetta.ConfigMgr.QuickTools
{
    partial class ResultSoftwareUpdatesControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSURefresh = new System.Windows.Forms.Button();
            this.listViewListSoftwareUpdates = new Microsoft.ConfigurationManagement.AdminConsole.Common.SmsSearchableListView();
            this.columnHeaderKB = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Software Updates for this resource:";
            // 
            // buttonSURefresh
            // 
            this.buttonSURefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSURefresh.Location = new System.Drawing.Point(342, 4);
            this.buttonSURefresh.Name = "buttonSURefresh";
            this.buttonSURefresh.Size = new System.Drawing.Size(24, 24);
            this.buttonSURefresh.TabIndex = 3;
            this.buttonSURefresh.UseVisualStyleBackColor = true;
            this.buttonSURefresh.Click += new System.EventHandler(this.buttonSURefresh_Click);
            // 
            // listViewListSoftwareUpdates
            // 
            this.listViewListSoftwareUpdates.Activation = System.Windows.Forms.ItemActivation.Standard;
            this.listViewListSoftwareUpdates.Alignment = System.Windows.Forms.ListViewAlignment.Top;
            this.listViewListSoftwareUpdates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewListSoftwareUpdates.AutoSort = true;
            this.listViewListSoftwareUpdates.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.listViewListSoftwareUpdates.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderKB,
            this.columnHeaderTitle,
            this.columnHeaderStatus});
            this.listViewListSoftwareUpdates.CustomNoResultsText = null;
            this.listViewListSoftwareUpdates.FullRowSelect = true;
            this.listViewListSoftwareUpdates.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Clickable;
            this.listViewListSoftwareUpdates.HideSelection = false;
            this.listViewListSoftwareUpdates.IsLoading = false;
            this.listViewListSoftwareUpdates.LargeImageList = null;
            this.listViewListSoftwareUpdates.Location = new System.Drawing.Point(12, 32);
            this.listViewListSoftwareUpdates.MultiSelect = true;
            this.listViewListSoftwareUpdates.Name = "listViewListSoftwareUpdates";
            this.listViewListSoftwareUpdates.ShowSearchBar = true;
            this.listViewListSoftwareUpdates.Size = new System.Drawing.Size(354, 319);
            this.listViewListSoftwareUpdates.SmallImageList = null;
            this.listViewListSoftwareUpdates.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewListSoftwareUpdates.StateImageList = null;
            this.listViewListSoftwareUpdates.TabIndex = 4;
            this.listViewListSoftwareUpdates.TileSize = new System.Drawing.Size(0, 0);
            this.listViewListSoftwareUpdates.UseCompatibleStateImageBehavior = false;
            this.listViewListSoftwareUpdates.View = System.Windows.Forms.View.Details;
            this.listViewListSoftwareUpdates.CopyKeyEvent += new System.EventHandler<System.EventArgs>(this.listViewListSoftwareUpdates_CopyKeyEvent);
            // 
            // columnHeaderKB
            // 
            this.columnHeaderKB.Text = "Article ID";
            this.columnHeaderKB.Width = 70;
            // 
            // columnHeaderTitle
            // 
            this.columnHeaderTitle.Text = "Title";
            this.columnHeaderTitle.Width = 260;
            // 
            // columnHeaderStatus
            // 
            this.columnHeaderStatus.Text = "Status";
            // 
            // ResultSoftwareUpdatesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listViewListSoftwareUpdates);
            this.Controls.Add(this.buttonSURefresh);
            this.Controls.Add(this.label1);
            this.Name = "ResultSoftwareUpdatesControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonSURefresh;
        private Microsoft.ConfigurationManagement.AdminConsole.Common.SmsSearchableListView listViewListSoftwareUpdates;
        private System.Windows.Forms.ColumnHeader columnHeaderKB;
        private System.Windows.Forms.ColumnHeader columnHeaderTitle;
        private System.Windows.Forms.ColumnHeader columnHeaderStatus;
    }
}
