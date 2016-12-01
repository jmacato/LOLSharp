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
using System.Windows.Input;

namespace LOLpreter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Lexer Lexer = new Lexer();
        Tokenizer Tokenizer = new Tokenizer();
        DebugWindow DebugWindow = new DebugWindow();

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
                    saveFile.IsEnabled = true;
                    this.Title = "LOLCODE Integrated Interpreter Environment - " + DocTitle.Text;

                } else{DocTitle.Text = currentDocumentTitle;}
                if (!(CurrentDocumentPath == ""))
                {
                    DocTitle.Text += "- (" + CurrentDocumentPath + ")";
                    this.Title = "LOLCODE Integrated Interpreter Environment - " + DocTitle.Text;
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        /* Event Handlers */

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
            DocTitle.Text = currentDocumentTitle;
            this.Title = "LOLCODE Integrated Interpreter Environment - " + DocTitle.Text;

            LOLinput.TextArea.Caret.PositionChanged += new EventHandler(updatePosBar);
            LOLinput.Document.TextChanged += new EventHandler(LOLinput_TextChanged);

            DebugWindow.Show();

        }
        private void newFile_Click(object sender, RoutedEventArgs e)
        {
            SafeSaveHandler();
            LOLinput.Text = "";
            LOLinput.Document.UndoStack.ClearAll();
            LOLinput.Document.UndoStack.ClearRedoStack();
            CurrentDocumentPath = "";
            CurrentDocumentModified = false;
            CurrentDocumentTitle = "Untitled.lol";
            saveFile.IsEnabled = false;
        }
        private void startProg_Click(object sender, RoutedEventArgs e)
        {
            var x = Lexer.PreProccess(LOLinput.Text);
            if (x == null && ErrorHelper.CountBreakingErrors(Lexer.ErrorList) > 0)
            {
                DebugWindow.ErrorTable.ItemsSource = Lexer.ErrorList;
                Debug.WriteLine("Parsing Halted.");
            }
            else
            {
                Tokenizer.Tokenize(x);
                DebugWindow.SymbolTable.ItemsSource=Lexer.StringConstTable;
            }
        }
        private void openFile_Click(object sender, RoutedEventArgs e)
        {
            SafeSaveHandler();
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
        private void LOLinput_TextChanged(object sender, EventArgs e)
        {
            CurrentDocumentModified = true;
            CurrentDocumentTitle = CurrentDocumentTitle;
            redoText.IsEnabled = LOLinput.Document.UndoStack.CanRedo;
            undoText.IsEnabled = LOLinput.Document.UndoStack.CanUndo;
        }
        private void Window_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            updatePosBar(null, null);
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            updatePosBar(null, null);
        }
        private void saveFile_Click(object sender, RoutedEventArgs e)
        {
            SafeSaveHandler(true);
        }
        private void redoText_Click(object sender, RoutedEventArgs e)
        {
            LOLinput.Redo();
        }
        private void undoText_Click(object sender, RoutedEventArgs e)
        {
            LOLinput.Undo();
        }
        
        /// <summary>
        /// Safely save documents
        /// </summary>
        private void SafeSaveHandler(bool showdialog = true)
        {
            if (CurrentDocumentModified)
            {

                MessageBoxResult result;
                if (showdialog)
                {
                    result = MessageBox.Show("Do you want to save your changes?", CurrentDocumentTitle + " is unsaved.", MessageBoxButton.YesNo, MessageBoxImage.Question);
                } else
                {
                    result = MessageBoxResult.Yes;
                }

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
                            saveFile.IsEnabled = false;
                            CurrentDocumentTitle = saveFileDialog1.SafeFileName;
                            CurrentDocumentPath = Path.GetDirectoryName(saveFileDialog1.FileName);
                        }
                        File.WriteAllText(CurrentDocumentPath + "\\" + CurrentDocumentTitle, LOLinput.Text);
                        CurrentDocumentModified = false;
                        CurrentDocumentTitle = CurrentDocumentTitle;
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
        
        /// <summary>
        /// Update Statusbars
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updatePosBar(object sender, EventArgs e)
        {
            var x = LOLinput.TextArea.Caret.Position.Location;
            LineCount.Content = "LN " + x.Line.ToString();
            CharCount.Content = "CH " + x.Column.ToString();

            var isNumLockToggled = Keyboard.IsKeyToggled(Key.NumLock);
            var isCapsLockToggled = Keyboard.IsKeyToggled(Key.CapsLock);
            var isInsToggled = Keyboard.IsKeyToggled(Key.Insert);

            if (isNumLockToggled) { NumStat.Opacity = 1; } else { NumStat.Opacity = 0.2; }
            if (isCapsLockToggled) { CapStat.Opacity = 1; } else { CapStat.Opacity = 0.2; }
            if (isInsToggled) { InsStat.Opacity = 1; } else { InsStat.Opacity = 0.2; }

            CurrentDocumentTitle = CurrentDocumentTitle;
            redoText.IsEnabled = LOLinput.Document.UndoStack.CanRedo;
            undoText.IsEnabled = LOLinput.Document.UndoStack.CanUndo;

        }

    }

}
