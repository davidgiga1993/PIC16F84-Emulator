using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC16F84_Emulator.PIC.Register;
using PIC16F84_Emulator.PIC.Data;
using PIC16F84_Emulator.PIC.IO;
using System.IO;

namespace PIC16F84_Emulator.PIC
{
    public class PIC
    {
        public RegisterFileMap RegisterMap = new RegisterFileMap();
        public DataAdapter<byte> WRegister = new DataAdapter<byte>();
        public SourceLine[] ProgramData;
        public string CurrentFile;

        public PIC(string Sourcefile)
        {
            CurrentFile = (new FileInfo(Sourcefile).Name);
            BytecodeReader Reader = new BytecodeReader();
            ProgramData = Reader.ReadSourcecode(Sourcefile);
        }
    }
}
