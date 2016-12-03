using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace LOLpreter
{
    public class Tokenizer
    {
        public DebugWindow DebugWin;

        #region Declares
        public List<Error> ErrorList;
        //First pass token table
        public Dictionary<Int64,string[]> ProgTokenTableStg1 = new Dictionary<Int64, string[]>();
        //Second table for classified tokens
        public List<Token> ProgTokenTableStg2 = new List<Token>();
        //Lists definitions of each token, to be displayed in debugwin
        public Dictionary<Token, string> TokenDescriptorTable = new Dictionary<Token, string>();
        public Dictionary<string, Variable> VariableMemory = new Dictionary<string, Variable>();

        public Stack<Token> BranchingStack = new Stack<Token>();

        public static Dictionary<string, Variable> BuiltInVariables()
        {
            Dictionary<string, Variable> x = new Dictionary<string, Variable>() {
            {"IT", new Variable() { name="IT", DataType=DataTypes.NOOB, value=null} },
            {"REG_CMP", new Variable() { name="REG_CMP", DataType=DataTypes.NOOB, value=null}},
            {"CON_BUF", new Variable() { name="CON_BUF", DataType=DataTypes.NOOB, value=null}}

            };
            return x;
        }

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
                                        {@"BOTH OF","_AND"},
                                        {@"NOT","_NOT"},
                                        {@"EITHER OF","_OR"},
                                        {@"WON OF","_XOR"},
                                        {@"ALL OF","AAND"},
                                        {@"ANY OF","A_OR"},
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

        public string lolasm = "";
        public List<string> prog_asm = new List<string>();
        public Dictionary<string,Int64> jumplabels = new Dictionary<string, Int64>();


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
            VariableMemory = BuiltInVariables();
            prog_asm.Clear();
            BranchingStack.Clear();
            jumplabels.Clear();
            lolasm = "";

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
            tokenRestruct(ProgTokenTableStg1);
        }

        /// <summary>
        /// Big-ass function to restructure the program
        /// </summary>
        /// <param name="progTokenTableStg1"></param>
        private void tokenRestruct(Dictionary<Int64, string[]> progTokenTableStg1)
        {
            for (Int64 line = 0;line < progTokenTableStg1.Keys.ToArray().Length; line++)
            {
                string[] curline = ProgTokenTableStg1[line];
                Int64 tokenCount = curline.Length;
                for (Int64 indx = 0; indx < tokenCount; indx++){
                    string frame = curline[indx].Trim();
                    switch (frame)
                    {
                        case "HAI": //Program start
                            lolasm += Newline("START");
                            indx = tokenCount;
                            break;
                        case "KTHXBYE": //Program end
                            lolasm += Newline("END");
                            indx = tokenCount;
                            break;
                        case "I-HAS-A": //Variable declare
                            indx++;
                            var varname = curline[indx];
                            indx++;
                            if (tokenCount > 2 && curline[indx] == "ITZ")
                            {
                                indx++;
                                var varvalue = String.Join("|", curline.Skip(3));
                                VariableMemory.Add(varname, new Variable() { name = varname, DataType = DetermineDataType(varvalue), value = varvalue });
                                lolasm += Newline("DCLV "+ varname + " EQ " + varvalue);
                            }
                            else
                            {
                                VariableMemory.Add(varname, new Variable() { name = varname, DataType = DataTypes.NOOB });
                                lolasm += Newline("DCLN " + varname);
                            }
                            indx = tokenCount;
                            break;

                        case "VISIBLE":

                            String.Join(" ", curline.Skip(2));

                            for (Int64 vsargs = 1; vsargs < tokenCount; vsargs++) //Enumerate all arguments and make separate ASMlike commands
                            {
                                lolasm += Newline("CPRT " + curline[vsargs]);
                            }

                            indx = tokenCount;
                            break;

                        case "GIMMEH":
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
                            jumplabels.Add(labelName, line);
                            lolasm += Newline("NOP");
                            indx = tokenCount;
                            break;

                        case "YA-RLY":
                        case "YA-RLY?":

                            if (BranchingStack.Count == 0) { ErrorHelper.throwError(ErrorLevel.Fatal, ErrorCodes.STRAY_CONDITIONALS, ErrorList,line); break; }
                            var yr = BranchingStack.Peek();

                            var labelNameYRL = yr.tokenStr + "_" + yr.lineAddress.ToString();
                            lolasm += Newline("JNT " + labelNameYRL + "");

                            indx = tokenCount;
                            break;

                        case "NO-WAI":
                        case "NO-WAI?":

                            if (BranchingStack.Count == 0) { ErrorHelper.throwError(ErrorLevel.Fatal, ErrorCodes.STRAY_CONDITIONALS, ErrorList, line); break; }
                            var nr = BranchingStack.Peek();

                            var labelNameNRL = nr.tokenStr + "_" + nr.lineAddress.ToString();
                            lolasm += Newline("JNF " + labelNameNRL + "");

                            indx = tokenCount;
                            break;

                        case "GTFO":

                            if (BranchingStack.Count == 0) { ErrorHelper.throwError(ErrorLevel.Fatal, ErrorCodes.STRAY_CONDITIONALS, ErrorList, line); break; }
                            var gt = BranchingStack.Peek();

                            var labelNameGTFO = gt.tokenStr + "_" + gt.lineAddress.ToString();
                            lolasm += Newline("JMP " + labelNameGTFO + "");
                            indx = tokenCount;
                            break;

                        case "ADD":
                        case "SUB":
                        case "PROD":
                        case "QUOT":
                        case "MOD":
                        case "MAX":
                        case "MIN":
                        case "_AND":
                        case "__OR":
                        case "_XOR":
                        case "_NOT":
                        case "AAND":
                        case "A_OR":
                            lolasm += Newline("ASGN IT "+String.Join(" ", curline));
                            break;
                        default:
                            if (VariableMemory.ContainsKey(curline[indx]))
                            {                                
                                if (tokenCount == 1)
                                {
                                    lolasm += Newline("ASGN IT " + curline[indx]); //IT <-- variable
                                    indx = tokenCount;
                                } else
                                {
                                    if (curline[indx+1] == "R")
                                    {
                                        var x = String.Join(" ", curline.Skip(2));
                                        lolasm += Newline("ASGN " + curline[indx] + " " + x);
                                        indx = tokenCount;
                                        break;
                                    }
                                }
                                break;
                            }
                            

                            lolasm += Newline(String.Join(" ", curline));
                            ErrorHelper.throwError(ErrorLevel.Error, ErrorCodes.SYNTAX_ERROR, ErrorList, line);
                            indx = tokenCount;
                            break;

                    }
                }
            }


            //Print the processed assembly and remove empty lines
            Int64 lin = 0;
            foreach (string ln in lolasm.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                prog_asm.Add(ln);
            }

            List<string> tempproglist = new List<string>();

            //Replace labels with hard-address
            foreach(string prgline in prog_asm)
            {
                var x = prgline;
                foreach(string lbl in jumplabels.Keys)
                {
                    x = x.Replace(lbl, "0x" + jumplabels[lbl].ToString("X"));
                }
                tempproglist.Add(x);
            }

            prog_asm = tempproglist;

            lin = 0;
            foreach (string item in prog_asm)
            {
               DebugWin.Print(lin.ToString("X").PadRight(8) + item);
                lin++;
            }

            //lolasm = filter;

        }

        public string Newline(string s)
        {
            return s + "\r\n";
        }
        public void Add2ndStgToken(Int64 programAddress, OperandType OperandType, string tokenStr, DataTypes DataType = DataTypes.NOOB)
        {
            ProgTokenTableStg2.Add(new Token() { OperandType = OperandType, tokenStr = tokenStr, DataType = DataType, lineAddress=programAddress });
        }

        public Token CreateToken(Int64 programAddress, OperandType OperandType, string tokenStr, DataTypes DataType = DataTypes.NOOB)
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
        public object value { get; set; }

    }

    public class Token
    {
        public Int64 lineAddress { get; set; }
        public OperandType OperandType { get; set; }
        public string tokenStr { get; set; }
        public DataTypes DataType { get; set; }
    }

}


