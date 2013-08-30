using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;


namespace jimaku
{
    public partial class jimaku : Form
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint flags);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, GetWindow_Cmd uCmd);

        enum GetWindow_Cmd : uint
        {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6
        }

        private const int WM_NCHITTEST = 0x84;
        private const int WM_WINDOWPOSCHANGING = 0x46;
        private const int WM_KILLFOCUS = 0x8;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        private IntPtr HWND_TOP = (IntPtr)(0);
        private IntPtr HWND_TOPMOST = (IntPtr)(-1);
        private const int SWP_NOACTIVATE = 0x0010;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOSENDCHANGING = 0x0400;
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_SHOWWINDOW = 0x0040;

        public jimaku()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Timer timer = new Timer();
            timer.Tick += new EventHandler(MyClock);
            timer.Interval = 100;
            timer.Enabled = true; 
        }
        public void MyClock(object sender, EventArgs e)
        {
            IntPtr dlgHandle = GetWindow(this.Handle, GetWindow_Cmd.GW_HWNDPREV);
            if (dlgHandle != null)
            {
                SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOSENDCHANGING | SWP_NOSIZE | SWP_SHOWWINDOW);
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    if ((int)m.Result == HTCLIENT)
                        m.Result = (IntPtr)HTCAPTION;
                    return;
                case WM_WINDOWPOSCHANGING:
                case WM_KILLFOCUS:
                    SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOSENDCHANGING | SWP_NOSIZE | SWP_SHOWWINDOW);
                    return;
            }
            base.WndProc(ref m);
        }

    }

}
