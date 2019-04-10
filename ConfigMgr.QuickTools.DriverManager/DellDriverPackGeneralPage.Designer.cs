namespace ConfigMgr.QuickTools.DriverManager
{
    partial class DellDriverPackGeneralPage
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
            this.comboBoxOS = new System.Windows.Forms.ComboBox();
            this.comboBoxArchitecture = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelOptions = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonOptions = new System.Windows.Forms.Button();
            this.labelInformation = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxOS
            // 
            this.comboBoxOS.FormattingEnabled = true;
            this.comboBoxOS.Items.AddRange(new object[] {
            "Windows 10",
            "Windows 8.1",
            "Windows 7"});
            this.comboBoxOS.Location = new System.Drawing.Point(21, 162);
            this.comboBoxOS.Name = "comboBoxOS";
            this.comboBoxOS.Size = new System.Drawing.Size(188, 21);
            this.comboBoxOS.TabIndex = 9;
            // 
            // comboBoxArchitecture
            // 
            this.comboBoxArchitecture.FormattingEnabled = true;
            this.comboBoxArchitecture.Items.AddRange(new object[] {
            "x64",
            "x86"});
            this.comboBoxArchitecture.Location = new System.Drawing.Point(21, 209);
            this.comboBoxArchitecture.Name = "comboBoxArchitecture";
            this.comboBoxArchitecture.Size = new System.Drawing.Size(188, 21);
            this.comboBoxArchitecture.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 146);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Operating System";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 193);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Architecture";
            // 
            // labelOptions
            // 
            this.labelOptions.AutoSize = true;
            this.labelOptions.Location = new System.Drawing.Point(59, 282);
            this.labelOptions.Name = "labelOptions";
            this.labelOptions.Size = new System.Drawing.Size(0, 13);
            this.labelOptions.TabIndex = 24;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(47, 264);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Driver Manager Options";
            // 
            // buttonOptions
            // 
            this.buttonOptions.Location = new System.Drawing.Point(21, 258);
            this.buttonOptions.Name = "buttonOptions";
            this.buttonOptions.Size = new System.Drawing.Size(24, 24);
            this.buttonOptions.TabIndex = 22;
            this.buttonOptions.UseVisualStyleBackColor = true;
            this.buttonOptions.Click += new System.EventHandler(this.ButtonOptions_Click);
            // 
            // labelInformation
            // 
            this.labelInformation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelInformation.Location = new System.Drawing.Point(18, 24);
            this.labelInformation.Name = "labelInformation";
            this.labelInformation.Size = new System.Drawing.Size(337, 110);
            this.labelInformation.TabIndex = 29;
            this.labelInformation.Text = " ";
            // 
            // DellDriverPackGeneralPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelInformation);
            this.Controls.Add(this.labelOptions);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonOptions);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxArchitecture);
            this.Controls.Add(this.comboBoxOS);
            this.Name = "DellDriverPackGeneralPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxOS;
        private System.Windows.Forms.ComboBox comboBoxArchitecture;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelOptions;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonOptions;
        private System.Windows.Forms.Label labelInformation;
    }
}
