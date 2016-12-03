using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LOLpreter
{
    /// <summary>
    /// Interaction logic for Console.xaml
    /// </summary>
    public partial class Console : Window
    {

        public string inputbuffer = "";
        public bool awaitinput = false;

        public Console()
        {
            InitializeComponent();
            
        }

        public void Clear()
        {
            this.Dispatcher.Invoke(() => Out.Document.Blocks.Clear());
        }

        public void Write(string s)
        {     
            this.Dispatcher.Invoke(() => Out.AppendText(s));
        }

        public void WriteLine(string s)
        {
            this.Dispatcher.Invoke(() => Out.AppendText(s + "\n"));
        }

        public void StartIn()
        {
            awaitinput = true;
            Out.CaretBrush = new SolidColorBrush(Colors.White);

        }

        /// <summary>
        /// Read and clear the input buffer
        /// </summary>
        /// <returns>User input</returns>
        public string ReadBuffer()
        {
            Out.CaretBrush = new SolidColorBrush(Colors.Transparent);
            var x = inputbuffer;
            inputbuffer = "";
            return x;
        }

        private void Out_KeyUp(object sender, KeyEventArgs e)
        {
            if (awaitinput)
            {
                if (e.Key == Key.Enter) {
                    awaitinput = false;
                    Out.AppendText(e.Key.ToString());
                    e.Handled = true;
                    return; }
                inputbuffer += e.Key;
                Out.AppendText(e.Key.ToString());
                e.Handled = true;
            }
        }

        private void Out_Loaded(object sender, RoutedEventArgs e)
        {
            Out.CaretBrush = new SolidColorBrush(Colors.Transparent);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            e.Cancel = true;
        }
    }
}
