namespace LOLpreter
{
    partial class Console
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
            this.cli = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // cli
            // 
            this.cli.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cli.BackColor = System.Drawing.Color.Black;
            this.cli.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.cli.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cli.ForeColor = System.Drawing.Color.White;
            this.cli.Location = new System.Drawing.Point(-1, 0);
            this.cli.Name = "cli";
            this.cli.ReadOnly = true;
            this.cli.Size = new System.Drawing.Size(724, 363);
            this.cli.TabIndex = 0;
            this.cli.Text = "";
            // 
            // Console
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(720, 361);
            this.ControlBox = false;
            this.Controls.Add(this.cli);
            this.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "Console";
            this.Text = "Console";
            this.Load += new System.EventHandler(this.Console_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox cli;
    }
}