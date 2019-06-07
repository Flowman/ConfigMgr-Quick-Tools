namespace ConfigMgr.QuickTools.CollectionManagment
{
    partial class IncrementalCollectionsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IncrementalCollectionsPage));
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.labelDescription = new System.Windows.Forms.Label();
            this.dataGridViewCollections = new System.Windows.Forms.DataGridView();
            this.labelCount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.columnSelected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnCollection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnQuery = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnCollectionID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCollections)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 294);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(479, 42);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(3, 336);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(493, 13);
            this.linkLabel1.TabIndex = 12;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "https://docs.microsoft.com/en-us/sccm/core/clients/manage/collections/best-practi" +
    "ces-for-collections";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel1_LinkClicked);
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(3, 0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(118, 13);
            this.labelDescription.TabIndex = 14;
            this.labelDescription.Text = "Incremental collections:";
            // 
            // dataGridViewCollections
            // 
            this.dataGridViewCollections.AllowUserToAddRows = false;
            this.dataGridViewCollections.AllowUserToDeleteRows = false;
            this.dataGridViewCollections.AllowUserToOrderColumns = true;
            this.dataGridViewCollections.AllowUserToResizeRows = false;
            this.dataGridViewCollections.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewCollections.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCollections.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnSelected,
            this.columnCollection,
            this.ColumnQuery,
            this.columnCollectionID});
            this.dataGridViewCollections.Location = new System.Drawing.Point(6, 16);
            this.dataGridViewCollections.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.dataGridViewCollections.Name = "dataGridViewCollections";
            this.dataGridViewCollections.RowHeadersVisible = false;
            this.dataGridViewCollections.Size = new System.Drawing.Size(470, 253);
            this.dataGridViewCollections.StandardTab = true;
            this.dataGridViewCollections.TabIndex = 15;
            this.dataGridViewCollections.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewCollections_CellValueChanged);
            this.dataGridViewCollections.CurrentCellDirtyStateChanged += new System.EventHandler(this.DataGridViewCollections_CurrentCellDirtyStateChanged);
            this.dataGridViewCollections.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.DataGridViewCollections_RowsAdded);
            this.dataGridViewCollections.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DataGridViewCollections_KeyUp);
            // 
            // labelCount
            // 
            this.labelCount.AutoSize = true;
            this.labelCount.Location = new System.Drawing.Point(40, 273);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(0, 13);
            this.labelCount.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 273);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Total:";
            // 
            // columnDisable
            // 
            this.columnSelected.HeaderText = "Disable";
            this.columnSelected.MinimumWidth = 20;
            this.columnSelected.Name = "columnDisable";
            this.columnSelected.Width = 50;
            // 
            // columnCollection
            // 
            this.columnCollection.HeaderText = "Collection";
            this.columnCollection.MinimumWidth = 50;
            this.columnCollection.Name = "columnCollection";
            this.columnCollection.ReadOnly = true;
            this.columnCollection.Width = 255;
            // 
            // ColumnQuery
            // 
            this.ColumnQuery.HeaderText = "Query";
            this.ColumnQuery.Name = "ColumnQuery";
            this.ColumnQuery.ReadOnly = true;
            this.ColumnQuery.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnQuery.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnQuery.Width = 50;
            // 
            // columnCollectionID
            // 
            this.columnCollectionID.HeaderText = "Collection ID";
            this.columnCollectionID.Name = "columnCollectionID";
            this.columnCollectionID.ReadOnly = true;
            this.columnCollectionID.Width = 90;
            // 
            // IncrementalCollectionsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelCount);
            this.Controls.Add(this.dataGridViewCollections);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label1);
            this.Name = "IncrementalCollectionsPage";
            this.Size = new System.Drawing.Size(500, 360);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCollections)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.DataGridView dataGridViewCollections;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnCollection;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnQuery;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnCollectionID;
    }
}
