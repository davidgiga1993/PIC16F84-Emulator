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
    public partial class FormDebugActions : Form
    {
        private PIC.PIC Pic;

        public FormDebugActions(PIC.PIC Pic)
        {
            InitializeComponent();
            this.Pic = Pic;
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            Pic.Reset();
        }

        private void buttonStep_Click(object sender, EventArgs e)
        {
            Pic.RunTimer.Stop();
            Pic.Step();
        }

        private void buttonExecute_Click(object sender, EventArgs e)
        {
            Pic.RunTimer.Start();
        }
    }
}
