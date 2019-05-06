namespace ConfigMgr.QuickTools.Device.PropertiesDialog
{
    partial class ResultMaintenanceWindowsControl
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
            this.listViewListWindows = new Microsoft.ConfigurationManagement.AdminConsole.Common.SmsSearchableListView();
            this.columnHeaderWindows = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderCollection = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.listViewListUpcomingWindows = new Microsoft.ConfigurationManagement.AdminConsole.Common.SmsSearchableListView();
            this.columnHeaderUpcomingTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderUpcomingName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(197, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Maintenance Windows for this resource:";
            // 
            // listViewListWindows
            // 
            this.listViewListWindows.Activation = System.Windows.Forms.ItemActivation.Standard;
            this.listViewListWindows.Alignment = System.Windows.Forms.ListViewAlignment.Top;
            this.listViewListWindows.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewListWindows.AutoSort = true;
            this.listViewListWindows.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.listViewListWindows.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderWindows,
            this.columnHeaderCollection});
            this.listViewListWindows.CustomNoResultsText = null;
            this.listViewListWindows.FullRowSelect = true;
            this.listViewListWindows.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Clickable;
            this.listViewListWindows.HideSelection = false;
            this.listViewListWindows.IsLoading = false;
            this.listViewListWindows.LargeImageList = null;
            this.listViewListWindows.Location = new System.Drawing.Point(15, 37);
            this.listViewListWindows.MultiSelect = true;
            this.listViewListWindows.Name = "listViewListWindows";
            this.listViewListWindows.ShowSearchBar = true;
            this.listViewListWindows.Size = new System.Drawing.Size(354, 162);
            this.listViewListWindows.SmallImageList = null;
            this.listViewListWindows.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewListWindows.StateImageList = null;
            this.listViewListWindows.TabIndex = 2;
            this.listViewListWindows.TileSize = new System.Drawing.Size(0, 0);
            this.listViewListWindows.UseCompatibleStateImageBehavior = false;
            this.listViewListWindows.View = System.Windows.Forms.View.Details;
            this.listViewListWindows.CopyKeyEvent += new System.EventHandler<System.EventArgs>(this.ListViewListWindows_CopyKeyEvent);
            // 
            // columnHeaderWindows
            // 
            this.columnHeaderWindows.Text = "Maintenance Windows";
            this.columnHeaderWindows.Width = 150;
            // 
            // columnHeaderCollection
            // 
            this.columnHeaderCollection.Text = "Collection";
            this.columnHeaderCollection.Width = 200;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 213);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(248, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Upcoming Maintenance Windows for this resource:";
            // 
            // listViewListUpcomingWindows
            // 
            this.listViewListUpcomingWindows.Activation = System.Windows.Forms.ItemActivation.Standard;
            this.listViewListUpcomingWindows.Alignment = System.Windows.Forms.ListViewAlignment.Top;
            this.listViewListUpcomingWindows.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewListUpcomingWindows.AutoSort = true;
            this.listViewListUpcomingWindows.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.listViewListUpcomingWindows.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderUpcomingTime,
            this.columnHeaderUpcomingName});
            this.listViewListUpcomingWindows.CustomNoResultsText = null;
            this.listViewListUpcomingWindows.FullRowSelect = true;
            this.listViewListUpcomingWindows.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Clickable;
            this.listViewListUpcomingWindows.HideSelection = false;
            this.listViewListUpcomingWindows.IsLoading = false;
            this.listViewListUpcomingWindows.LargeImageList = null;
            this.listViewListUpcomingWindows.Location = new System.Drawing.Point(13, 237);
            this.listViewListUpcomingWindows.MultiSelect = true;
            this.listViewListUpcomingWindows.Name = "listViewListUpcomingWindows";
            this.listViewListUpcomingWindows.ShowSearchBar = true;
            this.listViewListUpcomingWindows.Size = new System.Drawing.Size(354, 104);
            this.listViewListUpcomingWindows.SmallImageList = null;
            this.listViewListUpcomingWindows.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewListUpcomingWindows.StateImageList = null;
            this.listViewListUpcomingWindows.TabIndex = 4;
            this.listViewListUpcomingWindows.TileSize = new System.Drawing.Size(0, 0);
            this.listViewListUpcomingWindows.UseCompatibleStateImageBehavior = false;
            this.listViewListUpcomingWindows.View = System.Windows.Forms.View.Details;
            this.listViewListUpcomingWindows.CopyKeyEvent += new System.EventHandler<System.EventArgs>(this.ListViewListUpcomingWindows_CopyKeyEvent);
            // 
            // columnHeaderUpcomingTime
            // 
            this.columnHeaderUpcomingTime.Text = "Date";
            this.columnHeaderUpcomingTime.Width = 150;
            // 
            // columnHeaderUpcomingName
            // 
            this.columnHeaderUpcomingName.Text = "Name";
            this.columnHeaderUpcomingName.Width = 250;
            // 
            // ResultMaintenanceWindowsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listViewListUpcomingWindows);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listViewListWindows);
            this.Name = "ResultMaintenanceWindowsControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Microsoft.ConfigurationManagement.AdminConsole.Common.SmsSearchableListView listViewListWindows;
        private System.Windows.Forms.Label label2;
        private Microsoft.ConfigurationManagement.AdminConsole.Common.SmsSearchableListView listViewListUpcomingWindows;
        private System.Windows.Forms.ColumnHeader columnHeaderWindows;
        private System.Windows.Forms.ColumnHeader columnHeaderCollection;
        private System.Windows.Forms.ColumnHeader columnHeaderUpcomingTime;
        private System.Windows.Forms.ColumnHeader columnHeaderUpcomingName;
    }
}
