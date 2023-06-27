using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace VideoArrows
{
    public partial class Form1 : Form
    {
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const UInt32 SWP_NOSIZE = 0x0001;
        private const UInt32 SWP_NOMOVE = 0x0002;
        private const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        public Form1()
        {
            InitializeComponent();
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams param = base.CreateParams;
                //
                param.ExStyle |= 0x08000000;
                param.Style |= 0x20000; // <--- use 0x20000
                return param;
            }
        }
        protected override void WndProc(ref Message m)
        {
            const int RESIZE_HANDLE_SIZE = 10;

            switch (m.Msg)
            {
                case 0x0084/*NCHITTEST*/ :
                    base.WndProc(ref m);

                    if ((int)m.Result == 0x01/*HTCLIENT*/)
                    {
                        Point screenPoint = new Point(m.LParam.ToInt32());
                        Point clientPoint = this.PointToClient(screenPoint);
                        if (clientPoint.Y <= RESIZE_HANDLE_SIZE)
                        {
                            if (clientPoint.X <= RESIZE_HANDLE_SIZE)
                                m.Result = (IntPtr)13/*HTTOPLEFT*/ ;
                            else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
                                m.Result = (IntPtr)12/*HTTOP*/ ;
                            else
                                m.Result = (IntPtr)14/*HTTOPRIGHT*/ ;
                        }
                        else if (clientPoint.Y <= (Size.Height - RESIZE_HANDLE_SIZE))
                        {
                            if (clientPoint.X <= RESIZE_HANDLE_SIZE)
                                m.Result = (IntPtr)10/*HTLEFT*/ ;
                            else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
                                m.Result = (IntPtr)2/*HTCAPTION*/ ;
                            else
                                m.Result = (IntPtr)11/*HTRIGHT*/ ;
                        }
                        else
                        {
                            if (clientPoint.X <= RESIZE_HANDLE_SIZE)
                                m.Result = (IntPtr)16/*HTBOTTOMLEFT*/ ;
                            else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
                                m.Result = (IntPtr)15/*HTBOTTOM*/ ;
                            else
                                m.Result = (IntPtr)17/*HTBOTTOMRIGHT*/ ;
                        }
                    }
                    return;
            }
            base.WndProc(ref m);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{RIGHT}");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{LEFT}");
        }

        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            //panel1.Padding = new Padding(0,0,panel1.ClientSize.Width /2 - button1.Width,0);
            button1.Visible = true;
            button2.Visible = true;
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            button1.Visible = false;
            button2.Visible = false;

        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            panel1.Padding = new Padding(0, 0, panel1.ClientSize.Width / 2 - button1.Width, 0);

        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            this.Opacity = 1;
            if ((sender as Button).Name == "button1")
                {

                button1.ForeColor = Color.LightYellow;
                button2.ForeColor = Color.FromArgb(100,100,100,100);

            }
            else
            {
                button2.ForeColor = Color.LightYellow;
                button1.ForeColor = Color.FromArgb(100, 100, 100, 100);
                

            }
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            this.Opacity = 0.1;
            button1.ForeColor = Color.LightGray;
            button2.ForeColor = Color.LightGray;


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
            SetWindowPos(this.Handle, HWND_TOPMOST, 100, 100, 300, 300, TOPMOST_FLAGS);

        }
    }
}
