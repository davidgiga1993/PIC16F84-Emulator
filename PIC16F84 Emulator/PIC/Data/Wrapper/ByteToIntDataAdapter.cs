using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Data.Wrapper
{
    class ByteToIntDataAdapter : DataAdapter<int>
    {
        private DataAdapter<byte> Adapter;

        public ByteToIntDataAdapter(DataAdapter<byte> Adapter)
        {
            this.Adapter = Adapter;
            Adapter.DataChanged += Adapter_DataChanged;
            Adapter_DataChanged(Adapter.Value, null);
        }

        public DataAdapter<byte> SourceAdapter
        {
            get
            {
                return Adapter;
            }
        }

        private void Adapter_DataChanged(byte Value, object Sender)
        {
            if(this.Value != Value)
                this.Value = Value;
        }

        protected override void InternalValueChanged()
        {
            if(Adapter.Value != _Data)
                Adapter.Value = (byte)_Data;
        }
    }
}
