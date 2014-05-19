using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC16F84_Emulator.PIC.Register;
using PIC16F84_Emulator.PIC.Data;
using PIC16F84_Emulator.PIC.IO;
using System.IO;
using PIC16F84_Emulator.PIC.Functions;
using System.Windows.Forms;

namespace PIC16F84_Emulator.PIC
{
    public class PIC
    {
        /// <summary>
        /// Frequency of the crystal for the pic
        /// </summary>
        public DataAdapter<float> CrystalFrequency = new DataAdapter<float>(5);

        /// <summary>
        /// Type of the crystal frequency:
        /// 0 = kHz
        /// 1 = MHz
        /// </summary>
        public DataAdapter<int> CrystalFrequencyType = new DataAdapter<int>(1);
                
        /// <summary>
        /// Time for one execution cyclus
        /// </summary>
        public DataAdapter<double> CycleTime = new DataAdapter<double>();

        /// <summary>
        /// Register
        /// </summary>
        public RegisterFileMap RegisterMap;
        /// <summary>
        /// Working Register
        /// </summary>
        public DataAdapter<byte> WRegister = new DataAdapter<byte>();
        /// <summary>
        /// Stack vom PIC
        /// </summary>
        public StackData Stack = new StackData();

        /// <summary>
        /// Breakpoints von der UI
        /// </summary>
        public List<int> Breakpoints = new List<int>();

        /// <summary>
        /// Alle implementierten Funktionen des PICs
        /// </summary>
        public BaseFunction[] Functions;

        /// <summary>
        /// Die Quellcodezeilen für den ByteCode
        /// </summary>
        public SourceCodeLine[] SourceCode;
        /// <summary>
        /// Der geparste Bytecode
        /// </summary>
        public BytecodeLine[] ByteCode;

        /// <summary>
        /// Der Pfad zur aktuell geladenen Datei
        /// </summary>
        public string CurrentFile;

        //public bool Running = false;

        /// <summary>
        /// True wenn kein Timer für die Ausführung benutzt wird
        /// </summary>
        public bool SingleStepMode = true;

        /// <summary>
        /// Aktuelle Laufzeit des Programms
        /// </summary>
        public DataAdapter<double> Runtime = new DataAdapter<double>();

        /// <summary>
        /// Gibt an ob ein Interrupt aktiviert ist
        /// Wird in RegisterMap auf true gesetzt
        /// </summary>
        public bool RunInterrupt = false;

        /// <summary>
        /// Timer für die automatische Ausführung
        /// </summary>
        public System.Windows.Forms.Timer RunTimer = new System.Windows.Forms.Timer();

        public Timer.Timer0 TMR0;

        public PIC(string Sourcefile)
        {
            CrystalFrequencyType.DataChanged += _CrystalFrequencyType_DataChanged;
            CrystalFrequency.DataChanged += _CrystalFrequency_DataChanged;

            RunTimer.Tick += RunTimer_Tick;
            RunTimer.Interval = 5;

            RegisterMap = new RegisterFileMap(this);

            //Einlesen der lst Datei
            CurrentFile = (new FileInfo(Sourcefile).Name);
            BytecodeReader Reader = new BytecodeReader();
            LSTLine[] LSTLines = Reader.ReadSourcecode(Sourcefile);

            // Aufteilen des Codes in Quellcode (ASM) und Bytecode
            SourceCode = new SourceCodeLine[LSTLines.Length];
            List<BytecodeLine> ByteCode = new List<BytecodeLine>();

            for (int X = 0; X < SourceCode.Length; X++)
            {
                SourceCode[X] = new SourceCodeLine(LSTLines[X]);
                if (LSTLines[X].ContainsBytecode)
                    ByteCode.Add(new BytecodeLine(LSTLines[X], X));
            }
            this.ByteCode = ByteCode.ToArray();

            // Alle verfügbaren Funktionen werden in einem Array angeordnet (schneller als in einer Liste)
            List<BaseFunction> Functions = new List<BaseFunction>();
            Functions.Add(new AddLW());
            Functions.Add(new AddWF());
            Functions.Add(new AndLW());
            Functions.Add(new AndWF());
            Functions.Add(new BcF());
            Functions.Add(new BsF());
            Functions.Add(new BtFSC());
            Functions.Add(new BtFSS());
            Functions.Add(new Call());
            Functions.Add(new ClrF());
            Functions.Add(new ClrW());
            Functions.Add(new ClrWdt());
            Functions.Add(new ComF());
            Functions.Add(new DecF());
            Functions.Add(new DecFSZ());
            Functions.Add(new Goto());
            Functions.Add(new IncF());
            Functions.Add(new IncFSZ());
            Functions.Add(new IOrLW());
            Functions.Add(new IOrWF());
            Functions.Add(new MovF());
            Functions.Add(new MovLW());
            Functions.Add(new MovWF());
            Functions.Add(new NOP());
            Functions.Add(new RetLW());
            Functions.Add(new Return());
            Functions.Add(new RlF());
            Functions.Add(new RrF());
            Functions.Add(new SubLW());
            Functions.Add(new SubWF());
            Functions.Add(new SwapF());
            Functions.Add(new XOrLW());
            Functions.Add(new XOrWF());
            this.Functions = Functions.ToArray();
            TMR0 = new Timer.Timer0(this);
        }
        /// <summary>
        /// Setzt den PIC zurück
        /// </summary>
        public void Reset()
        {
            RunTimer.Stop();
            RegisterMap.Reset();
            Runtime.Value = 0;
            WRegister.Value = 0;
        }

        private void _CrystalFrequency_DataChanged(float Value, object Sender)
        {
            CalculateCyclusTime();
        }

        private void _CrystalFrequencyType_DataChanged(int Value, object Sender)
        {
            CalculateCyclusTime();
        }

        private void RunTimer_Tick(object sender, EventArgs e)
        {
            int PC = RegisterMap.ProgrammCounter;
            if (PC < ByteCode.Length)
            {
                int SourceCodeIndex = ByteCode[PC].SourceCodeLineIndex;
                foreach (int I in Breakpoints)
                {
                    if (I == SourceCodeIndex)
                    {
                        RunTimer.Stop();
                        return;
                    }
                }
                Step();
            }
        }

        /// <summary>
        /// Überprüft ob der Interrupt ausgeführt werden soll
        /// </summary>
        private void CheckInterrupt()
        {
            if (RegisterMap.GlobalInterruptEnable) // Interrupt angeschaltet
            {
                if ((RegisterMap.RB0ExternalInterrupt && RegisterMap.RB0ExternalInterruptEnable)||(RegisterMap.TMR0OverflowInterruptEnable && RegisterMap.TMR0Overflow)) // RB0 interrupt gesetzt und aktiv oder TMR 0 interrupt gesetzt und overflow
                {
                    DoInterrupt();
                }
            }
            RunInterrupt = false;
        }

        /// <summary>
        /// Ruft den Interrupt auf
        /// </summary>
        private void DoInterrupt()
        {
            Stack.Push(RegisterMap.ProgrammCounter);
            RegisterMap.GlobalInterruptEnable = false;
            RegisterMap.ProgrammCounter = 0x4;
        }

        /// <summary>
        /// Führt ein einzelnen Schritt aus.
        /// Dieser kann je nach ausgeführter Funktion auch mehrere Zyklen dauern
        /// </summary>
        public void Step()
        {
            int PC = RegisterMap.ProgrammCounter;
            if (PC < ByteCode.Length)
            {
                ExecuteFunction(ByteCode[PC].Command, ByteCode[PC]);
            }

            if (RunInterrupt) // Interrupt soll überprüft werden
                CheckInterrupt();
        }

        /// <summary>
        /// Führt die in Command kodierte Funktion aus
        /// </summary>
        /// <param name="Command">Befehl</param>
        /// <param name="Line">Zu Befehl gehörige Zeile</param>
        private void ExecuteFunction(int Command, BytecodeLine Line)
        {
            for (int X = 0; X < Functions.Length; X++)
            {
                BaseFunction Func = Functions[X];
                if (Func.Match(Command))
                {
                    Func.Execute(this, Line);
                    int Cycles = Func.Cycles;
                    for (int C = 0; C < Cycles; C++)
                    {
                        TMR0.Tick();
                    }
                        Runtime.Value += Cycles * CycleTime.Value;
                    break;
                }
            }
        }

        private void CalculateCyclusTime()
        {
            CycleTime.Value = 1.0 / (CrystalFrequency.Value * Math.Pow(10, CrystalFrequencyType.Value == 0 ? 3 : 6)) * 4;
        }
    }
}