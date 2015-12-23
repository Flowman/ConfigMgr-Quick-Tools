namespace Zetta.ConfigMgr.IntegrationKit
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
            this.columnDisable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnCollection = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.linkLabel1.Size = new System.Drawing.Size(283, 13);
            this.linkLabel1.TabIndex = 12;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "http://technet.microsoft.com/en-us/library/gg699372.aspx";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
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
            this.dataGridViewCollections.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCollections.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnDisable,
            this.columnCollection,
            this.columnCollectionID});
            this.dataGridViewCollections.Location = new System.Drawing.Point(6, 16);
            this.dataGridViewCollections.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.dataGridViewCollections.Name = "dataGridViewCollections";
            this.dataGridViewCollections.RowHeadersVisible = false;
            this.dataGridViewCollections.Size = new System.Drawing.Size(467, 266);
            this.dataGridViewCollections.StandardTab = true;
            this.dataGridViewCollections.TabIndex = 15;
            this.dataGridViewCollections.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridViewCollections_CurrentCellDirtyStateChanged);
            // 
            // columnDisable
            // 
            this.columnDisable.HeaderText = "Disable";
            this.columnDisable.MinimumWidth = 20;
            this.columnDisable.Name = "columnDisable";
            this.columnDisable.Width = 50;
            // 
            // columnCollection
            // 
            this.columnCollection.HeaderText = "Collection";
            this.columnCollection.MinimumWidth = 50;
            this.columnCollection.Name = "columnCollection";
            this.columnCollection.ReadOnly = true;
            this.columnCollection.Width = 305;
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
            this.Controls.Add(this.dataGridViewCollections);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label1);
            this.Name = "IncrementalCollectionsPage";
            this.Size = new System.Drawing.Size(485, 360);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCollections)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.DataGridView dataGridViewCollections;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnCollectionID;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnCollection;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnDisable;
    }
}
