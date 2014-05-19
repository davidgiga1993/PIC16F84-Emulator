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
    public partial class FormComPort : Form
    {
        private PIC.PIC Pic;

        public FormComPort(PIC.PIC Pic)
        {
            InitializeComponent();
            this.Pic = Pic;
            comboBoxPorts.Items.AddRange(Pic.ComPort.Ports);
            buttonOpenClosePort.Text = Pic.ComPort.Active ? "Stop" : "Start";
        }

        private void buttonOpenClosePort_Click(object sender, EventArgs e)
        {
            if (Pic.ComPort.Active)
            {
                Pic.ComPort.Stop();
                buttonOpenClosePort.Text = "Start";
            }
            else
            {
                if (comboBoxPorts.SelectedIndex > -1)
                {
                    Pic.ComPort.Start(comboBoxPorts.SelectedItem.ToString());
                    buttonOpenClosePort.Text = "Stop";
                }
            }
        }
    }
}
