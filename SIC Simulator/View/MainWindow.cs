﻿using SIC_Simulator.Extensions;
using System;
using System.IO;
using System.Media;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace SIC_Simulator
{
    public partial class MainWindow : Form
    {
        public string LastLoadedFileName = string.Empty;
        public int LastLoadedStart = 0;
        public int LastLoadedLength = 0;
        private int MemorizedLastMemoryWordAddress = 0;

        private SIC_CPU SICVirtualMachine;

        public MainWindow()
        {
            InitializeComponent();

            tsmAbout_About.Click += new EventHandler(TsmAbout_About_DropDownItemClicked);
            tsmzeroAllMemory.Click += new EventHandler(TsmZeroAllMemory_Click);
            randomizeAllMemory.Click += new EventHandler(RandomizeAllMemory_Click);
            rbMemBinary.Click += new EventHandler(BtnSnd_Click);
            rbMemHex.Click += new EventHandler(BtnSnd_Click);
            rbMemDecimal.Click += new EventHandler(BtnSnd_Click);
            rbMemAscii.Click += new EventHandler(BtnSnd_Click);
            SICVirtualMachine = new SIC_CPU(true);
        }

        private void TsmAbout_About_DropDownItemClicked(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;

            switch (menuItem.Text)
            {
                case "About":
                    frmAbout fa = new frmAbout();
                    fa.ShowDialog();
                    break;

                case "Check for Updates":

                    break;
            }
        }

        private void TsmZeroAllMemory_Click(object sender, EventArgs e)
        {
            SICVirtualMachine.ZeroAllMemory();
            RefreshCPUDisplays();
        }

        private void RandomizeAllMemory_Click(object sender, EventArgs e)
        {
            SICVirtualMachine.RandomizeMemory();
            RefreshCPUDisplays();
        }

        private static string ByteArrayToHexStringViaBitConverter(byte[] bytes)
        {
            string hex = BitConverter.ToString(bytes);
            return hex.Replace("-", "");
        }

        private void TsmSaveMachineState_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                AddExtension = true,
                Filter = "SIC VM State Files|*.sicstate"
            };

            if (sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            using (FileStream stream = File.Open(sfd.FileName, FileMode.Create))
            {
                SoapFormatter sf = new SoapFormatter();
                sf.Serialize(stream, SICVirtualMachine);
            }

            SICVirtualMachine.MachineStateIsNotSaved = false;
        }

        private void BtnStep_Click(object sender, EventArgs e)
        {
            if (SICVirtualMachine.PC == -1)
            {
                MessageBox.Show("Program Stepping Halted. L=0, RSUB, PC = -1", "Program Halted");
                return;
            }

            SICVirtualMachine.PerformStep();

            RefreshCPUDisplays();
        }

        /// <summary>
        /// Refreshes the memory table whenever the Binary, Hex or Decimal buttons are clicked on
        /// Francisco Romero
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSnd_Click(object sender, EventArgs e)
        {
            int num = new Random().Next(5000);//I should remove this but w/e

            if (num == 0)
            {
                SoundPlayer snd = new SoundPlayer(Properties.Resources._1);
                snd.Play();
            }
            else if (num == 1)
            {
                SoundPlayer snd = new SoundPlayer(Properties.Resources._2);
                snd.Play();
            }
            else if (num == 2)
            {
                SoundPlayer snd = new SoundPlayer(Properties.Resources._3);
                snd.Play();
            }
            else if (num == 3)
            {
                SoundPlayer snd = new SoundPlayer(Properties.Resources._4);
                snd.Play();
            }

            RefreshCPUDisplays();
        }

        private async void RefreshCPUDisplays()
        {
            await RegRefreshAsync();
            await MemoryRefreshAsync();
            await DeviceRefreshAsync();

        }

        private async Task DeviceRefreshAsync()
        {
            await Task.Run(() => { });
            for (int i = 0; i < lvDevices.Items.Count; i++)
            { // update the sub list views with data found in the device array 
                lvDevices.Items[i].SubItems[1].Text = SICVirtualMachine.Devices[i].GetWriteBufferASCIIByteString;
            }
        }

        /// <summary>
        /// Refreshes Memory Display on background thread. Calls are marshalled to UI thread
        /// </summary>
        private async Task MemoryRefreshAsync()
        {
            string blob = ByteArrayToHexStringViaBitConverter(SICVirtualMachine.MemoryBytes);
            int lineWidth = rbMemBinary.Checked ? 8 : 16;
            Func<int, string> conversion;

            if (rbMemHex.Checked)
            {
                conversion = (add) => blob.Substring(add * 2, 2);
            }
            else if (rbMemDecimal.Checked)
            {
                conversion = (add) => $"{GetValueFromBlob(add):D3}";
            }
            else if (rbMemBinary.Checked)
            {
                conversion = (add) => Convert.ToString(GetValueFromBlob(add), 2).PadLeft(8, '0');
            }
            else
            {
                conversion = (add) =>
                {
                    int val = GetValueFromBlob(add);

                    return val < 32 ? "." : char.ConvertFromUtf32(val);
                };
            }

            int scrollTo = 0;
            StringBuilder sb = new StringBuilder((0x8000 * 2) + 512);

            await Task.Run(() =>
            {
                sb.AppendLine("{\\rtf1 \\ansi ");
                sb.AppendLine("{\\colortbl ;\\red0\\green255\\blue0;\\red255\\green255\\blue0;}");

                for (int add = 0; add < 0x8000; add++) // add = address
                {
                    if (add % lineWidth == 0)
                    {   // prints counters on very left of table
                        if (add > 0)
                        {
                            sb.Append("\\line");
                        }

                        sb.Append($"\\fs20 {add:X4}:");
                    }

                    string value = conversion(add);

                    if (add >= SICVirtualMachine.PC && add <= SICVirtualMachine.PC + 2)
                    {   // the highlighted section
                        sb.Append($" \\fs20 \\b \\highlight2 {value}\\highlight0 \\b0 \\fs20 ");
                        scrollTo = sb.Length;
                    }
                    else // all non highlighted bits
                    {
                        sb.Append($"\\fs20  {value}");
                    }
                }

                sb.Append("}");
            });

            rtfMemory.Rtf = sb.ToString();
            rtfMicroSteps.Text = SICVirtualMachine.MicrocodeSteps;

            rtfMemory.Select(scrollTo, 0);
            rtfMemory.ScrollToCaret();

            int GetValueFromBlob(int address)
            {
                return int.Parse(blob.Substring(address * 2, 2), NumberStyles.HexNumber);
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
            string PC_BIN = SICVirtualMachine.PC.To24BITBinary();
            txtPC_BIN_MSB.Text = PC_BIN.Substring(0, 8);
            txtPC_BIN_MIB.Text = PC_BIN.Substring(8, 8);
            txtPC_BIN_LSB.Text = PC_BIN.Substring(16);

            string L_BIN = SICVirtualMachine.L.To24BITBinary();
            txtL_BIN_MSB.Text = L_BIN.Substring(0, 8);
            txtL_BIN_MIB.Text = L_BIN.Substring(8, 8);
            txtL_BIN_LSB.Text = L_BIN.Substring(16);

            string A_BIN = SICVirtualMachine.A.To24BITBinary();
            txtA_BIN_MSB.Text = A_BIN.Substring(0, 8);
            txtA_BIN_MIB.Text = A_BIN.Substring(8, 8);
            txtA_BIN_LSB.Text = A_BIN.Substring(16);

            string X_BIN = SICVirtualMachine.X.To24BITBinary();
            txtX_BIN_MSB.Text = X_BIN.Substring(0, 8);
            txtX_BIN_MIB.Text = X_BIN.Substring(8, 8);
            txtX_BIN_LSB.Text = X_BIN.Substring(16);

            string SW_BIN = SICVirtualMachine.SW.To24BITBinary();
            txtSW_BIN_MSB.Text = SW_BIN.Substring(0, 8);
            txtSW_BIN_MIB.Text = SW_BIN.Substring(8, 8);
            txtSW_BIN_LSB.Text = SW_BIN.Substring(16);
            txtSW_CC.Text = txtSW_BIN_LSB.Text.Substring(0, 2);
            lblComp_Result.Text = txtSW_BIN_LSB.Text[0] == 49 ? "Greater than" : txtSW_BIN_LSB.Text[1] == 49 ? "Less than" : "Equal";

            string NextInstructionD;

            if (SICVirtualMachine.PC >= 0)
            {
                NextInstructionD = SICVirtualMachine.GetInstructionDescription(SICVirtualMachine.PC);

                string[] NextInstructionPieces;

                NextInstructionPieces = NextInstructionD.Split('|');


                lblNextInstruction.Text = NextInstructionPieces[0];
                lblNI_Description.Text = NextInstructionPieces[1];
                lblNextInstruction_Effect.Text = NextInstructionPieces[2];
            }
            else
            {
                lblNextInstruction.Text = "Program Halted";
                lblNI_Description.Text = "VM Halted by Software Instruction";
                lblNextInstruction_Effect.Text = "VM Halted";
            }
        }


        private void LoadSavedSICMachineStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "SIC VM State Files|*.sicstate";
            ofd.Multiselect = false;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = File.Open(ofd.FileName, FileMode.Open))
                {
                    SoapFormatter osf = new SoapFormatter();
                    SICVirtualMachine = (SIC_CPU)osf.Deserialize(stream);
                }

                // Refresh Memory and Register Displays to Show Saved State
                RefreshCPUDisplays();
            }

            SICVirtualMachine.MachineStateIsNotSaved = false;
        }

        private void TsmSetMemoryBYTE_Click(object sender, EventArgs e)
        {
            dlgSetMemoryByte SetMemByte = new dlgSetMemoryByte();

            if (SetMemByte.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            SICVirtualMachine.StoreByte(SetMemByte.MemoryAddress, SetMemByte.ByteValue);

            RefreshCPUDisplays();
        }

        private void SetMemoryWORDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlgSetMemoryWord SetMemWord;

            if (MemorizedLastMemoryWordAddress == 0)
            {
                SetMemWord = new dlgSetMemoryWord();
            }
            else
            {
                SetMemWord = new dlgSetMemoryWord(MemorizedLastMemoryWordAddress);
            }

            if (SetMemWord.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            MemorizedLastMemoryWordAddress = SetMemWord.MemoryAddress;
            SICVirtualMachine.StoreWord(SetMemWord.MemoryAddress, SetMemWord.WordValue);

            RefreshCPUDisplays();
        }

        private void TsmResetSICVirtualMachine_Click(object sender, EventArgs e)
        {
            DialogResult Result;

            Result = MessageBox.Show("This will zero all memory locations and reset all registers to zero. Are you sure you want to proceed?", "Confirm", MessageBoxButtons.YesNo);

            if (Result == DialogResult.Yes)
            {
                SICVirtualMachine = new SIC_CPU(true);
                RefreshCPUDisplays();
            }
        }

        private void TsmOpen_SIC_Object_File_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "SIC Object Files|*.sic.obj";
            ofd.Multiselect = false;

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            (int start, int length) last;
            using (StreamReader file = new StreamReader(ofd.FileName))
            {
                string fileText = file.ReadToEnd();

                txtObjectCode.Text = fileText;
                last = SICLoader.LoadObjectFileIntoCPU(fileText.Split('\n'), SICVirtualMachine);
            }

            LastLoadedStart = last.start;
            LastLoadedLength = last.length;

            RefreshCPUDisplays();
        }

        private void TsmloadAndAssembleSICSourceFIle_Click(object sender, EventArgs e)
        {
            if (loadSICSourceFD.ShowDialog() == DialogResult.OK)
            {
                Assembler assembler;

                try
                {
                    assembler = new Assembler(loadSICSourceFD.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                if (!string.IsNullOrEmpty(assembler.ObjectCode))
                {
                    // We need to call the loader, or use the quick loader in this form
                    // to load the assembled code into memory

                    txtSICInput.Text = assembler.InstructionSource;
                    txtObjectCode.Text = assembler.ObjectCode;

                    string[] lines = assembler.ObjectCode.Split('\n');
                    var last = SICLoader.LoadObjectFileIntoCPU(lines, SICVirtualMachine);

                    LastLoadedStart = last.start;
                    LastLoadedLength = last.length;
                }

                RefreshCPUDisplays(); // refresh memory after object code is loaded
                LastLoadedFileName = Path.GetFileName(loadSICSourceFD.FileName);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lvDevices.Dock = DockStyle.Fill;

            for (int i = 0; i < SIC_CPU.NumDevices; i++)
            { //seed list view for devices with 64 items
                ListViewItem lvItem = new ListViewItem(string.Format("{0,2:D2}", i));

                lvItem.SubItems.Add("");
                lvDevices.Items.Add(lvItem);
            }

            lvDevices.View = View.Details;
            RefreshCPUDisplays();
        }

        private void TsmFile_Ext_Click(object sender, EventArgs e)
        {
            if (SICVirtualMachine.MachineStateIsNotSaved == false)
            {
                DialogResult stop = MessageBox.Show("The current machine state has not been saved. Do you want to cancel exit and save your machine state?", "Machine State Not Saved", MessageBoxButtons.YesNo);

                if (stop == DialogResult.Yes)
                {
                    return;
                }
            }

            Application.Exit();
        }

        private void SetProgramCounterToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlgSetRegisterWord SetRegWord = new dlgSetRegisterWord("PC");
            if (SetRegWord.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            SICVirtualMachine.PC = SetRegWord.WordValue;
            RefreshCPUDisplays();
        }

        private void BtnRun_Click(object sender, EventArgs e)
        {
            dlgStopAtMemoryAddress setStop = new dlgStopAtMemoryAddress(LastLoadedFileName, LastLoadedStart, LastLoadedLength);
            if (setStop.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            int StopAtPCAddress = setStop.HaltAtMemoryAddress;
            while (SICVirtualMachine.PC != StopAtPCAddress)
            {
                SICVirtualMachine.PerformStep();
            }

            RefreshCPUDisplays();
        }

        private void BtnResetProgram_Click(object sender, EventArgs e)
        {
            var last = SICLoader.LoadObjectFileIntoCPU(txtObjectCode.Text.Split('\n'), SICVirtualMachine);

            LastLoadedStart = last.start;
            LastLoadedLength = last.length;

            RefreshCPUDisplays();
        }

        private void BtnThreeStep_Click(object sender, EventArgs e)
        {
            SICVirtualMachine.PerformStep();
            RefreshCPUDisplays();
            SICVirtualMachine.PerformStep();
            RefreshCPUDisplays();
            SICVirtualMachine.PerformStep();
            RefreshCPUDisplays();
        }

        private void LoadObjectFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "SIC Object Files|*.sic.obj",
                Multiselect = false,
                Title = "Select SIC Object File"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // we need to open ofd.FileName
            // Find out where it was assembled
            // Ask for new load location
            int NewAddress = 0;
            int StartAddress = 0;
            int PLength = 0;
            int ModRecordCount = 0;
            string ObjectFileName = ofd.FileName;
            string ProgramName = string.Empty;

            try
            {
                string[] lines = File.ReadAllLines(ofd.FileName);

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    if (line[0] == 'H')
                    {
                        // We need to retrieve First address and program size
                        StartAddress = int.Parse(line.Substring(7, 6), System.Globalization.NumberStyles.HexNumber);
                        PLength = int.Parse(line.Substring(13, 6), System.Globalization.NumberStyles.HexNumber);
                        ProgramName = line.Substring(1, 6).TrimEnd();
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

            dlgRelocateObjectFile RelocationDialog = new dlgRelocateObjectFile(ProgramName, StartAddress, PLength, ModRecordCount);

            if (RelocationDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            NewAddress = RelocationDialog.RelocatedToAddress;

            // Call the loader!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            // Return address (absolute) of first instruction after relocation
            // This will be placed in the PC

            //this.SICVirtualMachine.PC =    (start value from relocated program code)
        }

        private void ClearDevicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < SIC_CPU.NumDevices; i++)
            {
                SICVirtualMachine.Devices[i].Clear();
            }

            RefreshCPUDisplays();
        }
    }
}