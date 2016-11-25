using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LOLpreter
{
    public partial class Console : Form
    {
        public Console()
        {
            InitializeComponent();
        }

        public void Console_Load(object sender, EventArgs e)
        {
            Clear();
        }

        public void Clear()
        {
            cli.Clear();
            Random rand = new Random();
            cli.AppendText("\u2554" + new string((char)0x2550, 78) + "\u2557\r\n\u2551" + new string(' ', 29));
            cli.SelectionColor = Color.FromArgb(rand.Next(127, 256), rand.Next(127, 256), rand.Next(127, 256));
            cli.SelectionFont = new Font(cli.SelectionFont, FontStyle.Bold);
            cli.AppendText("LOLcode");
            cli.SelectionFont = new Font(cli.SelectionFont, FontStyle.Regular);
            cli.SelectionColor = Color.White;
            cli.AppendText(" Command Line" + new string(' ', 29) + "\u2551\r\n\u255A" + new string((char)0x2550, 78) + "\u255D\r\n");
        }

        public void WriteLine(object message, Color? textcolor = null)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                cli.SelectionColor = textcolor ?? Color.White;
                cli.AppendText(message.ToString() + "\r\n");
                cli.SelectionColor = Color.White;
            });
        }

        public void Write(object message, Color? textcolor = null)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                cli.SelectionColor = textcolor ?? Color.White;
                cli.AppendText(message.ToString());
                cli.SelectionColor = Color.White;
            });
        }

    }
}
