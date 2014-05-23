using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PIC16F84_Emulator.PIC.Data;

namespace PIC16F84_Emulator.UIElements
{
    /// <summary>
    /// Entspricht dem CheckBox UI Element mit der Funktionalität sich an eine Datenquelle zu binden
    /// </summary>
    class BindCheckBox : CheckBox
    {
        private DataAdapter<byte> Adapter;
        private int BitPosition;

        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);
            byte newValue = 0;
            if(Checked)
                newValue = Helper.SetBit(BitPosition, Adapter.Value);
            else
                newValue = Helper.UnsetBit(BitPosition, Adapter.Value);

            if (Adapter.Value != newValue)
                Adapter.Value = newValue;
        }

        /// <summary>
        /// Bindet die CheckBox an eine Datenquelle
        /// </summary>
        /// <param name="Adapter">Datenquelle</param>
        /// <param name="BitPosition">Bit Position die verwendet werden soll</param>
        public void Bind(DataAdapter<byte> Adapter, int BitPosition)
        {
            this.Adapter = Adapter;
            this.BitPosition = BitPosition;
            Adapter.DataChanged += Adapter_DataChanged;
            Adapter_DataChanged(Adapter.Value, null);
        }

        private void Adapter_DataChanged(byte Value, object Sender)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate { Adapter_DataChanged(Value, Sender); });
            }
            else
            {
                Checked = (Value & (1 << BitPosition)) != 0;
            }
        }
    }
}
