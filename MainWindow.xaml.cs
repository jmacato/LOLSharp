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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
