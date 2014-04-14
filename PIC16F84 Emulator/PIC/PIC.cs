﻿using System;
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
        public RegisterFileMap RegisterMap;
        public DataAdapter<byte> WRegister = new DataAdapter<byte>();
        public StackData Stack = new StackData();

        public List<int> Breakpoints = new List<int>();

        public BaseFunction[] Functions;

        public SourceCodeLine[] SourceCode;
        public BytecodeLine[] ByteCode;

        public string CurrentFile;

        public bool Running = false;
        public bool SingleStepMode = true;
        public DataAdapter<int> Runtime = new DataAdapter<int>();

        public bool RunInterrupt = false;

        public Timer RunTimer = new Timer();

        public PIC(string Sourcefile)
        {
            RunTimer.Tick += RunTimer_Tick;
            RunTimer.Interval = 1;

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
                if (RegisterMap.RB0ExternalInterrupt && RegisterMap.RB0ExternalInterruptEnable) // RB0 interrupt gesetzt und aktiv
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
                    Runtime.Value += Cycles;
                    break;
                }
            }
        }
    }
}