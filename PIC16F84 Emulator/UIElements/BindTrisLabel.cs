using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PIC16F84_Emulator.PIC.Data;

namespace PIC16F84_Emulator.UIElements
{
    class BindTrisLabel : Label
    {
        private DataAdapter<byte> Adapter;
        private int BitPosition;

        public void Bind(DataAdapter<byte> TrisAdapter, int BitPosition)
        {
            this.Adapter = TrisAdapter;
            this.BitPosition = BitPosition;
            Adapter.DataChanged += Adapter_DataChanged;
            Adapter_DataChanged(Adapter.Value, null);
        }

        private void Adapter_DataChanged(byte Value, object Sender)
        {
            Text = Helper.CheckBit(BitPosition, Value) ? "I" : "O";
        }
    }
}
