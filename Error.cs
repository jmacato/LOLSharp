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
        WARNING,
        ERROR,
        FATAL
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
        public static string ErrorLevelDescriptor(ErrorLevel ErrLvl)
        {
            switch (ErrLvl)
            {
                case ErrorLevel.WARNING:
                    return "WARNING";
                case ErrorLevel.FATAL:
                    return "FATAL";
                case ErrorLevel.ERROR:
                    return "ERROR";
                default:
                    return "UNKNOWN";
            }
        }


        public static void throwError(ErrorLevel ErrorLevel, ErrorCodes ErrorCode, int line, int pos, List<Error> ErrorList)
        {
            Error curError = new Error();
            curError.line = line + 1;
            curError.position = pos + 1;
            curError.ErrorCode = ErrorCode;
            curError.ErrorLevel = ErrorLevel;
            ErrorList.Add(curError);
        }

        public static string generateErrorMessage(Error Err)
        {
            return (ErrorLevelDescriptor(Err.ErrorLevel) + " : " + Err.ErrorCode.ToString() + " at line " + Err.line.ToString() + ", pos " + Err.position.ToString());
        }

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
