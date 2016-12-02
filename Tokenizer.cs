using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LOLpreter
{
    class Tokenizer
    {
        public DebugWindow DebugWin;
        
        #region Declares

        //First pass token table
        public Dictionary<int,string[]> ProgTokenTableStg1 = new Dictionary<int, string[]>();
        //Second table for classified tokens
        public List<Token> ProgTokenTableStg2 = new List<Token>();
        //Lists definitions of each token, to be displayed in debugwin
        public Dictionary<Token, string> TokenDescriptorTable = new Dictionary<Token, string>();
        public Dictionary<string, Variable> VariableMemory = new Dictionary<string, Variable>();
        public Stack<string> BranchingStack = new Stack<string>();

        Dictionary<string, string> CompressOps = new Dictionary<string, string>
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
        public Dictionary<string, string> DTRegex = new Dictionary<string, string>
                        {
                            {@"^(-?[1-9]+\d*)$|^0$","Integer Literal"},
                            {@"^[0-9]*(?:\.[0-9]*)?$","Float Literal"},
                            {@"^[+-][0-9]*(?:\.[0-9]*)?$","Float Literal"},
                            {@"WIN|FAIL","Boolean Literal"},
                            {@"""[^\""]*""","String"},
                            {@"[A-Za-z][A-Za-z0-9_]*","Variable"}
                        };
        #endregion

        /// <summary>
        /// Tokenize input from lexer
        /// </summary>
        /// <param name="raw"></param>
        public void Tokenize(string raw)
        {

            ProgTokenTableStg1.Clear();
            ProgTokenTableStg2.Clear();
            TokenDescriptorTable.Clear();
            VariableMemory.Clear();

            foreach (string x in raw.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
               var curline = x;
                foreach (string s in CompressOps.Keys)
                {
                    curline = Regex.Replace(curline, s, CompressOps[s]);
                }
                var linenum = ProgTokenTableStg1.Count;
                ProgTokenTableStg1.Add(linenum, curline.Split(' '));
                DebugWin.Print(linenum.ToString().PadRight(5) + curline);

            }
            DebugWin.Print("-**-*-*-**--* er *-*-*-*-*--*-*-");


            tokenClassify(ProgTokenTableStg1);

            DebugWin.Print("-**-*-*-**--* OPCODES *-*-*-*-*--*-*-");

            foreach (Token s in ProgTokenTableStg2)
            {
                DebugWin.Print("0x"+s.lineAddress.ToString("X").PadRight(10, ' ') + "| " + atos(s.OperandType).PadLeft(10, ' ')+ " "+ s.tokenStr );
            }

        }

        /// <summary>
        /// Big-ass function to classify the variables
        /// </summary>
        /// <param name="progTokenTableStg1"></param>
        private void tokenClassify(Dictionary<int, string[]> progTokenTableStg1)
        {

            for (int line = 0;line < progTokenTableStg1.Keys.ToArray().Length - 1; line++)
            {
                string[] curline = ProgTokenTableStg1[line];
                int tokenCount = curline.Length;
                for (int indx = 0; indx < tokenCount; indx++){
                    string frame = curline[indx].Trim();
                    switch (frame)
                    {
                        case "I-HAS-A":
                            indx++;
                            var varname = curline[indx];
                            indx++;
                            if (tokenCount > 2 && curline[indx] == "ITZ")
                            {
                                indx++;
                                var varvalue = curline[indx];
                                Create2ndStgToken(line, OperandType.Command, "DCLV");
                                Create2ndStgToken(line, OperandType.Variable, varname);
                                Create2ndStgToken(line, OperandType.Literal, varvalue);
                                VariableMemory.Add(varname, new Variable() { name = varname, DataType = DetermineDataType(varvalue), Value = varvalue });
                                DebugWin.Print(atos(line) + " DECLARE-->" + varname + " TYPE " + VariableMemory[varname].DataType.ToString());
                                DebugWin.Print(" " + varname + "  ASSIGN-->" + varvalue);
                            }
                            else
                            {
                                Create2ndStgToken(line, OperandType.Command, "DCLN");
                                Create2ndStgToken(line, OperandType.Variable, varname);
                                VariableMemory.Add(varname, new Variable() { name = varname, DataType = DataTypes.NOOB });
                                DebugWin.Print(atos(line) + " DECLARE-->" + varname + " TYPE " + VariableMemory[varname].DataType.ToString());
                            }
                            indx = tokenCount;
                            break;

                        case "VISIBLE":
                            var command = "CPRT";
                            if (curline.Last() == "!") { command = "CPLN"; }
                            if (tokenCount > 2)
                            {
                                Create2ndStgToken(line, OperandType.Command, command);

                                DebugWin.Print(atos(line) + " " + command + "-->" + curline[indx + 1]);
                            }
                            else
                            {
                                Create2ndStgToken(line, OperandType.Command, command);
                                Create2ndStgToken(line, OperandType.Expression, curline[indx + 1]);
                                DebugWin.Print(atos(line) + " " + command + "-->" + curline[indx + 1]);
                            }
                            indx = tokenCount;
                            break;

                        case "GIMMEH":
                            Create2ndStgToken(line, OperandType.Command, "HLTI");
                            DebugWin.Print("HLTI" + "-->" + curline[indx + 1]);
                            indx = tokenCount;
                            break;

                        default:

                            if (VariableMemory.ContainsKey(curline[indx]))
                            {
                                if (tokenCount == 1)
                                {
                                    DebugWin.Print(atos(line) + "  VAREXIST-->" + curline[indx] + " | TYPE: " + VariableMemory[curline[indx]].DataType.ToString());
                                    indx = tokenCount;
                                } else
                                {
                                    if (curline[indx] == "R")
                                    {
                                        var x = string.Join(" ", curline, indx + 2, tokenCount - 2);
                                        DebugWin.Print(atos(line) + " " + curline[indx] + "  ASSIGN-->" + x);
                                        indx = tokenCount;
                                        break;
                                    }
                                }

                                break;
                            }



                            DebugWin.Print(atos(line) + " - UNPROCESSED - " + String.Join(" ", curline));
                            indx = tokenCount;
                            break;

                    }
                }
            }
        }

        public void Create2ndStgToken(int programAddress, OperandType OperandType, string tokenStr, DataTypes DataType = DataTypes.NOOB)
        {
            ProgTokenTableStg2.Add(new Token() { OperandType = OperandType, tokenStr = tokenStr, DataType = DataType, lineAddress=programAddress });
        }

        public string atos(object stringIn)
        {
            return stringIn.ToString();
        }

        /// <summary>
        /// Determines the datatypes of literals
        /// </summary>
        /// <param name="value"></param>
        ///// <returns></returns>
        //public DataTypes DetermineDataType(object value)
        //{
        //    int x1 ;
        //    bool isInteger = int.TryParse(value.ToString(), out x1);
        //    double x2;
        //    bool isFloat = double.TryParse(value.ToString(), out x2);
        //    if (isInteger) { return DataTypes.NUMBR; }
        //    if (isFloat) { return DataTypes.NUMBAR; }
        //    if (value.ToString().Contains(Lexer.UNICODE_SECTION)) { return DataTypes.YARN; }
        //    if (value.ToString().Contains("WIN") || value.ToString().Contains("FAIL")) { return DataTypes.TROOF;}
        //    return DataTypes.NOOB;
        //}

        public DataTypes DetermineDataType(string expression)
        {
            var results = from Arg in DTRegex
                          where Regex.Match(expression, Arg.Key, RegexOptions.Singleline).Success
                          select Arg;

            foreach (var result in results)
            {
                switch (result.Value)
                {
                    case "Float Literal":
                        return DataTypes.NUMBAR;
                    case "Boolean Literal":
                        return DataTypes.TROOF;
                    case "Integer Literal":
                        return DataTypes.NUMBR;
                }
            }

            if (expression.Contains(Lexer.UNICODE_SECTION))
            {return DataTypes.YARN;}
            return DataTypes.NOOB;
        }


    }



    public enum OperandType
    {
        Command,
        Expression,
        Literal,
        Variable
    }

    public enum DataTypes
    {
        YARN,
        NUMBR,
        NUMBAR,
        TROOF,
        NOOB
    }

    public class Variable
    {
        public string name { get; set; }
        public DataTypes DataType { get; set; }
        public object Value { get; set; }

    }

    public class Token
    {
        public int lineAddress { get; set; }
        public OperandType OperandType { get; set; }
        public string tokenStr { get; set; }
        public DataTypes DataType { get; set; }
    }

}
