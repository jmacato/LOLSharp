using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace LOLpreter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Lexer Lexer = new Lexer();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void startParsing_Click(object sender, RoutedEventArgs e)
        {
            var x = Lexer.PreProccess(LOLinput.Text);
            if (x == null) {
                Debug.WriteLine("Parsing Halted.");
            } else
            {
                x += "\r\n----- STRING CONSTANTS TABLE ------\r\n";
                foreach(KeyValuePair<string,string> y in Lexer.StringConstTable)
                {
                    x += y.Key.PadRight(20,' ')+" | \""+y.Value+"\"\r\n";
                }
                LOLinput.Text = x;

            }


        }
        Dictionary<string, string> LexemeDefinitions = new Dictionary<string, string>
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

            {@"AN","Separates arguments."},
            {@"R", "Assignment operator" },
            {@"HAI","Delimiter to mark the start of the program"},
            {@"KTHXBYE","Delimiter to mark the end of the program"},
            {@"BTW","Single-line comment"},
            {@"OBTW","Start of a multi-line comment"},
            {@"TLDR","End of a multi-line comment"},
            {@"ITZ","Assignment operator in declaring a variable"},
            {@"GIMMEH","Input statement"},
            {@"VISIBLE","Output statement"},
            {@"OIC","Signals the end of the If-Else block"},
            {@"WTF?","Signals the start of a Switch Case block"},
            {@"OMG","Comparison block for a Switch Case. Followed by a literal"},
            {@"OMGWTF","The default optional case in a Switch Case block."},
            {@"GTFO","Break statement."},
            {@"MEBBE","Appears between the ​YA RLY ​and ​NO WAI​ blocks. Similar to an Elseif statement."},
            {@"NOT","Negation"},
            {@"MAEK","Used for re-casting a variable to a different type"},
            {@"UPPIN","Increments a variable by one."},
            {@"NERFIN","Decrements a variable by one."},
            {@"MKAY","Delimiter of a function call."},
            {@"WILE","Evaluates an expression as a Boolean statement. If it evaluates to true, the execution is continued. Else, the execution stops."},
            {@"TILL","Evaluates an expression as a Boolean statement. If it evaluates to true, the execution is continued. Else, the execution stops."},
            {@"IT","Temporary variable. Remains in local scope until it is replaced with a bare expression."},
            {@"SMOOSH","Expects strings as its input arguments for concatenation"},
            {@"DIFFRINT"," Comparison Operator; True if operands are not equal"},
            {@"and","Logic gate"},
            {@"or","Logic gate"},
            {@"xor","Logic gate"},
            {@"WIN","Boolean Literal"},
            {@"FAIL","Boolean Literal"},




        };

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            foreach(KeyValuePair<string,string> sx in LexemeDefinitions)
            {
                Debug.WriteLine("<Word>" + sx.Key.Replace("-", " ")+"</Word>");
            }

            //Load LOLCODE syntax highlighting file
            using (Stream s = this.GetType().Assembly.GetManifestResourceStream("LOLpreter.LOL.xshd"))
            {
                using (XmlTextReader reader = new XmlTextReader(s))
                {
                    LOLinput.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
        }
    }

}
