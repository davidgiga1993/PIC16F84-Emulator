using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PIC16F84_Emulator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        PIC.Data.DataAdapter<byte> Test = new PIC.Data.DataAdapter<byte>();

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormRegister R = new FormRegister(Test);
            R.MdiParent = this;
            R.Show();
        }
    }
}
