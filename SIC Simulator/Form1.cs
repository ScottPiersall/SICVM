using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;
using SIC_Simulator.Extensions;
using static System.Windows.Forms.ListViewItem;
using System.Diagnostics;
using System.Drawing;
using System.Media;


namespace SIC_Simulator
{

    public partial class Form1 : Form
    {

        public String LastLoadedFileName = String.Empty;
        public int LastLoadedStart = 0;
        public int LastLoadedLength = 0;

        private SIC_CPU SICVirtualMachine;
        private int addressRightClicked;
        private const char ASCII_8 = '8';


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

            this.txtSICInput.MouseDown += TxtSICInput_MouseClick;

            var changeWord = new ToolStripMenuItem("Set Memory Word");
            changeWord.Click += ChangeWord_Click;

            var setPcHere = new ToolStripMenuItem("Set PC Here");
            setPcHere.Click += SetPcHere_ClickAsync;

            var run = new ToolStripMenuItem("Run To");
            run.Click += Run_Click;

            var cms = new ContextMenuStrip();
            cms.ShowImageMargin = false;
            cms.Items.Add(changeWord);
            cms.Items.Add(setPcHere);
            cms.Items.Add(run);

            this.txtSICInput.ContextMenuStrip = cms;
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

            if (this.SICVirtualMachine.PC == -1)
            {
                MessageBox.Show("Program Stepping Halted. L=0, RSUB, PC = -1", "Program Halted");
                return;
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
        private void btnSnd_Click(object sender, EventArgs e)
        {
            int num = new Random().Next(5000);
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
            await UpdateSicSymbolTableHighlightAsync();

        }

        private async Task DeviceRefreshAsync()
        {
            await Task.Run(() => { });
            for (int i = 0; i < this.lvDevices.Items.Count; i++)
            { // update the sub list views with data found in the device array 
                try
                {
                    // this.lvDevices.Items[i].SubItems[1].Text = this.SICVirtualMachine.Devices[i].GetWriteBufferASCIIByteString;   
                    this.lvDevices.Items[i].SubItems[1].Text = this.SICVirtualMachine.Devices[i].GetASCIIStringWrites();
                    this.lvDevices.Items[i].SubItems[2].Text = this.SICVirtualMachine.Devices[i].GetHEXStringWrites();

                }
                catch (NullReferenceException)
                {
                    this.lvDevices.Items[i].SubItems[1].Text = "";
                }


            }
        }


        /// <summary>
        /// Refreshes Memory Display on background thread. Calls are marshalled to UI thread
        /// </summary>
        private async Task MemoryRefreshAsync()
        {
            if (rbMemHex.Checked == true)
            { // show in hex
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
            else if (rbMemDecimal.Checked == true)
            { // Show in Decimal

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
                                StartIndex += 6;
                        }
                        if (Add % 16 == 0)
                        { // prints counters on very left of table
                            if (Add > 0)
                            {
                                sb.Append("\\line \\fs20 " + string.Format("{0:D4}: ", Add));
                                Line += 1;
                            }
                            else // prints 0th counter
                                sb.Append(string.Format("\\fs20 {0:D4}: ", Add));
                        }
                        if (Add == this.SICVirtualMachine.PC || Add == this.SICVirtualMachine.PC + 1 || Add == this.SICVirtualMachine.PC + 2)
                        { // the highlighted section
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
            else if (rbMemBinary.Checked == true)
            { // Show in Binary
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
            else
            { // Show ASCII table
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
                            if (temp < 32)
                            {
                                sb.Append(String.Format("\\fs24 \\b \\highlight2 {0}\\highlight0\\b0 \\fs24 ", "." + ' ') + " ");
                            }
                            else
                            {
                                sb.Append(String.Format("\\fs24 \\b \\highlight2 {0}\\highlight0\\b0 \\fs24 ", Char.ConvertFromUtf32(temp) + ' ') + " ");
                            }
                            PCLine = Line;
                        }
                        else
                        { // all non highlighted bits. This is where the ASCII values get printed
                            int temp = Int32.Parse(Blob.Substring(Add * 2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                            if (temp < 32)
                            {
                                sb.Append(String.Format("{0}", "." + ' ') + " ");
                            }
                            else
                            {
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

            //remove leading characters in case value is negative
            txtX_Hex.Text = txtX_Hex.Text.Substring(txtX_Hex.Text.Length - 6);
            txtA_Hex.Text = txtA_Hex.Text.Substring(txtA_Hex.Text.Length - 6);
            txtL_Hex.Text = txtL_Hex.Text.Substring(txtL_Hex.Text.Length - 6);
            txtPC_Hex.Text = txtPC_Hex.Text.Substring(txtPC_Hex.Text.Length - 6);
            txtSW_Hex.Text = txtSW_Hex.Text.Substring(txtSW_Hex.Text.Length - 6);

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
            txtSW_CC.Text = txtSW_BIN_LSB.Text.Substring(0, 2);
            lblComp_Result.Text = txtSW_BIN_LSB.Text[0] == 49 ? "Greater than" : txtSW_BIN_LSB.Text[1] == 49 ? "Less than" : "Equal";

            String NextInstructionD;


            if (this.SICVirtualMachine.PC >= 0)
            {
                NextInstructionD = SICVirtualMachine.GetInstructionDescription(SICVirtualMachine.PC);

                String[] NextInstructionPieces;

                NextInstructionPieces = NextInstructionD.Split('|');

                lblCurrentInstruction.Text = lblNextInstruction.Text;
                lblCI_Description.Text = lblNI_Description.Text;
                lblCurrentInstruction_Effect.Text = lblNextInstruction_Effect.Text;

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


        private void loadSavedSICMachineStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            DialogResult Res;
            ofd.Filter = "SIC VM State Files|*.sicstate";
            ofd.Multiselect = false;

            Res = ofd.ShowDialog();

            if (Res == DialogResult.OK)
            {
                using (var stream = File.Open(ofd.FileName, FileMode.Open))
                {
                    SoapFormatter osf = new SoapFormatter();
                    this.SICVirtualMachine = (SIC_CPU)osf.Deserialize(stream);
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

            if (Result == DialogResult.Cancel)
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

            if (this.MemorizedLastMemoryWordAddress == 0)
            {
                SetMemWord = new dlgSetMemoryWord();
            }
            else
            {
                SetMemWord = new dlgSetMemoryWord(this.MemorizedLastMemoryWordAddress);
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

            if (Result == DialogResult.Yes)
            {
                this.SICVirtualMachine = new SIC_CPU(true);
                this.RefreshCPUDisplays();
            }

        }


        private void ReadEndRecord(string line, ref int FirstExecIns)
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



        private void ReadTextRecord(string line, ref int RecordStartAdd, ref int RecordLength)
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
                file.Close();

                dlgRelocatePrompt relPrompt = new dlgRelocatePrompt();
                String[] fullText = fileText.Split('\n');



                String[] lines = new string[0];
                String[] mods = new string[0];
                //Split the strings in fullText into an absolute object file and mod records.
                foreach (String line in fullText)
                {
                    if (line[0] == 'H' || line[0] == 'E' || line[0] == 'T')
                    {
                        Array.Resize(ref lines, lines.Length + 1);
                        lines[lines.Length - 1] = line;
                    }
                    else if (line[0] == 'M')
                    {
                        Array.Resize(ref mods, mods.Length + 1);
                        mods[mods.Length - 1] = line;
                    }
                }
                this.txtObjectCode.Text = string.Join("\n", lines);
                this.txtModRecs.Text = string.Join("\n", mods);


                if (relPrompt.ShowDialog() == DialogResult.Yes)
                {
                    //Call the Relocating Loader
                    dlgRelocateObjectFile relocate = new dlgRelocateObjectFile(lines, mods);

                    if (relocate.ShowDialog() == DialogResult.OK)
                    {
                        int startad = relocate.RelocatedToAddress;
                        //Debug.WriteLine("Open Object file: Address is: " + startad.ToString("X"));
                        RelocateLoadObjectFile(startad, lines, mods);

                    }




                }
                else
                {
                    LoadObjectFile(lines);
                }



            }




            this.RefreshCPUDisplays();

        }

        /*
         * Loads in SIC assembly code from a file
         * Passes the code through an assembler
         * Prompts the User for loading preference (absolute or relocating)
         */
        private void tsmloadAndAssembleSICSourceFIle_Click(object sender, EventArgs e)
        {
            if (loadSICSourceFD.ShowDialog() == DialogResult.OK)
            {
                Assembler assembler = new Assembler(loadSICSourceFD.FileName);
                this.SICVirtualMachine.getSICSource(assembler);

                if (!String.IsNullOrEmpty(assembler.ObjectCode))
                {
                    // We need to call the loader, or use the quick loader in this form
                    // to load the assembled code into memory

                    this.txtSICInput.Text = assembler.InstructionSource;
                    this.txtObjectCode.Text = assembler.ObjectCode;
                    this.txtModRecs.Text = assembler.ModRecords;

                    //Contains only the H, T, E records
                    String[] lines = assembler.ObjectCode.Split('\n');
                    //Contains only the modification records
                    String[] mods = assembler.ModRecords.Split('\n');
                    dlgRelocatePrompt relPrompt = new dlgRelocatePrompt();
                    if (relPrompt.ShowDialog() == DialogResult.Yes)
                    {
                        //Call the Relocating Loader
                        dlgRelocateObjectFile relocate = new dlgRelocateObjectFile(lines, mods);
                        int startad;
                        if (relocate.ShowDialog() == DialogResult.OK)
                        {
                            startad = relocate.RelocatedToAddress;
                            RelocateLoadObjectFile(startad, lines, mods);
                        }
                        else //If they cancel or ignore this dialogue box, they default to absolute loader
                        {
                            LoadObjectFile(lines);
                        }
                    }
                    else
                    {
                        //Call the Absolute Loader
                        LoadObjectFile(lines);
                    }

                }
                this.RefreshCPUDisplays(); // refresh memory after object code is loaded
                this.LastLoadedFileName = System.IO.Path.GetFileName(loadSICSourceFD.FileName);

            }
        }//END tsmloadAndAssembleSICSourceFIle_Click()

        /*
         * Handles the portion of the T-record that doesn't corresspond to memory
         * Works on everything in the T-record up to length
         * Changes the inital address of the Trecord, and adjusts the memory address
         * Declarations:
         *          int lineNum = Which line of the object is being modified
         * ref String[] lines   = String Array containing H,T,E records only
         *          int offset  = Difference between new and old address (can be negative)
         *      ref int address = Position in Memory corressponding to position in T-record
         * Returns:
         * the new position in the Trecord
         */
        private int TRecordOverhead(int lineNum, ref String[] lines, int offset, ref int address)
        {
            //Debug.WriteLine(lines[lineNum]);
            int pos = Int32.Parse(lines[lineNum].Substring(3, 4), System.Globalization.NumberStyles.HexNumber);
            address = pos; //Update the contents of address to reflect where we are in memory
            //Debug.WriteLine("We believe we are modifying the position "+ address.ToString("X4"));
            pos += offset;
            //Debug.WriteLine("We believe we are adding {0}(decimal) {1}(hex) to reach {2} ",offset,offset.ToString("X"), pos.ToString("X"));

            String replacement = pos.ToString("X4"); //Encode as a Hex String
            lines[lineNum] = lines[lineNum].Substring(0, 3) + replacement + lines[lineNum].Substring(7); //Remove(3,4).Insert(3,replacement);
            //Debug.WriteLine(lines[lineNum]);
            return 9;
        }//END TRecordOverhead()

        /*
         * Finds the address stored in the target address and adjusts it by the offset
         * works with the part of the T-record that corresponds to memory
         * Declarations:
         * ref String[] lines         = String Array containing H,T,E records only
         *      ref int lineNum       = Which line of the object is being modified
         *      ref int linePos       = index of the character of the T-record string currently being checked
         *      ref int Address       = Position in Memory corressponding to position in T-record
         *          int TargetAddress = Address to be modified given from M-record
         *          int offset        = Difference between new and old address (can be negative)
         * returns:
         * true if an adjustment was made
         * false if an adjustment cannot be made
         */
        private Boolean adjustString(ref String[] lines, ref int lineNum, ref int linePos, ref int Address, int TargetAddress, int offset)
        {
            //Move through string until you hit target address. If you hit end of string go to next.
            //If you hit the address make modification and return true.
            //If you hit end of lines, return false.

            //loop through string until target address is hit
            while (lineNum < lines.Length - 1)
            {//While there are still lines.
                if (Address == TargetAddress && linePos <= lines[lineNum].Length - 4)
                {
                    //old address to be updated
                    int oldpos = Int32.Parse(lines[lineNum].Substring(linePos, 4), System.Globalization.NumberStyles.HexNumber);
                    //calculate the new address
                    oldpos += offset;
                    String replacement = oldpos.ToString("X4"); //Encode as a Hex String

                    //Debug.WriteLine("First: " + lines[lineNum].Substring(0, linePos) + " Second: " + replacement + " Third: " + lines[lineNum].Substring(linePos + 4));
                    //Debug.WriteLine("PreAdjust  : " + lines[lineNum]);

                    //update the T-record
                    lines[lineNum] = lines[lineNum].Substring(0, linePos) + replacement + lines[lineNum].Substring(linePos + 4);
                    //Debug.WriteLine("Post Adjust: "+lines[lineNum]);

                    //Move 4 characters which is 2 bytes
                    linePos += 4;
                    Address += 2;
                    return true;
                }
                else
                {
                    //Move 2 characters which is 1 byte
                    linePos += 2;
                    Address += 1;

                    if (linePos >= lines[lineNum].Length)
                    {
                        lineNum++;
                        linePos = TRecordOverhead(lineNum, ref lines, offset, ref Address);

                    }
                }
            }
            return false;
        }//END adjustString()

        /*
         * Before calling this method, proccess object code elsewhere
         * This methodfies modifies the object code given in "unmodified" using M-records contained in "Mrec"
         * Make sure newline characters are not passed in with the string arrays
         * This only works for the SIC, not SICXE
         * Declarations:
         *      int newAddress = the new address we want to move our program to
         * String[] unmodified = Original object code given, it has been modified previously to exclude M-records (includes H, T, E records)
         * String[]       Mrec = string array containing only the M-records
         */
        public void RelocateLoadObjectFile(int newAddress, String[] unmodified, String[] Mrec)
        {
            int lineNum = 0;         //each T record 0 is Header and last is E Record
            int linePos = 0;         // our position in the current T record
            int curMemoryAddress = 0; //current memory address corressponding to our current position in the string

            //the original starting address of the object code
            int oldAddress = Int32.Parse(unmodified[lineNum].Substring(9, 4), System.Globalization.NumberStyles.HexNumber);
            int offset = newAddress - oldAddress;

            //Debug.WriteLine("We are moving from {0} to {1} which is a jump of {2}(dec) {3}(hex)", oldAddress.ToString("X4"), newAddress.ToString("X4"), offset, offset.ToString("X4"));
            String[] lines = new string[unmodified.Length]; //copy of Records

            unmodified.CopyTo((String[])lines, 0);          //Copy the unmodified records into the ones we modify

            //Modify H-Record
            lines[lineNum] = lines[lineNum].Substring(0, 9) + newAddress.ToString("X4") + lines[lineNum].Substring(13);

            //Modify E-Record
            //Grabs End record, specifically address of first executable instruction and then adjusts that instruction
            int firstInstruction = Int32.Parse(lines[unmodified.Length - 1].Substring(3, 4), System.Globalization.NumberStyles.HexNumber);
            firstInstruction += offset;
            //Debug.WriteLine("New first executable instruction: " + firstInstruction.ToString("X4"));
            lines[lines.Length - 1] = "E00" + firstInstruction.ToString("X4") + "\n"; //update the new E-record
            //Debug.WriteLine("New E record : " + lines[unmodified.Length - 1]);

            //Modify T-Records
            lineNum++;
            // isolate part of the T-records that correspond to memory
            linePos = TRecordOverhead(lineNum, ref lines, offset, ref curMemoryAddress);


            //loop through the M-records
            foreach (string rec in Mrec)
            {
                if (String.IsNullOrWhiteSpace(rec))
                {
                    continue;
                }
                //getting the address that needs to be modified from the Mod records
                String addressSubstring = rec.Substring(3, 4);
                //converting to int to match the starting address in the corresponding T record
                int targetAddress = Int32.Parse(addressSubstring, System.Globalization.NumberStyles.HexNumber);

                //Debug.WriteLine(address);
                //Chage address if any
                adjustString(ref lines, ref lineNum, ref linePos, ref curMemoryAddress, targetAddress, offset);

            }
            //Make sure our lines looks like what we think it does
            this.txtModdedObjectCode.Text = String.Join("\n", lines);
            //Call the absolute loader on the relocated object code
            LoadObjectFile(lines);
        }//END RelocateLoadObjectFile()

        private void LoadObjectFile(String[] lines)
        {
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
                lvItem.SubItems.Add("");
                this.lvDevices.Items.Add(lvItem);
            }
            lvDevices.View = View.Details;
            RefreshCPUDisplays();
        }

        private void tsmFile_Ext_Click(object sender, EventArgs e)
        {

            if (this.SICVirtualMachine.MachineStateIsNotSaved == false)
            {
                DialogResult NotProceed;
                NotProceed = MessageBox.Show("The current machine state has not been saved. Do you want to cancel exit and save your machine state?", "Machine State Not Saved", MessageBoxButtons.YesNo);

                if (NotProceed == DialogResult.Yes)
                {
                    return;
                }

            }
            Application.Exit();

        }

        private void setProgramCounterToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlgSetRegisterWord SetRegWord = new dlgSetRegisterWord("PC");
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
            while (this.SICVirtualMachine.PC != -1)
            {
                this.SICVirtualMachine.PerformStep();
                this.RefreshCPUDisplays();
                Application.DoEvents();
                System.Threading.Thread.Sleep(250);
            }
            this.RefreshCPUDisplays();
        }

        private void btnResetProgram_Click(object sender, EventArgs e)
        {
            LoadObjectFile(this.txtObjectCode.Text.Split('\n'));
            this.RefreshCPUDisplays();
        }

        private void btnThreeStep_Click(object sender, EventArgs e)
        {
            int i = 0;

            while (this.SICVirtualMachine.PC >= 0 && this.SICVirtualMachine.PC <= 32767 && i < 3)
            {
                this.SICVirtualMachine.PerformStep();
                this.RefreshCPUDisplays();
                i++;
            }
            //this.SICVirtualMachine.PerformStep();
            //this.RefreshCPUDisplays();
            //this.SICVirtualMachine.PerformStep();
            //this.RefreshCPUDisplays();
            //this.SICVirtualMachine.PerformStep();
            //this.RefreshCPUDisplays();
        }
        private void relocateCurrentProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Reload current object code into memory
            String[] lines = this.txtObjectCode.Text.Split('\n');
            String[] mods = this.txtModRecs.Text.Split('\n'); //ADJUST TO use function above


            //Warn user that Relocating Object code that has already been placed in memory will copy to a new location
            //It will not remove the existing copy of the code unless the new location overlaps.
            dlgRelocationWarning warn = new dlgRelocationWarning();

            if (warn.ShowDialog() == DialogResult.OK)
            {
                //Open relocation prompt on current object code
                dlgRelocateObjectFile rlcPrompt = new dlgRelocateObjectFile(lines, mods);
                DialogResult decision = rlcPrompt.ShowDialog();


                if (decision == DialogResult.OK)
                {
                    int startingValue = rlcPrompt.RelocatedToAddress;

                    RelocateLoadObjectFile(startingValue, lines, mods);
                    //DebugSuccessDisplay NoteHere = new DebugSuccessDisplay();
                    //NoteHere.ShowDialog();
                }
                this.RefreshCPUDisplays();
            }


        }

        private void resetSICDevicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult Result;

            Result = MessageBox.Show("This will clear all SIC Devices. Are you sure you want to proceed?", "Confirm", MessageBoxButtons.YesNo);

            if (Result == DialogResult.Yes)
            {
                for (int i = 0; i < SIC_CPU.NumDevices; i++)
                {
                    this.SICVirtualMachine.Devices[i].reset();
                }
                this.RefreshCPUDisplays();
            }
        }



        private void lvDevices_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) { return; }
            ListViewHitTestInfo ht = lvDevices.HitTest(e.X, e.Y);
            ContextMenu ct = new ContextMenu();
            int deviceNum = ht.Item.Index;
            if (ht.Location == ListViewHitTestLocations.Label)
            {
                MenuItem title = new MenuItem("Device " + deviceNum);
                title.Enabled = false;
                ct.MenuItems.Add(title);

                MenuItem resetDeviceMenuOption = new MenuItem("Reset Device");
                resetDeviceMenuOption.Click += resetDevice;
                ct.MenuItems.Add(resetDeviceMenuOption);

                MenuItem addStringMenuOption = new MenuItem("Write String to Device");
                addStringMenuOption.Click += addString;
                ct.MenuItems.Add(addStringMenuOption);

                MenuItem setHexMenuOption = new MenuItem("Set Device Contents");
                setHexMenuOption.Click += setHex;
                ct.MenuItems.Add(setHexMenuOption);

                ct.Show(lvDevices, new Point(e.X, e.Y));
            }
            //local methods for menu items
            void resetDevice(object s, EventArgs ev)
            {
                DialogResult Result;

                Result = MessageBox.Show("This will clear SIC Device " + deviceNum +
                    "\nAre you sure you want to proceed?", "Confirm", MessageBoxButtons.YesNo);

                if (Result == DialogResult.Yes)
                {
                    this.SICVirtualMachine.Devices[deviceNum].reset();
                    this.RefreshCPUDisplays();
                }
            }
            void addString(object s, EventArgs ev)
            {
                dlgWriteStringToDevice strToDevice = new dlgWriteStringToDevice();

                DialogResult result = strToDevice.ShowDialog();

                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.SICVirtualMachine.Devices[deviceNum].WriteString(strToDevice.result);

                this.RefreshCPUDisplays();
            }


            void setHex(object s, EventArgs ev)
            {
                dlgSetDeviceContent devContent = new dlgSetDeviceContent(this.SICVirtualMachine.Devices[deviceNum].GetHEXStringWrites());
                DialogResult Result = devContent.ShowDialog();

                if (Result == DialogResult.Cancel)
                {
                    return;
                }

                this.SICVirtualMachine.Devices[deviceNum].WriteBuffer = devContent.result;
                this.SICVirtualMachine.Devices[deviceNum].status = devContent.result.Count > 0 ? 2 : 1;

                this.RefreshCPUDisplays();
            }
        }

        /// <summary>
        /// This method clears all previous highlights in the symbol table and highlights
        /// the instruction referenced by the pc computer in yellow, and the address referenced
        /// by the instruction in green. Furthermore, it updates the values of word variables
        /// </summary>
        /// <returns></returns>
        private async Task UpdateSicSymbolTableHighlightAsync()
        {
            await Task.Run(() =>
            {
                string[] lines = null;
                this.txtSICInput.Invoke(new MethodInvoker(delegate ()
                {
                    txtSICInput.SelectionStart = 0;
                    txtSICInput.SelectAll();
                    txtSICInput.SelectionBackColor = Color.White;
                    lines = this.txtSICInput.Text.Split('\n');
                }));

                int length = lines.Length;

                if (length < 2)
                {
                    return;
                }
                //String.Format("{0}\t{1}\t{2}\t{3}\t{4}\n",
                string textBoxText = lines[0] + "\n" + lines[1] + "\n";

                int highlightOffset = lines[0].Length + lines[1].Length + 2;
                int addr = int.Parse(this.txtPC_Hex.Text, System.Globalization.NumberStyles.HexNumber);
                string hexValue = addr.ToString("X"); //this.txtPC_Hex.Text.TrimStart(new Char[] { '0' });
                string hexAddr = this.SICVirtualMachine.FetchWord(addr).ToString("X6").Substring(2);

                if (hexAddr[0] >= ASCII_8)
                {
                    int firstHexCharacter = (int)hexAddr[0];
                    firstHexCharacter = firstHexCharacter >= 65 ? firstHexCharacter - 15 : firstHexCharacter - 8;
                    hexAddr = $"{(char)firstHexCharacter}{hexAddr.Substring(1)}";
                }

                int highlighStart1 = -1;
                int highlightLength1 = -1;
                int highlighStart2 = -1;
                int highlightLength2 = -1;

                for (int i = 2; i < length; i++)
                {
                    var line = lines[i];
                    var values = line.Split('\t');
                    if (values.Length > 3)
                    {
                        if (values[3].Equals("WORD"))
                        {
                            var currentValue = this.SICVirtualMachine.FetchWord(int.Parse(values[1], System.Globalization.NumberStyles.HexNumber));
                            string currentValueStr;
                            if (this.rbMemHex.Checked)
                            {
                                currentValueStr = currentValue.ToString("X");
                            }else if(this.rbMemBinary.Checked)
                            {
                                currentValueStr =  Convert.ToString(currentValue, 2);
                            }
                            else
                            {
                                currentValueStr = currentValue.ToString();
                            }


                            line = String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\n", values[0], values[1], values[2], values[3], values[4], currentValueStr);
                        }
                        else
                        {
                            line += "\n";
                        }

                        if (values[1].Equals(hexValue))
                        {
                            highlighStart1 = highlightOffset;
                            highlightLength1 = line.Length;
                        }
                        else if (values[1].Equals(hexAddr))
                        {
                            highlighStart2 = highlightOffset;
                            highlightLength2 = line.Length;
                        }

                        textBoxText += line;
                    }

                    highlightOffset += line.Length;
                }

                this.txtSICInput.Invoke(new MethodInvoker(delegate ()
                {
                    this.txtSICInput.Text = textBoxText;
                    if (highlighStart1 != -1)
                    {
                        this.txtSICInput.SelectionStart = highlighStart1;
                        this.txtSICInput.SelectionLength = highlightLength1;
                        this.txtSICInput.SelectionBackColor = Color.Yellow;
                    }

                    if (highlighStart2 != -1)
                    {
                        this.txtSICInput.SelectionStart = highlighStart2;
                        this.txtSICInput.SelectionLength = highlightLength2;
                        this.txtSICInput.SelectionBackColor = Color.LightBlue;
                    }
                }));
            });
        }

        private async void Run_Click(object sender, EventArgs e)
        {
            await RunAndStopAtAddress(addressRightClicked);
        }

        private async Task RunAndStopAtAddress(int addressRightClicked)
        {
            while(SICVirtualMachine.PC != -1 && SICVirtualMachine.PC != addressRightClicked)
            {
                SICVirtualMachine.PerformStep();
                RefreshCPUDisplays();
            }
        }

        private async void SetPcHere_ClickAsync(object sender, EventArgs e)
        {
            this.SICVirtualMachine.PC = addressRightClicked;
            RefreshCPUDisplays();
        }

        private void TxtSICInput_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var point = new Point(e.X, e.Y);
                var lineRightClicked = this.txtSICInput.GetLineFromCharIndex(this.txtSICInput.GetCharIndexFromPosition(point));
                if (lineRightClicked == 0)
                {
                    return;
                }

                var lines = this.txtSICInput.Text.Split('\n');
                var line = lines[lineRightClicked].Split('\t');
                addressRightClicked = int.Parse(line[1], System.Globalization.NumberStyles.HexNumber);
            }
        }

        private void ChangeWord_Click(object sender, EventArgs e)
        {
            this.MemorizedLastMemoryWordAddress = addressRightClicked;
            setMemoryWORDToolStripMenuItem_Click(this, null);

        }
    }
}
