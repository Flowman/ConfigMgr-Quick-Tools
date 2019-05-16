namespace ConfigMgr.QuickTools.CollectionManagment
{
    partial class AddResourcesListDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddResourcesListDialog));
            this.textBoxSeachList = new System.Windows.Forms.TextBox();
            this.listViewSelectedResources = new Microsoft.ConfigurationManagement.AdminConsole.Common.SmsListView();
            this.columnMachineName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDomain = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSite = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.smsListView1 = new Microsoft.ConfigurationManagement.AdminConsole.Common.SmsListView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxSeachList
            // 
            this.textBoxSeachList.Location = new System.Drawing.Point(12, 25);
            this.textBoxSeachList.Multiline = true;
            this.textBoxSeachList.Name = "textBoxSeachList";
            this.textBoxSeachList.Size = new System.Drawing.Size(355, 185);
            this.textBoxSeachList.TabIndex = 1;
            // 
            // listViewSelectedResources
            // 
            this.listViewSelectedResources.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.listViewSelectedResources.AutoSort = true;
            this.listViewSelectedResources.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnMachineName,
            this.columnDomain,
            this.columnSite});
            this.listViewSelectedResources.CustomNoResultsText = null;
            this.listViewSelectedResources.FullRowSelect = true;
            this.listViewSelectedResources.HideSelection = false;
            this.listViewSelectedResources.IsLoading = false;
            this.listViewSelectedResources.Location = new System.Drawing.Point(12, 241);
            this.listViewSelectedResources.Name = "listViewSelectedResources";
            this.listViewSelectedResources.ShowItemToolTips = true;
            this.listViewSelectedResources.Size = new System.Drawing.Size(415, 146);
            this.listViewSelectedResources.TabIndex = 1;
            this.listViewSelectedResources.UseCompatibleStateImageBehavior = false;
            this.listViewSelectedResources.View = System.Windows.Forms.View.Details;
            this.listViewSelectedResources.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListViewSelectedResources_MouseDoubleClick);
            // 
            // columnMachineName
            // 
            this.columnMachineName.Text = "Name";
            this.columnMachineName.Width = 150;
            // 
            // columnDomain
            // 
            this.columnDomain.Text = "Domain";
            this.columnDomain.Width = 100;
            // 
            // columnSite
            // 
            this.columnSite.Text = "Site Code";
            // 
            // smsListView1
            // 
            this.smsListView1.AutoSort = true;
            this.smsListView1.CustomNoResultsText = null;
            this.smsListView1.IsLoading = false;
            this.smsListView1.Location = new System.Drawing.Point(12, 386);
            this.smsListView1.Name = "smsListView1";
            this.smsListView1.Size = new System.Drawing.Size(415, 85);
            this.smsListView1.TabIndex = 1;
            this.smsListView1.UseCompatibleStateImageBehavior = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 43);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(415, 209);
            this.textBox1.TabIndex = 0;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(372, 393);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(291, 393);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(372, 54);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 4;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.ButtonClear_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 225);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Selected resources:";
            // 
            // buttonSearch
            // 
            this.buttonSearch.Location = new System.Drawing.Point(372, 25);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(75, 23);
            this.buttonSearch.TabIndex = 2;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.ButtonSearch_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "List of resources to search for:";
            // 
            // AddResourcesListDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 428);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.listViewSelectedResources);
            this.Controls.Add(this.textBoxSeachList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddResourcesListDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxSeachList;
        private Microsoft.ConfigurationManagement.AdminConsole.Common.SmsListView listViewSelectedResources;
        private Microsoft.ConfigurationManagement.AdminConsole.Common.SmsListView smsListView1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColumnHeader columnMachineName;
        private System.Windows.Forms.ColumnHeader columnDomain;
        private System.Windows.Forms.ColumnHeader columnSite;
    }
}
