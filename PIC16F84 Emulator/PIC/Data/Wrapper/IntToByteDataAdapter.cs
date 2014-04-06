using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Data.Wrapper
{
    class IntToByteDataAdapter : DataAdapter<byte>
    {
        private DataAdapter<int> Adapter;

        public IntToByteDataAdapter(DataAdapter<int> Adapter)
        {
            this.Adapter = Adapter;
            Adapter.DataChanged += Adapter_DataChanged;
        }

        public DataAdapter<int> SourceAdapter
        {
            get
            {
                return Adapter;
            }
        }

        private void Adapter_DataChanged(int Value, object Sender)
        {
            if (this.Value != Value)
                this.Value = (byte)Value;
        }

        protected override void InternalValueChanged()
        {
            Adapter.Value = (int)_Data;
        }
    }
}
