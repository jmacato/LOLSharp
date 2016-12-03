using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
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
            this.Dispatcher.Invoke(() => Out.Clear());
        }

        public void Write(string s)
        {
            this.Dispatcher.Invoke(() => {
                Out.AppendText(s);
                Out.CaretIndex = Out.Text.Length;
            });
        }

        public void WriteLine(string s)
        {
            this.Dispatcher.Invoke(() => {
                Out.AppendText(s + "\n");
                Out.CaretIndex = Out.Text.Length;
            });
        }

        public void StartIn()
        {
            awaitinput = true;
            this.Dispatcher.Invoke(() => {
                Out.CaretBrush = new SolidColorBrush(Colors.White);
                Out.CaretIndex = Out.Text.Length;
            });
        }

        /// <summary>
        /// Read and clear the input buffer
        /// </summary>
        /// <returns>User input</returns>
        public string ReadBuffer()
        {
            this.Dispatcher.Invoke(() => {
                Out.CaretBrush = new SolidColorBrush(Colors.Transparent);
                Out.CaretIndex = Out.Text.Length;
            });
            var x = inputbuffer;
            inputbuffer = "";
            return x;
        }

        private void Out_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (awaitinput)
            {
                if (e.Key == Key.Enter)
                {
                    awaitinput = false;
                    Out.Text += "\r\n";
                    return;
                }

                inputbuffer += GetCharFromKey(e.Key);
            }
            Out.CaretIndex = Out.Text.Length;
        }

        private void Out_Loaded(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(() => Out.CaretBrush = new SolidColorBrush(Colors.Transparent));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            e.Cancel = true;
        }

        public enum MapType : uint
        {
            MAPVK_VK_TO_VSC = 0x0,
            MAPVK_VSC_TO_VK = 0x1,
            MAPVK_VK_TO_CHAR = 0x2,
            MAPVK_VSC_TO_VK_EX = 0x3,
        }

        [DllImport("user32.dll")]
        public static extern int ToUnicode(
            uint wVirtKey,
            uint wScanCode,
            byte[] lpKeyState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)]
            StringBuilder pwszBuff,
            int cchBuff,
            uint wFlags);

        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, MapType uMapType);

        public static char GetCharFromKey(Key key)
        {
            char ch = ' ';

            int virtualKey = KeyInterop.VirtualKeyFromKey(key);
            byte[] keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            uint scanCode = MapVirtualKey((uint)virtualKey, MapType.MAPVK_VK_TO_VSC);
            StringBuilder stringBuilder = new StringBuilder(2);

            int result = ToUnicode((uint)virtualKey, scanCode, keyboardState, stringBuilder, stringBuilder.Capacity, 0);
            switch (result)
            {
                case -1:
                    break;
                case 0:
                    break;
                case 1:
                    {
                        ch = stringBuilder[0];
                        break;
                    }
                default:
                    {
                        ch = stringBuilder[0];
                        break;
                    }
            }
            return ch;
        }


    }
}
