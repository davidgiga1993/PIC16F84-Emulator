using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PIC16F84_Emulator.PIC.Data;
using PIC16F84_Emulator.UIElements;


namespace PIC16F84_Emulator
{
    public partial class Form1 : Form
    {
        protected PIC.PIC Pic;
                
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Wird aufgerufen wenn eine neue Datei geladen und gesparst wurde.
        /// -> Alle alten Fenster schließen und Oberfläche neu aufbauen
        /// </summary>
        private void HasNewSource()
        {
            // Alle alten Fenster schließen
            foreach (Form frm in this.MdiChildren)
            {
                frm.Close();
            }

            Text = "PIC16F84 - " + Pic.CurrentFile;
            FormSourcecode Source = new FormSourcecode(Pic);
            Source.MdiParent = this;
            Source.Location = new Point(0, 0);
            Source.StartPosition = FormStartPosition.Manual;
            Source.Show();

            FormRegisterOverview Data = new FormRegisterOverview(Pic);
            Data.MdiParent = this;
            Data.StartPosition = FormStartPosition.Manual;
            Data.Location = new Point(Source.Location.X + Source.Width + 10, Source.Location.Y);
            Data.Show();

            Form Register = ShowIORegister(new Point(0, Source.Location.Y + Source.Height + 10), 0x5, 0x85, "Port A");
            Register = ShowIORegister(new Point(Register.Location.X + Register.Width, Source.Location.Y + Source.Height + 10), 0x6, 0x86, "Port B");

            ShowWRegister(new Point(Register.Location.X + Register.Width, Source.Location.Y + Source.Height + 10));

            FormTime Time = new FormTime(Pic);
            Time.MdiParent = this;
            Time.StartPosition = FormStartPosition.Manual;
            Time.Location = new Point(0, Source.Location.Y + Source.Height + 10 + Register.Height);
            Time.Show();

            FormDebugActions Debug = new FormDebugActions(Pic);
            Debug.MdiParent = this;
            Debug.StartPosition = FormStartPosition.Manual;
            Debug.Location = new Point(Time.Location.X + Time.Width, Source.Location.Y + Source.Height + 10 + Register.Height);
            Debug.Show();
        }

        /// <summary>
        /// Tastenevents
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.F11)
            {
                SingleStep();
            }
            else if (e.KeyCode == Keys.F5)
            {
                Run();
            }
            else if(e.KeyCode == Keys.O && e.Modifiers == Keys.Control)
            {
                toolStripMenuItem2_Click(null, null);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Filter = "LST file|*.lst|All files|*.*";
            OFD.CheckFileExists = true;
            OFD.Multiselect = false;

            if (OFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //TODO: Change path
                Pic = new PIC.PIC(OFD.FileName);
                //Pic = new PIC.PIC(@"C:\Users\David\Documents\DH\Semester 3\Rechnerarchitektur\picsimu\BCDZahler.lst");
                HasNewSource();
            }
        }

        private void runF5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Run();
        }

        private void singleStepF11ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SingleStep();
        }

        private void sourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Pic == null)
            {
                Dialogs.ShowNoFileDialog();
                return;
            }
            FormSourcecode Source = new FormSourcecode(Pic);
            Source.MdiParent = this;
            Source.Show();
        }

        private void registerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Pic == null)
            {
                Dialogs.ShowNoFileDialog();
                return;
            }
            FormRegisterOverview Data = new FormRegisterOverview(Pic);
            Data.MdiParent = this;
            Data.Show();
        }

        private void portAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowIORegister(new Point(0, 0), 0x5, 0x85, "Port A");
        }

        private void portBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowIORegister(new Point(0, 0), 0x6, 0x86, "Port B");
        }

        private void trisAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowRegisterDetail(new Point(0, 0), 0x85, "Tris A");
        }

        private void trisBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowRegisterDetail(new Point(0, 0), 0x86, "Tris B");
        }

        private void pCRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormRegister Register = new FormRegister(Pic.RegisterMap.ProgrammCounterAdapter);
            Register.MdiParent = this;
            Register.Text = "Programmcounter";
            Register.Show();
        }

        private void wRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowWRegister(new Point(0, 0));
        }

        private void ShowWRegister(Point StartPosition)
        {
            if (Pic == null)
            {
                Dialogs.ShowNoFileDialog();
                return;
            }
            FormRegister Register = new FormRegister(Pic.WRegister);
            Register.StartPosition = FormStartPosition.Manual;
            Register.Location = StartPosition;
            Register.MdiParent = this;
            Register.Text = "W Register";
            Register.Show();
        }

        private Form ShowIORegister(Point StartPosition, int RegisterAddress, int TrisRegisterAddress, string Title)
        {
            if (Pic == null)
            {
                Dialogs.ShowNoFileDialog();
                return null;
            }
            FormIORegister Register = new FormIORegister(Pic.RegisterMap.GetAdapter(RegisterAddress), Pic.RegisterMap.GetAdapter(TrisRegisterAddress));
            Register.StartPosition = FormStartPosition.Manual;
            Register.Location = StartPosition;
            Register.MdiParent = this;
            Register.Text = Title;
            Register.Show();
            return Register;
        }

        private void ShowRegisterDetail(Point StartPosition, int RegisterAddress, string Title)
        {
            if (Pic == null)
            {
                Dialogs.ShowNoFileDialog();
                return;
            }
            FormRegister Register = new FormRegister(Pic.RegisterMap.GetAdapter(RegisterAddress));
            Register.StartPosition = FormStartPosition.Manual;
            Register.Location = StartPosition;
            Register.MdiParent = this;
            Register.Text = Title;
            Register.Show();
        }

        /// <summary>
        /// Führt ein Einzelnen Schritt im PIC aus.
        /// </summary>
        private void SingleStep()
        {
            if (Pic == null)
            {
                Dialogs.ShowNoFileDialog();
                return;
            }
            Pic.RunTimer.Stop();
            Pic.Step();
        }

        private void Run()
        {
            if (Pic == null)
            {
                Dialogs.ShowNoFileDialog();
                return;
            }
            if (Pic.RunTimer.Enabled == false)
                Pic.RunTimer.Start();
            else
                Pic.RunTimer.Stop();
        }

        private void interruptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Pic == null)
            {
                Dialogs.ShowNoFileDialog();
                return;
            }
            FormRegisterInterrupt I = new FormRegisterInterrupt(Pic.RegisterMap.GetAdapter(0xB));
            I.MdiParent = this;
            I.Show();
        }

        private void stackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Pic == null)
            {
                Dialogs.ShowNoFileDialog();
                return;
            }
            FormStack S = new FormStack(Pic);
            S.MdiParent = this;
            S.Show();
        }

        private void timeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Pic == null)
            {
                Dialogs.ShowNoFileDialog();
                return;
            }
            FormTime S = new FormTime(Pic);
            S.MdiParent = this;
            S.Show();
        }

        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Pic == null)
            {
                Dialogs.ShowNoFileDialog();
                return;
            }
            FormDebugActions S = new FormDebugActions(Pic);
            S.MdiParent = this;
            S.Show();
        }

        private void hilfeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: PFAD ZUR PDF!!!!
            System.Diagnostics.Process.Start("");
        }

        private void überToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            MessageBox.Show("PIC16F84 Emulator" + Environment.NewLine + "von David Schumann und Simon Isele" + Environment.NewLine + "Version " + Version, "Info");
        }

        private void comPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Pic == null)
            {
                Dialogs.ShowNoFileDialog();
                return;
            }

            foreach (Form frm in this.MdiChildren)
            {
                if(frm is FormComPort)
                { // Verhindert mehrfaches öffnen vom ComPort Fenster
                    return;
                }
            }

            FormComPort Form = new FormComPort(Pic);
            Form.MdiParent = this;
            Form.Show();
        }

        /// <summary>
        /// Schließen Option
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void schließenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
