namespace Zetta.ConfigMgr.QuickTools
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
            this.listViewListClientHealth = new Microsoft.ConfigurationManagement.AdminConsole.Common.SmsSearchableListView();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.columnHeaderDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            this.listViewListClientHealth.Activation = System.Windows.Forms.ItemActivation.Standard;
            this.listViewListClientHealth.Alignment = System.Windows.Forms.ListViewAlignment.Top;
            this.listViewListClientHealth.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewListClientHealth.AutoSort = true;
            this.listViewListClientHealth.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.listViewListClientHealth.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderDescription,
            this.columnHeaderStatus});
            this.listViewListClientHealth.CustomNoResultsText = null;
            this.listViewListClientHealth.FullRowSelect = true;
            this.listViewListClientHealth.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Clickable;
            this.listViewListClientHealth.HideSelection = false;
            this.listViewListClientHealth.IsLoading = false;
            this.listViewListClientHealth.LargeImageList = null;
            this.listViewListClientHealth.Location = new System.Drawing.Point(14, 34);
            this.listViewListClientHealth.Name = "listViewListClientHealth";
            this.listViewListClientHealth.ShowSearchBar = true;
            this.listViewListClientHealth.Size = new System.Drawing.Size(354, 271);
            this.listViewListClientHealth.SmallImageList = null;
            this.listViewListClientHealth.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewListClientHealth.StateImageList = null;
            this.listViewListClientHealth.TabIndex = 9;
            this.listViewListClientHealth.TileSize = new System.Drawing.Size(0, 0);
            this.listViewListClientHealth.UseCompatibleStateImageBehavior = false;
            this.listViewListClientHealth.View = System.Windows.Forms.View.Details;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRefresh.Location = new System.Drawing.Point(344, 4);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(24, 24);
            this.buttonRefresh.TabIndex = 8;
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonSURefresh_Click);
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
            this.buttonEval.Click += new System.EventHandler(this.buttonEval_Click);
            // 
            // ResultClientHealthControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonEval);
            this.Controls.Add(this.labelLastScan);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listViewListClientHealth);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.label1);
            this.Name = "ResultClientHealthControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Microsoft.ConfigurationManagement.AdminConsole.Common.SmsSearchableListView listViewListClientHealth;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.ColumnHeader columnHeaderStatus;
        private System.Windows.Forms.ColumnHeader columnHeaderDescription;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelLastScan;
        private System.Windows.Forms.Button buttonEval;
    }
}
