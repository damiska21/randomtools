using NonInvasiveKeyboardHookLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace random_tools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //tohle je NuGet knihovna, nikoliv funkce uvnitř normálního c#
        //http://www.kbdedit.com/manual/low_level_vk_list.html kódy na klávesy
        //https://www.codeproject.com/Articles/1273010/Global-Hotkeys-within-Desktop-Applications ukázky kódu
        public KeyboardHookManager keyboardHookManager = new KeyboardHookManager();
        public bool keyboardHookManagerRun = true;

        private void Form1_Load(object sender, EventArgs e)
        {
            keyboardHookManager.Start();
            //F6 - F10 nedělají prakticky nic
            keyboardHookManager.RegisterHotkey(0x75/*F6*/, () =>
            {
                
            });
        }
        
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            notifyIcon1.Visible = false;
            WindowState = FormWindowState.Normal;
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(1000);
            }else if(FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible=false;
            }
        }

        #region autoclicker
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public void DoMouseClick()
        {
            //Call the imported function with the cursor's current position
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }
        bool clickerOn = false;
        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (!clickerOn)
            {
                int tick;
                if (Int32.TryParse(textBox1.Text, out tick))
                {
                    timer1.Interval = tick;
                    timer1.Enabled = true;
                }
            }
            clickerOn = !clickerOn;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DoMouseClick();
        }
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            if (keyboardHookManagerRun)
            {
                keyboardHookManager.Stop();
            }
            else
            {
                keyboardHookManager.Start();
            }
            keyboardHookManagerRun = !keyboardHookManagerRun;
        }
    }
}
