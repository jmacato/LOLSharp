using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace LOLpreter
{
    public enum ErrorCodes
    {
        COMPILER_ERROR,
        NO_PROG_START,
        NO_PROG_END,
        UNKNOWN_ESCAPE_SEQUENCE,
        UNTERMINATED_STRING_DELIMITER,
        INVALID_MULTILINE_COMMENT,
        STRAY_COMMENT_DELIMITER,
        STRAY_ENDING,
        STRAY_CONDITIONALS,
        SYNTAX_ERROR,
        MULTIPLE_DECLARATION,
        UNKNOWN_OR_UNDECLARED_VARIABLE
    }

    public enum ErrorLevel
    {
        Warning, //I MEZZED UP A LIL
        Error,   //I POURED MILKZ INTO ZE KEYBOARDZ
        Fatal    //GTFO RN 
    }

    public class Error
    {
        public ErrorLevel ErrorLevel { get; set; }
        public ErrorCodes ErrorCode { get; set; }
        public Int64 line { get; set; }
        public Int64 position { get; set; }
    }
    
    public static class ErrorHelper
    {

        //Oopsie, somebody messed up ze lolz
        public static void throwError(ErrorLevel ErrorLevel, ErrorCodes ErrorCode, List<Error> ErrorList, Int64 line = 0, Int64 pos = 0)
        {
            Error curError = new Error();
            curError.line = line;
            curError.position = pos;
            curError.ErrorCode = ErrorCode;
            curError.ErrorLevel = ErrorLevel;
            ErrorList.Add(curError);
        }

        //Generate standard error message
        public static string generateErrorMessage(Error Err)
        {
            if (Err.line == 0)
            {
                return (Err.ErrorLevel.ToString() + " : " + Err.ErrorCode.ToString());
            }
            return (Err.ErrorLevel.ToString() + " : " + Err.ErrorCode.ToString() + " at line " + Err.line.ToString() + ", character " + Err.position.ToString());
        }

        //Count all unignorable errors
        public static Int64 CountBreakingErrors(List<Error> ErrList)
        {
            Int64 totalerrors = 0;
            if (ErrList.Count == 0) { return totalerrors; }
            foreach (Error Errors in ErrList)
            {
                if (Errors.ErrorLevel == ErrorLevel.Fatal | Errors.ErrorLevel == ErrorLevel.Error)
                {
                    totalerrors += 1;
                }
            }
            return totalerrors;
        }

    }

}
