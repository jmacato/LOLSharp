using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Xml;
using System.Text.RegularExpressions;

namespace LOLpreter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Lexer Lexer = new Lexer();

        public string CurrentDocumentPath { get; set; }
        public bool CurrentDocumentModified = false;
        private string currentDocumentTitle;

        public string CurrentDocumentTitle
        {
            get {return currentDocumentTitle;}
            set
            {
                currentDocumentTitle = value;
                if (CurrentDocumentModified)
                {
                    DocTitle.Text = currentDocumentTitle + " (Modified)";

                } else{DocTitle.Text = currentDocumentTitle;}
                if (!(CurrentDocumentPath == ""))
                {
                    DocTitle.Text += "- (" + CurrentDocumentPath + ")";
                }
            }
        }


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            currentDocumentTitle = "Untitled.lol";
            //Load LOLCODE syntax highlighting file
            using (Stream s = this.GetType().Assembly.GetManifestResourceStream("LOLpreter.LOL.xshd"))
            {
                using (XmlTextReader reader = new XmlTextReader(s))
                {
                    LOLinput.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
            
            CurrentDocumentPath = "";
            CurrentDocumentModified = false;
            CurrentDocumentTitle = "Untitled.lol";
        }
        private void newFile_Click(object sender, RoutedEventArgs e)
        {
            saveFileX();
            LOLinput.Text = "";
            LOLinput.Document.UndoStack.ClearAll();
            LOLinput.Document.UndoStack.ClearRedoStack();
            CurrentDocumentPath = "";
            CurrentDocumentModified = false;
            CurrentDocumentTitle = "Untitled.lol";
        }
        private void startProg_Click(object sender, RoutedEventArgs e)
        {
            var x = Lexer.PreProccess(LOLinput.Text);
            if (x == null)
            {
                Debug.WriteLine("Parsing Halted.");
            }
            else
            {
                x += "\r\n----- STRING CONSTANTS TABLE ------\r\n";
                foreach (KeyValuePair<string, string> y in Lexer.StringConstTable)
                {
                    x += y.Key.PadRight(20, ' ') + " | \"" + y.Value + "\"\r\n";
                }

                LOLinput.Document.Text = x;
            }
        }
        private void openFile_Click(object sender, RoutedEventArgs e)
        {
            saveFileX();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "LOLCODE Source files|*.lol|All files (*.*)|*.*";
            dialog.Title = "Select source file";
            if (dialog.ShowDialog() == true)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(dialog.FileName);

                var lolstream = NormalizeEndings(sr.ReadToEnd());

                LOLinput.Document.UndoStack.ClearAll();
                LOLinput.Document.UndoStack.ClearRedoStack();
                LOLinput.Document.FileName = (dialog.FileName);
                LOLinput.Text = lolstream;
                CurrentDocumentModified = false;
                CurrentDocumentPath = Path.GetDirectoryName(dialog.FileName);
                CurrentDocumentTitle = dialog.SafeFileName;
                sr.Close();
            }
        }
        
        /// <summary>
        /// Safely save documents
        /// </summary>
        private void saveFileX()
        {
            if (CurrentDocumentModified)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save your changes?", CurrentDocumentTitle + " is unsaved.", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        if (CurrentDocumentPath == "")
                        {
                            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                            saveFileDialog1.Filter = "LOLCODE Source files|*.lol|All files (*.*)|*.*";
                            saveFileDialog1.Title = "Save source file";
                            saveFileDialog1.ShowDialog();

                            if (saveFileDialog1.FileName == "")
                            {
                                return;
                            }
                        }
                        File.WriteAllText(CurrentDocumentPath + CurrentDocumentTitle, LOLinput.Text);
                        CurrentDocumentModified = false;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }
        
        /// <summary>
        /// Standardize line endings to windows (CR LF) instead of linux (LF) only
        /// </summary>
        /// <param name="input">The raw text to process</param>
        /// <returns></returns>
        private string NormalizeEndings(string input)
        {
            if (Regex.IsMatch(input, "\r"))
            {
                return input;
            }
            else
            {
                return Regex.Replace(input, "\n", Environment.NewLine);
            }
        }
        private void LOLinput_TextChanged(object sender, EventArgs e)
        {
            CurrentDocumentModified = true;
            CurrentDocumentTitle = CurrentDocumentTitle;

        }

    }

}
