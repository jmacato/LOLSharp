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
        public MainWindow MainWindow;

        public Dictionary<string, string> StringTable { get; set; }
        public Dictionary<string, Variable> WorkingMem = new Dictionary<string, Variable>();

        BackgroundWorker ExecutionThread = new BackgroundWorker();
        public string[] prog;

        public Interpreter()
        {
            Console = new Console();
            Tokenizer = new Tokenizer();
            ExecutionThread.WorkerSupportsCancellation = true;
            ExecutionThread.DoWork += ExecutionThread_DoWork;
            ExecutionThread.RunWorkerCompleted += ExecutionThread_RunWorkerCompleted; ;

        }

        private void ExecutionThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("\r\n\r\n[PROGRAM TERMINATED]");
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

                        case "END":
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
                            else if (CheckIfVar(lex[i + 1]) == true)
                            {
                                i++;
                                string tmp = GetVar(lex[i]).ToString();
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
                            var inputvar = lex[i + 1];
                            if (!CheckIfVar(inputvar)) { return; }
                            Console.StartIn();
                            while (Console.awaitinput) { System.Threading.Thread.Sleep(2); }
                            SetVar(inputvar, Console.ReadBuffer());
                            i = lex.Length;
                            break;

                        case "ASGN":
                            i++;
                            var ASGNvar = lex[i];

                            var ASGN_ExprArray = lex.Skip(2).Take(lex.Length - 2).ToArray();
                            var ASGN_ExprString = String.Join(" ", ASGN_ExprArray);
                            var ASGN_ExpType = DetExpressionType(ASGN_ExprString);
                            SetVar(ASGNvar, ProcessExpression(ASGN_ExprArray));
                            i = lex.Length;
                            break;

                        case "DCLV":
                            var varname = lex[i+1];
                            object value = lex[i+3];

                            if (WorkingMem.ContainsKey(varname))
                            {
                                ErrorHelper.throwError(ErrorLevel.Error, ErrorCodes.MULTIPLE_DECLARATION, Tokenizer.ErrorList, l);
                                l = prog.Length;
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
                                l = prog.Length;
                                break;
                            }

                            WorkingMem.Add(varname_n, new Variable() { name = varname_n, DataType = DataTypes.NOOB, value = ""});

                            i = lex.Length;
                            break;
                        
                    }
                }
            }

            MainWindow.Dispatcher.BeginInvoke((Action)(() =>
            {
                MainWindow.startProg.IsEnabled = true;
            }));

        }

        public void Run(string[] progin)
        {
            MainWindow.Dispatcher.BeginInvoke((Action)(() =>
            {
                MainWindow.startProg.IsEnabled = false;
                Console.Visibility = Visibility.Visible;
                Console.Activate();
            }));

            WorkingMem.Clear();
            WorkingMem = Tokenizer.BuiltInVariables();

            prog = progin;
            ExecutionThread.RunWorkerAsync();
        }

        public Stack<object> LiteralsStack = new Stack<object>();

        public double CDb(object x)
        {
            return Convert.ToDouble(x);
        }
        public bool CBl(object x)
        {
            if (x.ToString().ToUpper().Contains("WIN"))
            {
                return true;
            }
            else if (x.ToString().ToUpper().Contains("FAIL"))
            {
                return false;
            }
            return Convert.ToBoolean(x);
        }

        /// <summary>
        /// Parse a math expression to final result
        /// </summary>
        /// <param name="Ops"></param>
        /// <returns></returns>
        public object ProcessExpression(string[] Ops)
        {
            object x = 0, y = 0, r = 0;
            foreach (string op in Ops.Reverse())
            {
                switch (op)
                {
                    case "ADD":
                        y = LiteralsStack.Pop();
                        x = LiteralsStack.Pop();
                        r = CDb(x) + CDb(y);
                        LiteralsStack.Push(r);
                        break;
                    case "SUB":
                        y = LiteralsStack.Pop();
                        x = LiteralsStack.Pop();
                        r = CDb(x) - CDb(y);
                        LiteralsStack.Push(r);
                        break;
                    case "PROD":
                        y = LiteralsStack.Pop();
                        x = LiteralsStack.Pop();
                        r = CDb(x) * CDb(y);
                        LiteralsStack.Push(r);
                        break;
                    case "QUOT":
                        y = LiteralsStack.Pop();
                        x = LiteralsStack.Pop();
                        r = CDb(y) / CDb(x);
                        LiteralsStack.Push(r);
                        break;
                    case "MOD":
                        y = LiteralsStack.Pop();
                        x = LiteralsStack.Pop();
                        r = CDb(x) % CDb(y);
                        LiteralsStack.Push(r);
                        break;
                    case "MAX":
                        y = LiteralsStack.Pop();
                        x = LiteralsStack.Pop();
                        r = Math.Max(CDb(x), CDb(y));
                        LiteralsStack.Push(r);
                        break;
                    case "MIN":
                        y = LiteralsStack.Pop();
                        x = LiteralsStack.Pop();
                        r = Math.Min(CDb(x), CDb(y));
                        LiteralsStack.Push(r);
                        break;
                    case "_AND":
                        y = LiteralsStack.Pop();
                        x = LiteralsStack.Pop();
                        r = CBl(x) & CBl(y);
                        LiteralsStack.Push(r);
                        break;
                    case "__OR":
                        y = LiteralsStack.Pop();
                        x = LiteralsStack.Pop();
                        r = CBl(x) | CBl(y);
                        LiteralsStack.Push(r);
                        break;
                    case "_XOR":
                        y = LiteralsStack.Pop();
                        x = LiteralsStack.Pop();
                        r = CBl(x) ^ CBl(y);
                        LiteralsStack.Push(r);
                        break;
                    case "_NOT":
                        x = LiteralsStack.Pop();
                        r = !CBl(x);
                        LiteralsStack.Push(r);
                        break;
                    case "AAND":
                        r = CBl(LiteralsStack.Pop());
                        do
                        {
                            r = CBl(r) & CBl(LiteralsStack.Pop());
                        } while (LiteralsStack.Count != 0);
                        LiteralsStack.Push(r);
                        break;
                    case "A_OR":
                        r = CBl(LiteralsStack.Pop());
                        do
                        {
                            r = CBl(r) | CBl(LiteralsStack.Pop());
                        } while (LiteralsStack.Count != 0);
                        LiteralsStack.Push(r);
                        break;
                    default:
                        if (CheckIfVar(op))
                        {
                            var u = GetVar(op);
                            LiteralsStack.Push(u);
                        } else
                        {
                            LiteralsStack.Push(op);
                        }
                        break;
                }
            }

            if(LiteralsStack.Count > 1) { ErrorHelper.throwError(ErrorLevel.Error, ErrorCodes.MULTIPLE_DECLARATION, Tokenizer.ErrorList); return null; }
            return LiteralsStack.Pop();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetVar(string key)
        {
            if (!WorkingMem.ContainsKey(key))
            {
                ErrorHelper.throwError(ErrorLevel.Error, ErrorCodes.UNKNOWN_OR_UNDECLARED_VARIABLE, Tokenizer.ErrorList);
                return null;
            }
            return WorkingMem[key].value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetVar(string key,object value)
        {
            if (!WorkingMem.ContainsKey(key))
            {
                ErrorHelper.throwError(ErrorLevel.Error, ErrorCodes.UNKNOWN_OR_UNDECLARED_VARIABLE, Tokenizer.ErrorList);
                return false;
            }

            WorkingMem[key].value = value;
            WorkingMem[key].DataType = Tokenizer.DetermineDataType(value.ToString());

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool CheckIfVar(string key)
        {
            return WorkingMem.ContainsKey(key);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public ExpressionType DetExpressionType(string exp)
        {
            string[] BoolOps = {"BOTH-OF",
                                "EITHER-OF",
                                "WON-OF",
                                "NOT",
                                "ALL-OF",
                                "ANY-OF"};

            string[] NumOps = {"ADD",
                                "SUB",
                                "PROD",
                                "QUOT",
                                "MOD",
                                "MAX",
                                "MIN"};

            foreach(string x in BoolOps)
            {
                if (exp.Contains(x)) { return ExpressionType.Logical; }
            }

            foreach (string x in NumOps)
            {
                if (exp.Contains(x)) { return ExpressionType.Numerical; }
            }

            return ExpressionType.None;
        }


    }
    enum ExpressionType
    {
        Logical,
        Numerical,
        None
    }
}
