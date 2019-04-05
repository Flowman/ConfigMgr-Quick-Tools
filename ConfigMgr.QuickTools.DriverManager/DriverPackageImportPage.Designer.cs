namespace ConfigMgr.QuickTools.DriverManager
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
            this.labelProcessingObject = new System.Windows.Forms.Label();
            this.progressBarObjects = new System.Windows.Forms.ProgressBar();
            this.panelProcessing = new System.Windows.Forms.Panel();
            this.panelComplete = new System.Windows.Forms.Panel();
            this.labelDescription = new System.Windows.Forms.Label();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.buttonDeselectAll = new System.Windows.Forms.Button();
            this.dataGridViewDriverPackages = new System.Windows.Forms.DataGridView();
            this.columnImport = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnPackage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelProcessing.SuspendLayout();
            this.panelComplete.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDriverPackages)).BeginInit();
            this.SuspendLayout();
            // 
            // labelProcessingObject
            // 
            this.labelProcessingObject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelProcessingObject.Location = new System.Drawing.Point(22, 95);
            this.labelProcessingObject.Name = "labelProcessingObject";
            this.labelProcessingObject.Size = new System.Drawing.Size(441, 170);
            this.labelProcessingObject.TabIndex = 1;
            this.labelProcessingObject.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // progressBarObjects
            // 
            this.progressBarObjects.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarObjects.Location = new System.Drawing.Point(25, 33);
            this.progressBarObjects.Name = "progressBarObjects";
            this.progressBarObjects.Size = new System.Drawing.Size(438, 23);
            this.progressBarObjects.TabIndex = 0;
            // 
            // panelProcessing
            // 
            this.panelProcessing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panelProcessing.Controls.Add(this.labelProcessingObject);
            this.panelProcessing.Controls.Add(this.progressBarObjects);
            this.panelProcessing.Location = new System.Drawing.Point(485, 0);
            this.panelProcessing.Name = "panelProcessing";
            this.panelProcessing.Size = new System.Drawing.Size(485, 360);
            this.panelProcessing.TabIndex = 12;
            this.panelProcessing.Visible = false;
            // 
            // panelComplete
            // 
            this.panelComplete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panelComplete.Controls.Add(this.labelDescription);
            this.panelComplete.Controls.Add(this.buttonSelectAll);
            this.panelComplete.Controls.Add(this.buttonDeselectAll);
            this.panelComplete.Controls.Add(this.dataGridViewDriverPackages);
            this.panelComplete.Location = new System.Drawing.Point(0, 0);
            this.panelComplete.Name = "panelComplete";
            this.panelComplete.Size = new System.Drawing.Size(485, 360);
            this.panelComplete.TabIndex = 13;
            this.panelComplete.Visible = false;
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(3, 4);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(227, 13);
            this.labelDescription.TabIndex = 17;
            this.labelDescription.Text = "The following driver packages will be imported:";
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelectAll.Cursor = System.Windows.Forms.Cursors.Default;
            this.buttonSelectAll.Location = new System.Drawing.Point(370, 323);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(91, 23);
            this.buttonSelectAll.TabIndex = 3;
            this.buttonSelectAll.Text = "Select All";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.ButtonSelect_Click);
            // 
            // buttonDeselectAll
            // 
            this.buttonDeselectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDeselectAll.Location = new System.Drawing.Point(273, 323);
            this.buttonDeselectAll.Name = "buttonDeselectAll";
            this.buttonDeselectAll.Size = new System.Drawing.Size(91, 23);
            this.buttonDeselectAll.TabIndex = 4;
            this.buttonDeselectAll.Text = "Deselect All";
            this.buttonDeselectAll.UseVisualStyleBackColor = true;
            this.buttonDeselectAll.Click += new System.EventHandler(this.ButtonSelect_Click);
            // 
            // dataGridViewDriverPackages
            // 
            this.dataGridViewDriverPackages.AllowUserToAddRows = false;
            this.dataGridViewDriverPackages.AllowUserToDeleteRows = false;
            this.dataGridViewDriverPackages.AllowUserToOrderColumns = true;
            this.dataGridViewDriverPackages.AllowUserToResizeRows = false;
            this.dataGridViewDriverPackages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewDriverPackages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDriverPackages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnImport,
            this.columnPackage,
            this.columnStatus});
            this.dataGridViewDriverPackages.Location = new System.Drawing.Point(6, 20);
            this.dataGridViewDriverPackages.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.dataGridViewDriverPackages.Name = "dataGridViewDriverPackages";
            this.dataGridViewDriverPackages.RowHeadersVisible = false;
            this.dataGridViewDriverPackages.Size = new System.Drawing.Size(455, 300);
            this.dataGridViewDriverPackages.StandardTab = true;
            this.dataGridViewDriverPackages.TabIndex = 7;
            this.dataGridViewDriverPackages.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewDrivers_CellValueChanged);
            this.dataGridViewDriverPackages.CurrentCellDirtyStateChanged += new System.EventHandler(this.DataGridViewDrivers_CurrentCellDirtyStateChanged);
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
            // DriverPackageImportPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelComplete);
            this.Controls.Add(this.panelProcessing);
            this.Name = "DriverPackageImportPage";
            this.Size = new System.Drawing.Size(970, 360);
            this.panelProcessing.ResumeLayout(false);
            this.panelComplete.ResumeLayout(false);
            this.panelComplete.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDriverPackages)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelProcessingObject;
        private System.Windows.Forms.ProgressBar progressBarObjects;
        private System.Windows.Forms.Panel panelProcessing;
        private System.Windows.Forms.Panel panelComplete;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Button buttonSelectAll;
        private System.Windows.Forms.Button buttonDeselectAll;
        private System.Windows.Forms.DataGridView dataGridViewDriverPackages;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnImport;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnPackage;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnStatus;
    }
}
