using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LOLpreter
{
    class Interpreter
    {
        public Console Console;
        public Tokenizer Tokenizer;

        public Dictionary<string, string> StringTable { get; set; }
        public Dictionary<string, Variable> WorkingMem = new Dictionary<string, Variable>() {
            {"IT", null } ,   {"REG_CMP",null }
        };

        BackgroundWorker ExecutionThread = new BackgroundWorker();
        public string[] prog;

        public Interpreter()
        {
            Console = new Console();
            Tokenizer = new Tokenizer();
            ExecutionThread.WorkerSupportsCancellation = true;
            ExecutionThread.DoWork += ExecutionThread_DoWork;

        }

        private void ExecutionThread_DoWork(object sender, DoWorkEventArgs e)
        {

            for (Int64 l = 0; l < prog.Length - 1; l++){

                string[] lex;
                lex = prog[l].Split(' ');
                var head = lex[0];

                for (Int64 i = 0; i < lex.Length +1; i++)
                {
                    switch (head)
                    {
                        case "START":
                            Console.Clear();
                            i = lex.Length;
                            break;

                        case "CPRT":
                            if (lex[i + 1] == "!")
                            {
                                i++;
                                Console.WriteLine("");
                                i = lex.Length;
                                break;
                            }
                            else if (WorkingMem.ContainsKey(lex[i + 1]) == true)
                            {
                                i++;
                                string tmp = WorkingMem[lex[i]].value.ToString();
                                if (tmp.Contains(Lexer.UNICODE_SECTION)){
                                    tmp = StringTable[tmp];
                                }
                                Console.Write(tmp);
                                i = lex.Length;
                                break;
                            }
                            else if (StringTable.ContainsKey(lex[i + 1]))
                            {
                                i++;
                                Console.Write(StringTable[lex[i]].ToString());
                                i = lex.Length;
                                break;
                            }
                            i = lex.Length;
                            break;

                        case "NOP":
                            i = lex.Length;
                            break;
                        case "INPT":
                            i = lex.Length;
                            break;
                        case "DCLV":

                            var varname = lex[i+1];
                            object value = lex[i+3];

                            if (WorkingMem.ContainsKey(varname))
                            {
                                ErrorHelper.throwError(ErrorLevel.Error, ErrorCodes.MULTIPLE_DECLARATION, Tokenizer.ErrorList, l);
                                return;
                            }

                            var dt_dclv = Tokenizer.DetermineDataType(value.ToString());
                            WorkingMem.Add(varname, new Variable() { name = varname, DataType = dt_dclv, value = value });

                            i = lex.Length;
                            break;
                        case "DCLN":
                            var varname_n = lex[i+1];

                            if (WorkingMem.ContainsKey(varname_n)) {
                                ErrorHelper.throwError(ErrorLevel.Error, ErrorCodes.MULTIPLE_DECLARATION, Tokenizer.ErrorList, l);
                                return;
                            }

                            WorkingMem.Add(varname_n, new Variable() { name = varname_n, DataType = DataTypes.NOOB, value = ""});
                            i = lex.Length;
                            break;
                    }
                }
            }

        }

        public void Run(string[] progin)
        {
            WorkingMem.Clear();
            if (Console.Visibility == Visibility.Collapsed) { Console.Visibility = Visibility.Visible;}
            prog = progin;
            ExecutionThread.RunWorkerAsync();
        }

        public Stack<string> OperatorStack = new Stack<string>();
        public Stack<object> LiteralsStack = new Stack<object>();


    }
}
