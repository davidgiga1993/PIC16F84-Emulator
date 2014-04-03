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
    public partial class FormSourcecode : Form
    {
        private PIC.PIC Pic;
        public FormSourcecode()
        {
            InitializeComponent();
        }

        public FormSourcecode(PIC.PIC Pic) : this()
        {
            this.Pic = Pic;
            for(int X = 0; X < Pic.ProgramData.Length; X++)
            {
                string LineNr = "";
                string Code = "";
                string Line = Pic.ProgramData[X].Source;
                if (Line.Length > 5)
                {
                    LineNr = Pic.ProgramData[X].Source.Remove(5);
                    Code = Pic.ProgramData[X].Source.Remove(0, 5);
                }
                dataGridView1.Rows.Add(new object[]{LineNr, Code});
            }
        }
    }
}
