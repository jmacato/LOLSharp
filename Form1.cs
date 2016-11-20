using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace LOLpreter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            tableLayoutPanel1.Controls.Clear();
            string str = "";
            foreach (char s in textBox1.Text)
            {
                str += s;
                MatchCollection check1 = Regex.Matches(str, @"HAI");
                foreach (Match i in check1)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Delimiter to mark the start of the program " });
                    str = "";
                    continue;
                }
           
                MatchCollection check2 = Regex.Matches(str, @"KTHXBYE");
                foreach (Match i in check2)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Delimiter to mark the end of the program " });
                    str = "";
                    continue;
                }
                
                MatchCollection check3 = Regex.Matches(str, @"BTW");
                foreach (Match i in check3)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Single-line comment " });
                    str = "";
                    continue;
                }
                MatchCollection check4 = Regex.Matches(str, @"I HAS A ");
                foreach (Match i in check4)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Initialize a variable " });
                    str = "";
                    continue;
                }
                MatchCollection check5 = Regex.Matches(str, @"ITZ");
                foreach (Match i in check5)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Assignment operator in declaring a variable " });
                    str = "";
                    continue;
                }
                MatchCollection check6 = Regex.Matches(str, @"GIMMEH");
                foreach (Match i in check6)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Input  " });
                    str = "";
                    continue;
                }
                MatchCollection check7 = Regex.Matches(str, @"VISIBLE");
                foreach (Match i in check7)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Output " });
                    str = "";
                    continue;
                }
                MatchCollection check8 = Regex.Matches(str, @"BOTH SAEM");
                foreach (Match i in check8)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Comparison Operator; True if operands are equal " });
                    str = "";
                    continue;
                }
                MatchCollection check9 = Regex.Matches(str, @"DIFFRINT");
                foreach (Match i in check9)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Comparison Operator; True if operands are not equal" });
                    str = "";
                    continue;
                }
                MatchCollection check10 = Regex.Matches(str, @"SUM OF");
                foreach (Match i in check10)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Arithmetic Operator; Adds operands" });
                    str = "";
                    continue;
                }
                MatchCollection check11 = Regex.Matches(str, @"DIFF OF");
                foreach (Match i in check11)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Arithmetic Operator; Subtracts operand" });
                    str = "";
                    continue;
                }
                MatchCollection check12 = Regex.Matches(str, @"PRODUKT OF");
                foreach (Match i in check12)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Arithmetic Operator; Multiplies operands " });
                    str = "";
                    continue;
                }
                MatchCollection check13 = Regex.Matches(str, @"QUOSHUNT OF");
                foreach (Match i in check13)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Arithmetic Operator; Divides operands " });
                    str = "";
                    continue;
                }
                MatchCollection check14 = Regex.Matches(str, @"MOD OF");
                foreach (Match i in check14)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Arithmetic Operator; Returns the remainder of the operands " });
                    str = "";
                    continue;
                }
                MatchCollection check15 = Regex.Matches(str, @"BIGGR OF");
                foreach (Match i in check15)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Comparison Operator; Returns the biggest of the given integers " });
                    str = "";
                    continue;
                }
                MatchCollection check16 = Regex.Matches(str, @"SMALLR OF");
                foreach (Match i in check16)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Comparison Operator; Returns the smallest of the given integers " });
                    str = "";
                    continue;
                }
                MatchCollection check17 = Regex.Matches(str, @"O RLY?");
                foreach (Match i in check17)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "If-Else Delimiter; Signals the start of the If-Else block " });
                    str = "";
                    continue;
                }
                MatchCollection check18 = Regex.Matches(str, @"YA RLY");
                foreach (Match i in check18)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "If the expression provided in the If-Else block is true, the code in this block will be executed " });
                    str = "";
                    continue;
                }
                MatchCollection check19 = Regex.Matches(str, @"NO WAI");
                foreach (Match i in check19)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "If the expression provided in the If-Else block is false, the code in this block will be executed. Also signals the end of the ​YA RLY​ block " });
                    str = "";
                    continue;
                }
                MatchCollection check20 = Regex.Matches(str, @"OIC");
                foreach (Match i in check20)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Signals the end of the If-Else block " });
                    str = "";
                    continue;
                }
                MatchCollection check21 = Regex.Matches(str, @"WTF[?]");
                foreach (Match i in check21)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Signals the start of a Switch Case block " });
                    str = "";
                    continue;
                }
                MatchCollection check22 = Regex.Matches(str, @"OMG");
                foreach (Match i in check22)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Comparison block for a Switch Case. Followed by a literal" });
                    str = "";
                    continue;
                }
                MatchCollection check23 = Regex.Matches(str, @"OMGWTF");
                foreach (Match i in check23)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "The default optional case in a Switch Case block." });
                    str = "";
                    continue;
                }
                MatchCollection check24 = Regex.Matches(str, @"IM IN YR");
                foreach (Match i in check24)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Signals the start of a loop. Followed by a label. " });
                    str = "";
                    continue;
                }
                MatchCollection check25 = Regex.Matches(str, @"IM OUTTA YR");
                foreach (Match i in check25)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Signals the end of a loop. Followed by a label." });
                    str = "";
                    continue;
                }
                MatchCollection check26 = Regex.Matches(str, @"HOW IZ I");
                foreach (Match i in check26)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Initializes a function. Followed by the function name. " });
                    str = "";
                    continue;
                }
                MatchCollection check27 = Regex.Matches(str, @"IF U SAY SO");
                foreach (Match i in check27)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Closes a function block. " });
                    str = "";
                    continue;
                }
                MatchCollection check28 = Regex.Matches(str, @"I IZ");
                foreach (Match i in check28)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Calls a function. Followed by the function name and its parameters. 29 " });
                    str = "";
                    continue;
                }
                MatchCollection check29 = Regex.Matches(str, @"GTFO");
                foreach (Match i in check29)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Break statement. " });
                    str = "";
                    continue;
                }
                MatchCollection check30 = Regex.Matches(str, @"MEBBE");
                foreach (Match i in check30)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Appears between the ​YA RLY ​and ​NO WAI​ blocks. Similar to an Elseif statement. " });
                    str = "";
                    continue;
                }
                MatchCollection check31 = Regex.Matches(str, @"AN");
                foreach (Match i in check31)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Separates arguments. " });
                    str = "";
                    continue;
                }
                MatchCollection check32 = Regex.Matches(str, @"BOTH OF");
                foreach (Match i in check32)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Boolean Operator; Similar to AND; 1 or 2 operands " });
                    str = "";
                    continue;
                }
                MatchCollection check33 = Regex.Matches(str, @"EITHER OF");
                foreach (Match i in check33)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Boolean Operator; Similar to OR; 1 or 2 operands " });
                    str = "";
                    continue;
                }
                MatchCollection check34 = Regex.Matches(str, @"WON OF");
                foreach (Match i in check34)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Boolean Operator; Similar to XOR; Infinite operands " });
                    str = "";
                    continue;
                }
                MatchCollection check35 = Regex.Matches(str, @"NOT");
                foreach (Match i in check35)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Negation " });
                    str = "";
                    continue;
                }
                MatchCollection check36 = Regex.Matches(str, @"ALL OF");
                foreach (Match i in check36)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Boolean Operator; Similar to AND; Infinite operands " });
                    str = "";
                    continue;
                }
                MatchCollection check37 = Regex.Matches(str, @"ANY OF");
                foreach (Match i in check37)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Boolean Operator; Similar to OR; Infinite operands " });
                    str = "";
                    continue;
                }
                MatchCollection check38 = Regex.Matches(str, @"IS NOW A");
                foreach (Match i in check38)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Used for re-casting a variable to a different type " });
                    str = "";
                    continue;
                }
                MatchCollection check39 = Regex.Matches(str, @"MAEK");
                foreach (Match i in check39)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Used for re-casting a variable to a different type " });
                    str = "";
                    continue;
                }
                MatchCollection check40 = Regex.Matches(str, @"UPPIN");
                foreach (Match i in check40)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Increments a variable by one. " });
                    str = "";
                    continue;
                }
                MatchCollection check41 = Regex.Matches(str, @"NERFIN");
                foreach (Match i in check41)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Decrements a variable by one. " });
                    str = "";
                    continue;
                }
                MatchCollection check42 = Regex.Matches(str, @"FOUND YR");
                foreach (Match i in check42)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Returns the value of succeeding expression. " });
                    str = "";
                    continue;
                }
                MatchCollection check43 = Regex.Matches(str, @"MKAY");
                foreach (Match i in check43)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Delimiter of a function call. " });
                    str = "";
                    continue;
                }
                MatchCollection check44 = Regex.Matches(str, @"WILE");
                foreach (Match i in check44)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Evaluates an expression as a Boolean statement. If it evaluates to true, the execution is continued. Else, the execution stops. " });
                    str = "";
                    continue;
                }
                MatchCollection check45 = Regex.Matches(str, @"TILL");
                foreach (Match i in check45)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Evaluates an expression as a Boolean statement. If it evaluates to true, the execution is continued. Else, the execution stops. " });
                    str = "";
                    continue;
                }
                MatchCollection check46 = Regex.Matches(str, @"IT\s");
                foreach (Match i in check46)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Temporary variable. Remains in local scope until it is replaced with a bare expression. 47 " });
                    str = "";
                    continue;
                }
                MatchCollection check47 = Regex.Matches(str, @"OBTW");
                foreach (Match i in check47)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Start of a multi-line comment " });
                    str = "";
                    continue;
                }
                MatchCollection check48 = Regex.Matches(str, @"TLDR");
                foreach (Match i in check48)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "End of a multi-line comment " });
                    str = "";
                    continue;
                }
                MatchCollection check49 = Regex.Matches(str, @"SMOOSH");
                foreach (Match i in check49)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Expects strings as its input arguments for concatenation " });
                    str = "";
                    continue;
                }
               
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void tableLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(openFileDialog1.FileName);
                textBox1.Text = sr.ReadToEnd();
                sr.Close();
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
