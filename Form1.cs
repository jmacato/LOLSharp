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
            foreach (string s in ParseDict.MonoglyphyOperators.Keys)
            {
                lolstream = Regex.Replace(lolstream, s, ParseDict.MonoglyphyOperators[s]);
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
            int lineaddress = 0; //Actively parsed line
            foreach (string cl in wrk_array)
            {
                parseLine(cl,orig_array,lineaddress);
                lineaddress += 1;
            }
        }

        //Parse through a line 
        private void parseLine(string s, string[] orig_array, int i)
        {
            string Keyword = SplitKeysArgs(s).Keyword.Trim().Trim(',');
            string Argument = SplitKeysArgs(s).Argument;
            string Output = "";
            if (IsKeyword(Keyword))
            {
                switch (Keyword)
                {
                    case "HAI":
                        double progVersion;
                        LexTableAdd(Keyword, ParseDict.LexemeDefinitions[Keyword]);
                        if (GetArgDataType(Argument.ToString()) == ParseDict.DataType.FLOT)
                        {
                            Output = GetArgDataContent(Argument).ToString();
                            progVersion = Convert.ToDouble(Argument);
                            LexTableAdd(Output, ParseDict.DTDesc[GetArgDataType(Output)]+"; Program Version");
                        }
                        else
                        { //Throw error here?
                        }
                        break;
                    case "VISIBLE":
                        LexTableAdd(Keyword, ParseDict.LexemeDefinitions[Keyword]);
                        switch (GetArgDataType(Argument))
                        {
                            case ParseDict.DataType.STRING:
                                Output = GetArgDataContent(Argument).ToString();
                                LexTableAdd("Console Output", Output);
                                break;
                            case ParseDict.DataType.VARIABLE:
                                Output = GetArgDataContent(Argument).ToString();
                                LexTableAdd("Console Output", "VAR:" + Output);
                                break;
                            default:
                                break;
                        }
                        break;
                    case "I-HAS-A": //Variable Declaration Parsing Block
                        LexTableAdd(Keyword, ParseDict.LexemeDefinitions[Keyword]);
                        if (IsKeyword(SplitKeysArgs(Argument).Keyword))
                        {
                            //throw error here (Reserved Keyword error)
                        }
                        switch (GetArgDataType(SplitKeysArgs(Argument).Keyword))
                        {
                            case ParseDict.DataType.VARIABLE: //Next token is a variable
                                string variablename = SplitKeysArgs(Argument).Keyword;
                                object varval = "";
                                Output = GetArgDataContent(variablename).ToString();
                                LexTableAdd(Output, "Variable");
                                string ITZ_ARG = SplitKeysArgs(Argument).Argument;

                                //Check if the next keyword
                                if (IsKeyword(SplitKeysArgs(ITZ_ARG).Keyword))
                                {
                                    if (SplitKeysArgs(ITZ_ARG).Keyword == "ITZ")
                                    {
                                        //Theres a initializer~
                                        LexTableAdd("ITZ", ParseDict.LexemeDefinitions["ITZ"]);
                                        varval = SplitKeysArgs(ITZ_ARG).Argument;
                                        ParseDict.DataType vardt = GetArgDataType(varval.ToString());
                                        LexTableAdd(varval.ToString(), ParseDict.DTDesc[vardt]);
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
                        if (ParseDict.LexemeDefinitions.ContainsKey(Keyword))
                        {
                            LexTableAdd(Keyword, ParseDict.LexemeDefinitions[Keyword]);
                            break;
                        }
                        else
                        {
                            LexTableAdd(Keyword, "Variable");
                        }
                        break;
                }
            }

        }

        public struct KeyArgPair
        {
            public string Keyword;
            public string Argument;
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

        //Check if token is a reserved word
        private bool IsKeyword(string token)
        {
            var keywrd = from Arg in ParseDict.LexemeDefinitions
                          where Regex.Match(token, Arg.Key, RegexOptions.Singleline).Success
                          select Arg;
            return keywrd != null && keywrd.Any();
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
        
        private KeyArgPair SplitKeysArgs(string sx)
        {
            string s = sx.Trim();
            string Keyword = s.Split(' ').First();
            string Argument = s.Substring(Keyword.Length, s.Length - Keyword.Length);
            KeyArgPair x;
            x.Keyword = Keyword.Trim();
            x.Argument = Argument.Trim();
            return x;
        }

        //Get datatype of command argument
        private ParseDict.DataType GetArgDataType(string token)
        {
            var results = from Arg in ParseDict.DTRegex
                          where Regex.Match(token, Arg.Key, RegexOptions.Singleline).Success
                          select Arg;
            foreach (var result in results)
            {
                switch (result.Value)
                {
                    case "Float Literal":
                        return ParseDict.DataType.FLOT;
                    case "Boolean Literal":
                        return ParseDict.DataType.BOOL;
                    case "Integer Literal":
                        return ParseDict.DataType.INT;
                    case "String":
                        return ParseDict.DataType.STRING;
                    case "Variable":
                        return ParseDict.DataType.VARIABLE;
                    default:
                        return ParseDict.DataType.UNKNOWN;
                }
            }
            return ParseDict.DataType.UNKNOWN;
        }

        //Get the value of the argument
        private object GetArgDataContent(string token)
        {
            var results = from Arg in ParseDict.DTRegex
                          where Regex.Match(token, Arg.Key, RegexOptions.Singleline).Success
                          select Arg;
            foreach (var result in results)
            {
                switch (result.Value)
                {
                    case "Float Literal":
                        return Convert.ToDouble(Regex.Match(token,result.Key).Value);
                    case "Boolean Literal":
                        return Regex.Match(token, result.Key).Value;
                    case "Integer Literal":
                        return Convert.ToDouble(Regex.Match(token, result.Key).Value);
                    case "String":
                        return Regex.Match(token, result.Key).Value;
                    case "Variable":
                        return Regex.Match(token, result.Key).Value;
                    default:
                        return null;
                }
            }
            return ParseDict.DataType.UNKNOWN;
        }

        //Add to Lexeme table
        private void LexTableAdd(string token, string val)
        {
            tableLayoutPanel1.Controls.Add(new Label() { Text = token });
            tableLayoutPanel1.Controls.Add(new Label() { Text = val, AutoSize = true});
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
            ParseDict.CInitial(); 

        }
    }
}
