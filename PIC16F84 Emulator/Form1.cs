using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PIC16F84_Emulator.PIC.Data;

namespace PIC16F84_Emulator
{
    public partial class Form1 : Form
    {
        protected PIC.PIC Pic;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void HasNewSource()
        {
            foreach (Form frm in this.MdiChildren)
            {
                frm.Close();
            }

            Text = "PIC16F84 - " + Pic.CurrentFile;
            FormSourcecode Source = new FormSourcecode(Pic);
            Source.MdiParent = this;
            
            Source.Show();
        }


        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Filter = "LST file|*.lst|All files|*.*";
            OFD.CheckFileExists = true;
            OFD.Multiselect = false;

            //if (OFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //TODO: Change path
                Pic = new PIC.PIC(@"C:\Users\David\Documents\DH\Semester 3\Rechnerarchitektur\picsimu\BCDZahler.LST");
                HasNewSource();
            }
        }
    }
}
