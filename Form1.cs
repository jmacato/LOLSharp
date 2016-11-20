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
            int lineaddress = 0; //Actively parsed line
            int iToken = 0; // Current Token

            tableLayoutPanel1.Controls.Clear();

            //Split them 'up
            string[] lollines = lolstream.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (string curline in lollines)
            {
                foreach (string s in curline.Split(' '))
                {
                    //Compiler Directives
                    MatchCollection check1 = Regex.Matches(s, @"HAI"); //Delimiter to mark the start of the program
                    MatchCollection check2 = Regex.Matches(s, @"KTHXBYE"); //Delimiter to mark the end of the program              
                    MatchCollection check3 = Regex.Matches(s, @"BTW"); //Single-line comment
                    MatchCollection check47 = Regex.Matches(s, @"OBTW"); //Start of a multi-line comment 
                    MatchCollection check48 = Regex.Matches(s, @"TLDR"); //End of a multi-line comment 


                    //Operands
                    MatchCollection checkFloat = Regex.Matches(s, @"-?\d+\.\d+"); //Float Literal
                    MatchCollection checkBoolean = Regex.Matches(s, @"WIN|FAIL"); //Boolean Literal
                    MatchCollection checkInteger = Regex.Matches(s, @"[^/./n]-?\d+[^/.]"); //Integer Literal
                    MatchCollection check4 = Regex.Matches(s, @"I HAS A"); //Initialize a variable
                    MatchCollection check5 = Regex.Matches(s, @"ITZ"); //Assignment operator in declaring a variable 
                    MatchCollection check6 = Regex.Matches(s, @"GIMMEH"); //Input
                    MatchCollection check7 = Regex.Matches(s, @"VISIBLE"); //Output
                    MatchCollection check8 = Regex.Matches(s, @"BOTH SAEM"); //Comparison Operator; True if operands are equal
                    MatchCollection check9 = Regex.Matches(s, @"DIFFRINT"); // Comparison Operator; True if operands are not equal
                    MatchCollection check10 = Regex.Matches(s, @"SUM OF"); //Arithmetic Operator; Adds operands
                    MatchCollection check11 = Regex.Matches(s, @"DIFF OF");//Arithmetic Operator; Subtracts operand
                    MatchCollection check12 = Regex.Matches(s, @"PRODUKT OF"); //Arithmetic Operator; Multiplies operands
                    MatchCollection check13 = Regex.Matches(s, @"QUOSHUNT OF"); //"Arithmetic Operator; Divides operands 
                    MatchCollection check14 = Regex.Matches(s, @"MOD OF"); //Arithmetic Operator; Returns the remainder of the operands 
                    MatchCollection check15 = Regex.Matches(s, @"BIGGR OF"); //Comparison Operator; Returns the biggest of the given integers 
                    MatchCollection check16 = Regex.Matches(s, @"SMALLR OF"); //Comparison Operator; Returns the smallest of the given integers ",
                    MatchCollection check17 = Regex.Matches(s, @"O RLY?"); //If-Else Delimiter; Signals the start of the If-Else block
                    MatchCollection check18 = Regex.Matches(s, @"YA RLY"); //If the expression provided in the If-Else block is true, the code in this block will be executed ",
                    MatchCollection check19 = Regex.Matches(s, @"NO WAI"); //If the expression provided in the If-Else block is false, the code in this block will be executed. Also signals the end of the ​YA RLY​ block
                    MatchCollection check20 = Regex.Matches(s, @"OIC"); //Signals the end of the If-Else block
                    MatchCollection check21 = Regex.Matches(s, @"WTF[?]"); //Signals the start of a Switch Case block 
                    MatchCollection check22 = Regex.Matches(s, @"OMG"); //Comparison block for a Switch Case. Followed by a literal
                    MatchCollection check23 = Regex.Matches(s, @"OMGWTF"); //The default optional case in a Switch Case block.
                    MatchCollection check24 = Regex.Matches(s, @"IM IN YR"); //Signals the start of a loop. Followed by a label. 
                    MatchCollection check25 = Regex.Matches(s, @"IM OUTTA YR"); // Signals the end of a loop.Followed by a label.
                    MatchCollection check26 = Regex.Matches(s, @"HOW IZ I"); // Initializes a function. Followed by the function name.
                    MatchCollection check27 = Regex.Matches(s, @"IF U SAY SO"); //Closes a function block. 
                    MatchCollection check28 = Regex.Matches(s, @"I IZ"); //Calls a function. Followed by the function name and its parameters. 
                    MatchCollection check29 = Regex.Matches(s, @"GTFO");//Break statement. 
                    MatchCollection check30 = Regex.Matches(s, @"MEBBE"); //Appears between the ​YA RLY ​and ​NO WAI​ blocks. Similar to an Elseif statement. 
                    MatchCollection check31 = Regex.Matches(s, @"AN"); //Separates arguments. 
                    MatchCollection check32 = Regex.Matches(s, @"BOTH OF"); // Boolean Operator; Similar to AND; 1 or 2 operands
                    MatchCollection check33 = Regex.Matches(s, @"EITHER OF"); //Boolean Operator; Similar to OR; 1 or 2 operands
                    MatchCollection check34 = Regex.Matches(s, @"WON OF"); //Boolean Operator; Similar to XOR; Infinite operands
                    MatchCollection check35 = Regex.Matches(s, @"NOT"); //Negation
                    MatchCollection check36 = Regex.Matches(s, @"ALL OF"); // Boolean Operator; Similar to AND; Infinite operands
                    MatchCollection check37 = Regex.Matches(s, @"ANY OF"); //Boolean Operator; Similar to OR; Infinite operands 
                    MatchCollection check38 = Regex.Matches(s, @"IS NOW A"); //Used for re-casting a variable to a different type
                    MatchCollection check39 = Regex.Matches(s, @"MAEK"); //Used for re-casting a variable to a different type 
                    MatchCollection check40 = Regex.Matches(s, @"UPPIN"); //Increments a variable by one. 
                    MatchCollection check41 = Regex.Matches(s, @"NERFIN"); //Decrements a variable by one. 
                    MatchCollection check42 = Regex.Matches(s, @"FOUND YR"); //Returns the value of succeeding expression.
                    MatchCollection check43 = Regex.Matches(s, @"MKAY"); //Delimiter of a function call. 
                    MatchCollection check44 = Regex.Matches(s, @"WILE"); //Evaluates an expression as a Boolean statement. If it evaluates to true, the execution is continued. Else, the execution stops. 
                    MatchCollection check45 = Regex.Matches(s, @"TILL"); //Evaluates an expression as a Boolean statement. If it evaluates to true, the execution is continued. Else, the execution stops. 
                    MatchCollection check46 = Regex.Matches(s, @"IT\s"); //Temporary variable. Remains in local scope until it is replaced with a bare expression. 
                    MatchCollection check49 = Regex.Matches(s, @"SMOOSH"); //Expects strings as its input arguments for concatenation   
                    iToken += 1;
                }
                lineaddress += 1;             
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
