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
    public partial class FormStack : Form
    {
        public FormStack()
        {
            InitializeComponent();
        }
        public FormStack(PIC.PIC Pic) : this()
        {
            bindLabelStack1.Bind(Pic.Stack.Entrys[0], 1);
            bindLabelStack2.Bind(Pic.Stack.Entrys[1], 2);
            bindLabelStack3.Bind(Pic.Stack.Entrys[2], 3);
            bindLabelStack4.Bind(Pic.Stack.Entrys[3], 4);
            bindLabelStack5.Bind(Pic.Stack.Entrys[4], 5);
            bindLabelStack6.Bind(Pic.Stack.Entrys[5], 6);
            bindLabelStack7.Bind(Pic.Stack.Entrys[6], 7);
            bindLabelStack8.Bind(Pic.Stack.Entrys[7], 8);
        }
    }
}
