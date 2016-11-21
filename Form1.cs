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

        Dictionary<string, string> LexemeDefinitions = new Dictionary<string, string>
        {
            {@"HAI","Delimiter to mark the start of the program"},
            {@"KTHXBYE","Delimiter to mark the end of the program"},
            {@"BTW","Single-line comment"},
            {@"OBTW","Start of a multi-line comment"},
            {@"TLDR","End of a multi-line comment"},
            {@"-?\d+\.\d+","Float Literal"},
            {@"WIN|FAIL","Boolean Literal"},
            {@"[^/./n]-?\d+[^/.]","Integer Literal"},
            {@"I HAS A","Initialize a variable"},
            {@"ITZ","Assignment operator in declaring a variable"},
            {@"GIMMEH","Input"},
            {@"VISIBLE","Output"},
            {@"BOTH SAEM","Comparison Operator; True if operands are equal"},
            {@"DIFFRINT"," Comparison Operator; True if operands are not equal"},
            {@"SUM OF","Arithmetic Operator; Adds operands"},
            {@"DIFF OF","Arithmetic Operator; Subtracts operand"},
            {@"PRODUKT OF","Arithmetic Operator; Multiplies operands"},
            {@"QUOSHUNT OF","Arithmetic Operator; Divides operands"},
            {@"MOD OF","Arithmetic Operator; Returns the remainder of the operands"},
            {@"BIGGR OF","Comparison Operator; Returns the biggest of the given integers"},
            {@"SMALLR OF","Comparison Operator; Returns the smallest of the given integers"}, 
            {@"O RLY?","If-Else Delimiter; Signals the start of the If-Else block"},
            {@"YA RLY","If the expression provided in the If-Else block is true, the code in this block will be executed"}, 
            {@"NO WAI","If the expression provided in the If-Else block is false, the code in this block will be executed. Also signals the end of the ​YA RLY​ block"},
            {@"OIC","Signals the end of the If-Else block"},
            {@"WTF[?]","Signals the start of a Switch Case block"},
            {@"OMG","Comparison block for a Switch Case. Followed by a literal"},
            {@"OMGWTF","The default optional case in a Switch Case block."},
            {@"IM IN YR","Signals the start of a loop. Followed by a label."},
            {@"IM OUTTA YR"," Signals the end of a loop.Followed by a label."},
            {@"HOW IZ I"," Initializes a function. Followed by the function name."},
            {@"IF U SAY SO","Closes a function block."},
            {@"I IZ","Calls a function. Followed by the function name and its parameters."},
            {@"GTFO","Break statement."}, 
            {@"MEBBE","Appears between the ​YA RLY ​and ​NO WAI​ blocks. Similar to an Elseif statement."}, 
            {@"AN","Separates arguments."}, 
            {@"BOTH OF"," Boolean Operator; Similar to AND; 1 or 2 operands"}, 
            {@"EITHER OF","Boolean Operator; Similar to OR; 1 or 2 operands"}, 
            {@"WON OF","Boolean Operator; Similar to XOR; Infinite operands"}, 
            {@"NOT","Negation"}, 
            {@"ALL OF"," Boolean Operator; Similar to AND; Infinite operands"}, 
            {@"ANY OF","Boolean Operator; Similar to OR; Infinite operands"}, 
            {@"IS NOW A","Used for re-casting a variable to a different type"}, 
            {@"MAEK","Used for re-casting a variable to a different type"}, 
            {@"UPPIN","Increments a variable by one."}, 
            {@"NERFIN","Decrements a variable by one."}, 
            {@"FOUND YR","Returns the value of succeeding expression."}, 
            {@"MKAY","Delimiter of a function call."}, 
            {@"WILE","Evaluates an expression as a Boolean statement. If it evaluates to true, the execution is continued. Else, the execution stops."}, 
            {@"TILL","Evaluates an expression as a Boolean statement. If it evaluates to true, the execution is continued. Else, the execution stops."}, 
            {@"IT\s","Temporary variable. Remains in local scope until it is replaced with a bare expression."}, 
            {@"SMOOSH","Expects strings as its input arguments for concatenation"} 
        };

        private void button2_Click(object sender, EventArgs e)
        {
            lolstream = textBox1.Text;

            Regex SCRegex = new Regex(@"BTW(.*?)(\n|\r|\r\n)");
            lolstream = SCRegex.Replace(lolstream, "");

            int lineaddress = 0; //Actively parsed line
            int iToken = 0; // Current Token
            tableLayoutPanel1.Controls.Clear();

            //Directives flags
            double progVersion = 0;

            //Split them 'up and remove extra lines
            string[] lollines = lolstream.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (string curline in lollines)
            {
                string[] activeline = curline.Split(' ');
                foreach (string s in activeline)
                {
                  //  bool ValidToken = false; //Error flag
                    int TokenCount = activeline.Count();

                    var results = from Lexeme in LexemeDefinitions
                                  where Regex.Match(s, Lexeme.Key, RegexOptions.Singleline).Success
                                  select Lexeme;

                    foreach (var result in results)
                    {
                        Console.WriteLine(result.Value);
                    }


                    /*
                    //Compiler Directives
                    Match checkStart = Regex.Match(s, @"HAI"); //Delimiter to mark the start of the program
                    Match checkEnd = Regex.Match(s, @"KTHXBYE"); //Delimiter to mark the end of the program              
                    Match checkSC = Regex.Match(s, @"BTW"); //Single-line comment
                    Match checkMLCS = Regex.Match(s, @"OBTW"); //Start of a multi-line comment 
                    Match checkMLCE = Regex.Match(s, @"TLDR"); //End of a multi-line comment */
                    /*
                    if (checkStart.Success)
                    {
                        //Start of program
                        lolstream = lolstream.Replace(@"HAI", @"");
                        progVersion = Convert.ToDouble(activeline[iToken + 1]);
                        ValidToken = true;
                        continue;
                    }

                    if (checkEnd.Success)
                    {
                        //End of program
                        ValidToken = true;
                        continue;
                    }

                    if (checkSC.Success)
                    {
                        //End of program
                        ValidToken = true;
                        continue;
                    }

                    if (!ValidToken)
                    {

                    }*/
                    iToken += 1;
                }
                lineaddress += 1;
                iToken = 0;
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
