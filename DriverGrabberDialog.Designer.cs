namespace Zetta.ConfigMgr.QuickTools
{
    partial class S
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.dataGridViewDriverPackages = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.labelStatus = new System.Windows.Forms.Label();
            this.columnImport = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnPackage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.comboBoxOS = new System.Windows.Forms.ComboBox();
            this.comboBoxArch = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDriverPackages)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(24, 369);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(268, 23);
            this.progressBar1.TabIndex = 3;
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
            this.dataGridViewDriverPackages.Location = new System.Drawing.Point(24, 36);
            this.dataGridViewDriverPackages.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.dataGridViewDriverPackages.Name = "dataGridViewDriverPackages";
            this.dataGridViewDriverPackages.RowHeadersVisible = false;
            this.dataGridViewDriverPackages.Size = new System.Drawing.Size(467, 284);
            this.dataGridViewDriverPackages.StandardTab = true;
            this.dataGridViewDriverPackages.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(340, 369);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(500, 368);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(228, 409);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(35, 13);
            this.labelStatus.TabIndex = 7;
            this.labelStatus.Text = "label1";
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
            // comboBoxOS
            // 
            this.comboBoxOS.FormattingEnabled = true;
            this.comboBoxOS.Items.AddRange(new object[] {
            "Windows 7",
            "Windows 8.1",
            "Windows 10"});
            this.comboBoxOS.Location = new System.Drawing.Point(625, 36);
            this.comboBoxOS.Name = "comboBoxOS";
            this.comboBoxOS.Size = new System.Drawing.Size(121, 21);
            this.comboBoxOS.TabIndex = 8;
            // 
            // comboBoxArch
            // 
            this.comboBoxArch.FormattingEnabled = true;
            this.comboBoxArch.Items.AddRange(new object[] {
            "x64",
            "x86"});
            this.comboBoxArch.Location = new System.Drawing.Point(625, 80);
            this.comboBoxArch.Name = "comboBoxArch";
            this.comboBoxArch.Size = new System.Drawing.Size(121, 21);
            this.comboBoxArch.TabIndex = 9;
            // 
            // S
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.comboBoxArch);
            this.Controls.Add(this.comboBoxOS);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridViewDriverPackages);
            this.Controls.Add(this.progressBar1);
            this.Name = "S";
            this.Text = "DriverGrabberDialog";
            this.Title = "DriverGrabberDialog";
            this.Shown += new System.EventHandler(this.DriverGrabberDialog_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDriverPackages)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.DataGridView dataGridViewDriverPackages;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnImport;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnPackage;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnStatus;
        private System.Windows.Forms.ComboBox comboBoxOS;
        private System.Windows.Forms.ComboBox comboBoxArch;
    }
}