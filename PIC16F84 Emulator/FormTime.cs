using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace PIC16F84_Emulator
{
    public partial class FormTime : Form
    {
        private PIC.PIC Pic;
        public FormTime(PIC.PIC Pic)
        {
            this.Pic = Pic;
            InitializeComponent();

            Pic.CrystalFrequencyType.DataChanged += CrystalFrequencyType_DataChanged;
            Pic.CrystalFrequency.DataChanged += CrystalFrequency_DataChanged;
            Pic.CycleTime.DataChanged += CyclusTime_DataChanged;
            Pic.Runtime.DataChanged += Runtime_DataChanged;

            CrystalFrequencyType_DataChanged(Pic.CrystalFrequencyType.Value, null);
            CrystalFrequency_DataChanged(Pic.CrystalFrequency.Value, null);
        }

        private void Runtime_DataChanged(double Value, object Sender)
        {
            labelRunTime.Text = ParseValue(Value);
        }

        private string ParseValue(double Value)
        {
            int Count = 0;
            if (Value > 0)
            {
                string Unit = "";
                while (Value < 1)
                {
                    Value = Value * 1000;
                    Count++;
                }
                switch (Count)
                {
                    case 0:
                        Unit = "s";
                        break;
                    case 1:
                        Unit = "ms";
                        break;
                    case 2:
                        Unit = "µs";
                        break;
                    case 3:
                        Unit = "ns";
                        break;
                    default: // Sollte nicht passieren
                        Unit = "";
                        break;
                }
                return Value.ToString() + " " + Unit;
            }
                return Value.ToString();
          }

        private void CyclusTime_DataChanged(double Value, object Sender)
        {
            labelTickTime.Text = ParseValue(Value);
        }

        private void CrystalFrequency_DataChanged(float Value, object Sender)
        {
            textBox1.Text = Value.ToString();
        }

        private void CrystalFrequencyType_DataChanged(int Value, object Sender)
        {
            comboBoxType.SelectedIndex = Pic.CrystalFrequencyType.Value;
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Pic.CrystalFrequencyType.Value = comboBoxType.SelectedIndex;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            // Es wurde Enter gedrückt -> Wert aus Textfeld parsen
            if(e.KeyCode == Keys.Return)
            {
                float Input = 0;
                if (float.TryParse(textBox1.Text.Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture, out Input))
                {
                    Pic.CrystalFrequency.Value = Input;
                }
            }
        }
    }
}
