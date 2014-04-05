using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PIC16F84_Emulator.PIC.Data;
using PIC16F84_Emulator.UIElements;

namespace PIC16F84_Emulator
{
    public partial class FormDataOverview : Form
    {
        private PIC.PIC Pic;

        public FormDataOverview()
        {
            InitializeComponent();
        }

        public FormDataOverview(PIC.PIC Pic)
            : this()
        {
            this.Pic = Pic;
            AddHeader();
            for (int X = 0; X < Pic.RegisterMap.Length; X += 8)
            {
                Label L = new Label();
                L.Text = X.ToString("X2");
                L.Width = 25;
                tableLayoutPanel1.Controls.Add(L, 0, X + 1);
                for (int Y = 0; Y < 8; Y++)
                {
                    DataAdapter<byte> Adapter = Pic.RegisterMap.GetAdapter(X + Y);
                    BindTextBoxHex T = new BindTextBoxHex();
                    T.EnableDetailForm = true;
                    T.EnableChangeColor = true;
                    T.Width = 20;
                    T.Bind(Adapter);
                    tableLayoutPanel1.Controls.Add(T, Y + 1, X + 1);
                }
            }
        }

        private void AddHeader()
        {
            for (int X = 0; X < 8; X++)
            {
                Label L = new Label();
                L.Text = X.ToString("X2");
                tableLayoutPanel1.Controls.Add(L, X + 1, 0);
            }
        }
    }
}
