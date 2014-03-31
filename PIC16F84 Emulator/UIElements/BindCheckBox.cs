using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PIC16F84_Emulator.PIC.Data;

namespace PIC16F84_Emulator.UIElements
{
    class BindCheckBox : CheckBox
    {
        private DataAdapter<byte> Adapter;
        private int BitPosition;

        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);
            if(Checked)
                Adapter.Value = Helper.SetBit(BitPosition, Adapter.Value);
            else
                Adapter.Value = Helper.UnsetBit(BitPosition, Adapter.Value);
        }

        public void Bind(DataAdapter<byte> Adapter, int BitPosition)
        {
            this.Adapter = Adapter;
            this.BitPosition = BitPosition;
            Adapter.DataChanged += Adapter_DataChanged;
            Adapter_DataChanged(Adapter.Value, null);
        }

        private void Adapter_DataChanged(byte Value, object Sender)
        {
            Checked = (Value & (1 << BitPosition)) != 0;
        }
    }
}
