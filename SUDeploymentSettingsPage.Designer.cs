namespace Zetta.ConfigMgr.QuickTools
{
    partial class SUDeploymentSettingsPage
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
            this.dataGridViewSettings = new System.Windows.Forms.DataGridView();
            this.columnEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnCollectionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.columnNotification = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.columnSoftwareInstallation = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnAllowRestart = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnSuppressRestart = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnStartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnEnforcementDeadline = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelDescription = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSettings)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewSettings
            // 
            this.dataGridViewSettings.AllowUserToAddRows = false;
            this.dataGridViewSettings.AllowUserToDeleteRows = false;
            this.dataGridViewSettings.AllowUserToResizeRows = false;
            this.dataGridViewSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSettings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnEnabled,
            this.columnCollectionName,
            this.columnType,
            this.columnNotification,
            this.columnSoftwareInstallation,
            this.columnAllowRestart,
            this.columnSuppressRestart,
            this.columnStartTime,
            this.columnEnforcementDeadline});
            this.dataGridViewSettings.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewSettings.Location = new System.Drawing.Point(13, 16);
            this.dataGridViewSettings.Name = "dataGridViewSettings";
            this.dataGridViewSettings.RowHeadersVisible = false;
            this.dataGridViewSettings.RowTemplate.Height = 30;
            this.dataGridViewSettings.Size = new System.Drawing.Size(563, 300);
            this.dataGridViewSettings.TabIndex = 0;
            this.dataGridViewSettings.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewSettings_CellValueChanged);
            // 
            // columnEnabled
            // 
            this.columnEnabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnEnabled.HeaderText = "Enabled";
            this.columnEnabled.Name = "columnEnabled";
            this.columnEnabled.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.columnEnabled.Width = 52;
            // 
            // columnCollectionName
            // 
            this.columnCollectionName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnCollectionName.HeaderText = "Collection";
            this.columnCollectionName.Name = "columnCollectionName";
            this.columnCollectionName.ReadOnly = true;
            this.columnCollectionName.Width = 78;
            // 
            // columnType
            // 
            this.columnType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnType.HeaderText = "Type";
            this.columnType.Items.AddRange(new object[] {
            "Available",
            "Required"});
            this.columnType.Name = "columnType";
            this.columnType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnType.Width = 37;
            // 
            // columnNotification
            // 
            this.columnNotification.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnNotification.HeaderText = "Notification";
            this.columnNotification.Items.AddRange(new object[] {
            "Hide all notifications",
            "Show only restart notification",
            "Show all notifactions"});
            this.columnNotification.Name = "columnNotification";
            this.columnNotification.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnNotification.Width = 66;
            // 
            // columnSoftwareInstallation
            // 
            this.columnSoftwareInstallation.HeaderText = "Force Installation";
            this.columnSoftwareInstallation.Name = "columnSoftwareInstallation";
            this.columnSoftwareInstallation.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnSoftwareInstallation.Width = 60;
            // 
            // columnAllowRestart
            // 
            this.columnAllowRestart.HeaderText = "Force Restart";
            this.columnAllowRestart.Name = "columnAllowRestart";
            this.columnAllowRestart.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnAllowRestart.Width = 60;
            // 
            // columnSuppressRestart
            // 
            this.columnSuppressRestart.HeaderText = "Suppress Restart";
            this.columnSuppressRestart.Name = "columnSuppressRestart";
            this.columnSuppressRestart.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnSuppressRestart.Width = 60;
            // 
            // columnStartTime
            // 
            this.columnStartTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnStartTime.HeaderText = "Available Time";
            this.columnStartTime.Name = "columnStartTime";
            this.columnStartTime.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnStartTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.columnStartTime.Width = 74;
            // 
            // columnEnforcementDeadline
            // 
            this.columnEnforcementDeadline.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnEnforcementDeadline.HeaderText = "Deadline Time";
            this.columnEnforcementDeadline.Name = "columnEnforcementDeadline";
            this.columnEnforcementDeadline.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnEnforcementDeadline.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.columnEnforcementDeadline.Width = 73;
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(10, 0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(105, 13);
            this.labelDescription.TabIndex = 5;
            this.labelDescription.Text = "Deployment settings:";
            // 
            // SUDeploymentSettingsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.dataGridViewSettings);
            this.MinimumSize = new System.Drawing.Size(600, 360);
            this.Name = "SUDeploymentSettingsPage";
            this.Size = new System.Drawing.Size(600, 360);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSettings)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewSettings;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnCollectionName;
        private System.Windows.Forms.DataGridViewComboBoxColumn columnType;
        private System.Windows.Forms.DataGridViewComboBoxColumn columnNotification;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnSoftwareInstallation;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnAllowRestart;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnSuppressRestart;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnStartTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnEnforcementDeadline;
    }
}
