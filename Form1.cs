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
        string lolstream = "";

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

        /* Standardize line endings to windows (CR LF) instead of linux (LF) only*/
        private string NormalizeEndings(string input)
        {
            if (Regex.IsMatch(input, "\r"))
            {
                return input;
            } else
            {
                return Regex.Replace(input, "\n", Environment.NewLine);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lolstream = textBox1.Text;

            tableLayoutPanel1.Controls.Clear();

            string[] lollines = lolstream.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (string s in lollines)
            {
                MatchCollection checkFloat = Regex.Matches(s, @"-?\d+\.\d+"); //Float Literal
                foreach (Match i in checkFloat)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label()
                    {
                        Text = "Float Literal",
                        AutoSize = true
                    });
                    
                    continue;
                }

                MatchCollection checkBoolean = Regex.Matches(s, @"WIN|FAIL");
                foreach (Match i in checkBoolean)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label()
                    {
                        Text = "Boolean Literal",
                        AutoSize = true
                    });
                    continue;
                }

                MatchCollection checkInteger = Regex.Matches(s, @"[^/./n]-?\d+[^/.]");
                foreach (Match i in checkInteger)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label()
                    {
                        Text = "Integer Literal",
                        AutoSize = true
                    });
                    
                    continue;
                }

                MatchCollection check1 = Regex.Matches(s, @"HAI");
                foreach (Match i in check1)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { 
                        Text = "Delimiter to mark the start of the program",
                        AutoSize = true
                        });
                    
                    continue;
                }
           
                MatchCollection check2 = Regex.Matches(s, @"KTHXBYE");
                foreach (Match i in check2)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { 
                        Text = "Delimiter to mark the end of the program",
                        AutoSize = true
                    });
                    
                    continue;
                }
                
                MatchCollection check3 = Regex.Matches(s, @"BTW");
                foreach (Match i in check3)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { 
                        Text = "Single-line comment ",
                        AutoSize = true
                    });
                    
                    continue;
                }
                MatchCollection check4 = Regex.Matches(s, @"I HAS A");
                foreach (Match i in check4)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { 
                        Text = "Initialize a variable ",
                        AutoSize = true
                    });
                    
                    continue;
                }
                MatchCollection check5 = Regex.Matches(s, @"ITZ");
                foreach (Match i in check5)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { 
                        Text = "Assignment operator in declaring a variable ",
                        AutoSize = true
                    });
                    
                    continue;
                }
                MatchCollection check6 = Regex.Matches(s, @"GIMMEH");
                foreach (Match i in check6)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { 
                        Text = "Input  ",
                        AutoSize = true 
                    });
                    
                    continue;
                }
                MatchCollection check7 = Regex.Matches(s, @"VISIBLE");
                foreach (Match i in check7)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { 
                        Text = "Output ",
                        AutoSize = true
                    });
                    
                    continue;
                }

                MatchCollection checkVariable = Regex.Matches(s, @"[:\x22:].+[:\x22:]");
                foreach (Match i in checkVariable)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString().Remove(1) });
                    tableLayoutPanel1.Controls.Add(new Label()
                    {
                        Text = "String Delimiter",
                        AutoSize = true
                    });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString().Substring(1, i.Length-2) });
                    tableLayoutPanel1.Controls.Add(new Label()
                    {
                        Text = "String Literal",
                        AutoSize = true
                    });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString().Substring(i.Length-1) });
                    tableLayoutPanel1.Controls.Add(new Label()
                    {
                        Text = "String Delimiter",
                        AutoSize = true
                    });
                    
                    continue;
                }

                MatchCollection check8 = Regex.Matches(s, @"BOTH SAEM");
                foreach (Match i in check8)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { 
                        Text = "Comparison Operator; True if operands are equal ",
                        AutoSize = true
                    });
                    
                    continue;
                }
                MatchCollection check9 = Regex.Matches(s, @"DIFFRINT");
                foreach (Match i in check9)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { 
                        Text = "Comparison Operator; True if operands are not equal",
                        AutoSize = true
                    });
                    
                    continue;
                }
                MatchCollection check10 = Regex.Matches(s, @"SUM OF");
                foreach (Match i in check10)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Arithmetic Operator; Adds operands",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check11 = Regex.Matches(s, @"DIFF OF");
                foreach (Match i in check11)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Arithmetic Operator; Subtracts operand",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check12 = Regex.Matches(s, @"PRODUKT OF");
                foreach (Match i in check12)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Arithmetic Operator; Multiplies operands ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check13 = Regex.Matches(s, @"QUOSHUNT OF");
                foreach (Match i in check13)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Arithmetic Operator; Divides operands ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check14 = Regex.Matches(s, @"MOD OF");
                foreach (Match i in check14)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Arithmetic Operator; Returns the remainder of the operands ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check15 = Regex.Matches(s, @"BIGGR OF");
                foreach (Match i in check15)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Comparison Operator; Returns the biggest of the given integers ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check16 = Regex.Matches(s, @"SMALLR OF");
                foreach (Match i in check16)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Comparison Operator; Returns the smallest of the given integers ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check17 = Regex.Matches(s, @"O RLY?");
                foreach (Match i in check17)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "If-Else Delimiter; Signals the start of the If-Else block ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check18 = Regex.Matches(s, @"YA RLY");
                foreach (Match i in check18)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "If the expression provided in the If-Else block is true, the code in this block will be executed ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check19 = Regex.Matches(s, @"NO WAI");
                foreach (Match i in check19)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "If the expression provided in the If-Else block is false, the code in this block will be executed. Also signals the end of the ​YA RLY​ block ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check20 = Regex.Matches(s, @"OIC");
                foreach (Match i in check20)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Signals the end of the If-Else block ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check21 = Regex.Matches(s, @"WTF[?]");
                foreach (Match i in check21)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Signals the start of a Switch Case block ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check22 = Regex.Matches(s, @"OMG");
                foreach (Match i in check22)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Comparison block for a Switch Case. Followed by a literal",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check23 = Regex.Matches(s, @"OMGWTF");
                foreach (Match i in check23)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "The default optional case in a Switch Case block.",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check24 = Regex.Matches(s, @"IM IN YR");
                foreach (Match i in check24)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Signals the start of a loop. Followed by a label. ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check25 = Regex.Matches(s, @"IM OUTTA YR");
                foreach (Match i in check25)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Signals the end of a loop. Followed by a label.",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check26 = Regex.Matches(s, @"HOW IZ I");
                foreach (Match i in check26)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Initializes a function. Followed by the function name. ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check27 = Regex.Matches(s, @"IF U SAY SO");
                foreach (Match i in check27)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Closes a function block. ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check28 = Regex.Matches(s, @"I IZ");
                foreach (Match i in check28)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Calls a function. Followed by the function name and its parameters. ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check29 = Regex.Matches(s, @"GTFO");
                foreach (Match i in check29)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Break statement. ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check30 = Regex.Matches(s, @"MEBBE");
                foreach (Match i in check30)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Appears between the ​YA RLY ​and ​NO WAI​ blocks. Similar to an Elseif statement. ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check31 = Regex.Matches(s, @"AN");
                foreach (Match i in check31)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Separates arguments. ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check32 = Regex.Matches(s, @"BOTH OF");
                foreach (Match i in check32)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Boolean Operator; Similar to AND; 1 or 2 operands ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check33 = Regex.Matches(s, @"EITHER OF");
                foreach (Match i in check33)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Boolean Operator; Similar to OR; 1 or 2 operands ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check34 = Regex.Matches(s, @"WON OF");
                foreach (Match i in check34)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Boolean Operator; Similar to XOR; Infinite operands ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check35 = Regex.Matches(s, @"NOT");
                foreach (Match i in check35)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Negation ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check36 = Regex.Matches(s, @"ALL OF");
                foreach (Match i in check36)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Boolean Operator; Similar to AND; Infinite operands ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check37 = Regex.Matches(s, @"ANY OF");
                foreach (Match i in check37)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Boolean Operator; Similar to OR; Infinite operands ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check38 = Regex.Matches(s, @"IS NOW A");
                foreach (Match i in check38)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Used for re-casting a variable to a different type ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check39 = Regex.Matches(s, @"MAEK");
                foreach (Match i in check39)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Used for re-casting a variable to a different type ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check40 = Regex.Matches(s, @"UPPIN");
                foreach (Match i in check40)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Increments a variable by one. ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check41 = Regex.Matches(s, @"NERFIN");
                foreach (Match i in check41)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Decrements a variable by one. ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check42 = Regex.Matches(s, @"FOUND YR");
                foreach (Match i in check42)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Returns the value of succeeding expression. ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check43 = Regex.Matches(s, @"MKAY");
                foreach (Match i in check43)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Delimiter of a function call. ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check44 = Regex.Matches(s, @"WILE");
                foreach (Match i in check44)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Evaluates an expression as a Boolean statement. If it evaluates to true, the execution is continued. Else, the execution stops. ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check45 = Regex.Matches(s, @"TILL");
                foreach (Match i in check45)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Evaluates an expression as a Boolean statement. If it evaluates to true, the execution is continued. Else, the execution stops. ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check46 = Regex.Matches(s, @"IT\s");
                foreach (Match i in check46)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Temporary variable. Remains in local scope until it is replaced with a bare expression. ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check47 = Regex.Matches(s, @"OBTW");
                foreach (Match i in check47)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Start of a multi-line comment ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check48 = Regex.Matches(s, @"TLDR");
                foreach (Match i in check48)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "End of a multi-line comment ",
                        AutoSize = true });
                    
                    continue;
                }
                MatchCollection check49 = Regex.Matches(s, @"SMOOSH");
                foreach (Match i in check49)
                {
                    tableLayoutPanel1.Controls.Add(new Label() { Text = i.ToString() });
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Expects strings as its input arguments for concatenation ",
                        AutoSize = true });
                    
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

                lolstream = NormalizeEndings(sr.ReadToEnd());
                textBox1.Text = lolstream;
                sr.Close();
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
