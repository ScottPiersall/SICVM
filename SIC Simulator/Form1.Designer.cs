﻿namespace SIC_Simulator
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.gbCPU = new System.Windows.Forms.GroupBox();
            this.lblComp_Result = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSW_CC = new System.Windows.Forms.TextBox();
            this.txtSW_BIN_LSB = new System.Windows.Forms.TextBox();
            this.txtSW_BIN_MIB = new System.Windows.Forms.TextBox();
            this.txtSW_BIN_MSB = new System.Windows.Forms.TextBox();
            this.txtPC_BIN_LSB = new System.Windows.Forms.TextBox();
            this.txtPC_BIN_MIB = new System.Windows.Forms.TextBox();
            this.txtPC_BIN_MSB = new System.Windows.Forms.TextBox();
            this.txtX_BIN_LSB = new System.Windows.Forms.TextBox();
            this.txtX_BIN_MIB = new System.Windows.Forms.TextBox();
            this.txtX_BIN_MSB = new System.Windows.Forms.TextBox();
            this.txtL_BIN_LSB = new System.Windows.Forms.TextBox();
            this.txtL_BIN_MIB = new System.Windows.Forms.TextBox();
            this.txtL_BIN_MSB = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtSW_Dec = new System.Windows.Forms.TextBox();
            this.txtPC_Dec = new System.Windows.Forms.TextBox();
            this.txtX_Dec = new System.Windows.Forms.TextBox();
            this.txtL_Dec = new System.Windows.Forms.TextBox();
            this.txtA_Dec = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtA_BIN_LSB = new System.Windows.Forms.TextBox();
            this.txtA_BIN_MIB = new System.Windows.Forms.TextBox();
            this.txtA_BIN_MSB = new System.Windows.Forms.TextBox();
            this.txtSW_Hex = new System.Windows.Forms.TextBox();
            this.txtPC_Hex = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtX_Hex = new System.Windows.Forms.TextBox();
            this.txtL_Hex = new System.Windows.Forms.TextBox();
            this.txtA_Hex = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmOpen_SIC_Object_File = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmloadAndAssembleSICSourceFIle = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSavedSICMachineStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmSaveMachineState = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmFile_Ext = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmAbout_CheckForUpdates = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmAbout_About = new System.Windows.Forms.ToolStripMenuItem();
            this.machineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmzeroAllMemory = new System.Windows.Forms.ToolStripMenuItem();
            this.randomizeAllMemory = new System.Windows.Forms.ToolStripMenuItem();
            this.setProgramCounterToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stepSingleInstructionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmsetMemoryBYTE = new System.Windows.Forms.ToolStripMenuItem();
            this.setMemoryWORDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmresetSICVirtualMachine = new System.Windows.Forms.ToolStripMenuItem();
            this.tcMachine = new System.Windows.Forms.TabControl();
            this.tpMemory = new System.Windows.Forms.TabPage();
            this.rtfMemory = new System.Windows.Forms.RichTextBox();
            this.rbMemHex = new System.Windows.Forms.RadioButton();
            this.rbMemBinary = new System.Windows.Forms.RadioButton();
            this.tpDevices = new System.Windows.Forms.TabPage();
            this.lvDevices = new System.Windows.Forms.ListView();
            this.colDeviceID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colOutput = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tpMicroSteps = new System.Windows.Forms.TabPage();
            this.rtfMicroSteps = new System.Windows.Forms.RichTextBox();
            this.btnStep = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.lblNextInstruction = new System.Windows.Forms.Label();
            this.lblNI_Description = new System.Windows.Forms.Label();
            this.lblNextInstruction_Effect = new System.Windows.Forms.Label();
            this.loadSICSourceFD = new System.Windows.Forms.OpenFileDialog();
            this.btnRun = new System.Windows.Forms.Button();
            this.txtSICInput = new System.Windows.Forms.RichTextBox();
            this.txtObjectCode = new System.Windows.Forms.RichTextBox();
            this.btnResetProgram = new System.Windows.Forms.Button();
            this.tbObjectCode = new System.Windows.Forms.TabControl();
            this.tbSICSymbol = new System.Windows.Forms.TabPage();
            this.tbObjCode = new System.Windows.Forms.TabPage();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnThreeStep = new System.Windows.Forms.Button();
            this.loadObjectFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gbCPU.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tcMachine.SuspendLayout();
            this.tpMemory.SuspendLayout();
            this.tpDevices.SuspendLayout();
            this.tpMicroSteps.SuspendLayout();
            this.tbObjectCode.SuspendLayout();
            this.tbSICSymbol.SuspendLayout();
            this.tbObjCode.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbCPU
            // 
            this.gbCPU.Controls.Add(this.lblComp_Result);
            this.gbCPU.Controls.Add(this.label6);
            this.gbCPU.Controls.Add(this.txtSW_CC);
            this.gbCPU.Controls.Add(this.txtSW_BIN_LSB);
            this.gbCPU.Controls.Add(this.txtSW_BIN_MIB);
            this.gbCPU.Controls.Add(this.txtSW_BIN_MSB);
            this.gbCPU.Controls.Add(this.txtPC_BIN_LSB);
            this.gbCPU.Controls.Add(this.txtPC_BIN_MIB);
            this.gbCPU.Controls.Add(this.txtPC_BIN_MSB);
            this.gbCPU.Controls.Add(this.txtX_BIN_LSB);
            this.gbCPU.Controls.Add(this.txtX_BIN_MIB);
            this.gbCPU.Controls.Add(this.txtX_BIN_MSB);
            this.gbCPU.Controls.Add(this.txtL_BIN_LSB);
            this.gbCPU.Controls.Add(this.txtL_BIN_MIB);
            this.gbCPU.Controls.Add(this.txtL_BIN_MSB);
            this.gbCPU.Controls.Add(this.label11);
            this.gbCPU.Controls.Add(this.txtSW_Dec);
            this.gbCPU.Controls.Add(this.txtPC_Dec);
            this.gbCPU.Controls.Add(this.txtX_Dec);
            this.gbCPU.Controls.Add(this.txtL_Dec);
            this.gbCPU.Controls.Add(this.txtA_Dec);
            this.gbCPU.Controls.Add(this.label10);
            this.gbCPU.Controls.Add(this.label9);
            this.gbCPU.Controls.Add(this.txtA_BIN_LSB);
            this.gbCPU.Controls.Add(this.txtA_BIN_MIB);
            this.gbCPU.Controls.Add(this.txtA_BIN_MSB);
            this.gbCPU.Controls.Add(this.txtSW_Hex);
            this.gbCPU.Controls.Add(this.txtPC_Hex);
            this.gbCPU.Controls.Add(this.label5);
            this.gbCPU.Controls.Add(this.label4);
            this.gbCPU.Controls.Add(this.label3);
            this.gbCPU.Controls.Add(this.label2);
            this.gbCPU.Controls.Add(this.label1);
            this.gbCPU.Controls.Add(this.txtX_Hex);
            this.gbCPU.Controls.Add(this.txtL_Hex);
            this.gbCPU.Controls.Add(this.txtA_Hex);
            this.gbCPU.Location = new System.Drawing.Point(12, 32);
            this.gbCPU.Name = "gbCPU";
            this.gbCPU.Size = new System.Drawing.Size(344, 188);
            this.gbCPU.TabIndex = 0;
            this.gbCPU.TabStop = false;
            this.gbCPU.Text = "SIC CPU";
            // 
            // lblComp_Result
            // 
            this.lblComp_Result.AutoSize = true;
            this.lblComp_Result.Location = new System.Drawing.Point(59, 166);
            this.lblComp_Result.Name = "lblComp_Result";
            this.lblComp_Result.Size = new System.Drawing.Size(17, 13);
            this.lblComp_Result.TabIndex = 35;
            this.lblComp_Result.Text = "xx";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 166);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 13);
            this.label6.TabIndex = 34;
            this.label6.Text = "CC";
            // 
            // txtSW_CC
            // 
            this.txtSW_CC.Location = new System.Drawing.Point(33, 163);
            this.txtSW_CC.Name = "txtSW_CC";
            this.txtSW_CC.ReadOnly = true;
            this.txtSW_CC.Size = new System.Drawing.Size(20, 20);
            this.txtSW_CC.TabIndex = 33;
            this.txtSW_CC.Text = "00";
            this.txtSW_CC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtSW_BIN_LSB
            // 
            this.txtSW_BIN_LSB.Location = new System.Drawing.Point(204, 137);
            this.txtSW_BIN_LSB.Name = "txtSW_BIN_LSB";
            this.txtSW_BIN_LSB.ReadOnly = true;
            this.txtSW_BIN_LSB.Size = new System.Drawing.Size(57, 20);
            this.txtSW_BIN_LSB.TabIndex = 32;
            this.txtSW_BIN_LSB.Text = "00000000";
            this.txtSW_BIN_LSB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtSW_BIN_MIB
            // 
            this.txtSW_BIN_MIB.Location = new System.Drawing.Point(145, 137);
            this.txtSW_BIN_MIB.Name = "txtSW_BIN_MIB";
            this.txtSW_BIN_MIB.ReadOnly = true;
            this.txtSW_BIN_MIB.Size = new System.Drawing.Size(57, 20);
            this.txtSW_BIN_MIB.TabIndex = 31;
            this.txtSW_BIN_MIB.Text = "00000000";
            this.txtSW_BIN_MIB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtSW_BIN_MSB
            // 
            this.txtSW_BIN_MSB.Location = new System.Drawing.Point(86, 137);
            this.txtSW_BIN_MSB.Name = "txtSW_BIN_MSB";
            this.txtSW_BIN_MSB.ReadOnly = true;
            this.txtSW_BIN_MSB.Size = new System.Drawing.Size(57, 20);
            this.txtSW_BIN_MSB.TabIndex = 30;
            this.txtSW_BIN_MSB.Text = "00000000";
            this.txtSW_BIN_MSB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtPC_BIN_LSB
            // 
            this.txtPC_BIN_LSB.Location = new System.Drawing.Point(204, 109);
            this.txtPC_BIN_LSB.Name = "txtPC_BIN_LSB";
            this.txtPC_BIN_LSB.ReadOnly = true;
            this.txtPC_BIN_LSB.Size = new System.Drawing.Size(57, 20);
            this.txtPC_BIN_LSB.TabIndex = 29;
            this.txtPC_BIN_LSB.Text = "00000000";
            this.txtPC_BIN_LSB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtPC_BIN_MIB
            // 
            this.txtPC_BIN_MIB.Location = new System.Drawing.Point(145, 109);
            this.txtPC_BIN_MIB.Name = "txtPC_BIN_MIB";
            this.txtPC_BIN_MIB.ReadOnly = true;
            this.txtPC_BIN_MIB.Size = new System.Drawing.Size(57, 20);
            this.txtPC_BIN_MIB.TabIndex = 28;
            this.txtPC_BIN_MIB.Text = "00000000";
            this.txtPC_BIN_MIB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtPC_BIN_MSB
            // 
            this.txtPC_BIN_MSB.Location = new System.Drawing.Point(86, 109);
            this.txtPC_BIN_MSB.Name = "txtPC_BIN_MSB";
            this.txtPC_BIN_MSB.ReadOnly = true;
            this.txtPC_BIN_MSB.Size = new System.Drawing.Size(57, 20);
            this.txtPC_BIN_MSB.TabIndex = 27;
            this.txtPC_BIN_MSB.Text = "00000000";
            this.txtPC_BIN_MSB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtX_BIN_LSB
            // 
            this.txtX_BIN_LSB.Location = new System.Drawing.Point(204, 82);
            this.txtX_BIN_LSB.Name = "txtX_BIN_LSB";
            this.txtX_BIN_LSB.ReadOnly = true;
            this.txtX_BIN_LSB.Size = new System.Drawing.Size(57, 20);
            this.txtX_BIN_LSB.TabIndex = 26;
            this.txtX_BIN_LSB.Text = "00000000";
            this.txtX_BIN_LSB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtX_BIN_MIB
            // 
            this.txtX_BIN_MIB.Location = new System.Drawing.Point(145, 82);
            this.txtX_BIN_MIB.Name = "txtX_BIN_MIB";
            this.txtX_BIN_MIB.ReadOnly = true;
            this.txtX_BIN_MIB.Size = new System.Drawing.Size(57, 20);
            this.txtX_BIN_MIB.TabIndex = 25;
            this.txtX_BIN_MIB.Text = "00000000";
            this.txtX_BIN_MIB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtX_BIN_MSB
            // 
            this.txtX_BIN_MSB.Location = new System.Drawing.Point(86, 82);
            this.txtX_BIN_MSB.Name = "txtX_BIN_MSB";
            this.txtX_BIN_MSB.ReadOnly = true;
            this.txtX_BIN_MSB.Size = new System.Drawing.Size(57, 20);
            this.txtX_BIN_MSB.TabIndex = 24;
            this.txtX_BIN_MSB.Text = "00000000";
            this.txtX_BIN_MSB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtL_BIN_LSB
            // 
            this.txtL_BIN_LSB.Location = new System.Drawing.Point(204, 56);
            this.txtL_BIN_LSB.Name = "txtL_BIN_LSB";
            this.txtL_BIN_LSB.ReadOnly = true;
            this.txtL_BIN_LSB.Size = new System.Drawing.Size(57, 20);
            this.txtL_BIN_LSB.TabIndex = 23;
            this.txtL_BIN_LSB.Text = "00000000";
            this.txtL_BIN_LSB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtL_BIN_MIB
            // 
            this.txtL_BIN_MIB.Location = new System.Drawing.Point(145, 56);
            this.txtL_BIN_MIB.Name = "txtL_BIN_MIB";
            this.txtL_BIN_MIB.ReadOnly = true;
            this.txtL_BIN_MIB.Size = new System.Drawing.Size(57, 20);
            this.txtL_BIN_MIB.TabIndex = 22;
            this.txtL_BIN_MIB.Text = "00000000";
            this.txtL_BIN_MIB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtL_BIN_MSB
            // 
            this.txtL_BIN_MSB.Location = new System.Drawing.Point(86, 56);
            this.txtL_BIN_MSB.Name = "txtL_BIN_MSB";
            this.txtL_BIN_MSB.ReadOnly = true;
            this.txtL_BIN_MSB.Size = new System.Drawing.Size(57, 20);
            this.txtL_BIN_MSB.TabIndex = 21;
            this.txtL_BIN_MSB.Text = "00000000";
            this.txtL_BIN_MSB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(284, 14);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(45, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "Decimal";
            // 
            // txtSW_Dec
            // 
            this.txtSW_Dec.Location = new System.Drawing.Point(271, 137);
            this.txtSW_Dec.Name = "txtSW_Dec";
            this.txtSW_Dec.ReadOnly = true;
            this.txtSW_Dec.Size = new System.Drawing.Size(65, 20);
            this.txtSW_Dec.TabIndex = 19;
            this.txtSW_Dec.Text = "0";
            this.txtSW_Dec.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtPC_Dec
            // 
            this.txtPC_Dec.Location = new System.Drawing.Point(271, 109);
            this.txtPC_Dec.Name = "txtPC_Dec";
            this.txtPC_Dec.ReadOnly = true;
            this.txtPC_Dec.Size = new System.Drawing.Size(65, 20);
            this.txtPC_Dec.TabIndex = 18;
            this.txtPC_Dec.Text = "0";
            this.txtPC_Dec.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtX_Dec
            // 
            this.txtX_Dec.Location = new System.Drawing.Point(271, 82);
            this.txtX_Dec.Name = "txtX_Dec";
            this.txtX_Dec.ReadOnly = true;
            this.txtX_Dec.Size = new System.Drawing.Size(65, 20);
            this.txtX_Dec.TabIndex = 17;
            this.txtX_Dec.Text = "0";
            this.txtX_Dec.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtL_Dec
            // 
            this.txtL_Dec.Location = new System.Drawing.Point(271, 56);
            this.txtL_Dec.Name = "txtL_Dec";
            this.txtL_Dec.ReadOnly = true;
            this.txtL_Dec.Size = new System.Drawing.Size(65, 20);
            this.txtL_Dec.TabIndex = 16;
            this.txtL_Dec.Text = "0";
            this.txtL_Dec.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtA_Dec
            // 
            this.txtA_Dec.Location = new System.Drawing.Point(271, 30);
            this.txtA_Dec.Name = "txtA_Dec";
            this.txtA_Dec.ReadOnly = true;
            this.txtA_Dec.Size = new System.Drawing.Size(65, 20);
            this.txtA_Dec.TabIndex = 15;
            this.txtA_Dec.Text = "0";
            this.txtA_Dec.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(155, 14);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 13);
            this.label10.TabIndex = 14;
            this.label10.Text = "BINARY";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(42, 14);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "HEX";
            // 
            // txtA_BIN_LSB
            // 
            this.txtA_BIN_LSB.Location = new System.Drawing.Point(204, 30);
            this.txtA_BIN_LSB.Name = "txtA_BIN_LSB";
            this.txtA_BIN_LSB.ReadOnly = true;
            this.txtA_BIN_LSB.Size = new System.Drawing.Size(57, 20);
            this.txtA_BIN_LSB.TabIndex = 12;
            this.txtA_BIN_LSB.Text = "00000000";
            this.txtA_BIN_LSB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtA_BIN_MIB
            // 
            this.txtA_BIN_MIB.Location = new System.Drawing.Point(145, 30);
            this.txtA_BIN_MIB.Name = "txtA_BIN_MIB";
            this.txtA_BIN_MIB.ReadOnly = true;
            this.txtA_BIN_MIB.Size = new System.Drawing.Size(57, 20);
            this.txtA_BIN_MIB.TabIndex = 11;
            this.txtA_BIN_MIB.Text = "00000000";
            this.txtA_BIN_MIB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtA_BIN_MSB
            // 
            this.txtA_BIN_MSB.Location = new System.Drawing.Point(86, 30);
            this.txtA_BIN_MSB.Name = "txtA_BIN_MSB";
            this.txtA_BIN_MSB.ReadOnly = true;
            this.txtA_BIN_MSB.Size = new System.Drawing.Size(57, 20);
            this.txtA_BIN_MSB.TabIndex = 10;
            this.txtA_BIN_MSB.Text = "00000000";
            this.txtA_BIN_MSB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtSW_Hex
            // 
            this.txtSW_Hex.Location = new System.Drawing.Point(33, 137);
            this.txtSW_Hex.Name = "txtSW_Hex";
            this.txtSW_Hex.ReadOnly = true;
            this.txtSW_Hex.Size = new System.Drawing.Size(49, 20);
            this.txtSW_Hex.TabIndex = 9;
            this.txtSW_Hex.Text = "000000";
            this.txtSW_Hex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtPC_Hex
            // 
            this.txtPC_Hex.Location = new System.Drawing.Point(34, 109);
            this.txtPC_Hex.Name = "txtPC_Hex";
            this.txtPC_Hex.ReadOnly = true;
            this.txtPC_Hex.Size = new System.Drawing.Size(49, 20);
            this.txtPC_Hex.TabIndex = 8;
            this.txtPC_Hex.Text = "000000";
            this.txtPC_Hex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 140);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "SW";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 111);
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
            this.label2.Location = new System.Drawing.Point(15, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "L";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "A";
            // 
            // txtX_Hex
            // 
            this.txtX_Hex.Location = new System.Drawing.Point(34, 82);
            this.txtX_Hex.Name = "txtX_Hex";
            this.txtX_Hex.ReadOnly = true;
            this.txtX_Hex.Size = new System.Drawing.Size(49, 20);
            this.txtX_Hex.TabIndex = 2;
            this.txtX_Hex.Text = "000000";
            this.txtX_Hex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtL_Hex
            // 
            this.txtL_Hex.Location = new System.Drawing.Point(33, 56);
            this.txtL_Hex.Name = "txtL_Hex";
            this.txtL_Hex.ReadOnly = true;
            this.txtL_Hex.Size = new System.Drawing.Size(49, 20);
            this.txtL_Hex.TabIndex = 1;
            this.txtL_Hex.Text = "000000";
            this.txtL_Hex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtA_Hex
            // 
            this.txtA_Hex.Location = new System.Drawing.Point(33, 30);
            this.txtA_Hex.Name = "txtA_Hex";
            this.txtA_Hex.ReadOnly = true;
            this.txtA_Hex.Size = new System.Drawing.Size(49, 20);
            this.txtA_Hex.TabIndex = 0;
            this.txtA_Hex.Text = "000000";
            this.txtA_Hex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmFile,
            this.tsmAbout,
            this.machineToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1061, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmFile
            // 
            this.tsmFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmOpen_SIC_Object_File,
            this.tsmloadAndAssembleSICSourceFIle,
            this.loadSavedSICMachineStateToolStripMenuItem,
            this.loadObjectFileToolStripMenuItem,
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
            this.tsmOpen_SIC_Object_File.Click += new System.EventHandler(this.tsmOpen_SIC_Object_File_Click);
            // 
            // tsmloadAndAssembleSICSourceFIle
            // 
            this.tsmloadAndAssembleSICSourceFIle.Name = "tsmloadAndAssembleSICSourceFIle";
            this.tsmloadAndAssembleSICSourceFIle.Size = new System.Drawing.Size(257, 22);
            this.tsmloadAndAssembleSICSourceFIle.Text = "Load and Assemble SIC Source FIle";
            this.tsmloadAndAssembleSICSourceFIle.Click += new System.EventHandler(this.tsmloadAndAssembleSICSourceFIle_Click);
            // 
            // loadSavedSICMachineStateToolStripMenuItem
            // 
            this.loadSavedSICMachineStateToolStripMenuItem.Name = "loadSavedSICMachineStateToolStripMenuItem";
            this.loadSavedSICMachineStateToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
            this.loadSavedSICMachineStateToolStripMenuItem.Text = "Load Saved SIC Machine State";
            this.loadSavedSICMachineStateToolStripMenuItem.Click += new System.EventHandler(this.loadSavedSICMachineStateToolStripMenuItem_Click);
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
            this.tsmFile_Ext.Click += new System.EventHandler(this.tsmFile_Ext_Click);
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
            this.randomizeAllMemory,
            this.setProgramCounterToToolStripMenuItem,
            this.stepSingleInstructionToolStripMenuItem,
            this.tsmsetMemoryBYTE,
            this.setMemoryWORDToolStripMenuItem,
            this.tsmresetSICVirtualMachine});
            this.machineToolStripMenuItem.Name = "machineToolStripMenuItem";
            this.machineToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.machineToolStripMenuItem.Text = "Machine";
            // 
            // tsmzeroAllMemory
            // 
            this.tsmzeroAllMemory.Name = "tsmzeroAllMemory";
            this.tsmzeroAllMemory.Size = new System.Drawing.Size(208, 22);
            this.tsmzeroAllMemory.Text = "Zero All Memory";
            // 
            // randomizeAllMemory
            // 
            this.randomizeAllMemory.Name = "randomizeAllMemory";
            this.randomizeAllMemory.Size = new System.Drawing.Size(208, 22);
            this.randomizeAllMemory.Text = "Randomize All Memory";
            // 
            // setProgramCounterToToolStripMenuItem
            // 
            this.setProgramCounterToToolStripMenuItem.Name = "setProgramCounterToToolStripMenuItem";
            this.setProgramCounterToToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.setProgramCounterToToolStripMenuItem.Text = "Set Program Counter To";
            this.setProgramCounterToToolStripMenuItem.Click += new System.EventHandler(this.setProgramCounterToToolStripMenuItem_Click);
            // 
            // stepSingleInstructionToolStripMenuItem
            // 
            this.stepSingleInstructionToolStripMenuItem.Name = "stepSingleInstructionToolStripMenuItem";
            this.stepSingleInstructionToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.stepSingleInstructionToolStripMenuItem.Text = "Step Single  Instruction";
            // 
            // tsmsetMemoryBYTE
            // 
            this.tsmsetMemoryBYTE.Name = "tsmsetMemoryBYTE";
            this.tsmsetMemoryBYTE.Size = new System.Drawing.Size(208, 22);
            this.tsmsetMemoryBYTE.Text = "Set Memory BYTE";
            this.tsmsetMemoryBYTE.Click += new System.EventHandler(this.tsmsetMemoryBYTE_Click);
            // 
            // setMemoryWORDToolStripMenuItem
            // 
            this.setMemoryWORDToolStripMenuItem.Name = "setMemoryWORDToolStripMenuItem";
            this.setMemoryWORDToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.setMemoryWORDToolStripMenuItem.Text = "Set Memory WORD";
            this.setMemoryWORDToolStripMenuItem.Click += new System.EventHandler(this.setMemoryWORDToolStripMenuItem_Click);
            // 
            // tsmresetSICVirtualMachine
            // 
            this.tsmresetSICVirtualMachine.Name = "tsmresetSICVirtualMachine";
            this.tsmresetSICVirtualMachine.Size = new System.Drawing.Size(208, 22);
            this.tsmresetSICVirtualMachine.Text = "Reset SIC Virtual Machine";
            this.tsmresetSICVirtualMachine.Click += new System.EventHandler(this.tsmresetSICVirtualMachine_Click);
            // 
            // tcMachine
            // 
            this.tcMachine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tcMachine.Controls.Add(this.tpMemory);
            this.tcMachine.Controls.Add(this.tpDevices);
            this.tcMachine.Controls.Add(this.tpMicroSteps);
            this.tcMachine.Location = new System.Drawing.Point(13, 226);
            this.tcMachine.Name = "tcMachine";
            this.tcMachine.SelectedIndex = 0;
            this.tcMachine.Size = new System.Drawing.Size(539, 514);
            this.tcMachine.TabIndex = 2;
            // 
            // tpMemory
            // 
            this.tpMemory.AutoScroll = true;
            this.tpMemory.Controls.Add(this.rtfMemory);
            this.tpMemory.Controls.Add(this.rbMemHex);
            this.tpMemory.Controls.Add(this.rbMemBinary);
            this.tpMemory.Location = new System.Drawing.Point(4, 22);
            this.tpMemory.Name = "tpMemory";
            this.tpMemory.Padding = new System.Windows.Forms.Padding(3);
            this.tpMemory.Size = new System.Drawing.Size(531, 488);
            this.tpMemory.TabIndex = 0;
            this.tpMemory.Text = "Memory";
            this.tpMemory.UseVisualStyleBackColor = true;
            // 
            // rtfMemory
            // 
            this.rtfMemory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtfMemory.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtfMemory.Location = new System.Drawing.Point(6, 31);
            this.rtfMemory.Name = "rtfMemory";
            this.rtfMemory.Size = new System.Drawing.Size(515, 448);
            this.rtfMemory.TabIndex = 3;
            this.rtfMemory.Text = "";
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
            // tpDevices
            // 
            this.tpDevices.Controls.Add(this.lvDevices);
            this.tpDevices.Location = new System.Drawing.Point(4, 22);
            this.tpDevices.Name = "tpDevices";
            this.tpDevices.Padding = new System.Windows.Forms.Padding(3);
            this.tpDevices.Size = new System.Drawing.Size(531, 488);
            this.tpDevices.TabIndex = 1;
            this.tpDevices.Text = "Devices";
            this.tpDevices.UseVisualStyleBackColor = true;
            // 
            // lvDevices
            // 
            this.lvDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvDevices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDeviceID,
            this.colOutput});
            this.lvDevices.HideSelection = false;
            this.lvDevices.Location = new System.Drawing.Point(5, 6);
            this.lvDevices.Name = "lvDevices";
            this.lvDevices.Size = new System.Drawing.Size(520, 476);
            this.lvDevices.TabIndex = 0;
            this.lvDevices.UseCompatibleStateImageBehavior = false;
            this.lvDevices.View = System.Windows.Forms.View.Details;
            // 
            // colDeviceID
            // 
            this.colDeviceID.Text = "Device ID";
            this.colDeviceID.Width = 64;
            // 
            // colOutput
            // 
            this.colOutput.Text = "ASCII Bytes Written";
            this.colOutput.Width = 300;
            // 
            // tpMicroSteps
            // 
            this.tpMicroSteps.Controls.Add(this.rtfMicroSteps);
            this.tpMicroSteps.Location = new System.Drawing.Point(4, 22);
            this.tpMicroSteps.Name = "tpMicroSteps";
            this.tpMicroSteps.Size = new System.Drawing.Size(531, 488);
            this.tpMicroSteps.TabIndex = 2;
            this.tpMicroSteps.Text = "Microsteps";
            this.tpMicroSteps.UseVisualStyleBackColor = true;
            // 
            // rtfMicroSteps
            // 
            this.rtfMicroSteps.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtfMicroSteps.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtfMicroSteps.Location = new System.Drawing.Point(5, 7);
            this.rtfMicroSteps.Name = "rtfMicroSteps";
            this.rtfMicroSteps.Size = new System.Drawing.Size(523, 472);
            this.rtfMicroSteps.TabIndex = 4;
            this.rtfMicroSteps.Text = "";
            // 
            // btnStep
            // 
            this.btnStep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStep.Location = new System.Drawing.Point(28, 746);
            this.btnStep.Name = "btnStep";
            this.btnStep.Size = new System.Drawing.Size(75, 23);
            this.btnStep.TabIndex = 4;
            this.btnStep.Text = "Step";
            this.btnStep.UseVisualStyleBackColor = true;
            this.btnStep.Click += new System.EventHandler(this.btnStep_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(362, 48);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(81, 13);
            this.label12.TabIndex = 5;
            this.label12.Text = "Next Instruction";
            // 
            // lblNextInstruction
            // 
            this.lblNextInstruction.AutoSize = true;
            this.lblNextInstruction.Location = new System.Drawing.Point(393, 64);
            this.lblNextInstruction.Name = "lblNextInstruction";
            this.lblNextInstruction.Size = new System.Drawing.Size(37, 13);
            this.lblNextInstruction.TabIndex = 6;
            this.lblNextInstruction.Text = "xxxxxx";
            // 
            // lblNI_Description
            // 
            this.lblNI_Description.AutoSize = true;
            this.lblNI_Description.Location = new System.Drawing.Point(393, 85);
            this.lblNI_Description.Name = "lblNI_Description";
            this.lblNI_Description.Size = new System.Drawing.Size(37, 13);
            this.lblNI_Description.TabIndex = 7;
            this.lblNI_Description.Text = "xxxxxx";
            // 
            // lblNextInstruction_Effect
            // 
            this.lblNextInstruction_Effect.AutoSize = true;
            this.lblNextInstruction_Effect.Location = new System.Drawing.Point(393, 105);
            this.lblNextInstruction_Effect.Name = "lblNextInstruction_Effect";
            this.lblNextInstruction_Effect.Size = new System.Drawing.Size(37, 13);
            this.lblNextInstruction_Effect.TabIndex = 8;
            this.lblNextInstruction_Effect.Text = "xxxxxx";
            // 
            // loadSICSourceFD
            // 
            this.loadSICSourceFD.FileName = "openFileDialog1";
            this.loadSICSourceFD.Filter = "SIC Source Files|*.sic";
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRun.Location = new System.Drawing.Point(190, 746);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 9;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // txtSICInput
            // 
            this.txtSICInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSICInput.Location = new System.Drawing.Point(6, 7);
            this.txtSICInput.Name = "txtSICInput";
            this.txtSICInput.Size = new System.Drawing.Size(469, 472);
            this.txtSICInput.TabIndex = 38;
            this.txtSICInput.Text = "";
            // 
            // txtObjectCode
            // 
            this.txtObjectCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtObjectCode.Location = new System.Drawing.Point(6, 7);
            this.txtObjectCode.Name = "txtObjectCode";
            this.txtObjectCode.Size = new System.Drawing.Size(542, 472);
            this.txtObjectCode.TabIndex = 39;
            this.txtObjectCode.Text = "";
            // 
            // btnResetProgram
            // 
            this.btnResetProgram.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnResetProgram.Location = new System.Drawing.Point(271, 746);
            this.btnResetProgram.Name = "btnResetProgram";
            this.btnResetProgram.Size = new System.Drawing.Size(74, 23);
            this.btnResetProgram.TabIndex = 40;
            this.btnResetProgram.Text = "Restart";
            this.btnResetProgram.UseVisualStyleBackColor = true;
            this.btnResetProgram.Click += new System.EventHandler(this.btnResetProgram_Click);
            // 
            // tbObjectCode
            // 
            this.tbObjectCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbObjectCode.Controls.Add(this.tbSICSymbol);
            this.tbObjectCode.Controls.Add(this.tbObjCode);
            this.tbObjectCode.Location = new System.Drawing.Point(558, 226);
            this.tbObjectCode.Name = "tbObjectCode";
            this.tbObjectCode.SelectedIndex = 0;
            this.tbObjectCode.Size = new System.Drawing.Size(491, 514);
            this.tbObjectCode.TabIndex = 41;
            // 
            // tbSICSymbol
            // 
            this.tbSICSymbol.Controls.Add(this.txtSICInput);
            this.tbSICSymbol.Location = new System.Drawing.Point(4, 22);
            this.tbSICSymbol.Name = "tbSICSymbol";
            this.tbSICSymbol.Padding = new System.Windows.Forms.Padding(3);
            this.tbSICSymbol.Size = new System.Drawing.Size(483, 488);
            this.tbSICSymbol.TabIndex = 0;
            this.tbSICSymbol.Text = "SIC Symbol Table";
            this.tbSICSymbol.UseVisualStyleBackColor = true;
            // 
            // tbObjCode
            // 
            this.tbObjCode.Controls.Add(this.txtObjectCode);
            this.tbObjCode.Location = new System.Drawing.Point(4, 22);
            this.tbObjCode.Name = "tbObjCode";
            this.tbObjCode.Padding = new System.Windows.Forms.Padding(3);
            this.tbObjCode.Size = new System.Drawing.Size(483, 488);
            this.tbObjCode.TabIndex = 1;
            this.tbObjCode.Text = "SIC Object Code";
            this.tbObjCode.UseVisualStyleBackColor = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // btnThreeStep
            // 
            this.btnThreeStep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnThreeStep.Location = new System.Drawing.Point(109, 746);
            this.btnThreeStep.Name = "btnThreeStep";
            this.btnThreeStep.Size = new System.Drawing.Size(75, 23);
            this.btnThreeStep.TabIndex = 9;
            this.btnThreeStep.Text = "3 Steps";
            this.btnThreeStep.UseVisualStyleBackColor = true;
            this.btnThreeStep.Click += new System.EventHandler(this.btnThreeStep_Click);
            // 
            // loadObjectFileToolStripMenuItem
            // 
            this.loadObjectFileToolStripMenuItem.Name = "loadObjectFileToolStripMenuItem";
            this.loadObjectFileToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
            this.loadObjectFileToolStripMenuItem.Text = "Load Object File";
            this.loadObjectFileToolStripMenuItem.Click += new System.EventHandler(this.loadObjectFileToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1061, 776);
            this.Controls.Add(this.tbObjectCode);
            this.Controls.Add(this.btnResetProgram);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnThreeStep);
            this.Controls.Add(this.lblNextInstruction_Effect);
            this.Controls.Add(this.lblNI_Description);
            this.Controls.Add(this.lblNextInstruction);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.btnStep);
            this.Controls.Add(this.tcMachine);
            this.Controls.Add(this.gbCPU);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "SIC Virtual Machine";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.gbCPU.ResumeLayout(false);
            this.gbCPU.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tcMachine.ResumeLayout(false);
            this.tpMemory.ResumeLayout(false);
            this.tpMemory.PerformLayout();
            this.tpDevices.ResumeLayout(false);
            this.tpMicroSteps.ResumeLayout(false);
            this.tbObjectCode.ResumeLayout(false);
            this.tbSICSymbol.ResumeLayout(false);
            this.tbObjCode.ResumeLayout(false);
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
        private System.Windows.Forms.TextBox txtX_Hex;
        private System.Windows.Forms.TextBox txtL_Hex;
        private System.Windows.Forms.TextBox txtA_Hex;
        private System.Windows.Forms.TextBox txtSW_Hex;
        private System.Windows.Forms.TextBox txtPC_Hex;
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
        private System.Windows.Forms.ToolStripMenuItem randomizeAllMemory;
        private System.Windows.Forms.ToolStripMenuItem setProgramCounterToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stepSingleInstructionToolStripMenuItem;
        private System.Windows.Forms.RadioButton rbMemHex;
        private System.Windows.Forms.RadioButton rbMemBinary;
        private System.Windows.Forms.ToolStripMenuItem tsmloadAndAssembleSICSourceFIle;
        private System.Windows.Forms.ToolStripMenuItem loadSavedSICMachineStateToolStripMenuItem;
        private System.Windows.Forms.TextBox txtSW_BIN_LSB;
        private System.Windows.Forms.TextBox txtSW_BIN_MIB;
        private System.Windows.Forms.TextBox txtSW_BIN_MSB;
        private System.Windows.Forms.TextBox txtPC_BIN_LSB;
        private System.Windows.Forms.TextBox txtPC_BIN_MIB;
        private System.Windows.Forms.TextBox txtPC_BIN_MSB;
        private System.Windows.Forms.TextBox txtX_BIN_LSB;
        private System.Windows.Forms.TextBox txtX_BIN_MIB;
        private System.Windows.Forms.TextBox txtX_BIN_MSB;
        private System.Windows.Forms.TextBox txtL_BIN_LSB;
        private System.Windows.Forms.TextBox txtL_BIN_MIB;
        private System.Windows.Forms.TextBox txtL_BIN_MSB;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtSW_Dec;
        private System.Windows.Forms.TextBox txtPC_Dec;
        private System.Windows.Forms.TextBox txtX_Dec;
        private System.Windows.Forms.TextBox txtL_Dec;
        private System.Windows.Forms.TextBox txtA_Dec;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtA_BIN_LSB;
        private System.Windows.Forms.TextBox txtA_BIN_MIB;
        private System.Windows.Forms.TextBox txtA_BIN_MSB;
        private System.Windows.Forms.Button btnStep;
        private System.Windows.Forms.ToolStripMenuItem tsmsetMemoryBYTE;
        private System.Windows.Forms.ToolStripMenuItem setMemoryWORDToolStripMenuItem;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblNextInstruction;
        private System.Windows.Forms.ToolStripMenuItem tsmresetSICVirtualMachine;
        private System.Windows.Forms.Label lblNI_Description;
        private System.Windows.Forms.Label lblNextInstruction_Effect;
        private System.Windows.Forms.OpenFileDialog loadSICSourceFD;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSW_CC;
        private System.Windows.Forms.ListView lvDevices;
        private System.Windows.Forms.ColumnHeader colDeviceID;
        private System.Windows.Forms.ColumnHeader colOutput;
        private System.Windows.Forms.Label lblComp_Result;
        private System.Windows.Forms.RichTextBox rtfMemory;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.RichTextBox txtSICInput;
        private System.Windows.Forms.RichTextBox txtObjectCode;
        private System.Windows.Forms.Button btnResetProgram;
        private System.Windows.Forms.TabControl tbObjectCode;
        private System.Windows.Forms.TabPage tbSICSymbol;
        private System.Windows.Forms.TabPage tbObjCode;
        private System.Windows.Forms.TabPage tpMicroSteps;
        private System.Windows.Forms.RichTextBox rtfMicroSteps;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Button btnThreeStep;
        private System.Windows.Forms.ToolStripMenuItem loadObjectFileToolStripMenuItem;
    }
}

