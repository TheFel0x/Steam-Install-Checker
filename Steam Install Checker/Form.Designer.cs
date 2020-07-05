namespace Steam_Install_Checker
{
    partial class Form
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
            this.btn_analyze = new System.Windows.Forms.Button();
            this.tb_output = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_analyze
            // 
            this.btn_analyze.Location = new System.Drawing.Point(621, 371);
            this.btn_analyze.Name = "btn_analyze";
            this.btn_analyze.Size = new System.Drawing.Size(167, 67);
            this.btn_analyze.TabIndex = 0;
            this.btn_analyze.Text = "Analyze Steam Folders";
            this.btn_analyze.UseVisualStyleBackColor = true;
            this.btn_analyze.Click += new System.EventHandler(this.Analyze_Click);
            // 
            // tb_output
            // 
            this.tb_output.Location = new System.Drawing.Point(12, 12);
            this.tb_output.Multiline = true;
            this.tb_output.Name = "tb_output";
            this.tb_output.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tb_output.Size = new System.Drawing.Size(776, 353);
            this.tb_output.TabIndex = 1;
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tb_output);
            this.Controls.Add(this.btn_analyze);
            this.Name = "Form";
            this.Text = "Steam Install Checker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_analyze;
        private System.Windows.Forms.TextBox tb_output;
    }
}

