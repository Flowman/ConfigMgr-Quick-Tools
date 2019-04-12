namespace ConfigMgr.QuickTools
{
    partial class OptionsControl
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
            this.checkEnable = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownInterval = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.labelLastChecked = new System.Windows.Forms.Label();
            this.buttonCheckNow = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // checkEnable
            // 
            this.checkEnable.AutoSize = true;
            this.checkEnable.Location = new System.Drawing.Point(10, 41);
            this.checkEnable.Name = "checkEnable";
            this.checkEnable.Size = new System.Drawing.Size(102, 17);
            this.checkEnable.TabIndex = 0;
            this.checkEnable.Text = "Enable Updates";
            this.checkEnable.UseVisualStyleBackColor = true;
            this.checkEnable.CheckedChanged += new System.EventHandler(this.CheckEnable_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Check for updates every:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(91, 143);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Last checked:";
            // 
            // numericUpDownInterval
            // 
            this.numericUpDownInterval.Location = new System.Drawing.Point(10, 95);
            this.numericUpDownInterval.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDownInterval.Minimum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.numericUpDownInterval.Name = "numericUpDownInterval";
            this.numericUpDownInterval.Size = new System.Drawing.Size(49, 20);
            this.numericUpDownInterval.TabIndex = 18;
            this.numericUpDownInterval.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.numericUpDownInterval.ValueChanged += new System.EventHandler(this.NumericUpDownInterval_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(65, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "days";
            // 
            // labelLastChecked
            // 
            this.labelLastChecked.AutoSize = true;
            this.labelLastChecked.Location = new System.Drawing.Point(161, 143);
            this.labelLastChecked.Name = "labelLastChecked";
            this.labelLastChecked.Size = new System.Drawing.Size(0, 13);
            this.labelLastChecked.TabIndex = 20;
            // 
            // buttonCheckNow
            // 
            this.buttonCheckNow.Location = new System.Drawing.Point(10, 138);
            this.buttonCheckNow.Name = "buttonCheckNow";
            this.buttonCheckNow.Size = new System.Drawing.Size(75, 23);
            this.buttonCheckNow.TabIndex = 21;
            this.buttonCheckNow.Text = "Check Now";
            this.buttonCheckNow.UseVisualStyleBackColor = true;
            this.buttonCheckNow.Click += new System.EventHandler(this.ButtonCheckNow_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(303, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Be up to date and enable auto update check for new versions.";
            // 
            // OptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonCheckNow);
            this.Controls.Add(this.labelLastChecked);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownInterval);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkEnable);
            this.Name = "OptionsControl";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkEnable;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownInterval;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelLastChecked;
        private System.Windows.Forms.Button buttonCheckNow;
        private System.Windows.Forms.Label label3;
    }
}
