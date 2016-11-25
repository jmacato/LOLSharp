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

        private void Console_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            cli.Clear();
            Random rand = new Random();
            cli.AppendText("\u2554" + new string((char)0x2550, 78) + "\u2557\r\n\u2551" + new string(' ', 29));
            cli.SelectionColor = Color.FromArgb(rand.Next(127, 256), rand.Next(127, 256), rand.Next(127, 256));
            cli.AppendText("LOLcode");
            cli.SelectionColor = Color.White;
            cli.AppendText(" Command Line" + new string(' ', 29) + "\u2551\r\n\u255A" + new string((char)0x2550, 78) + "\u255D");
        }

        public void WriteLine(object message)
        {
            cli.AppendText(message.ToString());
        }
    }
}
