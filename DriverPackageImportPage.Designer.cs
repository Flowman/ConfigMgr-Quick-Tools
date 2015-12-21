namespace Zetta.ConfigMgr.IntegrationKit
{
    partial class DriverPackageImportPage
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
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.buttonDeselectAll = new System.Windows.Forms.Button();
            this.labelDescription = new System.Windows.Forms.Label();
            this.panelProcessing = new System.Windows.Forms.Panel();
            this.labelProcessingObject = new System.Windows.Forms.Label();
            this.progressBarObjects = new System.Windows.Forms.ProgressBar();
            this.dataGridViewDriverPackages = new System.Windows.Forms.DataGridView();
            this.columnImport = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnPackage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelComplete = new System.Windows.Forms.Panel();
            this.panelProcessing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDriverPackages)).BeginInit();
            this.panelComplete.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Cursor = System.Windows.Forms.Cursors.Default;
            this.buttonSelectAll.Location = new System.Drawing.Point(294, 317);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(91, 23);
            this.buttonSelectAll.TabIndex = 1;
            this.buttonSelectAll.Text = "Select All";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // buttonDeselectAll
            // 
            this.buttonDeselectAll.Location = new System.Drawing.Point(391, 317);
            this.buttonDeselectAll.Name = "buttonDeselectAll";
            this.buttonDeselectAll.Size = new System.Drawing.Size(91, 23);
            this.buttonDeselectAll.TabIndex = 2;
            this.buttonDeselectAll.Text = "Deselect All";
            this.buttonDeselectAll.UseVisualStyleBackColor = true;
            this.buttonDeselectAll.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(10, 14);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(227, 13);
            this.labelDescription.TabIndex = 3;
            this.labelDescription.Text = "The following driver packages will be imported:";
            // 
            // panelProcessing
            // 
            this.panelProcessing.Controls.Add(this.labelProcessingObject);
            this.panelProcessing.Controls.Add(this.progressBarObjects);
            this.panelProcessing.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelProcessing.Location = new System.Drawing.Point(500, 0);
            this.panelProcessing.Name = "panelProcessing";
            this.panelProcessing.Size = new System.Drawing.Size(500, 360);
            this.panelProcessing.TabIndex = 4;
            // 
            // labelProcessingObject
            // 
            this.labelProcessingObject.Location = new System.Drawing.Point(19, 129);
            this.labelProcessingObject.Name = "labelProcessingObject";
            this.labelProcessingObject.Size = new System.Drawing.Size(467, 200);
            this.labelProcessingObject.TabIndex = 1;
            this.labelProcessingObject.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // progressBarObjects
            // 
            this.progressBarObjects.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarObjects.Location = new System.Drawing.Point(19, 90);
            this.progressBarObjects.Name = "progressBarObjects";
            this.progressBarObjects.Size = new System.Drawing.Size(459, 23);
            this.progressBarObjects.TabIndex = 0;
            // 
            // dataGridViewDriverPackages
            // 
            this.dataGridViewDriverPackages.AllowUserToAddRows = false;
            this.dataGridViewDriverPackages.AllowUserToDeleteRows = false;
            this.dataGridViewDriverPackages.AllowUserToOrderColumns = true;
            this.dataGridViewDriverPackages.AllowUserToResizeRows = false;
            this.dataGridViewDriverPackages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDriverPackages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnImport,
            this.columnPackage,
            this.columnStatus});
            this.dataGridViewDriverPackages.Location = new System.Drawing.Point(13, 30);
            this.dataGridViewDriverPackages.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.dataGridViewDriverPackages.Name = "dataGridViewDriverPackages";
            this.dataGridViewDriverPackages.RowHeadersVisible = false;
            this.dataGridViewDriverPackages.Size = new System.Drawing.Size(467, 284);
            this.dataGridViewDriverPackages.StandardTab = true;
            this.dataGridViewDriverPackages.TabIndex = 1;
            this.dataGridViewDriverPackages.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDriverPackages_CellValueChanged);
            this.dataGridViewDriverPackages.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridViewDriverPackages_CurrentCellDirtyStateChanged);
            this.dataGridViewDriverPackages.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridViewDriverPackages_RowsAdded);
            // 
            // columnImport
            // 
            this.columnImport.HeaderText = "Import";
            this.columnImport.MinimumWidth = 20;
            this.columnImport.Name = "columnImport";
            this.columnImport.Width = 42;
            // 
            // columnPackage
            // 
            this.columnPackage.HeaderText = "Driver Package";
            this.columnPackage.MinimumWidth = 50;
            this.columnPackage.Name = "columnPackage";
            this.columnPackage.ReadOnly = true;
            this.columnPackage.Width = 300;
            // 
            // columnStatus
            // 
            this.columnStatus.HeaderText = "Status";
            this.columnStatus.Name = "columnStatus";
            this.columnStatus.ReadOnly = true;
            // 
            // panelComplete
            // 
            this.panelComplete.Controls.Add(this.labelDescription);
            this.panelComplete.Controls.Add(this.dataGridViewDriverPackages);
            this.panelComplete.Controls.Add(this.buttonDeselectAll);
            this.panelComplete.Controls.Add(this.buttonSelectAll);
            this.panelComplete.Location = new System.Drawing.Point(0, 0);
            this.panelComplete.Name = "panelComplete";
            this.panelComplete.Size = new System.Drawing.Size(500, 360);
            this.panelComplete.TabIndex = 5;
            // 
            // DriverPackageImportPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelProcessing);
            this.Controls.Add(this.panelComplete);
            this.Name = "DriverPackageImportPage";
            this.Size = new System.Drawing.Size(1000, 360);
            this.panelProcessing.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDriverPackages)).EndInit();
            this.panelComplete.ResumeLayout(false);
            this.panelComplete.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSelectAll;
        private System.Windows.Forms.Button buttonDeselectAll;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Panel panelProcessing;
        private System.Windows.Forms.DataGridView dataGridViewDriverPackages;
        private System.Windows.Forms.Panel panelComplete;
        private System.Windows.Forms.ProgressBar progressBarObjects;
        private System.Windows.Forms.Label labelProcessingObject;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnImport;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnPackage;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnStatus;
    }
}
