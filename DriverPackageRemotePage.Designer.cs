namespace Zetta.ConfigMgr.QuickTools
{
    partial class DriverPackageRemotePage
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
            this.textBoxMachine = new System.Windows.Forms.TextBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonDeselectAll = new System.Windows.Forms.Button();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.dataGridViewDrivers = new System.Windows.Forms.DataGridView();
            this.columnCapture = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnProvider = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnOEMInf = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonCapture = new System.Windows.Forms.Button();
            this.labelProcessingObject = new System.Windows.Forms.Label();
            this.progressBarObjects = new System.Windows.Forms.ProgressBar();
            this.panelProcessing = new System.Windows.Forms.Panel();
            this.panelComplete = new System.Windows.Forms.Panel();
            this.labelDestination = new System.Windows.Forms.Label();
            this.textBoxDestination = new System.Windows.Forms.TextBox();
            this.labelPackage = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.panelDone = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.labelUsername = new System.Windows.Forms.Label();
            this.labelPassword = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDrivers)).BeginInit();
            this.panelProcessing.SuspendLayout();
            this.panelComplete.SuspendLayout();
            this.panelDone.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxMachine
            // 
            this.textBoxMachine.Location = new System.Drawing.Point(102, 28);
            this.textBoxMachine.Name = "textBoxMachine";
            this.textBoxMachine.Size = new System.Drawing.Size(150, 20);
            this.textBoxMachine.TabIndex = 0;
            this.textBoxMachine.TextChanged += new System.EventHandler(this.textBoxMachine_TextChanged);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Enabled = false;
            this.buttonConnect.Location = new System.Drawing.Point(387, 26);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(99, 23);
            this.buttonConnect.TabIndex = 1;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // buttonDeselectAll
            // 
            this.buttonDeselectAll.Location = new System.Drawing.Point(298, 233);
            this.buttonDeselectAll.Name = "buttonDeselectAll";
            this.buttonDeselectAll.Size = new System.Drawing.Size(91, 23);
            this.buttonDeselectAll.TabIndex = 4;
            this.buttonDeselectAll.Text = "Deselect All";
            this.buttonDeselectAll.UseVisualStyleBackColor = true;
            this.buttonDeselectAll.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Cursor = System.Windows.Forms.Cursors.Default;
            this.buttonSelectAll.Location = new System.Drawing.Point(395, 233);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(91, 23);
            this.buttonSelectAll.TabIndex = 3;
            this.buttonSelectAll.Text = "Select All";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // dataGridViewDrivers
            // 
            this.dataGridViewDrivers.AllowUserToAddRows = false;
            this.dataGridViewDrivers.AllowUserToDeleteRows = false;
            this.dataGridViewDrivers.AllowUserToOrderColumns = true;
            this.dataGridViewDrivers.AllowUserToResizeRows = false;
            this.dataGridViewDrivers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDrivers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnCapture,
            this.columnProvider,
            this.columnDesc,
            this.columnVersion,
            this.columnOEMInf,
            this.columnClass});
            this.dataGridViewDrivers.Location = new System.Drawing.Point(13, 6);
            this.dataGridViewDrivers.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.dataGridViewDrivers.Name = "dataGridViewDrivers";
            this.dataGridViewDrivers.RowHeadersVisible = false;
            this.dataGridViewDrivers.Size = new System.Drawing.Size(473, 224);
            this.dataGridViewDrivers.StandardTab = true;
            this.dataGridViewDrivers.TabIndex = 7;
            this.dataGridViewDrivers.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDrivers_CellValueChanged);
            this.dataGridViewDrivers.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridViewDrivers_CurrentCellDirtyStateChanged);
            this.dataGridViewDrivers.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridViewDrivers_RowsAdded);
            // 
            // columnCapture
            // 
            this.columnCapture.HeaderText = "";
            this.columnCapture.MinimumWidth = 20;
            this.columnCapture.Name = "columnCapture";
            this.columnCapture.Width = 25;
            // 
            // columnProvider
            // 
            this.columnProvider.HeaderText = "Provider";
            this.columnProvider.MinimumWidth = 50;
            this.columnProvider.Name = "columnProvider";
            this.columnProvider.ReadOnly = true;
            this.columnProvider.Width = 140;
            // 
            // columnDesc
            // 
            this.columnDesc.HeaderText = "Description";
            this.columnDesc.Name = "columnDesc";
            this.columnDesc.ReadOnly = true;
            this.columnDesc.Width = 210;
            // 
            // columnVersion
            // 
            this.columnVersion.HeaderText = "Version";
            this.columnVersion.Name = "columnVersion";
            this.columnVersion.Width = 77;
            // 
            // columnOEMInf
            // 
            this.columnOEMInf.HeaderText = "OEMInf";
            this.columnOEMInf.Name = "columnOEMInf";
            this.columnOEMInf.Visible = false;
            // 
            // columnClass
            // 
            this.columnClass.HeaderText = "Class";
            this.columnClass.Name = "columnClass";
            this.columnClass.Visible = false;
            // 
            // buttonCapture
            // 
            this.buttonCapture.Enabled = false;
            this.buttonCapture.Location = new System.Drawing.Point(395, 278);
            this.buttonCapture.Name = "buttonCapture";
            this.buttonCapture.Size = new System.Drawing.Size(91, 23);
            this.buttonCapture.TabIndex = 8;
            this.buttonCapture.Text = "Capture";
            this.buttonCapture.UseVisualStyleBackColor = true;
            this.buttonCapture.Click += new System.EventHandler(this.buttonCapture_Click);
            // 
            // labelProcessingObject
            // 
            this.labelProcessingObject.Location = new System.Drawing.Point(19, 95);
            this.labelProcessingObject.Name = "labelProcessingObject";
            this.labelProcessingObject.Size = new System.Drawing.Size(467, 170);
            this.labelProcessingObject.TabIndex = 1;
            this.labelProcessingObject.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // progressBarObjects
            // 
            this.progressBarObjects.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarObjects.Location = new System.Drawing.Point(22, 33);
            this.progressBarObjects.Name = "progressBarObjects";
            this.progressBarObjects.Size = new System.Drawing.Size(459, 23);
            this.progressBarObjects.TabIndex = 0;
            // 
            // panelProcessing
            // 
            this.panelProcessing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panelProcessing.Controls.Add(this.labelProcessingObject);
            this.panelProcessing.Controls.Add(this.progressBarObjects);
            this.panelProcessing.Location = new System.Drawing.Point(500, 59);
            this.panelProcessing.Name = "panelProcessing";
            this.panelProcessing.Size = new System.Drawing.Size(500, 301);
            this.panelProcessing.TabIndex = 9;
            this.panelProcessing.Visible = false;
            // 
            // panelComplete
            // 
            this.panelComplete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panelComplete.Controls.Add(this.labelDestination);
            this.panelComplete.Controls.Add(this.textBoxDestination);
            this.panelComplete.Controls.Add(this.labelPackage);
            this.panelComplete.Controls.Add(this.buttonCapture);
            this.panelComplete.Controls.Add(this.buttonSelectAll);
            this.panelComplete.Controls.Add(this.buttonDeselectAll);
            this.panelComplete.Controls.Add(this.dataGridViewDrivers);
            this.panelComplete.Location = new System.Drawing.Point(0, 59);
            this.panelComplete.Name = "panelComplete";
            this.panelComplete.Size = new System.Drawing.Size(500, 304);
            this.panelComplete.TabIndex = 10;
            this.panelComplete.Visible = false;
            // 
            // labelDestination
            // 
            this.labelDestination.AutoSize = true;
            this.labelDestination.Location = new System.Drawing.Point(108, 264);
            this.labelDestination.Name = "labelDestination";
            this.labelDestination.Size = new System.Drawing.Size(28, 13);
            this.labelDestination.TabIndex = 12;
            this.labelDestination.Text = "       ";
            // 
            // textBoxDestination
            // 
            this.textBoxDestination.Location = new System.Drawing.Point(13, 280);
            this.textBoxDestination.Name = "textBoxDestination";
            this.textBoxDestination.Size = new System.Drawing.Size(376, 20);
            this.textBoxDestination.TabIndex = 11;
            // 
            // labelPackage
            // 
            this.labelPackage.AutoSize = true;
            this.labelPackage.Location = new System.Drawing.Point(10, 264);
            this.labelPackage.Name = "labelPackage";
            this.labelPackage.Size = new System.Drawing.Size(103, 13);
            this.labelPackage.TabIndex = 10;
            this.labelPackage.Text = "Capture Destination:";
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(10, 31);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(86, 13);
            this.labelDescription.TabIndex = 9;
            this.labelDescription.Text = "Computer Name:";
            // 
            // panelDone
            // 
            this.panelDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panelDone.Controls.Add(this.label1);
            this.panelDone.Location = new System.Drawing.Point(1000, 59);
            this.panelDone.Name = "panelDone";
            this.panelDone.Size = new System.Drawing.Size(500, 301);
            this.panelDone.TabIndex = 11;
            this.panelDone.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(412, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Capture of drivers have completed, select Next to proceed with driver package imp" +
    "ort.";
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(102, 0);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(150, 20);
            this.textBoxUsername.TabIndex = 12;
            this.textBoxUsername.Visible = false;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(336, 0);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '•';
            this.textBoxPassword.Size = new System.Drawing.Size(150, 20);
            this.textBoxPassword.TabIndex = 13;
            this.textBoxPassword.Visible = false;
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new System.Drawing.Point(10, 3);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(58, 13);
            this.labelUsername.TabIndex = 14;
            this.labelUsername.Text = "Username:";
            this.labelUsername.Visible = false;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(274, 3);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(56, 13);
            this.labelPassword.TabIndex = 15;
            this.labelPassword.Text = "Password:";
            this.labelPassword.Visible = false;
            // 
            // DriverPackageRemotePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.labelUsername);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.textBoxUsername);
            this.Controls.Add(this.panelDone);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.panelProcessing);
            this.Controls.Add(this.panelComplete);
            this.Controls.Add(this.textBoxMachine);
            this.Controls.Add(this.buttonConnect);
            this.Name = "DriverPackageRemotePage";
            this.Size = new System.Drawing.Size(1500, 360);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDrivers)).EndInit();
            this.panelProcessing.ResumeLayout(false);
            this.panelComplete.ResumeLayout(false);
            this.panelComplete.PerformLayout();
            this.panelDone.ResumeLayout(false);
            this.panelDone.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxMachine;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonDeselectAll;
        private System.Windows.Forms.Button buttonSelectAll;
        private System.Windows.Forms.DataGridView dataGridViewDrivers;
        private System.Windows.Forms.Button buttonCapture;
        private System.Windows.Forms.Label labelProcessingObject;
        private System.Windows.Forms.ProgressBar progressBarObjects;
        private System.Windows.Forms.Panel panelProcessing;
        private System.Windows.Forms.Panel panelComplete;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Label labelPackage;
        private System.Windows.Forms.Label labelDestination;
        private System.Windows.Forms.TextBox textBoxDestination;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnCapture;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnProvider;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnOEMInf;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnClass;
        private System.Windows.Forms.Panel panelDone;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.Label labelPassword;
    }
}
