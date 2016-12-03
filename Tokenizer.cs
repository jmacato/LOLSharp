using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LOLpreter
{
    public class Tokenizer
    {
        public DebugWindow DebugWin;

        #region Declares
        public List<Error> ErrorList;
        //First pass token table
        public Dictionary<int,string[]> ProgTokenTableStg1 = new Dictionary<int, string[]>();
        //Second table for classified tokens
        public List<Token> ProgTokenTableStg2 = new List<Token>();
        //Lists definitions of each token, to be displayed in debugwin
        public Dictionary<Token, string> TokenDescriptorTable = new Dictionary<Token, string>();
        public Dictionary<string, Variable> VariableMemory = new Dictionary<string, Variable>();
        public Stack<Token> BranchingStack = new Stack<Token>();

        Dictionary<string, string> CompressOps = new Dictionary<string, string>
                                    {
                                        {@"I HAS A","I-HAS-A"},
                                        {@"BOTH SAEM","BOTH-SAEM"},
                                        {@"SUM OF","ADD"},
                                        {@"DIFF OF","SUB"},
                                        {@"PRODUKT OF","PROD"},
                                        {@"QUOSHUNT OF","QUOT"},
                                        {@"MOD OF","MOD"},
                                        {@"BIGGR OF","MAX"},
                                        {@"SMALLR OF","MIN"},
                                        {@"O RLY?","O-RLY?"},
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
        /// and simplify them
        /// </summary>
        /// <param name="progTokenTableStg1"></param>
        private void tokenClassify(Dictionary<int, string[]> progTokenTableStg1)
        {
            string lolasm = "";
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
                                var varvalue = String.Join("|",curline[indx]);

                                Add2ndStgToken(line, OperandType.Command, "DCLV");
                                Add2ndStgToken(line, OperandType.Variable, varname);
                                Add2ndStgToken(line, OperandType.Expression, varvalue);

                                VariableMemory.Add(varname, new Variable() { name = varname, DataType = DetermineDataType(varvalue), Value = varvalue });
                                lolasm += Newline("DCLV "+ varname + " EQ " + varvalue);
                            }
                            else
                            {
                                Add2ndStgToken(line, OperandType.Command, "DCLN");
                                Add2ndStgToken(line, OperandType.Variable, varname);

                                VariableMemory.Add(varname, new Variable() { name = varname, DataType = DataTypes.NOOB });
                                lolasm += Newline("DCLN " + varname);
                            }
                            indx = tokenCount;
                            break;

                        case "VISIBLE":

                            var command = "CPRT";
                            if (curline.Last() == "!") { command = "CPLN"; } //Switch to visible with feed when ! exist 
                            if (tokenCount > 2)
                            {
                                if (command=="CPLN") { tokenCount -= 1; } //Exclude the ! token
                                for(int vsargs=1; vsargs<tokenCount;vsargs++) //Enumerate all arguments and make separate ASMlike commands
                                {
                                    Add2ndStgToken(line, OperandType.Command, command);
                                    Add2ndStgToken(line, OperandType.Expression, curline[vsargs]);
                                    lolasm += Newline(command + " " + curline[vsargs]);
                                }
                            }
                            else
                            {
                                Add2ndStgToken(line, OperandType.Command, command);
                                Add2ndStgToken(line, OperandType.Expression, curline[indx + 1]);
                                lolasm += Newline(command + " " + curline[indx + 1]);
                            }

                            indx = tokenCount;
                            break;

                        case "GIMMEH":

                            Add2ndStgToken(line, OperandType.Command, "INPT");
                            Add2ndStgToken(line, OperandType.Argument, curline[indx + 1]);
                            lolasm += Newline("INPT" + " " + curline[indx + 1]);

                            indx = tokenCount;
                            break;

                        case "WTF":
                        case "WTF?":
                            BranchingStack.Push(CreateToken(line, OperandType.Command, "SWTC"));
                            lolasm += Newline("SWTC");

                            indx = tokenCount;
                            break;

                        case "O-RLY":
                        case "O-RLY?":

                            BranchingStack.Push(CreateToken(line, OperandType.Command, "COMP"));
                            lolasm += Newline("COMP");

                            indx = tokenCount;
                            break;

                        case "OIC":

                            if (BranchingStack.Count == 0) { ErrorHelper.throwError(ErrorLevel.Fatal, ErrorCodes.STRAY_ENDING, ErrorList, line); break; }
                            var k = BranchingStack.Pop();

                            var labelName = k.tokenStr + "_" + k.lineAddress.ToString();
                            lolasm += Newline(":" + labelName);
                            Add2ndStgToken(line, OperandType.Label, labelName);

                            indx = tokenCount;
                            break;

                        case "YA-RLY":
                        case "YA-RLY?":

                            if (BranchingStack.Count == 0) { ErrorHelper.throwError(ErrorLevel.Fatal, ErrorCodes.STRAY_CONDITIONALS, ErrorList,line); break; }
                            var yr = BranchingStack.Peek();

                            var labelNameYRL = yr.tokenStr + "_" + yr.lineAddress.ToString();
                            Add2ndStgToken(line, OperandType.Command, "JNT");
                            Add2ndStgToken(line, OperandType.Label, labelNameYRL);
                            lolasm += Newline("JNT [" + labelNameYRL + "]");

                            indx = tokenCount;
                            break;

                        case "NO-WAI":
                        case "NO-WAI?":

                            if (BranchingStack.Count == 0) { ErrorHelper.throwError(ErrorLevel.Fatal, ErrorCodes.STRAY_CONDITIONALS, ErrorList, line); break; }
                            var nr = BranchingStack.Peek();

                            var labelNameNRL = nr.tokenStr + "_" + nr.lineAddress.ToString();
                            Add2ndStgToken(line, OperandType.Command, "JNF");
                            Add2ndStgToken(line, OperandType.Label, labelNameNRL);
                            lolasm += Newline("JNF [" + labelNameNRL + "]");

                            indx = tokenCount;
                            break;

                        case "GTFO":

                            if (BranchingStack.Count == 0) { ErrorHelper.throwError(ErrorLevel.Fatal, ErrorCodes.STRAY_CONDITIONALS, ErrorList, line); break; }
                            var gt = BranchingStack.Peek();

                            var labelNameGTFO = gt.tokenStr + "_" + gt.lineAddress.ToString();
                            Add2ndStgToken(line, OperandType.Command, "JMP");
                            Add2ndStgToken(line, OperandType.Label, labelNameGTFO);
                            lolasm += Newline("JMP [" + labelNameGTFO + "]");

                            indx = tokenCount;
                            break;

                        default:
                            if (VariableMemory.ContainsKey(curline[indx]))
                            {
                                
                                if (tokenCount == 1)
                                {
                                    lolasm += Newline("EQUL " + curline[indx]+" "+ "REG_IT");
                                    indx = tokenCount;
                                } else
                                {
                                    if (curline[indx+1] == "R")
                                    {
                                        var x = string.Join(" ", curline, indx + 2, tokenCount - 2);
                                        lolasm += Newline("EQUL "+ curline[indx] + " " + x);
                                        indx = tokenCount;
                                        break;
                                    }
                                }

                                break;
                            }
                            

                            lolasm += Newline(String.Join(" ", curline));
                            indx = tokenCount;
                            break;

                    }
                }
            }
            int lin = 0;
            string filter = "";
            foreach(string ln in lolasm.Split("\r\n".ToCharArray(),StringSplitOptions.RemoveEmptyEntries))
            {
                DebugWin.Print(lin.ToString().PadRight(8) + ln);
                filter += ln;
                lin++;
            }
            lolasm = filter;
        }

        public string Newline(string s)
        {
            return s + "\r\n";
        }
        public void Add2ndStgToken(int programAddress, OperandType OperandType, string tokenStr, DataTypes DataType = DataTypes.NOOB)
        {
            ProgTokenTableStg2.Add(new Token() { OperandType = OperandType, tokenStr = tokenStr, DataType = DataType, lineAddress=programAddress });
        }

        public Token CreateToken(int programAddress, OperandType OperandType, string tokenStr, DataTypes DataType = DataTypes.NOOB)
        {
           return (new Token() { OperandType = OperandType, tokenStr = tokenStr, DataType = DataType, lineAddress = programAddress });
        }

        public string atos(object stringIn)
        {
            return stringIn.ToString();
        }

        /// <summary>
        /// Determines the datatypes of literals
        /// </summary>
        /// <param name="expression"></param>
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
        Variable,
        Argument,
        Label
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
