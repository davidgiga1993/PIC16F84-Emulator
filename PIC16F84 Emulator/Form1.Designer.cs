namespace PIC16F84_Emulator
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.containerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newChildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.schließenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dateiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.schließenToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.singleStepF11ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runF5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.portAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.portBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trisAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trisBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wRegisterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pCRegisterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.interruptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerToolStripMenuItem
            // 
            this.containerToolStripMenuItem.Name = "containerToolStripMenuItem";
            this.containerToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // newChildToolStripMenuItem
            // 
            this.newChildToolStripMenuItem.Name = "newChildToolStripMenuItem";
            this.newChildToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // schließenToolStripMenuItem
            // 
            this.schließenToolStripMenuItem.Name = "schließenToolStripMenuItem";
            this.schließenToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateiToolStripMenuItem,
            this.runToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(844, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dateiToolStripMenuItem
            // 
            this.dateiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.schließenToolStripMenuItem1});
            this.dateiToolStripMenuItem.Name = "dateiToolStripMenuItem";
            this.dateiToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.dateiToolStripMenuItem.Text = "File";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(103, 22);
            this.toolStripMenuItem2.Text = "Open";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // schließenToolStripMenuItem1
            // 
            this.schließenToolStripMenuItem1.Name = "schließenToolStripMenuItem1";
            this.schließenToolStripMenuItem1.Size = new System.Drawing.Size(103, 22);
            this.schließenToolStripMenuItem1.Text = "Close";
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.singleStepF11ToolStripMenuItem,
            this.runF5ToolStripMenuItem});
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.runToolStripMenuItem.Text = "Run";
            // 
            // singleStepF11ToolStripMenuItem
            // 
            this.singleStepF11ToolStripMenuItem.Name = "singleStepF11ToolStripMenuItem";
            this.singleStepF11ToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.singleStepF11ToolStripMenuItem.Text = "Single Step (F11)";
            this.singleStepF11ToolStripMenuItem.Click += new System.EventHandler(this.singleStepF11ToolStripMenuItem_Click);
            // 
            // runF5ToolStripMenuItem
            // 
            this.runF5ToolStripMenuItem.Name = "runF5ToolStripMenuItem";
            this.runF5ToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.runF5ToolStripMenuItem.Text = "Run (F5)";
            this.runF5ToolStripMenuItem.Click += new System.EventHandler(this.runF5ToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sourceToolStripMenuItem,
            this.registerToolStripMenuItem,
            this.portAToolStripMenuItem,
            this.portBToolStripMenuItem,
            this.trisAToolStripMenuItem,
            this.trisBToolStripMenuItem,
            this.wRegisterToolStripMenuItem,
            this.pCRegisterToolStripMenuItem,
            this.interruptToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // sourceToolStripMenuItem
            // 
            this.sourceToolStripMenuItem.Name = "sourceToolStripMenuItem";
            this.sourceToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.sourceToolStripMenuItem.Text = "Source";
            this.sourceToolStripMenuItem.Click += new System.EventHandler(this.sourceToolStripMenuItem_Click);
            // 
            // registerToolStripMenuItem
            // 
            this.registerToolStripMenuItem.Name = "registerToolStripMenuItem";
            this.registerToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.registerToolStripMenuItem.Text = "All register";
            this.registerToolStripMenuItem.Click += new System.EventHandler(this.registerToolStripMenuItem_Click);
            // 
            // portAToolStripMenuItem
            // 
            this.portAToolStripMenuItem.Name = "portAToolStripMenuItem";
            this.portAToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.portAToolStripMenuItem.Text = "Port A";
            this.portAToolStripMenuItem.Click += new System.EventHandler(this.portAToolStripMenuItem_Click);
            // 
            // portBToolStripMenuItem
            // 
            this.portBToolStripMenuItem.Name = "portBToolStripMenuItem";
            this.portBToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.portBToolStripMenuItem.Text = "Port B";
            this.portBToolStripMenuItem.Click += new System.EventHandler(this.portBToolStripMenuItem_Click);
            // 
            // trisAToolStripMenuItem
            // 
            this.trisAToolStripMenuItem.Name = "trisAToolStripMenuItem";
            this.trisAToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.trisAToolStripMenuItem.Text = "Tris A";
            this.trisAToolStripMenuItem.Click += new System.EventHandler(this.trisAToolStripMenuItem_Click);
            // 
            // trisBToolStripMenuItem
            // 
            this.trisBToolStripMenuItem.Name = "trisBToolStripMenuItem";
            this.trisBToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.trisBToolStripMenuItem.Text = "Tris B";
            this.trisBToolStripMenuItem.Click += new System.EventHandler(this.trisBToolStripMenuItem_Click);
            // 
            // wRegisterToolStripMenuItem
            // 
            this.wRegisterToolStripMenuItem.Name = "wRegisterToolStripMenuItem";
            this.wRegisterToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.wRegisterToolStripMenuItem.Text = "W register";
            this.wRegisterToolStripMenuItem.Click += new System.EventHandler(this.wRegisterToolStripMenuItem_Click);
            // 
            // pCRegisterToolStripMenuItem
            // 
            this.pCRegisterToolStripMenuItem.Name = "pCRegisterToolStripMenuItem";
            this.pCRegisterToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.pCRegisterToolStripMenuItem.Text = "PC register";
            this.pCRegisterToolStripMenuItem.Click += new System.EventHandler(this.pCRegisterToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(24, 20);
            this.toolStripMenuItem1.Text = "?";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // interruptToolStripMenuItem
            // 
            this.interruptToolStripMenuItem.Name = "interruptToolStripMenuItem";
            this.interruptToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.interruptToolStripMenuItem.Text = "Intcon register";
            this.interruptToolStripMenuItem.Click += new System.EventHandler(this.interruptToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 540);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "PIC16F84 Emulator";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem containerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newChildToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem öffnenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem schließenToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dateiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem schließenToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem singleStepF11ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sourceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem registerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem portAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem portBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trisAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trisBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wRegisterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runF5ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pCRegisterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem interruptToolStripMenuItem;
    }
}

