namespace ConfigMgr.QuickTools.Device.PropertiesDialog
{
    partial class ResultClientHealthControl
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
            this.listViewClientHealth = new Microsoft.ConfigurationManagement.AdminConsole.Common.SmsSearchableListView();
            this.columnHeaderDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.labelLastScan = new System.Windows.Forms.Label();
            this.buttonEval = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Client Health for this resource:";
            // 
            // listViewListClientHealth
            // 
            this.listViewClientHealth.Activation = System.Windows.Forms.ItemActivation.Standard;
            this.listViewClientHealth.Alignment = System.Windows.Forms.ListViewAlignment.Top;
            this.listViewClientHealth.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewClientHealth.AutoSort = true;
            this.listViewClientHealth.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.listViewClientHealth.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderDescription,
            this.columnHeaderStatus});
            this.listViewClientHealth.CustomNoResultsText = null;
            this.listViewClientHealth.FullRowSelect = true;
            this.listViewClientHealth.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Clickable;
            this.listViewClientHealth.HideSelection = false;
            this.listViewClientHealth.IsLoading = false;
            this.listViewClientHealth.LargeImageList = null;
            this.listViewClientHealth.Location = new System.Drawing.Point(14, 34);
            this.listViewClientHealth.MultiSelect = true;
            this.listViewClientHealth.Name = "listViewListClientHealth";
            this.listViewClientHealth.ShowSearchBar = true;
            this.listViewClientHealth.Size = new System.Drawing.Size(354, 271);
            this.listViewClientHealth.SmallImageList = null;
            this.listViewClientHealth.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewClientHealth.StateImageList = null;
            this.listViewClientHealth.TabIndex = 9;
            this.listViewClientHealth.TileSize = new System.Drawing.Size(0, 0);
            this.listViewClientHealth.UseCompatibleStateImageBehavior = false;
            this.listViewClientHealth.View = System.Windows.Forms.View.Details;
            this.listViewClientHealth.CopyKeyEvent += new System.EventHandler<System.EventArgs>(this.ListView_CopyKeyEvent);
            // 
            // columnHeaderDescription
            // 
            this.columnHeaderDescription.Text = "Description";
            this.columnHeaderDescription.Width = 220;
            // 
            // columnHeaderStatus
            // 
            this.columnHeaderStatus.Text = "Status";
            this.columnHeaderStatus.Width = 120;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRefresh.Location = new System.Drawing.Point(344, 4);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(24, 24);
            this.buttonRefresh.TabIndex = 8;
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.ButtonSURefresh_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 322);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Last Evaluation:";
            // 
            // labelLastScan
            // 
            this.labelLastScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelLastScan.AutoSize = true;
            this.labelLastScan.Location = new System.Drawing.Point(99, 322);
            this.labelLastScan.Name = "labelLastScan";
            this.labelLastScan.Size = new System.Drawing.Size(0, 13);
            this.labelLastScan.TabIndex = 11;
            // 
            // buttonEval
            // 
            this.buttonEval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEval.Location = new System.Drawing.Point(314, 4);
            this.buttonEval.Name = "buttonEval";
            this.buttonEval.Size = new System.Drawing.Size(24, 24);
            this.buttonEval.TabIndex = 12;
            this.buttonEval.UseVisualStyleBackColor = true;
            this.buttonEval.Click += new System.EventHandler(this.ButtonEval_Click);
            // 
            // ResultClientHealthControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonEval);
            this.Controls.Add(this.labelLastScan);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listViewClientHealth);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.label1);
            this.Name = "ResultClientHealthControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Microsoft.ConfigurationManagement.AdminConsole.Common.SmsSearchableListView listViewClientHealth;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.ColumnHeader columnHeaderStatus;
        private System.Windows.Forms.ColumnHeader columnHeaderDescription;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelLastScan;
        private System.Windows.Forms.Button buttonEval;
    }
}
