using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Windows;

namespace LOLpreter
{
    
    class Lexer
    {

        public const string UNICODE_ELLIPSIS = "\u2026";
        public const string UNICODE_SECTION = "\u00A7";
        public const string UNICODE_NEWLINE = "\n";

        //Stores Program Table
        public Dictionary<string, string> StringConstTable = new Dictionary<string, string>();
        public Dictionary<string, string> StringConstInverseTable= new Dictionary<string, string>();
        public ObservableCollection<Error> ErrorList = new ObservableCollection<Error>();


        /// <summary>
        /// LOLCODE Preprocessing function
        /// ~ Where all magic starts... ~
        /// </summary>
        /// <param name="raw">The raw text to process</param>
        /// <returns>Returns null when there's a breaking errror; 
        /// Returns the processed code when done.</returns>
        public string PreProccess(string raw)
        {
            //Reinitialize all working tables
            StringConstTable.Clear();
            StringConstInverseTable.Clear();
            ErrorList.Clear();

            Debug.WriteLine("-------------------");

            FirstFilter(ref raw);
            TrimToDust(ref raw);
            CheckForProgStartEnd(raw);
            ProcessComments(ref raw);
            GenerateStrConstTable(ref raw);
            SecondFilter(ref raw);
            CheckForUnclosedQuotes(raw);
            TrimToDust(ref raw);

            //Return nothing when there is a error
            if (ErrorHelper.CountBreakingErrors(ErrorList) > 0)
            {
                foreach (Error Errors in ErrorList)
                {
                    
                    Debug.WriteLine(ErrorHelper.generateErrorMessage(Errors));
                    Debug.WriteLine("-------------------");
                }
                return null;
            }

            Debug.WriteLine("Preprocessing complete, Passing to tokenizer...");
            Debug.WriteLine("-------------------");

            return raw;
        }

        /// <summary>
        /// Check for program start and end markers, throw up when theres none.
        /// </summary>
        /// <param name="raw">The raw text to process</param>
        private void CheckForProgStartEnd(string raw)
        {
            try
            {
                var x = Regex.Replace(raw.Trim(), "\\s+", "");
                if (x.Substring(0, 3) != "HAI")
                {
                    ErrorHelper.throwError(ErrorLevel.FATAL, ErrorCodes.NO_PROG_START, ErrorList);
                }
                if (x.Substring(x.Length - 7, 7) != "KTHXBYE")
                {
                    ErrorHelper.throwError(ErrorLevel.FATAL, ErrorCodes.NO_PROG_END, ErrorList);
                }
            } catch (Exception ex)
            {
                ErrorHelper.throwError(ErrorLevel.FATAL, ErrorCodes.COMPILER_ERROR, ErrorList);
                Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
                return;
            }

        }

        /// <summary>
        ///    Get all quoted strings and store them to the Constant string table & its inverse
        ///    match only per line so there'll be no multiline matching, which will throw off this damn thing 
        /// </summary> 
        private void GenerateStrConstTable(ref string raw)
        {
            MatchEvaluator StrKeyMatchEvaluator = new MatchEvaluator(MakeStrConstKey);
            string newraw = "";
            foreach (string target_line in raw.Split(UNICODE_NEWLINE.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                newraw += Regex.Replace(target_line, @"""[^\""]*""", StrKeyMatchEvaluator, RegexOptions.Singleline) + UNICODE_NEWLINE;
            }
            raw = newraw;
        }

        /// <summary>
        /// Trimming all leading and trailing whitespace, line-by-line.
        /// </summary>
        /// <param name="newraw">The raw text to process</param>
        private void TrimToDust(ref string newraw)
        {
            string finalraw = "";
            foreach (string target_line in newraw.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                finalraw += target_line.Trim().Trim('\t').Trim('\n') + "\r\n";
            }

            //Trim the leading and trailing newlines
            finalraw = finalraw.Trim("\r\n".ToCharArray());
            newraw = finalraw;
        }
        
        /// <summary>
        /// First RegEx Filter 
        /// </summary>
        /// <param name="raw">The raw text to process</param>
        private void FirstFilter(ref string raw)
        {
            raw = Regex.Replace(raw, "^/s+|/s+$/g", "");            //Remove whitespace.
            raw = Regex.Replace(raw, "\r", "");                     //Remove Line Returns first.
            raw = Regex.Replace(raw, "\n", "\r\n");                     //Remove Line Returns first.

        }

        /// <summary>
        /// Second RegEx Filter 
        /// </summary>
        /// <param name="raw">The raw text to process</param>
        private void SecondFilter(ref string raw)
        {
            raw = Regex.Replace(raw, @"\.\.\.\r\n", "");                      //Remove elipsis with newline
            raw = Regex.Replace(raw, UNICODE_ELLIPSIS+"\r\n", "");   //Remove elipsis pt2
            raw = Regex.Replace(raw, ",", "\r\n");                   //Convert soft linefeed to hard 
            raw = Regex.Replace(raw, "\t", "");                               //Remove Tabs
            raw = Regex.Replace(raw, @"AN \b|OF \b| AN\b| OF\b", "");         //Remove Arity and OF's 
            raw = Regex.Replace(raw, "[ ]{2,}", " ",RegexOptions.Multiline);  //Remove Tabs
        }

        /// <summary>
        /// Checks for unterminated string quotations.
        /// </summary>
        /// <param name="raw">The raw text to process</param>
        private void CheckForUnclosedQuotes(string raw)
        {
            int line = 1,pos;
            foreach (string target_line in raw.Split(UNICODE_NEWLINE.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                if (target_line.Contains('"'))  //Remaining quotes found means unclosed string delimiter, so throw up
                {
                    var unclosedquote = Regex.Match(target_line, "\"");
                    pos = unclosedquote.Index + 1;
                    ErrorHelper.throwError(ErrorLevel.FATAL, ErrorCodes.UNTERMINATED_STRING_DELIMITER, ErrorList, line, pos);
                }
                line++;
            }
        }

        /// <summary>
        /// Checks for stray comment delimiters and filters all valid comments.
        /// </summary>
        /// <param name="raw">The raw text to process</param>
        private void ProcessComments(ref string raw)
        {
            string newraw = "";
            foreach (string target_line in raw.Split(UNICODE_NEWLINE.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                newraw += Regex.Replace(target_line, @"""[^\""]*""", "$COLLAPSED$", RegexOptions.Singleline) + UNICODE_NEWLINE;
            }

            int line = 1, pos;
            foreach (string target_line in newraw.Split(UNICODE_NEWLINE.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                var ntarget_line = target_line.Replace("\r","").Replace("\n","").Trim();
                if (ntarget_line.Contains("TLDR")) //If line contains TLDR
                {
                    if (ntarget_line.Replace("TLDR","") != "") //If TLDR removed and still contains stuff, throw up
                    {
                        var unclosedquote = Regex.Match(target_line, "TDLR");
                        pos = unclosedquote.Index + 1;
                        ErrorHelper.throwError(ErrorLevel.ERROR, ErrorCodes.INVALID_MULTILINE_COMMENT, ErrorList, line, pos);
                    }
                }
                line++;
            }

            //HACK HACK: Replace comments with C style ones since regexing them is pain in the butt
            raw = raw.Replace("OBTW", "/*").Replace("TLDR", "*/").Replace("BTW","//");

            //Remove them properly, phew.
            string multilinec = @"/\*(.*?)\*/";
            string singllinec = @"//(.*?)\r?\n";
            string strings = @"""((\\[^\n]|[^""\n])*)""";

            raw = Regex.Replace(raw,
            multilinec + "|" + singllinec + "|" + strings,
            me => { if (me.Value.StartsWith("/*") || me.Value.StartsWith("//"))
                    return me.Value.StartsWith("//") ? Environment.NewLine : "";
                // Keep the literal strings
            return me.Value;}, RegexOptions.Singleline);

            //Now check for unprocessed comment markers, and throw up when there is
            line = 1; pos = 1;
            foreach (string target_line in raw.Split(UNICODE_NEWLINE.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                var ntarget_line = target_line.Replace("\r", "").Replace("\n", "").Trim();
                if (ntarget_line.Contains("/*") || ntarget_line.Contains("*/") || ntarget_line.Contains("//"))
                {
                    var unclosedquote = Regex.Match(target_line, "TDLR");
                    pos = unclosedquote.Index + 1;
                    ErrorHelper.throwError(ErrorLevel.ERROR, ErrorCodes.STRAY_COMMENT_DELIMITER, ErrorList, line, pos);
                }
                line++;
            }

        }

        /// <summary>
        /// Generate a indexable key for the String Constants Table.
        /// </summary>
        /// <param name="m">The matching string to be stored.</param>
        public string MakeStrConstKey(Match m)
        {

            int StrConstCount = StringConstTable.Count();

            //Remove leading and trailing quotes
            var match = m.ToString().Trim('"');

            //Check if the exact string already exist, if it does, return the existing key
            //From the inverse table
            if (StringConstTable.ContainsValue(match))
            {
                return StringConstInverseTable[match];
            }

            //Generate key
            var key = UNICODE_SECTION + StrConstCount.ToString("X").PadLeft(8, '0') + UNICODE_SECTION;

            //Add to tables
            StringConstTable.Add(key, match);
            StringConstInverseTable.Add(match, key);

            //Increment the key counter
            StrConstCount++;

            return key;
        }

        // UNUSED, for now //
        public string Seek(char[] raw, int index, int length = 1)
        {
            string ret = "";
            for (int i = index; i < length; i++){
                ret += raw[i];
            }
            return ret;
        }
    }
}
