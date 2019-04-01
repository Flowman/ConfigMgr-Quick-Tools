namespace ConfigMgr.QuickTools.Device
{
    partial class ProvisioningModeDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientActionsDialog));
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.buttonOK = new System.Windows.Forms.Button();
            this.listViewHosts = new Microsoft.ConfigurationManagement.AdminConsole.Common.SmsSearchableListView();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.labelCompleted = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelOther = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelTotal = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 209);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(268, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // buttonOK
            // 
            this.buttonOK.Enabled = false;
            this.buttonOK.Location = new System.Drawing.Point(205, 262);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Enabled = false;
            this.buttonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // listViewHosts
            // 
            this.listViewHosts.Activation = System.Windows.Forms.ItemActivation.Standard;
            this.listViewHosts.Alignment = System.Windows.Forms.ListViewAlignment.Top;
            this.listViewHosts.AutoSort = true;
            this.listViewHosts.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.listViewHosts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderStatus});
            this.listViewHosts.CustomNoResultsText = null;
            this.listViewHosts.FullRowSelect = true;
            this.listViewHosts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Clickable;
            this.listViewHosts.HideSelection = false;
            this.listViewHosts.IsLoading = false;
            this.listViewHosts.LargeImageList = null;
            this.listViewHosts.Location = new System.Drawing.Point(12, 12);
            this.listViewHosts.Name = "listViewHosts";
            this.listViewHosts.ShowSearchBar = true;
            this.listViewHosts.Size = new System.Drawing.Size(268, 191);
            this.listViewHosts.SmallImageList = null;
            this.listViewHosts.Sorting = System.Windows.Forms.SortOrder.None;
            this.listViewHosts.StateImageList = null;
            this.listViewHosts.TabIndex = 2;
            this.listViewHosts.TileSize = new System.Drawing.Size(0, 0);
            this.listViewHosts.UseCompatibleStateImageBehavior = false;
            this.listViewHosts.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            this.columnHeaderName.Width = 150;
            // 
            // columnHeaderStatus
            // 
            this.columnHeaderStatus.Text = "Status";
            this.columnHeaderStatus.Width = 90;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 239);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Completed:";
            // 
            // labelCompleted
            // 
            this.labelCompleted.AutoSize = true;
            this.labelCompleted.Location = new System.Drawing.Point(72, 239);
            this.labelCompleted.Name = "labelCompleted";
            this.labelCompleted.Size = new System.Drawing.Size(0, 13);
            this.labelCompleted.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(120, 239);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Other:";
            // 
            // labelOther
            // 
            this.labelOther.AutoSize = true;
            this.labelOther.Location = new System.Drawing.Point(155, 239);
            this.labelOther.Name = "labelOther";
            this.labelOther.Size = new System.Drawing.Size(0, 13);
            this.labelOther.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(202, 239);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Total:";
            // 
            // labelTotal
            // 
            this.labelTotal.AutoSize = true;
            this.labelTotal.Location = new System.Drawing.Point(235, 239);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(0, 13);
            this.labelTotal.TabIndex = 8;
            // 
            // ClientActionsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 295);
            this.Controls.Add(this.labelTotal);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelOther);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelCompleted);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listViewHosts);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.progressBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ClientActionsDialog";
            this.Shown += new System.EventHandler(this.ClientActionsDialog_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.ColumnHeader columnHeaderStatus;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private Microsoft.ConfigurationManagement.AdminConsole.Common.SmsSearchableListView listViewHosts;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelCompleted;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelOther;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelTotal;
    }
}
