using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LOLpreter
{
    public enum ErrorCodes
    {
        NO_PROG_START,
        NO_PROG_END,
        UNKNOWN_ESCAPE_SEQUENCE,
        UNTERMINATED_STRING_DELIMITER,
        INVALID_MULTILINE_COMMENT
    }

    public enum ErrorLevel
    {
        WARNING, //I MEZZED UP A LIL
        ERROR,   //I POURED MILKZ INTO ZE KEYBOARDZ
        FATAL    //GTFO RN 
    }

    public struct Error
    {
        public ErrorLevel ErrorLevel;
        public ErrorCodes ErrorCode;
        public int line;
        public int position;
    }
    
    public static class ErrorHelper
    {

        //Oopsie, somebody messed up ze lolz
        public static void throwError(ErrorLevel ErrorLevel, ErrorCodes ErrorCode, int line, int pos, List<Error> ErrorList)
        {
            Error curError = new Error();
            curError.line = line + 1;
            curError.position = pos + 1;
            curError.ErrorCode = ErrorCode;
            curError.ErrorLevel = ErrorLevel;
            ErrorList.Add(curError);
        }

        //Generate standard error message
        public static string generateErrorMessage(Error Err)
        {
            return (Err.ErrorLevel.ToString() + " : " + Err.ErrorCode.ToString() + " at line " + Err.line.ToString() + ", character " + Err.position.ToString());
        }

        //Count all unignorable errors
        public static int CountBreakingErrors(List<Error> ErrList)
        {
            int totalerrors = 0;
            if (ErrList.Count == 0) { return totalerrors; }
            foreach (Error Errors in ErrList)
            {
                if (Errors.ErrorLevel == ErrorLevel.FATAL | Errors.ErrorLevel == ErrorLevel.ERROR)
                {
                    totalerrors += 1;
                }
            }
            return totalerrors;
        }

    }

}
