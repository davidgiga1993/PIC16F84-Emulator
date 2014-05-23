using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Data
{
    /// <summary>
    /// Generischer Daten Adapter
    /// Dieser wird als wrapper für einen Wert verwendet um diesen mit Eventhandling auszustatten.
    /// </summary>
    /// <typeparam name="T">Typ des Wertes</typeparam>
    public class DataAdapter<T>
    {
        public delegate void OnDataChanged(T Value, object Sender);
        public event OnDataChanged DataChanged;

        protected T _Data;

        public DataAdapter()
        {

        }

        public DataAdapter(T defaultValue)
        {
            _Data = defaultValue;
        }

        public T Value
        {
            get
            {
                return _Data;
            }
            set
            {
                    _Data = value;
                    InternalValueChanged();
                    if (DataChanged != null)
                        DataChanged(value, this);                
            }
        }

        protected virtual void InternalValueChanged()
        {

        }
    }
}
