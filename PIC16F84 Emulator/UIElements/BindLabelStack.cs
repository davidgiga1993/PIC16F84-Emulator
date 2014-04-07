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
        private DataAdapter<int> Adapter;
        private int StackIndex;

        public void Bind(DataAdapter<int> Adapter, int StackIndex)
        {
            this.StackIndex = StackIndex;
            this.Adapter = Adapter;
            Adapter.DataChanged += Adapter_DataChanged;
            Adapter_DataChanged(Adapter.Value, null);
        }

        private void Adapter_DataChanged(int Value, object Sender)
        {
            Text = "Stack " + StackIndex + ": " + Value.ToString();
        }
    }
}
