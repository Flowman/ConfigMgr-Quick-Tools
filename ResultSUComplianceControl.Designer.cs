﻿namespace Zetta.ConfigMgr.QuickTools
{
    partial class ResultSUComplianceControl
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
            this.listViewListSoftwareUpdates = new Microsoft.ConfigurationManagement.AdminConsole.Common.SmsSearchableListView();
            this.buttonSURefresh = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelCABVersion = new System.Windows.Forms.Label();
            this.labelCABSource = new System.Windows.Forms.Label();
            this.labelLastScan = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.columnHeaderAssignment = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderCompliance = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
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
            this.columnHeaderAssignment,
            this.columnHeaderCompliance});
            this.listViewListSoftwareUpdates.CustomNoResultsText = null;
            this.listViewListSoftwareUpdates.FullRowSelect = true;
            this.listViewListSoftwareUpdates.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Clickable;
            this.listViewListSoftwareUpdates.HideSelection = false;
            this.listViewListSoftwareUpdates.IsLoading = false;
            this.listViewListSoftwareUpdates.LargeImageList = null;
            this.listViewListSoftwareUpdates.Location = new System.Drawing.Point(14, 34);
            this.listViewListSoftwareUpdates.Name = "listViewListSoftwareUpdates";
            this.listViewListSoftwareUpdates.ShowSearchBar = true;
            this.listViewListSoftwareUpdates.Size = new System.Drawing.Size(354, 238);
            this.listViewListSoftwareUpdates.SmallImageList = null;
            this.listViewListSoftwareUpdates.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewListSoftwareUpdates.StateImageList = null;
            this.listViewListSoftwareUpdates.TabIndex = 7;
            this.listViewListSoftwareUpdates.TileSize = new System.Drawing.Size(0, 0);
            this.listViewListSoftwareUpdates.UseCompatibleStateImageBehavior = false;
            this.listViewListSoftwareUpdates.View = System.Windows.Forms.View.Details;
            // 
            // buttonSURefresh
            // 
            this.buttonSURefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSURefresh.Location = new System.Drawing.Point(344, 4);
            this.buttonSURefresh.Name = "buttonSURefresh";
            this.buttonSURefresh.Size = new System.Drawing.Size(24, 24);
            this.buttonSURefresh.TabIndex = 6;
            this.buttonSURefresh.UseVisualStyleBackColor = true;
            this.buttonSURefresh.Click += new System.EventHandler(this.buttonSURefresh_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(263, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Software Update Groups Compliance for this resource:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 303);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "WSUS CAB Version:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 284);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "WSUS Scan CAB Source:";
            // 
            // labelCABVersion
            // 
            this.labelCABVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCABVersion.AutoSize = true;
            this.labelCABVersion.Location = new System.Drawing.Point(149, 303);
            this.labelCABVersion.Name = "labelCABVersion";
            this.labelCABVersion.Size = new System.Drawing.Size(0, 13);
            this.labelCABVersion.TabIndex = 12;
            // 
            // labelCABSource
            // 
            this.labelCABSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCABSource.AutoSize = true;
            this.labelCABSource.Location = new System.Drawing.Point(149, 284);
            this.labelCABSource.Name = "labelCABSource";
            this.labelCABSource.Size = new System.Drawing.Size(0, 13);
            this.labelCABSource.TabIndex = 13;
            // 
            // labelLastScan
            // 
            this.labelLastScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelLastScan.AutoSize = true;
            this.labelLastScan.Location = new System.Drawing.Point(149, 322);
            this.labelLastScan.Name = "labelLastScan";
            this.labelLastScan.Size = new System.Drawing.Size(0, 13);
            this.labelLastScan.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(59, 322);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Last Scan Time:";
            // 
            // columnHeaderAssignment
            // 
            this.columnHeaderAssignment.Text = "Assignment";
            this.columnHeaderAssignment.Width = 250;
            // 
            // columnHeaderCompliance
            // 
            this.columnHeaderCompliance.Text = "Compliance";
            this.columnHeaderCompliance.Width = 100;
            // 
            // ResultSUComplianceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelLastScan);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.labelCABSource);
            this.Controls.Add(this.labelCABVersion);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listViewListSoftwareUpdates);
            this.Controls.Add(this.buttonSURefresh);
            this.Controls.Add(this.label1);
            this.Name = "ResultSUComplianceControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.ConfigurationManagement.AdminConsole.Common.SmsSearchableListView listViewListSoftwareUpdates;
        private System.Windows.Forms.Button buttonSURefresh;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelCABVersion;
        private System.Windows.Forms.Label labelCABSource;
        private System.Windows.Forms.Label labelLastScan;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ColumnHeader columnHeaderAssignment;
        private System.Windows.Forms.ColumnHeader columnHeaderCompliance;
    }
}
