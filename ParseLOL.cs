using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LOLpreter
{
    public static class ParseLOL
    {
        public static Dictionary<string, string> LexemeDefinitions;
        public static Dictionary<string, string> DTRegex;
        public static Dictionary<DataType, string> DTDesc;
        public static Dictionary<string, string> MonoglyphyOperators;

        public struct Variable
        {
            DataType CurrentDataType;
            object Value;
            
        }

        
        public static object Initialize()
        {
            MonoglyphyOperators = new Dictionary<string, string>
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

            DTDesc = new Dictionary<DataType, string>
                        {
                            {DataType.FLOT,"Float Literal"},
                            {DataType.BOOL,"Boolean Literal"},
                            {DataType.INT,"Integer Literal"},
                            {DataType.STRING,"String"},
                            {DataType.VARIABLE,"Variable"},
                            {DataType.UNKNOWN,"Unknown" }
                        };

            DTRegex = new Dictionary<string, string>
                        {
                            {@"^(-?[1-9]+\d*)$|^0$","Integer Literal"},
                            {@"^[0-9]*(?:\.[0-9]*)?$","Float Literal"},
                            {@"^[+-][0-9]*(?:\.[0-9]*)?$","Float Literal"},
                            {@"WIN|FAIL","Boolean Literal"},
                            {@"""[^\""]*""","String"},
                            {@"[A-Za-z][A-Za-z0-9_]*","Variable"}
                        };

            /*
            Definitions of each reserved keywords
            */
            LexemeDefinitions = new Dictionary<string, string>
                        {
                            //Multi-spaced operands takes precedence
                            {@"I-HAS-A","Declare variable"},
                            {@"BOTH-SAEM","Comparison Operator"},
                            {@"SUM-OF","Addition Operator"},
                            {@"DIFF-OF","Subtraction Operator"},
                            {@"PRODUKT-OF","Multiplication Operator"},
                            {@"QUOSHUNT-OF","Division Operator"},
                            {@"MOD-OF","Modulo (Remainder) Operator"},
                            {@"BIGGR-OF","Greater-than Operator; Returns the greater variable"},
                            {@"SMALLR-OF","Lesser-than Operator; Returns the lesser variable"},
                            {@"O-RLY?","Start of If-Else Block"},
                            {@"YA-RLY","[If-Else] Executes inline code when condition is true"},
                            {@"NO-WAI","[If-Else] Executes inline code when condition is true and closes the If-Else Block"},
                            {@"IM-IN-YR","Signals the start of a loop. Followed by a label."},
                            {@"IM-OUTTA-YR"," Signals the end of a loop.Followed by a label."},
                            {@"HOW-IZ-I"," Declare function. Followed by the function name."},
                            {@"IF-U-SAY-SO","Closes a function block."},
                            {@"I-IZ","Calls a function. Followed by the function name and its parameters."},
                            {@"BOTH-OF","AND Boolean Operator; 1 or 2 operands"},
                            {@"EITHER-OF","OR Boolean Operator; 1 or 2 operands"},
                            {@"WON-OF","XOR Boolean Operator; Infinite operands"},
                            {@"ALL-OF","AND Boolean Operator; Infinite operands"},
                            {@"ANY-OF","OR Boolean Operator; Infinite operands"},
                            {@"IS-NOW-A","Type Recasting"},
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
            return null;
        }
            
        //For easier classification of in-program types
        public enum DataType
        {
            FLOT,
            BOOL,
            INT,
            STRING,
            VARIABLE,
            UNKNOWN
        }
        //Check if token is a reserved word
        public static bool IsKeyword(string token)
        {
            var keywrd = from Arg in ParseLOL.LexemeDefinitions
                         where Regex.Match(token, Arg.Key, RegexOptions.Singleline).Success
                         select Arg;
            return keywrd != null && keywrd.Any();
        }

        public static KeyArgPair SplitKeysArgs(string sx)
        {
            string s = sx.Trim();
            string Keyword = s.Split(' ').First();
            string Argument = s.Substring(Keyword.Length, s.Length - Keyword.Length);
            KeyArgPair x;
            x.Keyword = Keyword.Trim();
            x.Argument = Argument.Trim();
            return x;
        }

        public struct KeyArgPair
        {
            public string Keyword;
            public string Argument;
        }

        //Get datatype of the expression
        public static DataType GetArgDataType(string expression)
        {
            var results = from Arg in ParseLOL.DTRegex
                          where Regex.Match(expression, Arg.Key, RegexOptions.Singleline).Success
                          select Arg;

            

            foreach (var result in results)
            {
                switch (result.Value)
                {
                    case "Float Literal":
                        return ParseLOL.DataType.FLOT;
                    case "Boolean Literal":
                        return ParseLOL.DataType.BOOL;
                    case "Integer Literal":
                        return ParseLOL.DataType.INT;
                    case "String":
                        return ParseLOL.DataType.STRING;
                    case "Variable":
                        return ParseLOL.DataType.VARIABLE;
                    default:
                        return ParseLOL.DataType.UNKNOWN;
                }
            }
            return ParseLOL.DataType.UNKNOWN;
        }

        //Get the value of the argument
        public static object GetArgDataContent(string token)
        {
            var results = from Arg in ParseLOL.DTRegex
                          where Regex.Match(token, Arg.Key, RegexOptions.Singleline).Success
                          select Arg;
            foreach (var result in results)
            {
                switch (result.Value)
                { // All numerical data are signed. Keep it in mind to avoid being bitten in the ass
                    case "Float Literal":
                        return Convert.ToDouble(Regex.Match(token, result.Key).Value);
                    case "Boolean Literal":
                        if (token=="WIN") { return true; }
                        if (token=="FAIL") { return false; }
                        return null;
                    case "Integer Literal": 
                        return Convert.ToInt64(Regex.Match(token, result.Key).Value);
                    case "String":
                        return Regex.Match(token, result.Key).Value;
                    case "Variable":
                        return Regex.Match(token, result.Key).Value;
                    default:
                        return null;
                }
            }
            return ParseLOL.DataType.UNKNOWN;
        }


    }
}
