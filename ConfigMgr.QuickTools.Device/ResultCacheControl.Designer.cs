namespace ConfigMgr.QuickTools.Device
{
    partial class ResultCacheControl
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelLocation = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelCacheSize = new System.Windows.Forms.Label();
            this.labelSpaceToUse = new System.Windows.Forms.Label();
            this.labelUsedSize = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.listViewContent = new Microsoft.ConfigurationManagement.AdminConsole.Common.SmsSearchableListView();
            this.columnHeaderID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderLastUsed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderLocation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonClearCache = new System.Windows.Forms.Button();
            this.trackBarWithoutFocus1 = new ConfigMgr.QuickTools.Device.TrackBarWithoutFocus();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarWithoutFocus1)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown1.Enabled = false;
            this.numericUpDown1.Location = new System.Drawing.Point(273, 78);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(97, 20);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.NumericUpDown1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Amount of disk space to use:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Location: ";
            // 
            // labelLocation
            // 
            this.labelLocation.AutoSize = true;
            this.labelLocation.Location = new System.Drawing.Point(64, 10);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(0, 13);
            this.labelLocation.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Current cache size:";
            // 
            // labelCacheSize
            // 
            this.labelCacheSize.AutoSize = true;
            this.labelCacheSize.Location = new System.Drawing.Point(108, 35);
            this.labelCacheSize.Name = "labelCacheSize";
            this.labelCacheSize.Size = new System.Drawing.Size(0, 13);
            this.labelCacheSize.TabIndex = 7;
            // 
            // labelSpaceToUse
            // 
            this.labelSpaceToUse.AutoSize = true;
            this.labelSpaceToUse.Location = new System.Drawing.Point(154, 59);
            this.labelSpaceToUse.Name = "labelSpaceToUse";
            this.labelSpaceToUse.Size = new System.Drawing.Size(0, 13);
            this.labelSpaceToUse.TabIndex = 8;
            // 
            // labelUsedSize
            // 
            this.labelUsedSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelUsedSize.AutoSize = true;
            this.labelUsedSize.Location = new System.Drawing.Point(97, 333);
            this.labelUsedSize.Name = "labelUsedSize";
            this.labelUsedSize.Size = new System.Drawing.Size(0, 13);
            this.labelUsedSize.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 333);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Used cache size:";
            // 
            // listViewListContent
            // 
            this.listViewContent.Activation = System.Windows.Forms.ItemActivation.Standard;
            this.listViewContent.Alignment = System.Windows.Forms.ListViewAlignment.Top;
            this.listViewContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewContent.AutoSort = true;
            this.listViewContent.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.listViewContent.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderLocation,
            this.columnHeaderSize,
            this.columnHeaderLastUsed,
            this.columnHeaderID});
            this.listViewContent.ContextMenuStrip = this.contextMenuStrip1;
            this.listViewContent.CustomNoResultsText = null;
            this.listViewContent.FullRowSelect = true;
            this.listViewContent.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Clickable;
            this.listViewContent.HideSelection = false;
            this.listViewContent.IsLoading = false;
            this.listViewContent.LargeImageList = null;
            this.listViewContent.Location = new System.Drawing.Point(10, 119);
            this.listViewContent.MultiSelect = true;
            this.listViewContent.Name = "listViewListContent";
            this.listViewContent.ShowSearchBar = true;
            this.listViewContent.Size = new System.Drawing.Size(360, 203);
            this.listViewContent.SmallImageList = null;
            this.listViewContent.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewContent.StateImageList = null;
            this.listViewContent.TabIndex = 11;
            this.listViewContent.TileSize = new System.Drawing.Size(0, 0);
            this.listViewContent.UseCompatibleStateImageBehavior = false;
            this.listViewContent.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderID
            // 
            this.columnHeaderID.Text = "ID";
            this.columnHeaderID.Width = 80;
            // 
            // columnHeaderSize
            // 
            this.columnHeaderSize.Text = "Size";
            this.columnHeaderSize.Width = 80;
            // 
            // columnHeaderLastUsed
            // 
            this.columnHeaderLastUsed.Text = "Last Used";
            this.columnHeaderLastUsed.Width = 80;
            // 
            // columnHeaderLocation
            // 
            this.columnHeaderLocation.Text = "Location";
            this.columnHeaderLocation.Width = 150;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemDelete});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(108, 26);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStrip1_Opening);
            // 
            // toolStripMenuItemDelete
            // 
            this.toolStripMenuItemDelete.Name = "toolStripMenuItemDelete";
            this.toolStripMenuItemDelete.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItemDelete.Text = "Delete";
            this.toolStripMenuItemDelete.Click += new System.EventHandler(this.ToolStripMenuItemDelete_Click);
            // 
            // buttonClearCache
            // 
            this.buttonClearCache.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClearCache.Location = new System.Drawing.Point(295, 328);
            this.buttonClearCache.Name = "buttonClearCache";
            this.buttonClearCache.Size = new System.Drawing.Size(75, 23);
            this.buttonClearCache.TabIndex = 12;
            this.buttonClearCache.Text = "Clear Cache";
            this.buttonClearCache.UseVisualStyleBackColor = true;
            this.buttonClearCache.Click += new System.EventHandler(this.ButtonClearCache_Click);
            // 
            // trackBarWithoutFocus1
            // 
            this.trackBarWithoutFocus1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarWithoutFocus1.Enabled = false;
            this.trackBarWithoutFocus1.Location = new System.Drawing.Point(10, 78);
            this.trackBarWithoutFocus1.Name = "trackBarWithoutFocus1";
            this.trackBarWithoutFocus1.Size = new System.Drawing.Size(257, 45);
            this.trackBarWithoutFocus1.TabIndex = 2;
            this.trackBarWithoutFocus1.ValueChanged += new System.EventHandler(this.TrackBar1_ValueChanged);
            // 
            // ResultCacheControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonClearCache);
            this.Controls.Add(this.listViewContent);
            this.Controls.Add(this.labelUsedSize);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.labelSpaceToUse);
            this.Controls.Add(this.labelCacheSize);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelLocation);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackBarWithoutFocus1);
            this.Controls.Add(this.numericUpDown1);
            this.Name = "ResultCacheControl";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarWithoutFocus1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private TrackBarWithoutFocus trackBarWithoutFocus1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelLocation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelCacheSize;
        private System.Windows.Forms.Label labelSpaceToUse;
        private System.Windows.Forms.Label labelUsedSize;
        private System.Windows.Forms.Label label5;
        private Microsoft.ConfigurationManagement.AdminConsole.Common.SmsSearchableListView listViewContent;
        private System.Windows.Forms.Button buttonClearCache;
        private System.Windows.Forms.ColumnHeader columnHeaderID;
        private System.Windows.Forms.ColumnHeader columnHeaderSize;
        private System.Windows.Forms.ColumnHeader columnHeaderLastUsed;
        private System.Windows.Forms.ColumnHeader columnHeaderLocation;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDelete;
    }
}