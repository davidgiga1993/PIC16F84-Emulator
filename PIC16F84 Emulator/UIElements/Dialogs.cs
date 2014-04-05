using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.UIElements
{
    class Dialogs
    {
        public static void ShowNoFileDialog()
        {
            System.Windows.Forms.MessageBox.Show("No file loaded", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        }
    }
}
