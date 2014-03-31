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
    public partial class FormRegister : Form
    {
        public FormRegister()
        {
            InitializeComponent();
        }

        public FormRegister(DataAdapter<byte> Adapter) : this()
        {
            textBoxHex.Bind(Adapter);
            checkBoxBit0.Bind(Adapter, 0);
            checkBoxBit1.Bind(Adapter, 1);
            checkBoxBit2.Bind(Adapter, 2);
            checkBoxBit3.Bind(Adapter, 3);
            checkBoxBit4.Bind(Adapter, 4);
            checkBoxBit5.Bind(Adapter, 5);
            checkBoxBit6.Bind(Adapter, 6);
            checkBoxBit7.Bind(Adapter, 7);
        }
    }
}
