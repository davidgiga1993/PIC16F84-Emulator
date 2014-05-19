using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Timer
{
    public class Timer0
    {
        private PIC Pic;

        private int  InternalCounter = 0;

        private int PortALastValue;

        private bool TMR0Event = false;
        private int CycleSkipCount = 0;

        public Timer0(PIC Pic)
        {
            this.Pic = Pic;

            Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_PORT_A).DataChanged += PortA_DataChanged;
            Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_TIMER_ADDRESS).DataChanged += Timer0_DataChanged;
            PortALastValue = Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_PORT_A).Value & 0x4;
        }

        private void Timer0_DataChanged(byte Value, object Sender)
        {
            if(TMR0Event)
            {
                CycleSkipCount = 2;
            }
        }

        private void PortA_DataChanged(byte Value, object Sender)
        {
            if((Value & 0x4) != PortALastValue)
            {
                if(PortALastValue == 0x4) // Bit hat sich von 1 nach 0 geändert
                {
                    TickPortA(0);
                }
                else // Von 0 nach 1
                {
                    TickPortA(1);
                }
            }
        }

        /// <summary>
        /// Wird aufgerufen wenn sich bei Port A das 4. Bit geändert hat (falling/rising)
        /// </summary>
        /// <param name="Mode">1 wenn rising, 0 wenn falling</param>
        private void TickPortA(int Mode)
        {
            if(Pic.RegisterMap.Option_Timer_Mode)
            {
                if(Mode == 1 && Pic.RegisterMap.Option_Timer_Source_Edge == false)
                {
                    Tick();
                }
                else if (Mode == 0 && Pic.RegisterMap.Option_Timer_Source_Edge == true)
                {
                    Tick();
                }
            }
        }

        /// <summary>
        /// Returns the prescaler Value
        /// </summary>
        public byte prescalerValue
        {
            get
            {
                int power = (Pic.RegisterMap.Get(Register.RegisterFileMap.REG_OPTIONS_ADDRESS, true) & 0x07) + 1;
                return (byte)(Math.Pow(2, power) - 1);
            }
            set
            {
                int arg1 = Pic.RegisterMap.Get(Register.RegisterFileMap.REG_OPTIONS_ADDRESS, true) & 0xF8;
                int arg2 = value & 0x07;
                Pic.RegisterMap.Set((byte)(arg1 | arg2), Register.RegisterFileMap.REG_OPTIONS_ADDRESS, true);

                InternalCounter = 0;
            }
        }

        /// <summary>
        /// Wird bei jedem Takt aufgerufen
        /// </summary>
        public void Tick()
        {
            if (CycleSkipCount > 0)
            {
                CycleSkipCount--;
                return;
            }

            if (Pic.RegisterMap.Option_PSAAssigmentBit == false)
            {
                if (InternalCounter >= prescalerValue)
                {
                    InternalCounter = 0;
                    Timer0Increment();
                }
                else
                {
                    InternalCounter++;
                }
            }
            else
            {
                InternalCounter = 0;
                Timer0Increment();
            }
        }

        /// <summary>
        /// Erhöht den Timer um den Wert 1
        /// </summary>
        private void Timer0Increment()
        {
            TMR0Event = false;

            byte TimerValue = Pic.RegisterMap.Get(Register.RegisterFileMap.REG_TIMER_ADDRESS, true);
            if (TimerValue == 0xFF)
            {
                Pic.RegisterMap.TMR0Overflow = true;
                Pic.RegisterMap.Set(0, Register.RegisterFileMap.REG_TIMER_ADDRESS, true);
            }
            else
            {
                Pic.RegisterMap.Set((byte)(TimerValue + 1), Register.RegisterFileMap.REG_TIMER_ADDRESS, true);
            }
            TMR0Event = true;
        }
    }
}