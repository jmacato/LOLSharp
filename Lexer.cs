using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LOLpreter
{
    
    class Lexer
    {

        const string UNICODE_ELLIPSIS = "\u2026";
        const string UNICODE_SECTION = "\u00A7";
        const string UNICODE_NEWLINE = "\n";

        Dictionary<int, string[]> ProgTokenTable = new Dictionary<int, string[]>();
        Dictionary<string, string> StringConstTable = new Dictionary<string, string>();         
        Dictionary<string, string> StringConstInverseTable= new Dictionary<string, string>();
        List<Error> ErrorList = new List<Error>();

        int StrConstCount = 0;


        public string GenerateStrConst(Match m)
        // Replace each Regex cc match with the number of the occurrence.
        {

            //Remove leading and trailing quotes
            var match = m.ToString().Trim('"');

            //Check if the exact string already exist, if it does, return the existing key
            //From the inverse table
            if (StringConstTable.ContainsValue(match))
            {
                return StringConstInverseTable[match];
            }

            //Generate key
            var key = UNICODE_SECTION + StrConstCount.ToString().PadLeft(8, '0') + UNICODE_SECTION;

            //Add to tables
            StringConstTable.Add(key, match);
            StringConstInverseTable.Add(match, key);

            //Increment the key counter
            StrConstCount++;

            return key;
        }


        public string PreProccess(string raw)
        {
            char[] rawchars = raw.ToCharArray();

            //Reinitialize all working tables
            ProgTokenTable.Clear();  
            StringConstTable.Clear();
            StringConstInverseTable.Clear();
            ErrorList.Clear();
            Debug.WriteLine("-------------------");
            MatchEvaluator StrKeyMatchEvaluator = new MatchEvaluator(GenerateStrConst);


            raw = Regex.Replace(raw, "BTW.*", "");                  //Remove single line comment.
            raw = Regex.Replace(raw, "^/s+|/s+$/g", "");            //Remove whitespace.
            raw = Regex.Replace(raw, "\r", "");                     //Remove Line Returns first.

            string newraw = "";
            string finalraw = "";

            /*
            Get all quoted strings and store them
            to the Constant string table & its inverse
            - match only per line so there'll be no multiline
            - matching, which will throw off this damn thing 
            */
            foreach (string target_line in raw.Split(UNICODE_NEWLINE.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                newraw += Regex.Replace(target_line, @"""[^\""]*""", StrKeyMatchEvaluator, RegexOptions.Singleline)+UNICODE_NEWLINE;
            }

            //Check if theres remaining quotes, and tantrum when there is.
            CheckForUnclosedQuotes(newraw);

            //Check if theres a invalid obtw/tldr 
            CheckForInvalidMultilineComment(newraw);

            raw = Regex.Replace(newraw, @"\.\.\.", "");                        //Remove elipsis
            raw = Regex.Replace(newraw, UNICODE_ELLIPSIS.ToString(), ""); //Remove elipsis pt2
            raw = Regex.Replace(newraw, ",", UNICODE_NEWLINE);                            //Convert soft linefeed to hard 
            raw = Regex.Replace(newraw, "\t", "");                             //Remove Tabs
            raw = Regex.Replace(newraw, @"OBTW([\s\S] *?)TLDR", "");           //Remove multiline comment

            //Loop again on the entire program and trim it to dust.
            foreach (string target_line in newraw.Split(UNICODE_NEWLINE.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                finalraw += target_line.Trim() + "\r\n";
            }
            
            //Trim the leading and trailing newlines
            finalraw = finalraw.Trim("\r\n".ToCharArray());

            foreach(Error Errors in ErrorList)
            {
                Debug.WriteLine(ErrorHelper.generateErrorMessage(Errors));
            }

            //Return nothing when there is a error
            if (ErrorHelper.CountBreakingErrors(ErrorList) > 0) { return null; }

            Debug.WriteLine("Preprocessing complete, Passing to tokenizer...");
            Debug.WriteLine("-------------------");

            return finalraw;
        }

        private void CheckForUnclosedQuotes(string raw)
        {
            int line = 1,pos;
            foreach (string target_line in raw.Split(UNICODE_NEWLINE.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                if (target_line.Contains('"'))
                {
                    var unclosedquote = Regex.Match(target_line, "\"");
                    pos = unclosedquote.Index + 1;
                    ErrorHelper.throwError(ErrorLevel.FATAL, ErrorCodes.UNTERMINATED_STRING_DELIMITER, line, pos, ErrorList);
                }
                line++;
            }
        }

        private void CheckForInvalidMultilineComment(string raw)
        {
            int line = 1, pos;
            foreach (string target_line in raw.Split(UNICODE_NEWLINE.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                var ntarget_line = target_line.Replace("\r","").Replace("\n","").Trim();
                if (ntarget_line.Contains("TLDR"))
                {
                    if (ntarget_line.Replace("TLDR","") != "")
                    {
                        var unclosedquote = Regex.Match(target_line, "TDLR");
                        pos = unclosedquote.Index + 1;
                        ErrorHelper.throwError(ErrorLevel.ERROR, ErrorCodes.INVALID_MULTILINE_COMMENT, line, pos, ErrorList);
                    }

                }
                line++;
            }
        }

        public string Seek(char[] raw, int index, int length = 1)
        {
            string ret = "";
            for (int i = index; i < length; i++){
                ret += raw[i];
            }
            return ret;
        }
    }

    class LexemeNode
    {
        Lexeme PreviousNode { get; set; }
        Lexeme Content { get; set; }
        Lexeme NextNode { get; set; }

        public LexemeNode CreateLexemeNode(Lexeme PreviousNode, Lexeme Content, Lexeme NextNode)
        {
            LexemeNode Node = new LexemeNode();
            Node.PreviousNode = PreviousNode;
            Node.Content = Content;
            Node.NextNode = NextNode;
            return Node;
        } 

    }
    public struct Lexeme
    {
        int Address;
        string Token;
        string Expression;
    }


}
