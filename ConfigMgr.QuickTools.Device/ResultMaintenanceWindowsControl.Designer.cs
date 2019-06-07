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
            this.listViewWindows = new Microsoft.ConfigurationManagement.AdminConsole.Common.SmsSearchableListView();
            this.columnHeaderWindows = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderCollection = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.listViewUpcomingWindows = new Microsoft.ConfigurationManagement.AdminConsole.Common.SmsSearchableListView();
            this.columnHeaderUpcomingTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderUpcomingName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(197, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Maintenance Windows for this resource:";
            // 
            // listViewWindows
            // 
            this.listViewWindows.Activation = System.Windows.Forms.ItemActivation.Standard;
            this.listViewWindows.Alignment = System.Windows.Forms.ListViewAlignment.Top;
            this.listViewWindows.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewWindows.AutoSort = true;
            this.listViewWindows.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.listViewWindows.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderWindows,
            this.columnHeaderCollection});
            this.listViewWindows.CustomNoResultsText = null;
            this.listViewWindows.FullRowSelect = true;
            this.listViewWindows.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Clickable;
            this.listViewWindows.HideSelection = false;
            this.listViewWindows.IsLoading = false;
            this.listViewWindows.LargeImageList = null;
            this.listViewWindows.Location = new System.Drawing.Point(14, 34);
            this.listViewWindows.MultiSelect = true;
            this.listViewWindows.Name = "listViewWindows";
            this.listViewWindows.ShowSearchBar = true;
            this.listViewWindows.Size = new System.Drawing.Size(354, 162);
            this.listViewWindows.SmallImageList = null;
            this.listViewWindows.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewWindows.StateImageList = null;
            this.listViewWindows.TabIndex = 2;
            this.listViewWindows.TileSize = new System.Drawing.Size(0, 0);
            this.listViewWindows.UseCompatibleStateImageBehavior = false;
            this.listViewWindows.View = System.Windows.Forms.View.Details;
            this.listViewWindows.CopyKeyEvent += new System.EventHandler<System.EventArgs>(this.ListView_CopyKeyEvent);
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
            this.label2.Location = new System.Drawing.Point(10, 205);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(248, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Upcoming Maintenance Windows for this resource:";
            // 
            // listViewUpcomingWindows
            // 
            this.listViewUpcomingWindows.Activation = System.Windows.Forms.ItemActivation.Standard;
            this.listViewUpcomingWindows.Alignment = System.Windows.Forms.ListViewAlignment.Top;
            this.listViewUpcomingWindows.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewUpcomingWindows.AutoSort = true;
            this.listViewUpcomingWindows.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.listViewUpcomingWindows.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderUpcomingTime,
            this.columnHeaderUpcomingName});
            this.listViewUpcomingWindows.CustomNoResultsText = null;
            this.listViewUpcomingWindows.FullRowSelect = true;
            this.listViewUpcomingWindows.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Clickable;
            this.listViewUpcomingWindows.HideSelection = false;
            this.listViewUpcomingWindows.IsLoading = false;
            this.listViewUpcomingWindows.LargeImageList = null;
            this.listViewUpcomingWindows.Location = new System.Drawing.Point(13, 230);
            this.listViewUpcomingWindows.MultiSelect = true;
            this.listViewUpcomingWindows.Name = "listViewUpcomingWindows";
            this.listViewUpcomingWindows.ShowSearchBar = true;
            this.listViewUpcomingWindows.Size = new System.Drawing.Size(354, 111);
            this.listViewUpcomingWindows.SmallImageList = null;
            this.listViewUpcomingWindows.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewUpcomingWindows.StateImageList = null;
            this.listViewUpcomingWindows.TabIndex = 4;
            this.listViewUpcomingWindows.TileSize = new System.Drawing.Size(0, 0);
            this.listViewUpcomingWindows.UseCompatibleStateImageBehavior = false;
            this.listViewUpcomingWindows.View = System.Windows.Forms.View.Details;
            this.listViewUpcomingWindows.CopyKeyEvent += new System.EventHandler<System.EventArgs>(this.ListView_CopyKeyEvent);
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
            // buttonRefresh
            // 
            this.buttonRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRefresh.Location = new System.Drawing.Point(344, 4);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(24, 24);
            this.buttonRefresh.TabIndex = 9;
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.ButtonSURefresh_Click);
            // 
            // ResultMaintenanceWindowsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listViewUpcomingWindows);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listViewWindows);
            this.Name = "ResultMaintenanceWindowsControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Microsoft.ConfigurationManagement.AdminConsole.Common.SmsSearchableListView listViewWindows;
        private System.Windows.Forms.Label label2;
        private Microsoft.ConfigurationManagement.AdminConsole.Common.SmsSearchableListView listViewUpcomingWindows;
        private System.Windows.Forms.ColumnHeader columnHeaderWindows;
        private System.Windows.Forms.ColumnHeader columnHeaderCollection;
        private System.Windows.Forms.ColumnHeader columnHeaderUpcomingTime;
        private System.Windows.Forms.ColumnHeader columnHeaderUpcomingName;
        private System.Windows.Forms.Button buttonRefresh;
    }
}
