﻿using System;
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
        STRAY_COMMENT_DELIMITER
    }

    public enum ErrorLevel
    {
        WARNING, //I MEZZED UP A LIL
        ERROR,   //I POURED MILKZ INTO ZE KEYBOARDZ
        FATAL    //GTFO RN 
    }

    public class Error
    {
        public ErrorLevel ErrorLevel { get; set; }
        public ErrorCodes ErrorCode { get; set; }
        public int line { get; set; }
        public int position { get; set; }
    }
    
    public static class ErrorHelper
    {

        //Oopsie, somebody messed up ze lolz
        public static void throwError(ErrorLevel ErrorLevel, ErrorCodes ErrorCode, ObservableCollection<Error> ErrorList, int line = 0xFFFF, int pos = 0xFFFF)
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
            if (Err.line == 65536)
            {
                return (Err.ErrorLevel.ToString() + " : " + Err.ErrorCode.ToString());
            }
            return (Err.ErrorLevel.ToString() + " : " + Err.ErrorCode.ToString() + " at line " + Err.line.ToString() + ", character " + Err.position.ToString());
        }

        //Count all unignorable errors
        public static int CountBreakingErrors(ObservableCollection<Error> ErrList)
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
