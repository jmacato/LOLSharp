using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LOLpreter
{
    public partial class Console : Form
    {
        [DllImport("user32.dll")]
        public static extern int HideCaret(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern int ShowCaret(IntPtr hwnd);

        bool linefeed_entered = false;
        bool waitingforinput = false;
        string inputbuffer;


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
            //Border
            Write(("\u2554" + new string((char)0x2550, 78) + "\u2557\r\n\u2551" + new string(' ', 29)));
            Color SelectionColor = Color.FromArgb(rand.Next(127, 256), rand.Next(127, 256), rand.Next(127, 256));

            string lol="";

            //Just for the lulz
            if (rand.Next(10) > rand.Next(10)) { lol += "L"; } else { lol += "l"; }
            if (rand.Next(10) > rand.Next(10)) { lol += "O"; } else { lol += "o"; }
            if (rand.Next(10) > rand.Next(10)) { lol += "L"; } else { lol += "l"; }
            if (rand.Next(10) > rand.Next(10)) { lol += "C"; } else { lol += "c"; }
            if (rand.Next(10) > rand.Next(10)) { lol += "O"; } else { lol += "o"; }
            if (rand.Next(10) > rand.Next(10)) { lol += "D"; } else { lol += "d"; }
            if (rand.Next(10) > rand.Next(10)) { lol += "E"; } else { lol += "e"; }

            Write(lol,SelectionColor);

            Write(" Command Line" + new string(' ', 29) + "\u2551\r\n\u255A" + new string((char)0x2550, 78) + "\u255D\r\n");
        }

        public void WriteLine(object message, Color? textcolor = null) //Write to cli with linefeed
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                cli.SelectionColor = textcolor ?? Color.White;
                cli.AppendText(message.ToString() + "\r\n");
                cli.SelectionColor = Color.White;
            });
        }


        public string ReadKey(ref bool stopExecution, string message = null)
        {
            inputbuffer = null;
            if (!(message == null)) //Check if theres a message
            {
                this.Write(message);
            }
            waitingforinput = true;

            while (waitingforinput) //Stall the bgworker thread while waiting for a input
            {
                if (stopExecution) { break; }
                if (linefeed_entered) 
                {
                    waitingforinput = false;
                    linefeed_entered = false;
                    this.WriteLine("");
                    return inputbuffer;
                }
            }
            waitingforinput = false;
            linefeed_entered = false;
            return null;
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

        private void cli_Enter(object sender, EventArgs e)
        {
            //Pass the focus to the console window, not the richtextbox
            ActiveControl = this.ActiveControl;
        }

        private void cli_Click(object sender, EventArgs e)
        {
            //Hide cursor when not asking for input
            if (waitingforinput) { ShowCaret(cli.Handle); } else { HideCaret(cli.Handle); }
        }

        private void cli_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (waitingforinput)
            {
                if (e.KeyChar == (char)Keys.Enter) //Enter key is pressed
                {
                    linefeed_entered = true;
                    e.Handled = true;
                    return;
                }
                inputbuffer += e.KeyChar.ToString();
                this.Write(e.KeyChar.ToString());
                e.Handled = true;
            }
        }
    }
}
