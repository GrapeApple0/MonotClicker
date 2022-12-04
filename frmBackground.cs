using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MonoClicker
{
    class frmBackground : Form
    {
        public frmBackground(Bitmap bitmap)
        {
            InitializeComponent();
            SelectBitmap(bitmap);
        }

        public void SelectBitmap(Bitmap bitmap)
        {
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
            {
                throw new ApplicationException("The bitmap must be 32bpp with alpha-channel.");
            }

            IntPtr screenDc = ApiHelper.GetDC(IntPtr.Zero);
            IntPtr memDc = ApiHelper.CreateCompatibleDC(screenDc);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr hOldBitmap = IntPtr.Zero;
            try
            {
                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                hOldBitmap = ApiHelper.SelectObject(memDc, hBitmap);
                ApiHelper.ApiSize newApiSize = new ApiHelper.ApiSize(bitmap.Width, bitmap.Height);
                ApiHelper.ApiPoint sourceLocation = new ApiHelper.ApiPoint(0, 0);
                ApiHelper.ApiPoint newLocation = new ApiHelper.ApiPoint(this.Location.X, this.Location.Y);
                ApiHelper.BLENDFUNCTION blend = new ApiHelper.BLENDFUNCTION();
                blend.BlendOp = ApiHelper.AC_SRC_OVER;
                blend.BlendFlags = 0;
                blend.SourceConstantAlpha = 255;
                blend.AlphaFormat = ApiHelper.AC_SRC_ALPHA;
                ApiHelper.UpdateLayeredWindow(Handle, screenDc, ref newLocation, ref newApiSize, memDc, ref sourceLocation, 0, ref blend, ApiHelper.ULW_ALPHA);
            }
            catch { }
            finally
            {
                ApiHelper.ReleaseDC(IntPtr.Zero, screenDc);
                if (hBitmap != IntPtr.Zero)
                {
                    ApiHelper.SelectObject(memDc, hOldBitmap);
                    ApiHelper.DeleteObject(hBitmap);
                }
                ApiHelper.DeleteDC(memDc);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams p = base.CreateParams;
                p.ExStyle = p.ExStyle | ApiHelper.WS_EX_LAYERED;
                return p;
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frmBackground
            // 
            this.ClientSize = new System.Drawing.Size(128, 128);
            this.StartPosition = FormStartPosition.Manual;
            this.Name = "frmBackground";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmBackground_Load);
            this.Click += new System.EventHandler(this.frmBackground_Click);
            this.ResumeLayout(false);
            this.WindowState = FormWindowState.Minimized;

        }

        private void frmBackground_Load(object sender, EventArgs e)
        {
            Random random = new Random();
            var x = random.Next(0, Screen.PrimaryScreen.Bounds.Width);
            var y = random.Next(0, Screen.PrimaryScreen.Bounds.Height);
            Point point = new Point(x, y);
            this.Location = point;
            this.BringToFront();
            this.WindowState = FormWindowState.Normal;
        }

        private void frmBackground_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < 10; i++)
            {
                var frmbackground = new frmBackground(Properties.Resources.monot);
                frmbackground.Show();
            }
        }
    }

    public class ApiHelper
    {
        public const Int32 WS_EX_LAYERED = 524288;
        public const Int32 ULW_ALPHA = 2;
        public const byte AC_SRC_OVER = 0;
        public const byte AC_SRC_ALPHA = 1;


        [StructLayout(LayoutKind.Sequential)]
        public struct ApiPoint
        {
            public int X;
            public int Y;
            public ApiPoint(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public ApiPoint(Point pt)
            {
                this.X = pt.X;
                this.Y = pt.Y;
            }
        }

        public enum BoolEnum
        {
            False = 0,
            True = 1
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ApiSize
        {
            public Int32 cx;
            public Int32 cy;
            public ApiSize(Int32 cx, Int32 cy)
            {
                this.cx = cx;
                this.cy = cy;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct ARGB
        {
            public byte Blue;
            public byte Green;
            public byte Red;
            public byte Alpha;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern BoolEnum UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref ApiPoint pptDst, ref ApiSize psize, IntPtr hdcSrc, ref ApiPoint pprSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern BoolEnum DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern BoolEnum DeleteObject(IntPtr hObject);
    }
}