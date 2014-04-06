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

        protected Timer RunTimer = new Timer();

        public Form1()
        {
            InitializeComponent();
            RunTimer.Tick += RunTimer_Tick;
            RunTimer.Interval = 10;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Wird aufgerufen wenn eine neue Datei geladen und gesparst wurde
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

            FormDataOverview Data = new FormDataOverview(Pic);
            Data.MdiParent = this;
            Data.StartPosition = FormStartPosition.Manual;
            Data.Location = new Point(Source.Location.X + Source.Width + 10, Source.Location.Y);
            Data.Show();

            Form Register = ShowIORegister(new Point(0, Source.Location.Y + Source.Height + 10), 0x5, 0x85, "Port A");
            Register = ShowIORegister(new Point(Register.Location.X + Register.Width, Source.Location.Y + Source.Height + 10), 0x6, 0x86, "Port B");

            ShowWRegister(new Point(Register.Location.X + Register.Width, Source.Location.Y + Source.Height + 10));
        }

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

            //if (OFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //TODO: Change path
                Pic = new PIC.PIC(@"C:\Users\David\Documents\DH\Semester 3\Rechnerarchitektur\picsimu\BCDZahler.LST");
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
            FormDataOverview Data = new FormDataOverview(Pic);
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


        private void SingleStep()
        {
            if (Pic == null)
            {
                Dialogs.ShowNoFileDialog();
                return;
            }
            RunTimer.Stop();
            Pic.Step();
        }

        private void Run()
        {
            if (Pic == null)
            {
                Dialogs.ShowNoFileDialog();
                return;
            }
            if (RunTimer.Enabled == false)
                RunTimer.Start();
            else
                RunTimer.Stop();
        }

        private void RunTimer_Tick(object sender, EventArgs e)
        {
            Pic.Step();
        }
    }
}
