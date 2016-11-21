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
using System.Diagnostics;

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

        /*
        Definitions of each tokens/lexemes
        */
        Dictionary<string, string> LexemeDefinitions = new Dictionary<string, string>
        {
            {@"I-HAS-A","Initialize a variable"},
            {@"BOTH-SAEM","Comparison Operator; True if operands are equal"},
            {@"SUM-OF","Arithmetic Operator; Adds operands"},
            {@"DIFF-OF","Arithmetic Operator; Subtracts operand"},
            {@"PRODUKT-OF","Arithmetic Operator; Multiplies operands"},
            {@"QUOSHUNT-OF","Arithmetic Operator; Divides operands"},
            {@"MOD-OF","Arithmetic Operator; Returns the remainder of the operands"},
            {@"BIGGR-OF","Comparison Operator; Returns the biggest of the given integers"},
            {@"SMALLR-OF","Comparison Operator; Returns the smallest of the given integers"},
            {@"O-RLY?","If-Else Delimiter; Signals the start of the If-Else block"},
            {@"YA-RLY","If the expression provided in the If-Else block is true, the code in this block will be executed"},
            {@"NO-WAI","If the expression provided in the If-Else block is false, the code in this block will be executed. Also signals the end of the ​YA RLY​ block"},
            {@"IM-IN-YR","Signals the start of a loop. Followed by a label."},
            {@"IM-OUTTA-YR"," Signals the end of a loop.Followed by a label."},
            {@"HOW-IZ-I"," Initializes a function. Followed by the function name."},
            {@"IF-U-SAY-SO","Closes a function block."},
            {@"I-IZ","Calls a function. Followed by the function name and its parameters."},
            {@"BOTH-OF"," Boolean Operator; Similar to AND; 1 or 2 operands"},
            {@"EITHER-OF","Boolean Operator; Similar to OR; 1 or 2 operands"},
            {@"WON-OF","Boolean Operator; Similar to XOR; Infinite operands"},
            {@"ALL-OF"," Boolean Operator; Similar to AND; Infinite operands"},
            {@"ANY-OF","Boolean Operator; Similar to OR; Infinite operands"},
            {@"IS-NOW-A","Used for re-casting a variable to a different type"},
            {@"FOUND-YR","Returns the value of succeeding expression."},

            {@"HAI","Delimiter to mark the start of the program"},
            {@"KTHXBYE","Delimiter to mark the end of the program"},
            {@"BTW","Single-line comment"},
            {@"OBTW","Start of a multi-line comment"},
            {@"TLDR","End of a multi-line comment"},
            {@"ITZ","Assignment operator in declaring a variable"},
            {@"GIMMEH","Input"},
            {@"VISIBLE","Output"},
            {@"OIC","Signals the end of the If-Else block"},
            {@"WTF[?]","Signals the start of a Switch Case block"},
            {@"OMG","Comparison block for a Switch Case. Followed by a literal"},
            {@"OMGWTF","The default optional case in a Switch Case block."},
            {@"GTFO","Break statement."},
            {@"MEBBE","Appears between the ​YA RLY ​and ​NO WAI​ blocks. Similar to an Elseif statement."},
            {@"AN","Separates arguments."},
            {@"NOT","Negation"},
            {@"MAEK","Used for re-casting a variable to a different type"},
            {@"UPPIN","Increments a variable by one."},
            {@"NERFIN","Decrements a variable by one."},
            {@"MKAY","Delimiter of a function call."}, 
            {@"WILE","Evaluates an expression as a Boolean statement. If it evaluates to true, the execution is continued. Else, the execution stops."}, 
            {@"TILL","Evaluates an expression as a Boolean statement. If it evaluates to true, the execution is continued. Else, the execution stops."}, 
            {@"IT\s","Temporary variable. Remains in local scope until it is replaced with a bare expression."}, 
            {@"SMOOSH","Expects strings as its input arguments for concatenation"},
            {@"DIFFRINT"," Comparison Operator; True if operands are not equal"},
            
        };

        Dictionary<string, string> DataType = new Dictionary<string, string>
        {
            {@"-?\d+\.\d+","Float Literal"},
            {@"WIN|FAIL","Boolean Literal"},
            {@"[^/./n]-?\d+[^/.]","Integer Literal"}
            {@"""[^\""]*""","String"}


        };

        enum DataType
        {
            FLOT,
            BOOL,
            INT,
            STRING,
            VARIABLE,
            UNKNOWN
        }

        /*
        Since we split tokens by spaces
        It's necessary to turn multiple space operators into
        a single connected string
        */
        Dictionary<string, string> MonoglyphyOperators = new Dictionary<string, string>
        {
            {@"I HAS A","I-HAS-A"},
            {@"BOTH SAEM","BOTH-SAEM"},
            {@"SUM OF","SUM-OF"},
            {@"DIFF OF","DIFF OF"},
            {@"PRODUKT OF","PRODUKT-OF"},
            {@"QUOSHUNT OF","QUOSHUNT-OF"},
            {@"MOD OF","MOD-OF"},
            {@"BIGGR OF","BIGGR-OF"},
            {@"SMALLR OF","SMALLR-OF"},
            {@"O RLY?","O RLY?"},
            {@"YA RLY","YA-RLY"},
            {@"NO WAI","NO-WAI"},
            {@"IM IN YR","IM-IN-YR"},
            {@"IM OUTTA YR","IM-OUTTA-YR"},
            {@"HOW IZ I","HOW-IZ-I"},
            {@"IF U SAY SO","IF-U-SAY-SO"},
            {@"I IZ","I-IZ"},
            {@"BOTH OF","BOTH-OF"},
            {@"EITHER OF","EITHER-OF"},
            {@"WON OF","WON-OF"},
            {@"ALL OF","ALL-OF"},
            {@"ANY OF","ANY-OF"},
            {@"IS NOW A","IS-NOW-A"},
            {@"FOUND YR","FOUND-YR"}
        };



        private void button2_Click(object sender, EventArgs e)
        {
            lolstream = textBox1.Text;

            int lineaddress = 0; //Actively parsed line

            ClearTables();

            string[] orig_array = lolstream.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (string s in MonoglyphyOperators.Keys)
            {
                lolstream = Regex.Replace(lolstream, s, MonoglyphyOperators[s]);
            }

            //Split them 'up and remove extra lines
            string[] wrk_array = lolstream.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            int i = 0;

            foreach (string s in wrk_array)
            {
                orig_array[i] = orig_array[i].Trim();
                wrk_array[i] = wrk_array[i].Trim();
                i++;
            }

            foreach (string cl in wrk_array)
            {
                parseLine(cl,orig_array,lineaddress);
                lineaddress += 1;
            }
        }

        private void parseLine(string s, string[] orig_array, int i)
        {
            var results = from Lexeme in LexemeDefinitions
                          where Regex.Match(s, Lexeme.Key, RegexOptions.Singleline).Success
                          select Lexeme;

            foreach (var result in results)
            {
                string outp = "";
                string lexremoved = "";
                switch (result.Key)
                {
                    case "HAI":
                        lexremoved = orig_array[i].Replace("HAI", "");

                        break;
                    case "VISIBLE":
                        lexremoved = orig_array[i].Replace("VISIBLE", "");
                        LexTableAdd(s, result.Value);
                        if (Regex.IsMatch(lexremoved, @"""[^\""]*"""))
                        {
                            outp = Regex.Match(lexremoved, @"""[^\""]*""").Value;
                            SymTableAdd("CONSOLE OUTPUT", outp);
                        }
                        else if (Regex.IsMatch(lexremoved, @"[A-Za-z][A-Za-z0-9_]*"))
                        {
                            outp = Regex.Match(lexremoved, @"[A-Za-z][A-Za-z0-9_]*").Value;
                            SymTableAdd("CONSOLE OUTPUT", "VAR:"+outp);
                        }
                        break;
                        
                }
                LexTableAdd(result.Key, result.Value);
                continue;
            }
        }



        private void LexTableAdd(string token, string val)
        {
            tableLayoutPanel1.Controls.Add(new Label() { Text = token });
            tableLayoutPanel1.Controls.Add(new Label() { Text = val, AutoSize = true});
        }

        private void SymTableAdd(string token, string val)
        {
            tableLayoutPanel2.Controls.Add(new Label() { Text = token });
            tableLayoutPanel2.Controls.Add(new Label() { Text = val, AutoSize = true });
        }

        private void ClearTables()
        {
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel2.Controls.Clear();
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
