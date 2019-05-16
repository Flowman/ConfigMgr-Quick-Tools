namespace ConfigMgr.QuickTools.Warranty
{
    partial class ResultDellWarrantyControl
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
            this.listViewListWarranty = new Microsoft.ConfigurationManagement.AdminConsole.Common.SmsSearchableListView();
            this.columnHeaderDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderStartDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderEndDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonSURefresh = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.labelServiceTag = new System.Windows.Forms.Label();
            this.labelModel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelHttpResponse = new System.Windows.Forms.Label();
            this.buttonOptions = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.labelShipDate = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listViewListWarranty
            // 
            this.listViewListWarranty.Activation = System.Windows.Forms.ItemActivation.Standard;
            this.listViewListWarranty.Alignment = System.Windows.Forms.ListViewAlignment.Top;
            this.listViewListWarranty.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewListWarranty.AutoSort = true;
            this.listViewListWarranty.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.listViewListWarranty.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderDescription,
            this.columnHeaderType,
            this.columnHeaderStartDate,
            this.columnHeaderEndDate});
            this.listViewListWarranty.CustomNoResultsText = null;
            this.listViewListWarranty.FullRowSelect = true;
            this.listViewListWarranty.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Clickable;
            this.listViewListWarranty.HideSelection = false;
            this.listViewListWarranty.IsLoading = false;
            this.listViewListWarranty.LargeImageList = null;
            this.listViewListWarranty.Location = new System.Drawing.Point(14, 34);
            this.listViewListWarranty.MultiSelect = true;
            this.listViewListWarranty.Name = "listViewListWarranty";
            this.listViewListWarranty.ShowSearchBar = true;
            this.listViewListWarranty.Size = new System.Drawing.Size(354, 237);
            this.listViewListWarranty.SmallImageList = null;
            this.listViewListWarranty.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewListWarranty.StateImageList = null;
            this.listViewListWarranty.TabIndex = 7;
            this.listViewListWarranty.TileSize = new System.Drawing.Size(0, 0);
            this.listViewListWarranty.UseCompatibleStateImageBehavior = false;
            this.listViewListWarranty.View = System.Windows.Forms.View.Details;
            this.listViewListWarranty.CopyKeyEvent += new System.EventHandler<System.EventArgs>(this.ListViewListSoftwareUpdates_CopyKeyEvent);
            // 
            // columnHeaderDescription
            // 
            this.columnHeaderDescription.Text = "Description";
            this.columnHeaderDescription.Width = 100;
            // 
            // columnHeaderType
            // 
            this.columnHeaderType.Text = "Type";
            // 
            // columnHeaderStartDate
            // 
            this.columnHeaderStartDate.Text = "Start Date";
            this.columnHeaderStartDate.Width = 70;
            // 
            // columnHeaderEndDate
            // 
            this.columnHeaderEndDate.Text = "End Date";
            this.columnHeaderEndDate.Width = 70;
            // 
            // buttonSURefresh
            // 
            this.buttonSURefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSURefresh.Location = new System.Drawing.Point(344, 4);
            this.buttonSURefresh.Name = "buttonSURefresh";
            this.buttonSURefresh.Size = new System.Drawing.Size(24, 24);
            this.buttonSURefresh.TabIndex = 6;
            this.buttonSURefresh.UseVisualStyleBackColor = true;
            this.buttonSURefresh.Click += new System.EventHandler(this.ButtonSURefresh_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 277);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Service Tag:";
            // 
            // labelServiceTag
            // 
            this.labelServiceTag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelServiceTag.AutoSize = true;
            this.labelServiceTag.Location = new System.Drawing.Point(111, 277);
            this.labelServiceTag.Name = "labelServiceTag";
            this.labelServiceTag.Size = new System.Drawing.Size(0, 13);
            this.labelServiceTag.TabIndex = 12;
            // 
            // labelModel
            // 
            this.labelModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelModel.AutoSize = true;
            this.labelModel.Location = new System.Drawing.Point(111, 296);
            this.labelModel.Name = "labelModel";
            this.labelModel.Size = new System.Drawing.Size(0, 13);
            this.labelModel.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(66, 296);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Model:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 334);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Http Response:";
            // 
            // labelHttpResponse
            // 
            this.labelHttpResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelHttpResponse.AutoSize = true;
            this.labelHttpResponse.Location = new System.Drawing.Point(111, 334);
            this.labelHttpResponse.Name = "labelHttpResponse";
            this.labelHttpResponse.Size = new System.Drawing.Size(0, 13);
            this.labelHttpResponse.TabIndex = 17;
            // 
            // buttonOptions
            // 
            this.buttonOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOptions.Location = new System.Drawing.Point(314, 4);
            this.buttonOptions.Name = "buttonOptions";
            this.buttonOptions.Size = new System.Drawing.Size(24, 24);
            this.buttonOptions.TabIndex = 18;
            this.buttonOptions.UseVisualStyleBackColor = true;
            this.buttonOptions.Click += new System.EventHandler(this.ButtonOptions_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(48, 315);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Ship Date:";
            // 
            // labelShipDate
            // 
            this.labelShipDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelShipDate.AutoSize = true;
            this.labelShipDate.Location = new System.Drawing.Point(111, 315);
            this.labelShipDate.Name = "labelShipDate";
            this.labelShipDate.Size = new System.Drawing.Size(0, 13);
            this.labelShipDate.TabIndex = 20;
            // 
            // ResultDellWarrantyControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelShipDate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonOptions);
            this.Controls.Add(this.labelHttpResponse);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelModel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.labelServiceTag);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listViewListWarranty);
            this.Controls.Add(this.buttonSURefresh);
            this.Name = "ResultDellWarrantyControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.ConfigurationManagement.AdminConsole.Common.SmsSearchableListView listViewListWarranty;
        private System.Windows.Forms.Button buttonSURefresh;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelServiceTag;
        private System.Windows.Forms.Label labelModel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ColumnHeader columnHeaderDescription;
        private System.Windows.Forms.ColumnHeader columnHeaderStartDate;
        private System.Windows.Forms.ColumnHeader columnHeaderEndDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelHttpResponse;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
        private System.Windows.Forms.Button buttonOptions;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelShipDate;
    }
}
