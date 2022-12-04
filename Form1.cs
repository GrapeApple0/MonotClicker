using System;
using System.Drawing;
using System.Windows.Forms;

namespace MonoClicker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.StartPosition = FormStartPosition.Manual;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.TransparencyKey = this.BackColor;
            frmBackground frmBackground = new frmBackground(Properties.Resources.monot);
            Random random = new Random();
            var x = random.Next(0, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width);
            var y = random.Next(0, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
            this.Left = x;
            this.Top = y;
            Point point = new Point(x, y);
            this.Location = point;
            frmBackground.Location = point;
            frmBackground.StartPosition = FormStartPosition.Manual;
            frmBackground.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
