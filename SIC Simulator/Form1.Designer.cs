namespace SIC_Simulator
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbCPU = new System.Windows.Forms.GroupBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmOpen_SIC_Object_File = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmSaveMachineState = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmFile_Ext = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmAbout_CheckForUpdates = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmAbout_About = new System.Windows.Forms.ToolStripMenuItem();
            this.machineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmzeroAllMemory = new System.Windows.Forms.ToolStripMenuItem();
            this.randomizeAllMemoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setProgramCounterToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stepSingleInstructionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tcMachine = new System.Windows.Forms.TabControl();
            this.tpMemory = new System.Windows.Forms.TabPage();
            this.tpDevices = new System.Windows.Forms.TabPage();
            this.rbMemBinary = new System.Windows.Forms.RadioButton();
            this.rbMemHex = new System.Windows.Forms.RadioButton();
            this.txtMemory = new System.Windows.Forms.TextBox();
            this.tsmloadAndAssembleSICSourceFIle = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSavedSICMachineStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gbCPU.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tcMachine.SuspendLayout();
            this.tpMemory.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbCPU
            // 
            this.gbCPU.Controls.Add(this.textBox5);
            this.gbCPU.Controls.Add(this.textBox4);
            this.gbCPU.Controls.Add(this.label5);
            this.gbCPU.Controls.Add(this.label4);
            this.gbCPU.Controls.Add(this.label3);
            this.gbCPU.Controls.Add(this.label2);
            this.gbCPU.Controls.Add(this.label1);
            this.gbCPU.Controls.Add(this.textBox3);
            this.gbCPU.Controls.Add(this.textBox2);
            this.gbCPU.Controls.Add(this.textBox1);
            this.gbCPU.Location = new System.Drawing.Point(12, 37);
            this.gbCPU.Name = "gbCPU";
            this.gbCPU.Size = new System.Drawing.Size(225, 182);
            this.gbCPU.TabIndex = 0;
            this.gbCPU.TabStop = false;
            this.gbCPU.Text = "SIC CPU";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(33, 138);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(159, 20);
            this.textBox5.TabIndex = 9;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(33, 110);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(159, 20);
            this.textBox4.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 141);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "SW";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "(PC)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "L";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "A";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(34, 82);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(159, 20);
            this.textBox3.TabIndex = 2;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(33, 54);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(159, 20);
            this.textBox2.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(33, 24);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(159, 20);
            this.textBox1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmFile,
            this.tsmAbout,
            this.machineToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(767, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmFile
            // 
            this.tsmFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmOpen_SIC_Object_File,
            this.tsmloadAndAssembleSICSourceFIle,
            this.loadSavedSICMachineStateToolStripMenuItem,
            this.tsmSaveMachineState,
            this.tsmFile_Ext});
            this.tsmFile.Name = "tsmFile";
            this.tsmFile.Size = new System.Drawing.Size(37, 20);
            this.tsmFile.Text = "File";
            // 
            // tsmOpen_SIC_Object_File
            // 
            this.tsmOpen_SIC_Object_File.Name = "tsmOpen_SIC_Object_File";
            this.tsmOpen_SIC_Object_File.Size = new System.Drawing.Size(257, 22);
            this.tsmOpen_SIC_Object_File.Text = "Open SIC Object File";
            // 
            // tsmSaveMachineState
            // 
            this.tsmSaveMachineState.Name = "tsmSaveMachineState";
            this.tsmSaveMachineState.Size = new System.Drawing.Size(257, 22);
            this.tsmSaveMachineState.Text = "Save SIC Machine State";
            this.tsmSaveMachineState.Click += new System.EventHandler(this.tsmSaveMachineState_Click);
            // 
            // tsmFile_Ext
            // 
            this.tsmFile_Ext.Name = "tsmFile_Ext";
            this.tsmFile_Ext.Size = new System.Drawing.Size(257, 22);
            this.tsmFile_Ext.Text = "Exit";
            // 
            // tsmAbout
            // 
            this.tsmAbout.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmAbout_CheckForUpdates,
            this.tsmAbout_About});
            this.tsmAbout.Name = "tsmAbout";
            this.tsmAbout.Size = new System.Drawing.Size(52, 20);
            this.tsmAbout.Text = "About";
            // 
            // tsmAbout_CheckForUpdates
            // 
            this.tsmAbout_CheckForUpdates.Name = "tsmAbout_CheckForUpdates";
            this.tsmAbout_CheckForUpdates.Size = new System.Drawing.Size(171, 22);
            this.tsmAbout_CheckForUpdates.Text = "Check for Updates";
            // 
            // tsmAbout_About
            // 
            this.tsmAbout_About.Name = "tsmAbout_About";
            this.tsmAbout_About.Size = new System.Drawing.Size(171, 22);
            this.tsmAbout_About.Text = "About";
            // 
            // machineToolStripMenuItem
            // 
            this.machineToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmzeroAllMemory,
            this.randomizeAllMemoryToolStripMenuItem,
            this.setProgramCounterToToolStripMenuItem,
            this.stepSingleInstructionToolStripMenuItem});
            this.machineToolStripMenuItem.Name = "machineToolStripMenuItem";
            this.machineToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.machineToolStripMenuItem.Text = "Machine";
            // 
            // tsmzeroAllMemory
            // 
            this.tsmzeroAllMemory.Name = "tsmzeroAllMemory";
            this.tsmzeroAllMemory.Size = new System.Drawing.Size(200, 22);
            this.tsmzeroAllMemory.Text = "Zero All Memory";
            // 
            // randomizeAllMemoryToolStripMenuItem
            // 
            this.randomizeAllMemoryToolStripMenuItem.Name = "randomizeAllMemoryToolStripMenuItem";
            this.randomizeAllMemoryToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.randomizeAllMemoryToolStripMenuItem.Text = "Randomize All Memory";
            // 
            // setProgramCounterToToolStripMenuItem
            // 
            this.setProgramCounterToToolStripMenuItem.Name = "setProgramCounterToToolStripMenuItem";
            this.setProgramCounterToToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.setProgramCounterToToolStripMenuItem.Text = "Set Program Counter To";
            // 
            // stepSingleInstructionToolStripMenuItem
            // 
            this.stepSingleInstructionToolStripMenuItem.Name = "stepSingleInstructionToolStripMenuItem";
            this.stepSingleInstructionToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.stepSingleInstructionToolStripMenuItem.Text = "Step Single  Instruction";
            // 
            // tcMachine
            // 
            this.tcMachine.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcMachine.Controls.Add(this.tpMemory);
            this.tcMachine.Controls.Add(this.tpDevices);
            this.tcMachine.Location = new System.Drawing.Point(13, 226);
            this.tcMachine.Name = "tcMachine";
            this.tcMachine.SelectedIndex = 0;
            this.tcMachine.Size = new System.Drawing.Size(742, 320);
            this.tcMachine.TabIndex = 2;
            // 
            // tpMemory
            // 
            this.tpMemory.Controls.Add(this.txtMemory);
            this.tpMemory.Controls.Add(this.rbMemHex);
            this.tpMemory.Controls.Add(this.rbMemBinary);
            this.tpMemory.Location = new System.Drawing.Point(4, 22);
            this.tpMemory.Name = "tpMemory";
            this.tpMemory.Padding = new System.Windows.Forms.Padding(3);
            this.tpMemory.Size = new System.Drawing.Size(734, 294);
            this.tpMemory.TabIndex = 0;
            this.tpMemory.Text = "Memory";
            this.tpMemory.UseVisualStyleBackColor = true;
            // 
            // tpDevices
            // 
            this.tpDevices.Location = new System.Drawing.Point(4, 22);
            this.tpDevices.Name = "tpDevices";
            this.tpDevices.Padding = new System.Windows.Forms.Padding(3);
            this.tpDevices.Size = new System.Drawing.Size(734, 294);
            this.tpDevices.TabIndex = 1;
            this.tpDevices.Text = "Devices";
            this.tpDevices.UseVisualStyleBackColor = true;
            // 
            // rbMemBinary
            // 
            this.rbMemBinary.AutoSize = true;
            this.rbMemBinary.Location = new System.Drawing.Point(13, 7);
            this.rbMemBinary.Name = "rbMemBinary";
            this.rbMemBinary.Size = new System.Drawing.Size(54, 17);
            this.rbMemBinary.TabIndex = 0;
            this.rbMemBinary.Text = "Binary";
            this.rbMemBinary.UseVisualStyleBackColor = true;
            // 
            // rbMemHex
            // 
            this.rbMemHex.AutoSize = true;
            this.rbMemHex.Checked = true;
            this.rbMemHex.Location = new System.Drawing.Point(73, 7);
            this.rbMemHex.Name = "rbMemHex";
            this.rbMemHex.Size = new System.Drawing.Size(44, 17);
            this.rbMemHex.TabIndex = 1;
            this.rbMemHex.TabStop = true;
            this.rbMemHex.Text = "Hex";
            this.rbMemHex.UseVisualStyleBackColor = true;
            // 
            // txtMemory
            // 
            this.txtMemory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMemory.Location = new System.Drawing.Point(9, 29);
            this.txtMemory.Multiline = true;
            this.txtMemory.Name = "txtMemory";
            this.txtMemory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMemory.Size = new System.Drawing.Size(548, 257);
            this.txtMemory.TabIndex = 2;
            // 
            // tsmloadAndAssembleSICSourceFIle
            // 
            this.tsmloadAndAssembleSICSourceFIle.Name = "tsmloadAndAssembleSICSourceFIle";
            this.tsmloadAndAssembleSICSourceFIle.Size = new System.Drawing.Size(257, 22);
            this.tsmloadAndAssembleSICSourceFIle.Text = "Load and Assemble SIC Source FIle";
            // 
            // loadSavedSICMachineStateToolStripMenuItem
            // 
            this.loadSavedSICMachineStateToolStripMenuItem.Name = "loadSavedSICMachineStateToolStripMenuItem";
            this.loadSavedSICMachineStateToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
            this.loadSavedSICMachineStateToolStripMenuItem.Text = "Load Saved SIC Machine State";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 590);
            this.Controls.Add(this.tcMachine);
            this.Controls.Add(this.gbCPU);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "SIC Virtual Machine";
            this.gbCPU.ResumeLayout(false);
            this.gbCPU.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tcMachine.ResumeLayout(false);
            this.tpMemory.ResumeLayout(false);
            this.tpMemory.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbCPU;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmFile;
        private System.Windows.Forms.ToolStripMenuItem tsmOpen_SIC_Object_File;
        private System.Windows.Forms.ToolStripMenuItem tsmSaveMachineState;
        private System.Windows.Forms.ToolStripMenuItem tsmFile_Ext;
        private System.Windows.Forms.ToolStripMenuItem tsmAbout;
        private System.Windows.Forms.ToolStripMenuItem tsmAbout_CheckForUpdates;
        private System.Windows.Forms.ToolStripMenuItem tsmAbout_About;
        private System.Windows.Forms.TabControl tcMachine;
        private System.Windows.Forms.TabPage tpMemory;
        private System.Windows.Forms.TabPage tpDevices;
        private System.Windows.Forms.ToolStripMenuItem machineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmzeroAllMemory;
        private System.Windows.Forms.ToolStripMenuItem randomizeAllMemoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setProgramCounterToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stepSingleInstructionToolStripMenuItem;
        private System.Windows.Forms.RadioButton rbMemHex;
        private System.Windows.Forms.RadioButton rbMemBinary;
        private System.Windows.Forms.TextBox txtMemory;
        private System.Windows.Forms.ToolStripMenuItem tsmloadAndAssembleSICSourceFIle;
        private System.Windows.Forms.ToolStripMenuItem loadSavedSICMachineStateToolStripMenuItem;
    }
}

