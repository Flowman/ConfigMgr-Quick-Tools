namespace ConfigMgr.QuickTools.SoftwareUpdates
{
    partial class CleanSoftwareUpdatesPage
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
            this.dataGridViewUpdates = new System.Windows.Forms.DataGridView();
            this.ColumnIcon = new System.Windows.Forms.DataGridViewImageColumn();
            this.columnRemove = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnArticleID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelDescription = new System.Windows.Forms.Label();
            this.buttonDeselectAll = new System.Windows.Forms.Button();
            this.buttonSelectSuperseded = new System.Windows.Forms.Button();
            this.buttonSelectExpired = new System.Windows.Forms.Button();
            this.checkBoxRemoveContent = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUpdates)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewUpdates
            // 
            this.dataGridViewUpdates.AllowUserToAddRows = false;
            this.dataGridViewUpdates.AllowUserToDeleteRows = false;
            this.dataGridViewUpdates.AllowUserToOrderColumns = true;
            this.dataGridViewUpdates.AllowUserToResizeRows = false;
            this.dataGridViewUpdates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewUpdates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewUpdates.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnIcon,
            this.columnRemove,
            this.columnTitle,
            this.columnArticleID});
            this.dataGridViewUpdates.Location = new System.Drawing.Point(6, 16);
            this.dataGridViewUpdates.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.dataGridViewUpdates.Name = "dataGridViewUpdates";
            this.dataGridViewUpdates.RowHeadersVisible = false;
            this.dataGridViewUpdates.Size = new System.Drawing.Size(458, 286);
            this.dataGridViewUpdates.StandardTab = true;
            this.dataGridViewUpdates.TabIndex = 17;
            this.dataGridViewUpdates.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewUpdates_CellValueChanged);
            this.dataGridViewUpdates.CurrentCellDirtyStateChanged += new System.EventHandler(this.DataGridViewUpdates_CurrentCellDirtyStateChanged);
            // 
            // ColumnIcon
            // 
            this.ColumnIcon.HeaderText = "Icon";
            this.ColumnIcon.Name = "ColumnIcon";
            this.ColumnIcon.Width = 35;
            // 
            // columnRemove
            // 
            this.columnRemove.HeaderText = "Remove";
            this.columnRemove.MinimumWidth = 20;
            this.columnRemove.Name = "columnRemove";
            this.columnRemove.Width = 55;
            // 
            // columnTitle
            // 
            this.columnTitle.HeaderText = "Title";
            this.columnTitle.MinimumWidth = 50;
            this.columnTitle.Name = "columnTitle";
            this.columnTitle.ReadOnly = true;
            // 
            // columnArticleID
            // 
            this.columnArticleID.HeaderText = "Article ID";
            this.columnArticleID.Name = "columnArticleID";
            this.columnArticleID.ReadOnly = true;
            this.columnArticleID.Width = 80;
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(3, 0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(95, 13);
            this.labelDescription.TabIndex = 16;
            this.labelDescription.Text = "Software Updates:";
            // 
            // buttonDeselectAll
            // 
            this.buttonDeselectAll.Location = new System.Drawing.Point(6, 305);
            this.buttonDeselectAll.Name = "buttonDeselectAll";
            this.buttonDeselectAll.Size = new System.Drawing.Size(91, 23);
            this.buttonDeselectAll.TabIndex = 18;
            this.buttonDeselectAll.Text = "Deselect All";
            this.buttonDeselectAll.UseVisualStyleBackColor = true;
            this.buttonDeselectAll.Click += new System.EventHandler(this.ButtonDeselectAll_Click);
            // 
            // buttonSelectSuperseded
            // 
            this.buttonSelectSuperseded.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelectSuperseded.Cursor = System.Windows.Forms.Cursors.Default;
            this.buttonSelectSuperseded.Location = new System.Drawing.Point(262, 305);
            this.buttonSelectSuperseded.Name = "buttonSelectSuperseded";
            this.buttonSelectSuperseded.Size = new System.Drawing.Size(105, 23);
            this.buttonSelectSuperseded.TabIndex = 19;
            this.buttonSelectSuperseded.Text = "Select Superseded";
            this.buttonSelectSuperseded.UseVisualStyleBackColor = true;
            this.buttonSelectSuperseded.Click += new System.EventHandler(this.ButtonSelectSuperseded_Click);
            // 
            // buttonSelectExpired
            // 
            this.buttonSelectExpired.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelectExpired.Cursor = System.Windows.Forms.Cursors.Default;
            this.buttonSelectExpired.Location = new System.Drawing.Point(373, 305);
            this.buttonSelectExpired.Name = "buttonSelectExpired";
            this.buttonSelectExpired.Size = new System.Drawing.Size(91, 23);
            this.buttonSelectExpired.TabIndex = 20;
            this.buttonSelectExpired.Text = "Select Expired";
            this.buttonSelectExpired.UseVisualStyleBackColor = true;
            this.buttonSelectExpired.Click += new System.EventHandler(this.ButtonSelectExpired_Click);
            // 
            // checkBoxRemoveContent
            // 
            this.checkBoxRemoveContent.AutoSize = true;
            this.checkBoxRemoveContent.Location = new System.Drawing.Point(7, 334);
            this.checkBoxRemoveContent.Name = "checkBoxRemoveContent";
            this.checkBoxRemoveContent.Size = new System.Drawing.Size(204, 17);
            this.checkBoxRemoveContent.TabIndex = 22;
            this.checkBoxRemoveContent.Text = "Remove content for selected updates";
            this.checkBoxRemoveContent.UseVisualStyleBackColor = true;
            // 
            // CleanSoftwareUpdatesPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxRemoveContent);
            this.Controls.Add(this.buttonSelectExpired);
            this.Controls.Add(this.buttonSelectSuperseded);
            this.Controls.Add(this.buttonDeselectAll);
            this.Controls.Add(this.dataGridViewUpdates);
            this.Controls.Add(this.labelDescription);
            this.Name = "CleanSoftwareUpdatesPage";
            this.Size = new System.Drawing.Size(485, 360);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUpdates)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewUpdates;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Button buttonDeselectAll;
        private System.Windows.Forms.Button buttonSelectSuperseded;
        private System.Windows.Forms.Button buttonSelectExpired;
        private System.Windows.Forms.CheckBox checkBoxRemoveContent;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnArticleID;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnTitle;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnRemove;
        private System.Windows.Forms.DataGridViewImageColumn ColumnIcon;
    }
}
