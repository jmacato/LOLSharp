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
            this.components = new System.ComponentModel.Container();
            this.cli = new System.Windows.Forms.RichTextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // cli
            // 
            this.cli.BackColor = System.Drawing.Color.Black;
            this.cli.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.cli.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.cli.DetectUrls = false;
            this.cli.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cli.ForeColor = System.Drawing.Color.White;
            this.cli.Location = new System.Drawing.Point(12, 12);
            this.cli.Name = "cli";
            this.cli.ReadOnly = true;
            this.cli.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.cli.Size = new System.Drawing.Size(741, 364);
            this.cli.TabIndex = 0;
            this.cli.Text = "";
            this.cli.Click += new System.EventHandler(this.cli_Click);
            this.cli.Enter += new System.EventHandler(this.cli_Enter);
            this.cli.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cli_KeyPress);
            // 
            // Console
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(765, 388);
            this.ControlBox = false;
            this.Controls.Add(this.cli);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Console";
            this.Text = "Console";
            this.Load += new System.EventHandler(this.Console_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox cli;
        private System.Windows.Forms.Timer timer1;
    }
}