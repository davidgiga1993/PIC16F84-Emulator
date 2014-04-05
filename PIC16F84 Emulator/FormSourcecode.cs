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
        private int RowIndex;
        public FormSourcecode()
        {
            InitializeComponent();
        }

        public FormSourcecode(PIC.PIC Pic) : this()
        {
            this.Pic = Pic;
            Pic.RegisterMap.ProgrammCounterAdapter.DataChanged += ProgrammCounterAdapter_DataChanged;
            for(int X = 0; X < Pic.SourceCode.Length; X++)
            {
                string LineNr = "";
                string Code = "";
                string Line = Pic.SourceCode[X].SourceCode;
                if (Line.Length > 5)
                {
                    LineNr = Line.Remove(5);
                    Code = Line.Remove(0, 5);
                }
                dataGridView1.Rows.Add(new object[]{LineNr, Code});
            }
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Pic.RegisterMap.ProgrammCounterAdapter.DataChanged -= ProgrammCounterAdapter_DataChanged;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows[0].Index != RowIndex)
                dataGridView1.Rows[RowIndex].Selected = true;
        }

        private void ProgrammCounterAdapter_DataChanged(byte Value, object Sender)
        {
            RowIndex = Pic.ByteCode[Value].SourceCodeLineIndex;
            dataGridView1.FirstDisplayedScrollingRowIndex = Math.Max(0, RowIndex - 5);
            dataGridView1.Rows[RowIndex].Selected = true;
        }
    }
}
