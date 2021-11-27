﻿using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;
using SIC_Simulator.Extensions;
using static System.Windows.Forms.ListViewItem;
using System.Diagnostics;
using System.Media;


namespace SIC_Simulator
{

    public partial class Form1 : Form
    {

        public String LastLoadedFileName = String.Empty;
        public int LastLoadedStart = 0;
        public int LastLoadedLength = 0;

        private SIC_CPU SICVirtualMachine;


        public Form1()
        {
            InitializeComponent();

            tsmAbout_About.Click += new EventHandler(tsmAbout_About_DropDownItemClicked);
            tsmzeroAllMemory.Click += new EventHandler(tsmzeroAllMemory_Click);
            randomizeAllMemory.Click += new EventHandler(randomizeAllMemory_Click);
            rbMemBinary.Click += new EventHandler(btnSnd_Click);
            rbMemHex.Click += new EventHandler(btnSnd_Click);
            rbMemDecimal.Click += new EventHandler(btnSnd_Click);
            rbMemAscii.Click += new EventHandler(btnSnd_Click);
            this.SICVirtualMachine = new SIC_CPU(true);

           
            //System.Threading.Thread St = new System.Threading.Thread( this.RefreshCPUDisplays);
            
        }
        
        private void tsmAbout_About_DropDownItemClicked(object sender, EventArgs e)
        {
            var menuItem = sender as ToolStripMenuItem;
            var menuText = menuItem.Text;


            switch (menuText)
            {
                case "About":
                    frmAbout fa = new frmAbout();
                    fa.ShowDialog();
                    break;

                case "Check for Updates":

                    break;
                
            }
        }

        private void tsmzeroAllMemory_Click(object sender, EventArgs e)
        {
            this.SICVirtualMachine.ZeroizeMemory();
            this.RefreshCPUDisplays();
        }

        private void randomizeAllMemory_Click(object sender, EventArgs e)
        {
            this.SICVirtualMachine.RandomizeMemory();
            this.RefreshCPUDisplays();
        }

        static string ByteArrayToHexStringViaBitConverter(byte[] bytes)
        {
            string hex = BitConverter.ToString(bytes);
            return hex.Replace("-", "");
        }


        private void tsmSaveMachineState_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.AddExtension = true;
            sfd.Filter = "SIC VM State Files|*.sicstate";

            DialogResult Result;

            Result = sfd.ShowDialog();


            if (Result == DialogResult.OK)
            {
                using (var stream = File.Open(sfd.FileName, FileMode.Create))
                {
                    SoapFormatter sf = new SoapFormatter();
                    sf.Serialize(stream, this.SICVirtualMachine);
                }
            }

            this.SICVirtualMachine.MachineStateIsNotSaved = false;
            
        }

        private void btnStep_Click(object sender, EventArgs e)
        {
            
            if ( this.SICVirtualMachine.PC == -1 )
            {
                MessageBox.Show("Program Stepping Halted. L=0, RSUB, PC = -1", "Program Halted" );
                return ;
            }
            
            this.SICVirtualMachine.PerformStep();

            this.RefreshCPUDisplays();
        }
        
        /// <summary>
        /// Refreshes the memory table whenever the Binary, Hex or Decimal buttons are clicked on
        /// Francisco Romero
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSnd_Click(object sender, EventArgs e){
            int num = new Random().Next(5000);
            if (num == 0){
                SoundPlayer snd = new SoundPlayer(Properties.Resources._1);
                snd.Play();
            }
            else if (num == 1) {
                SoundPlayer snd = new SoundPlayer(Properties.Resources._2);
                snd.Play();
            }
            else if (num == 2) {
                SoundPlayer snd = new SoundPlayer(Properties.Resources._3);
                snd.Play();
            }
            else if (num == 3) {
                SoundPlayer snd = new SoundPlayer(Properties.Resources._4);
                snd.Play();
            }
            

            RefreshCPUDisplays();
        }


        private async void RefreshCPUDisplays(){
            await RegRefreshAsync();
            await MemoryRefreshAsync();
            await DeviceRefreshAsync();

        }

        private async Task DeviceRefreshAsync()
        {
            await Task.Run(() => { });
            for (int i = 0; i < this.lvDevices.Items.Count; i++)
            { // update the sub list views with data found in the device array 
                this.lvDevices.Items[i].SubItems[1].Text = this.SICVirtualMachine.Devices[i].GetWriteBufferASCIIByteString;
            }
        }
        
        /// <summary>
        /// Refreshes Memory Display on background thread. Calls are marshalled to UI thread
        /// #todo : I think this is the memory table area, modifying it changed things atleast
        /// </summary>
        private async Task MemoryRefreshAsync()
        {
            if (rbMemHex.Checked == true) { // show in hex
                String Blob = ByteArrayToHexStringViaBitConverter(this.SICVirtualMachine.MemoryBytes);

                StringBuilder sb = new StringBuilder((32768 * 2) + 512);
                int StartIndex = 0;
                int Line = 0;

                int PCLine = 0;

                await Task.Run(() =>
                {
                    sb.AppendLine("{\\rtf1\\ansi ");
                    sb.AppendLine("{\\colortbl ;\\red0\\green255\\blue0;\\red255\\green255\\blue0;}");
                    // goes from counter 0000 - 8000
                    for (int Add = 0; Add < 32768; Add++)
                    {
                        if (Add == this.SICVirtualMachine.PC)
                        {
                            StartIndex = sb.ToString().Length;
                            if (Add == 0)
                            {
                                StartIndex += 6;
                            }
                        }
                        if ((Add % 16) == 0)
                        {
                            if (Add > 0)
                            {
                                sb.Append("\\line \\fs24 " + string.Format("{0:X4}: ", Add));
                                Line += 1;
                            }
                            else
                            {
                                sb.Append(string.Format("\\fs24 {0:X4}: ", Add));
                            }
                        }
                        if ((Add == this.SICVirtualMachine.PC) || (Add == this.SICVirtualMachine.PC + 1) || (Add == this.SICVirtualMachine.PC + 2))
                        {
                            sb.Append(String.Format("\\fs24 \\b \\highlight2 {0:X2}\\highlight0\\b0 \\fs24 ", Blob.Substring(Add * 2, 2)) + " ");
                            PCLine = Line;
                        }
                        else
                        {
                            sb.Append(String.Format("\\fs24 {0:X2}", Blob.Substring(Add * 2, 2)) + " ");
                        }

                    }
                });
                sb.Append("}");
                rtfMemory.Rtf = sb.ToString();
                rtfMemory.Select(PCLine * 55, 0);
                rtfMemory.ScrollToCaret();


                rtfMicroSteps.Text = this.SICVirtualMachine.MicrocodeSteps;
                rtfMicroSteps.Select(rtfMicroSteps.Text.Length, 0);
                rtfMicroSteps.ScrollToCaret();

            }
            else if (rbMemDecimal.Checked == true){ // Show in Decimal
                
                String Blob = ByteArrayToHexStringViaBitConverter(this.SICVirtualMachine.MemoryBytes);

                StringBuilder sb = new StringBuilder((32768 * 2) + 512);
                int StartIndex = 0;
                int Line = 0;

                int PCLine = 0;
                await Task.Run(() => {
                    sb.AppendLine("{\\rtf1\\ansi ");
                    sb.AppendLine("{\\colortbl ;\\red0\\green255\\blue0;\\red255\\green255\\blue0;}");
                    // goes from counter 0000 - 8000
                    for (int Add = 0; Add < 32768; Add++) // Add = address
                    {
                        if (Add == this.SICVirtualMachine.PC) {
                            StartIndex = sb.ToString().Length;
                            if (Add == 0)
                                StartIndex += 6;
                        }
                        if (Add % 16 == 0) { // prints counters on very left of table
                            if (Add > 0){
                                sb.Append("\\line \\fs20 " + string.Format("{0:D4}: ", Add));
                                Line += 1;
                            }
                            else // prints 0th counter
                                sb.Append(string.Format("\\fs20 {0:D4}: ", Add));
                        }
                        if (Add == this.SICVirtualMachine.PC || Add == this.SICVirtualMachine.PC + 1 || Add == this.SICVirtualMachine.PC + 2) { // the highlighted section
                            sb.Append(String.Format("\\fs20 \\b \\highlight2 {0:D3}\\highlight0\\b0 \\fs20 ", Int32.Parse(Blob.Substring(Add * 2, 2), System.Globalization.NumberStyles.AllowHexSpecifier)) + " ");
                            PCLine = Line;
                        }
                        else // all non highlighted bits
                            sb.Append(String.Format("\\fs20 {0:D3}", Int32.Parse(Blob.Substring(Add * 2, 2), System.Globalization.NumberStyles.AllowHexSpecifier)) + " ");
                    }
                });
                sb.Append("}");
                rtfMemory.Rtf = sb.ToString();
                rtfMemory.Select(PCLine * 71, 0); // 16 more for decimal for some reason?
                rtfMemory.ScrollToCaret();


                rtfMicroSteps.Text = this.SICVirtualMachine.MicrocodeSteps;
                rtfMicroSteps.Select(rtfMicroSteps.Text.Length, 0);
                rtfMicroSteps.ScrollToCaret();
            }
            else if (rbMemBinary.Checked == true) { // Show in Binary
                String Blob = ByteArrayToHexStringViaBitConverter(this.SICVirtualMachine.MemoryBytes);

                StringBuilder sb = new StringBuilder((32768 * 2) + 512);
                int StartIndex = 0;
                int Line = 0;
                int PCLine = 0;
                await Task.Run(() => {
                    sb.AppendLine("{\\rtf1\\ansi ");
                    sb.AppendLine("{\\colortbl ;\\red0\\green255\\blue0;\\red255\\green255\\blue0;}");
                    // goes from counter 0000 - 8000
                    for (int Add = 0; Add < 32768; Add++) // Add = address
                    {
                        if (Add == this.SICVirtualMachine.PC)
                        {
                            StartIndex = sb.ToString().Length;
                            if (Add == 0)
                                StartIndex += 6;
                        }
                        if (Add % 6 == 0)
                        { // prints counters on very left of table
                            if (Add > 0)
                            {
                                sb.Append("\\line \\fs20 " + string.Format("{0}: ", Convert.ToString(Add, 2).PadLeft(16, '0')));
                                Line += 1;
                            }
                            else // prints 0th counter
                                sb.Append(string.Format("\\fs20 {0}: ", Convert.ToString(Add, 2).PadLeft(16, '0')));
                        }
                        if (Add == this.SICVirtualMachine.PC || Add == this.SICVirtualMachine.PC + 1 || Add == this.SICVirtualMachine.PC + 2)
                        { // the highlighted section
                            sb.Append(String.Format("\\fs20 \\b \\highlight2 {0}\\highlight0\\b0 \\fs20 ", Convert.ToString(Int32.Parse(Blob.Substring(Add * 2, 2), System.Globalization.NumberStyles.AllowHexSpecifier), 2).PadLeft(8, '0') + ' '));
                            PCLine = Line;
                        }
                        else // all non highlighted bits
                            sb.Append(String.Format("{0}", Convert.ToString(Int32.Parse(Blob.Substring(Add * 2, 2), System.Globalization.NumberStyles.AllowHexSpecifier), 2).PadLeft(8, '0') + ' '));
                    }
                });
                sb.Append("}");
                rtfMemory.Rtf = sb.ToString();
                rtfMemory.Select(Math.Max(PCLine * 73 - 73, 0), 0); // amount of characters in row + 1
                rtfMemory.ScrollToCaret();

                rtfMicroSteps.Text = this.SICVirtualMachine.MicrocodeSteps;
                rtfMicroSteps.Select(rtfMicroSteps.Text.Length, 0);
                rtfMicroSteps.ScrollToCaret();
            }
            else { // Show ASCII table
                String Blob = ByteArrayToHexStringViaBitConverter(this.SICVirtualMachine.MemoryBytes);

                StringBuilder sb = new StringBuilder((32768 * 2) + 512);
                int StartIndex = 0;
                int Line = 0;

                int PCLine = 0;

                await Task.Run(() =>
                {
                    sb.AppendLine("{\\rtf1\\ansi ");
                    sb.AppendLine("{\\colortbl ;\\red0\\green255\\blue0;\\red255\\green255\\blue0;}");
                    // goes from counter 0000 - 8000
                    for (int Add = 0; Add < 32768; Add++) // Add = address
                    {
                        if (Add == this.SICVirtualMachine.PC)
                        {
                            StartIndex = sb.ToString().Length;
                            if (Add == 0)
                            {
                                StartIndex += 6;
                            }
                        }
                        if ((Add % 16) == 0)
                        { // prints counters on very left of table
                            if (Add > 0)
                            {
                                sb.Append("\\line \\fs24 " + string.Format("{0:X4}: ", Add));
                                Line += 1;
                            }
                            else
                            { // prints 0th counter
                                sb.Append(string.Format("\\fs24 {0:X4}: ", Add));
                            }
                        }
                        if ((Add == this.SICVirtualMachine.PC) || (Add == this.SICVirtualMachine.PC + 1) || (Add == this.SICVirtualMachine.PC + 2))
                        { // the highlighted section
                            int temp = Int32.Parse(Blob.Substring(Add * 2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                            if(temp < 32)
                            {
                                sb.Append(String.Format("\\fs24 \\b \\highlight2 {0}\\highlight0\\b0 \\fs24 ", "." + ' ') + " ");
                            } else {
                                sb.Append(String.Format("\\fs24 \\b \\highlight2 {0}\\highlight0\\b0 \\fs24 ", Char.ConvertFromUtf32(temp) + ' ') + " ");
                            }
                            PCLine = Line;
                        }
                        else
                        { // all non highlighted bits. This is where the ASCII values get printed
                            int temp = Int32.Parse(Blob.Substring(Add * 2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                            if(temp < 32)
                            {
                                sb.Append(String.Format("{0}", "." + ' ') + " ");
                            } else {
                                sb.Append(String.Format("{0}", Char.ConvertFromUtf32(temp) + ' ') + " ");
                            }
                        }

                    }
                });
                sb.Append("}");
                rtfMemory.Rtf = sb.ToString();
                rtfMemory.Select(PCLine * 55, 0); // amount of characters in row + 1
                rtfMemory.ScrollToCaret();


                rtfMicroSteps.Text = this.SICVirtualMachine.MicrocodeSteps;
                rtfMicroSteps.Select(rtfMicroSteps.Text.Length, 0);
                rtfMicroSteps.ScrollToCaret();

            }

        }

        private async Task RegRefreshAsync()
        {
            await Task.Run(() => { }); //This is bad, but whatever.
            txtX_Hex.Text = SICVirtualMachine.X.ToString("X6");
            txtA_Hex.Text = SICVirtualMachine.A.ToString("X6");
            txtL_Hex.Text = SICVirtualMachine.L.ToString("X6");
            txtPC_Hex.Text = SICVirtualMachine.PC.ToString("X6");
            txtSW_Hex.Text = SICVirtualMachine.SW.ToString("X6");

            txtX_Dec.Text = SICVirtualMachine.X.ToString();
            txtA_Dec.Text = SICVirtualMachine.A.ToString();
            txtL_Dec.Text = SICVirtualMachine.L.ToString();
            txtPC_Dec.Text = SICVirtualMachine.PC.ToString();
            txtSW_Dec.Text = SICVirtualMachine.SW.ToString();

            // Now do the binary bytes for each register and Status Word
            String PC_BIN = this.SICVirtualMachine.PC.To24BITBinary();
            txtPC_BIN_MSB.Text = PC_BIN.Substring(0, 8);
            txtPC_BIN_MIB.Text = PC_BIN.Substring(8, 8);
            txtPC_BIN_LSB.Text = PC_BIN.Substring(16);


            String L_BIN = SICVirtualMachine.L.To24BITBinary();
            txtL_BIN_MSB.Text = L_BIN.Substring(0, 8);
            txtL_BIN_MIB.Text = L_BIN.Substring(8, 8);
            txtL_BIN_LSB.Text = L_BIN.Substring(16);

            String A_BIN = SICVirtualMachine.A.To24BITBinary();
            txtA_BIN_MSB.Text = A_BIN.Substring(0, 8);
            txtA_BIN_MIB.Text = A_BIN.Substring(8, 8);
            txtA_BIN_LSB.Text = A_BIN.Substring(16);

            String X_BIN = SICVirtualMachine.X.To24BITBinary();
            txtX_BIN_MSB.Text = X_BIN.Substring(0, 8);
            txtX_BIN_MIB.Text = X_BIN.Substring(8, 8);
            txtX_BIN_LSB.Text = X_BIN.Substring(16);



            String SW_BIN = this.SICVirtualMachine.SW.To24BITBinary();
            txtSW_BIN_MSB.Text = SW_BIN.Substring(0, 8);
            txtSW_BIN_MIB.Text = SW_BIN.Substring(8, 8);
            txtSW_BIN_LSB.Text = SW_BIN.Substring(16);
            txtSW_CC.Text = txtSW_BIN_LSB.Text.Substring(0,2);
            lblComp_Result.Text = txtSW_BIN_LSB.Text[0] == 49 ? "Greater than" : txtSW_BIN_LSB.Text[1] == 49 ? "Less than" : "Equal";

            String NextInstructionD;            


            if ( this.SICVirtualMachine.PC >= 0)
            {
            NextInstructionD = SICVirtualMachine.GetInstructionDescription(SICVirtualMachine.PC);

            String[] NextInstructionPieces;

            NextInstructionPieces = NextInstructionD.Split('|');


            lblNextInstruction.Text = NextInstructionPieces[0];
            lblNI_Description.Text = NextInstructionPieces[1];
            lblNextInstruction_Effect.Text = NextInstructionPieces[2];
            } else
            {
                lblNextInstruction.Text = "Program Halted";
                lblNI_Description.Text = "VM Halted by Software Instruction";
                lblNextInstruction_Effect.Text = "VM Halted";
            }



        }


        private void loadSavedSICMachineStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            DialogResult Res;
            ofd.Filter = "SIC VM State Files|*.sicstate";
            ofd.Multiselect = false;

            Res = ofd.ShowDialog();

            if ( Res == DialogResult.OK)
            {
                using (var stream = File.Open(ofd.FileName, FileMode.Open))
                {
                    SoapFormatter osf = new SoapFormatter();
                    this.SICVirtualMachine = (SIC_CPU) osf.Deserialize(stream); 
                }
                // Refresh Memory and Register Displays to Show Saved State
                this.RefreshCPUDisplays();

            }
            this.SICVirtualMachine.MachineStateIsNotSaved = false;

        }

        private void tsmsetMemoryBYTE_Click(object sender, EventArgs e)
        {
            dlgSetMemoryByte SetMemByte = new dlgSetMemoryByte();
            DialogResult Result;

            Result = SetMemByte.ShowDialog();

            if ( Result == DialogResult.Cancel )
            {
                return;
            }

            this.SICVirtualMachine.StoreByte(SetMemByte.MemoryAddress, SetMemByte.ByteValue);

            this.RefreshCPUDisplays();

        }


        private int MemorizedLastMemoryWordAddress = 0;

        private void setMemoryWORDToolStripMenuItem_Click(object sender, EventArgs e)
        {



            dlgSetMemoryWord SetMemWord;
                
            if (this.MemorizedLastMemoryWordAddress == 0 )
            {
               SetMemWord = new dlgSetMemoryWord();
            } else
            {
                SetMemWord = new dlgSetMemoryWord( this.MemorizedLastMemoryWordAddress);
            }
                
            DialogResult Result;

            Result = SetMemWord.ShowDialog();

            if (Result == DialogResult.Cancel)
            {
                return;
            }
            this.MemorizedLastMemoryWordAddress = SetMemWord.MemoryAddress;
            this.SICVirtualMachine.StoreWord(SetMemWord.MemoryAddress, SetMemWord.WordValue);
            this.RefreshCPUDisplays();
        }

        private void tsmresetSICVirtualMachine_Click(object sender, EventArgs e)
        {
            DialogResult Result;

            Result = MessageBox.Show("This will zero all memory locations and reset all registers to zero. Are you sure you want to proceed?", "Confirm", MessageBoxButtons.YesNo);
            
            if ( Result == DialogResult.Yes )
            {
            this.SICVirtualMachine = new SIC_CPU(true);
            this.RefreshCPUDisplays();
            }

        }


        private void ReadEndRecord( string line, ref int FirstExecIns)
        {
            int i = 1, num = 0;
            while (i < 7)
            {
                char ch = line[i++];
                if (ch >= 'A')
                {
                    ch -= (char)7;
                }

                ch -= (char)48;
                num += (int)ch;
                num = num << 4;
            }
            FirstExecIns = num >> 4;
        }



        private void ReadTextRecord( string line, ref int RecordStartAdd, ref int RecordLength)
        {
            int i = 1, num = 0;
            while (i < 7)
            {
                char ch = line[i++];
                if (ch >= 'A')
                {
                    ch -= (char)7;
                }

                ch -= (char)48;
                num += (int)ch;
                num = num << 4;
            }
            num = num >> 4;
            RecordStartAdd = num;
            num = 0;
            while (i < 9)
            {
                char ch = line[i++];
                if (ch >= 'A')
                {
                    ch -= (char)7;
                }

                ch -= (char)48;
                num += (int)ch;
                num = num << 4;
            }
            num = num >> 4;
            RecordLength = num;

        }

        private void tsmOpen_SIC_Object_File_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            DialogResult Res;
            ofd.Filter = "SIC Object Files|*.sic.obj";
            ofd.Multiselect = false;

            Res = ofd.ShowDialog();

            if (Res == DialogResult.OK)
            {
                System.IO.StreamReader file = new System.IO.StreamReader(ofd.FileName);
                String fileText = file.ReadToEnd();
                this.txtObjectCode.Text = fileText;
                LoadObjectFile(fileText.Split('\n'));
                file.Close();

                this.RefreshCPUDisplays();

            }
        }

        private void tsmloadAndAssembleSICSourceFIle_Click(object sender, EventArgs e)
        {
            if (loadSICSourceFD.ShowDialog() == DialogResult.OK)
            {
                Assembler assembler = new Assembler(loadSICSourceFD.FileName);

                if ( !String.IsNullOrEmpty(assembler.ObjectCode) )
                {
                    // We need to call the loader, or use the quick loader in this form
                    // to load the assembled code into memory

                    this.txtSICInput.Text = assembler.InstructionSource;
                    this.txtObjectCode.Text = assembler.ObjectCode;
                    
                    String[] lines = assembler.ObjectCode.Split('\n');
                    LoadObjectFile(lines);
                }
                this.RefreshCPUDisplays(); // refresh memory after object code is loaded


                this.LastLoadedFileName = System.IO.Path.GetFileName(loadSICSourceFD.FileName);
                
            }
        }

        private void LoadObjectFile(String[] lines) {
            foreach (string line in lines)
            {
                if (String.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (line[0] == 'H')
                {
                    var firstAddress = line.Substring(7, 6);
                    var programSize = line.Substring(13, 6);
                    this.SICVirtualMachine.CurrentProgramEndAddress = Int32.Parse(firstAddress, System.Globalization.NumberStyles.HexNumber) + Int32.Parse(programSize, System.Globalization.NumberStyles.HexNumber);
                    // Read The Header Record
                    // In this context, not much to do here.
                    // from header record. 
                    // The linker module and full-implementation loader
                    // will need to look at the H records


                    this.LastLoadedStart = Int32.Parse(firstAddress, System.Globalization.NumberStyles.HexNumber);
                    this.LastLoadedLength = Int32.Parse(programSize, System.Globalization.NumberStyles.HexNumber);

                }
                if (line[0] == 'T')
                {
                    // Read T Text Record
                    int RecordStartAddress = 0;
                    int RecordLength = 0;
                    ReadTextRecord(line, ref RecordStartAddress, ref RecordLength);
                    this.SICVirtualMachine.LoadToMemory(line, RecordStartAddress, RecordLength);
                }

                if (line[0] == 'E')
                {
                    // Read The End Record and Set PC
                    int AddressOfFirstInstruction = 0;
                    ReadEndRecord(line, ref AddressOfFirstInstruction);
                    this.SICVirtualMachine.PC = AddressOfFirstInstruction;
                    this.SICVirtualMachine.CurrentProgramStartAddress = AddressOfFirstInstruction;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.SICVirtualMachine.PerformStep();

            this.RefreshCPUDisplays();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lvDevices.Dock = DockStyle.Fill;
            for (int i = 0; i < SIC_CPU.NumDevices; i++)
            { //seed lsit view for devices with 64 items
                ListViewItem lvItem = new ListViewItem(String.Format("{0,2:D2}", i));
                lvItem.SubItems.Add("");
                this.lvDevices.Items.Add(lvItem);
            }
            lvDevices.View = View.Details;
            RefreshCPUDisplays();
        }

        private void tsmFile_Ext_Click(object sender, EventArgs e)
        {

            if ( this.SICVirtualMachine.MachineStateIsNotSaved == false)
            {
                DialogResult NotProceed;
                NotProceed = MessageBox.Show("The current machine state has not been saved. Do you want to cancel exit and save your machine state?", "Machine State Not Saved", MessageBoxButtons.YesNo);
        
                if ( NotProceed == DialogResult.Yes )
                {
                    return;
                }

            }
               Application.Exit();

        }

        private void setProgramCounterToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlgSetRegisterWord SetRegWord = new dlgSetRegisterWord( "PC");
            DialogResult Result;

            Result = SetRegWord.ShowDialog();

            if (Result == DialogResult.Cancel)
            {
                return;
            }
            this.SICVirtualMachine.PC = SetRegWord.WordValue;
            this.RefreshCPUDisplays();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            int StopAtPCAddress = 0;

            DialogResult Result;
            dlgStopAtMemoryAddress SetStop = new dlgStopAtMemoryAddress( this.LastLoadedFileName, this.LastLoadedStart, this.LastLoadedLength);
            Result = SetStop.ShowDialog();

            if ( Result == DialogResult.OK)
            {
                StopAtPCAddress = SetStop.HaltAtMemoryAddress;
                while (this.SICVirtualMachine.PC != StopAtPCAddress )
                {
                this.SICVirtualMachine.PerformStep();
                }
                this.RefreshCPUDisplays(); 
            }



        }

        private void btnResetProgram_Click(object sender, EventArgs e)
        {
            LoadObjectFile(this.txtObjectCode.Text.Split('\n'));
            this.RefreshCPUDisplays();
        }

        private void btnThreeStep_Click(object sender, EventArgs e)
        {
            this.SICVirtualMachine.PerformStep();
            this.RefreshCPUDisplays();
            this.SICVirtualMachine.PerformStep();
            this.RefreshCPUDisplays();
            this.SICVirtualMachine.PerformStep();
            this.RefreshCPUDisplays();
        }

        private void loadObjectFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd;
            System.Windows.Forms.DialogResult Result;

            ofd = new OpenFileDialog();
            ofd.Filter = "SIC Object Files|*.sic.obj";
            ofd.Multiselect = false;
            ofd.Title = "Select SIC Object File";

            Result = ofd.ShowDialog();

            if ( Result == DialogResult.OK)
            {
                // we need to open ofd.FileName
                // Find out where it was assembled
                // Ask for new load location
                int NewAddress = 0;
                int StartAddress = 0;
                int PLength = 0;
                int ModRecordCount = 0;
                String ObjectFileName = ofd.FileName;
                String ProgramName = string.Empty;

                try
                {

                    String[] lines = System.IO.File.ReadAllLines(ofd.FileName);
                    
                      
                    foreach (string line in lines)
                    {
                        if (String.IsNullOrWhiteSpace(line))
                        {
                            continue;
                        }


                        if (line[0] == 'H')
                        {
                            // We need to retrieve First address and program size
                            StartAddress = int.Parse(line.Substring(7, 6), System.Globalization.NumberStyles.HexNumber);
                            PLength= int.Parse(line.Substring(13, 6), System.Globalization.NumberStyles.HexNumber);
                            ProgramName = line.Substring(1,6).TrimEnd();
                            //this.SICVirtualMachine.CurrentProgramEndAddress = Int32.Parse(firstAddress, System.Globalization.NumberStyles.HexNumber) + Int32.Parse(programSize, System.Globalization.NumberStyles.HexNumber);
                        }
                        if (line[0] == 'M')
                        {
                            ModRecordCount += 1;
                            
                        }
                    }

                }
                catch (Exception Ex)
                {
                    MessageBox.Show("There was an error reading the object file you specified: " + Ex.ToString(), "Error Opening Object File");
                    return;

                }


                DialogResult RelocationResult;
                dlgRelocateObjectFile RelocationDialog = new dlgRelocateObjectFile(ProgramName, StartAddress, PLength, ModRecordCount);

                RelocationResult = RelocationDialog.ShowDialog();

                if ( RelocationResult != DialogResult.OK)
                {
                    return;
                }


                NewAddress = RelocationDialog.RelocatedToAddress;

                // Call the loader!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


                // Return address (absolute) of first instruction after relocation
                // This will be placed in the PC

                //this.SICVirtualMachine.PC =    (start value from relocated program code)


            }





        }

        private void loadSICSourceFD_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
