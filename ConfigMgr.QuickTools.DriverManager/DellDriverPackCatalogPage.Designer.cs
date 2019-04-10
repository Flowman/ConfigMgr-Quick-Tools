namespace ConfigMgr.QuickTools.DriverManager
{
    partial class DellDriverPackCatalogPage
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
            this.labelDescription = new System.Windows.Forms.Label();
            this.dataGridViewDriverPackages = new System.Windows.Forms.DataGridView();
            this.columnImport = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnPack = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonDeselectAll = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDriverPackages)).BeginInit();
            this.SuspendLayout();
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(3, 4);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(71, 13);
            this.labelDescription.TabIndex = 18;
            this.labelDescription.Text = "Driver Packs:";
            // 
            // dataGridViewDriverPackages
            // 
            this.dataGridViewDriverPackages.AllowUserToAddRows = false;
            this.dataGridViewDriverPackages.AllowUserToDeleteRows = false;
            this.dataGridViewDriverPackages.AllowUserToOrderColumns = true;
            this.dataGridViewDriverPackages.AllowUserToResizeRows = false;
            this.dataGridViewDriverPackages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewDriverPackages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDriverPackages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnImport,
            this.columnPack,
            this.ColumnVersion,
            this.columnStatus});
            this.dataGridViewDriverPackages.Location = new System.Drawing.Point(6, 20);
            this.dataGridViewDriverPackages.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.dataGridViewDriverPackages.Name = "dataGridViewDriverPackages";
            this.dataGridViewDriverPackages.RowHeadersVisible = false;
            this.dataGridViewDriverPackages.Size = new System.Drawing.Size(455, 300);
            this.dataGridViewDriverPackages.StandardTab = true;
            this.dataGridViewDriverPackages.TabIndex = 8;
            this.dataGridViewDriverPackages.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewDriverPackages_CellValueChanged);
            this.dataGridViewDriverPackages.CurrentCellDirtyStateChanged += new System.EventHandler(this.DataGridViewDriverPackages_CurrentCellDirtyStateChanged);
            this.dataGridViewDriverPackages.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.DataGridViewDriverPackages_RowsAdded);
            // 
            // columnImport
            // 
            this.columnImport.HeaderText = "Import";
            this.columnImport.MinimumWidth = 20;
            this.columnImport.Name = "columnImport";
            this.columnImport.Width = 50;
            // 
            // columnPack
            // 
            this.columnPack.HeaderText = "Driver Pack";
            this.columnPack.MinimumWidth = 50;
            this.columnPack.Name = "columnPack";
            this.columnPack.ReadOnly = true;
            this.columnPack.Width = 250;
            // 
            // ColumnVersion
            // 
            this.ColumnVersion.HeaderText = "Version";
            this.ColumnVersion.Name = "ColumnVersion";
            this.ColumnVersion.Width = 50;
            // 
            // columnStatus
            // 
            this.columnStatus.HeaderText = "Status";
            this.columnStatus.Name = "columnStatus";
            this.columnStatus.ReadOnly = true;
            // 
            // buttonDeselectAll
            // 
            this.buttonDeselectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDeselectAll.Location = new System.Drawing.Point(370, 323);
            this.buttonDeselectAll.Name = "buttonDeselectAll";
            this.buttonDeselectAll.Size = new System.Drawing.Size(91, 23);
            this.buttonDeselectAll.TabIndex = 4;
            this.buttonDeselectAll.Text = "Deselect All";
            this.buttonDeselectAll.UseVisualStyleBackColor = true;
            this.buttonDeselectAll.Click += new System.EventHandler(this.ButtonDeselectAll_Click);
            // 
            // DellDriverPackCatalogPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.dataGridViewDriverPackages);
            this.Controls.Add(this.buttonDeselectAll);
            this.Name = "DellDriverPackCatalogPage";
            this.Size = new System.Drawing.Size(485, 360);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDriverPackages)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridViewDriverPackages;
        private System.Windows.Forms.Button buttonDeselectAll;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnImport;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnPack;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnStatus;
    }
}
