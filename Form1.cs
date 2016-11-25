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
        int lineaddress = 0; //Actively parsed line
        Dictionary<string, object> GlobalVariableList = new Dictionary<string, object> { };

        public Form1()
        {
            InitializeComponent();            
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

            ClearTables();

            string[] orig_array = lolstream.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            //Turn multispace operands into single strings.
            foreach (string s in ParseLOL.MonoglyphyOperators.Keys)
            {
                lolstream = Regex.Replace(lolstream, s, ParseLOL.MonoglyphyOperators[s]);
            }

            //Split them 'up and remove empty lines.
            string[] wrk_array = lolstream.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);


            //Trim all lines.
            int i = 0;
            foreach (string s in wrk_array)
            {
                orig_array[i] = orig_array[i].Trim();
                wrk_array[i] = wrk_array[i].Trim();
                i++;
            }

            //Begin parsing
            foreach (string cl in wrk_array)
            {
                parseLine(cl,orig_array,lineaddress);
                lineaddress += 1;
            }
        }

        //Parse through a line 
        private void parseLine(string s, string[] orig_array, int i)
        {

            string Keyword = ParseLOL.SplitKeysArgs(s).Keyword.Trim().Trim(',');
            string Argument = ParseLOL.SplitKeysArgs(s).Argument;
            string Output = "";

            if (ParseLOL.IsKeyword(Keyword))
            {
                switch (Keyword)
                {

                    case "HAI":
                        double progVersion;
                        LexTableAdd(Keyword, ParseLOL.LexemeDefinitions[Keyword]);
                        if (ParseLOL.GetArgDataType(Argument.ToString()) == ParseLOL.DataType.FLOT)
                        {
                            Output = ParseLOL.GetArgDataContent(Argument).ToString();
                            progVersion = Convert.ToDouble(Argument);
                            LexTableAdd(Output, ParseLOL.DTDesc[ParseLOL.GetArgDataType(Output)]+"; Program Version");
                        }
                        else
                        { 
                            //Throw error here?
                        }
                        break;

                    case "VISIBLE":
                        LexTableAdd(Keyword, ParseLOL.LexemeDefinitions[Keyword]);
                        switch (ParseLOL.GetArgDataType(Argument))
                        {
                            case ParseLOL.DataType.STRING:
                                Output = ParseLOL.GetArgDataContent(Argument).ToString();
                                LexTableAdd("Console Output", Output);
                                break;
                            case ParseLOL.DataType.VARIABLE:
                                Output = ParseLOL.GetArgDataContent(Argument).ToString();
                                LexTableAdd("Console Output", "VAR:" + Output);
                                break;
                            default:
                                break;
                        }
                        break;

                    case "I-HAS-A": 
                        //Variable Declaration Parsing Block
                        LexTableAdd(Keyword, ParseLOL.LexemeDefinitions[Keyword]);
                        if (ParseLOL.IsKeyword(ParseLOL.SplitKeysArgs(Argument).Keyword))
                        {
                            //throw error here (Reserved Keyword error)
                        }
                        switch (ParseLOL.GetArgDataType(ParseLOL.SplitKeysArgs(Argument).Keyword))
                        {
                            case ParseLOL.DataType.VARIABLE: //Next token is a variable
                                string variablename = ParseLOL.SplitKeysArgs(Argument).Keyword;
                                object varval = "";
                                Output = ParseLOL.GetArgDataContent(variablename).ToString();
                                LexTableAdd(Output, "New variable");
                                string ITZ_ARG = ParseLOL.SplitKeysArgs(Argument).Argument;

                                //Check if the next keyword
                                if (ParseLOL.IsKeyword(ParseLOL.SplitKeysArgs(ITZ_ARG).Keyword))
                                {
                                    if (ParseLOL.SplitKeysArgs(ITZ_ARG).Keyword == "ITZ")
                                    {
                                        //Theres a initializer~
                                        LexTableAdd("ITZ", ParseLOL.LexemeDefinitions["ITZ"]);
                                        varval = ParseLOL.SplitKeysArgs(ITZ_ARG).Argument;
                                        ParseLOL.DataType vardt = ParseLOL.GetArgDataType(varval.ToString());
                                        LexTableAdd(varval.ToString(), ParseLOL.DTDesc[vardt]);
                                    }
                                    else
                                    {
                                        //No initialization~
                                        varval = null;
                                    }
                                }

                                AddVariable(Output, varval.ToString());
 
                                break;
                            default: //Somebody jacked up the declaration, throw some error here
                                break;
                        }
                        break;
                    case "BTW":
                        break;
                    default:
                        if (ParseLOL.LexemeDefinitions.ContainsKey(Keyword))
                        {
                            LexTableAdd(Keyword, "UNIMPLEMENTED: "+ ParseLOL.LexemeDefinitions[Keyword]);
                            break;
                        }
                        else
                        {
                            LexTableAdd(Keyword, "Unknown");
                        }
                        break;
                }
            }

        }
        
        //Add variable to the global pool
        private void AddVariable(string s,object value)
        {
            string sx = s.Trim();
            if (!GlobalVariableList.ContainsKey(sx))
            {
                GlobalVariableList.Add(sx, value);
                SymTableUpdate();
            }
        }
        
        //Clear and rewrite the symbols table
        private void SymTableUpdate()
        {
            tableLayoutPanel2.Controls.Clear();
            foreach(KeyValuePair<string,object> symbs in GlobalVariableList)
            {
                SymTableAdd(symbs.Key, symbs.Value.ToString());
            }
        }
        
        //Add to Lexeme table
        private void LexTableAdd(string token, string val)
        {
            tableLayoutPanel1.Controls.Add(new Label() { Text = token });
            tableLayoutPanel1.Controls.Add(new Label() { Text = val, AutoSize = true});
            //textBox2.Text += "\r\n" +"L"+ lineaddress.ToString() +"|" + token.PadRight(20,' ') + "| "+ val.ToString().PadRight(20, ' ');
        }

        //Add to Symbol table
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

        private void Form1_Load(object sender, EventArgs e)
        {
            ParseLOL.Initialize();
            Console frm = new Console();
            frm.Show();
        }
    }
}
