using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PIC16F84_Emulator.PIC.Data;

namespace PIC16F84_Emulator.UIElements
{
    class BindLabelStack : Label
    {
        private DataAdapter<byte> Adapter;

        public void Bind(DataAdapter<byte> StackAdapter)
        {
            this.Adapter = StackAdapter;
            Adapter.DataChanged += Adapter_DataChanged;
            Adapter_DataChanged(Adapter.Value, null);
        }

        private void Adapter_DataChanged(byte Value, object Sender)
        {
            Text = Value.ToString();
        }
    }
}
