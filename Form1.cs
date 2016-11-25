﻿using System;
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
using System.Threading;

namespace LOLpreter
{
    public partial class Form1 : Form
    {
        string lolstream = "";
        Console Console = new Console();
        int lineaddress = 0; //Actively parsed line
        Dictionary<string, object> GlobalVariableList = new Dictionary<string, object> { };
        bool StopExecution = false;

        TableLayoutPanel lex;

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
            enableStopButton();

            lolstream = textBox1.Text;

            //Clearing house
            ClearTables();
            Console.Clear();

             //Let it settle down first

            while (backgroundWorker1.IsBusy) //Force it to stop bgwrk
            {
                backgroundWorker1.CancelAsync();
            }

            //Let the parsing begin
            backgroundWorker1.RunWorkerAsync();

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
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
                if (StopExecution) // Check if stop flag has been triggered manually
                { break;}

                parseLine(cl, orig_array, lineaddress);
                lineaddress += 1;
                Thread.Sleep(250); //Simulated delay

                if (StopExecution) // Check if stop flag has been triggered in code
                { break; }

            }

            StopExecution = false;
            disableStopButton();
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
                        Output = ParseLOL.GetArgDataContent(Argument).ToString();

                        switch (ParseLOL.GetArgDataType(Argument))
                        {
                            case ParseLOL.DataType.STRING:
                                LexTableAdd("Console Output", Output);
                                Console.WriteLine(Output);
                                break;
                            case ParseLOL.DataType.VARIABLE:

                                if (GlobalVariableList.ContainsKey(Output))
                                {
                                    LexTableAdd("Console Output", "VAR:" + Output);
                                    Console.WriteLine(GlobalVariableList[Output]);
                                    break;
                                } else {
                                   Console.WriteLine("(line " + lineaddress.ToString() + ") ERROR: Unknown variable: " + Output, Color.Red);
                                    break;
                                }
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
                                        varval = ParseLOL.SplitKeysArgs(ITZ_ARG).Argument.Trim('"');
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
                    case "KTHXBYE":
                        LexTableAdd(Keyword, ParseLOL.LexemeDefinitions[Keyword]);
                        StopExecution = true;
                        break;
                    default:
                        if (ParseLOL.LexemeDefinitions.ContainsKey(Keyword))
                        {
                            LexTableAdd(Keyword, "UNIMPLEMENTED: "+ ParseLOL.LexemeDefinitions[Keyword]);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("(line " + lineaddress.ToString() + ") ERROR: Unknown keyword: " + Output, Color.Red);
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
            this.Invoke((MethodInvoker)delegate ()
            {
                tableLayoutPanel2.Controls.Clear();
            foreach(KeyValuePair<string,object> symbs in GlobalVariableList)
            {
                SymTableAdd(symbs.Key, symbs.Value.ToString());
                }
            });
        }
        
        //Add to Lexeme table
        private void LexTableAdd(string token, string val)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                tableLayoutPanel1.Controls.Add(new Label() { Text = token });
                tableLayoutPanel1.Controls.Add(new Label() { Text = val, AutoSize = true });
                tableLayoutPanel1.AutoScrollPosition = new Point(0, tableLayoutPanel1.VerticalScroll.Maximum);
            });
            //textBox2.Text += "\r\n" +"L"+ lineaddress.ToString() +"|" + token.PadRight(20,' ') + "| "+ val.ToString().PadRight(20, ' ');
        }

        //Add to Symbol table
        private void SymTableAdd(string token, string val)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                tableLayoutPanel2.Controls.Add(new Label() { Text = token });
                tableLayoutPanel2.Controls.Add(new Label() { Text = val, AutoSize = true });
                tableLayoutPanel2.AutoScrollPosition = new Point(0, tableLayoutPanel1.VerticalScroll.Maximum);
            });
        }

        //Clear lex & symb tables
        private void ClearTables()
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                //    foreach(Control x in tableLayoutPanel1.Controls)
                //    {
                //        x.Dispose();
                //    }
                //    foreach (Control x in tableLayoutPanel2.Controls)
                //    {
                //        x.Dispose();
                //    }

                //tableLayoutPanel1.Controls.Clear();

                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1 = lex;

                tableLayoutPanel2.Controls.Clear();
            });
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
            ParseLOL.Initialize(); //Initialize the dictionary
            Console.Show(); //Show console window
            disableStopButton();
            lex = tableLayoutPanel1;

        }

        void disableStopButton()
        {

            this.Invoke((MethodInvoker)delegate ()
            {
                stopExecButton.Enabled = false;
                stopExecButton.BackColor = Color.DarkGray;
                executeCodeButton.Enabled = true;
                executeCodeButton.BackColor = Color.Indigo;

            });

        }

        void enableStopButton()
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                stopExecButton.Enabled = true;
                stopExecButton.BackColor = Color.Maroon;
                executeCodeButton.Enabled = false;
                executeCodeButton.BackColor = Color.DarkGray;
            });
        }


        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                Console.WriteLine(e.Error.StackTrace,Color.Red);
        }

        private void stopExecButton_Click(object sender, EventArgs e)
        {
            StopExecution = true;
        }

        //private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    throw e.Error;
        //}
    }
}
