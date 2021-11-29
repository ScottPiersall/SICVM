using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;
using SIC_Simulator.Extensions;
using static System.Windows.Forms.ListViewItem;
using System.Diagnostics;

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
        /// </summary>
        private async Task MemoryRefreshAsync()
        {
            if (rbMemHex.Checked == true)
            {
                String Blob = ByteArrayToHexStringViaBitConverter(this.SICVirtualMachine.MemoryBytes);

                StringBuilder sb = new StringBuilder((32768 * 2) + 512);
                int StartIndex = 0;
                int Line = 0;

                int PCLine = 0;

                await Task.Run(() =>
                {
                    sb.AppendLine("{\\rtf1\\ansi ");
                    sb.AppendLine("{\\colortbl ;\\red0\\green255\\blue0;\\red255\\green255\\blue0;}");

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
                                sb.Append("\\line " + string.Format("{0:X4}: ", Add));
                                Line += 1;
                            }
                            else
                            {
                                sb.Append(string.Format("{0:X4}: ", Add));
                            }
                        }
                        if ((Add == this.SICVirtualMachine.PC) || (Add == this.SICVirtualMachine.PC + 1) || (Add == this.SICVirtualMachine.PC + 2))
                        {
                            sb.Append(String.Format("\\fs24 \\b \\highlight2 {0:X2}\\highlight0\\b0 \\fs20 ", Blob.Substring(Add * 2, 2)) + " ");
                            PCLine = Line;
                        }
                        else
                        {
                            sb.Append(String.Format("{0:X2}", Blob.Substring(Add * 2, 2)) + " ");
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
            else
            {
                // Show in Binary

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
                
                dlgRelocatePrompt relPrompt = new dlgRelocatePrompt();
                String[] fullText = fileText.Split('\n');



                String[] lines = new string[0];
                String[] mods = new string[0];
                //Split the strings in fullText into an absolute object file and mod records.
                foreach (String line in fullText)
                {
                    if(line[0] == 'H' || line[0] ==  'E' || line[0] == 'T')
                    {
                        Array.Resize(ref lines, lines.Length + 1);
                        lines[lines.Length-1] = line;
                    }
                    else if (line[0] == 'M')
                    {
                        Array.Resize(ref mods, mods.Length + 1);
                        mods[mods.Length-1] = line;
                    }
                }
                
                this.txtObjectCode.Text = string.Join("\n",lines);
                this.txtModRecs.Text = string.Join("\n",mods);
                if (relPrompt.ShowDialog() == DialogResult.Yes)
                {
                    //Call the Relocating Loader
                    dlgRelocateObjectFile relocate = new dlgRelocateObjectFile(lines,mods);
                    
                    if (relocate.ShowDialog() == DialogResult.OK)
                    {
                        int startad = relocate.RelocatedToAddress;
                        Debug.WriteLine("Open Object file: Address is: " + startad.ToString("X"));
                        RelocateLoadObjectFile(startad, lines, mods);
                        //Temporary demo purposes
                        //LoadObjectFile(lines);
                        DebugSuccessDisplay NoteHere = new DebugSuccessDisplay();
                        NoteHere.ShowDialog();
                        //end of demo
                    }
                    else //If they cancel or ignore this dialogue box, they default to absolute loader
                    {
                       LoadObjectFile(lines);
                    }



                }
               
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
                    this.txtModRecs.Text = assembler.ModRecords;
                    
                    String[] lines = assembler.ObjectCode.Split('\n');
                    String[] mods = assembler.ModRecords.Split('\n');
                    dlgRelocatePrompt relPrompt = new dlgRelocatePrompt();
                    if(relPrompt.ShowDialog() == DialogResult.Yes)
                    {
                        //Call the Relocating Loader
                        dlgRelocateObjectFile relocate = new dlgRelocateObjectFile(lines,mods);
                        int startad; 
                        if (relocate.ShowDialog() == DialogResult.OK)
                        {
                            startad = relocate.RelocatedToAddress;
                             RelocateLoadObjectFile(startad,lines,mods);
                            //Temporary demo purposes
                            //LoadObjectFile(lines);
                            
                            //NoteHere.ShowDialog();
                        }
                        else //If they cancel or ignore this dialogue box, they default to absolute loader
                        {
                           LoadObjectFile(lines);
                           
                        }



                    }
                    else
                    {
                        //Call the Absolute Loader
                        DebugSuccessDisplay NoteHere = new DebugSuccessDisplay();
                        LoadObjectFile(lines);
                       
                    }
                    
                }
                this.RefreshCPUDisplays(); // refresh memory after object code is loaded


                this.LastLoadedFileName = System.IO.Path.GetFileName(loadSICSourceFD.FileName);
                
            }
        }
        private int TRecordOverhead(int lineNum, ref String[] lines, int offset, ref int address){
            Debug.WriteLine(lines[lineNum]);
                int pos = Int32.Parse(lines[lineNum].Substring(3,4), System.Globalization.NumberStyles.HexNumber);
                address=pos; //Update the contents of address to reflect where we are in memory
            Debug.WriteLine("We believe we are modifying the position "+ address.ToString("X4"));
                pos +=offset;
            Debug.WriteLine("We believe we are adding {0}(decimal) {1}(hex) to reach {2} ",offset,offset.ToString("X"), pos.ToString("X"));
            String replacement = pos.ToString("X4");//Encode as a Hex String
            lines[lineNum] = lines[lineNum].Substring(0, 3) + replacement + lines[lineNum].Substring(7); //Remove(3,4).Insert(3,replacement);

                Debug.WriteLine(lines[lineNum]);
            return 9;
            //string s = "ABCDEFGH";
            //s = s.Remove(3, 2).Insert(3, "ZX");
            //output += String.Format("{0} {1} {2}\nLine {3}: UNDEFINED SYMBOL {4}", row.Symbol, row.OpCode, row.Operand, row.LineNumber, row.Operand);
        }
        private Boolean adjustString(ref String[] lines,ref int lineNum, ref int linePos, ref int Address, int TargetAddress, int offset){
            //Move through string until you hit target address. If you hit end of string go to next.
            //If you hit the address make modification and return true.
            //If you hit end of lines, return false.
            while(lineNum<lines.Length-1){//While there are still lines.
                if(Address == TargetAddress && linePos <= lines[lineNum].Length - 4)
                {
                    int oldpos = Int32.Parse(lines[lineNum].Substring(linePos,4), System.Globalization.NumberStyles.HexNumber);
                    oldpos += offset;
                    String replacement = oldpos.ToString("X4");//Encode as a Hex String
                    //lines[lineNum] = lines[lineNum].Remove(linePos,4).Insert(linePos,replacement);
                    
                    Debug.WriteLine("First: " + lines[lineNum].Substring(0, linePos) + " Second: " + replacement + " Third: " + lines[lineNum].Substring(linePos + 4));
                    Debug.WriteLine("PreAdjust  : " + lines[lineNum]);
                    lines[lineNum] = lines[lineNum].Substring(0, linePos) + replacement + lines[lineNum].Substring(linePos + 4);
                    Debug.WriteLine("Post Adjust: "+lines[lineNum]);
                    
                    //Move 4 character which is 2 bytes
                    linePos +=4;
                    Address+=2;
                    return true;
                }
                else{
                    //Move 2 characters which is 1 byte
                    linePos+=2;
                    Address+=1;
                    if(linePos>=lines[lineNum].Length){
                        lineNum++;

                        linePos = TRecordOverhead(lineNum, ref lines,offset, ref Address);
                        
                    }
                }
            }
            return false;

        }
        private void RelocateLoadObjectFile(int startAdd, String[] unmodified, String[] Mrec)
        {
            // collection of the entire obj code and M-recs
            // 2 different ararys of strings
            // 1 will be lines 1 will be mods
            // adjust the title record to new starting address
            // for each mod record, find the T record and adjust it
            // loop through M-recs, loop through T records
            // the M-record can occur somewhere within a T record
            // keep a location counter for the T record, and every character read increments it
            // reset the location counter at the start of each T record
            // Output:
            // Modified obj code as String array
            // this is a test from Daniel

            int lineNum =0; //each T record 0 is Header and last is E Record
            int linePos =0; // our position in the current T record
            int curMemoryAddress=0; //current memory address corressponding to our current position in the string
            int oldAddress = Int32.Parse(unmodified[lineNum].Substring(9,4), System.Globalization.NumberStyles.HexNumber);  //the original starting address for each T record
            int offset = startAdd-oldAddress;
            Debug.WriteLine("We are moving from {0} to {1} which is a jump of {2}(dec) {3}(hex)", oldAddress.ToString("X4"), startAdd.ToString("X4"), offset, offset.ToString("X4"));
            String[] lines = new string[unmodified.Length]; //copy of Records
            //unmodified.CopyTo(lines, 0); //Copy the unmodified records into the ones we modify
            unmodified.CopyTo((String[])lines, 0);
            
            //Modify H-Record
            //lines[lineNum] = lines[lineNum].Remove(9,4).Insert(9,startAdd.ToString("X"));
            lines[lineNum] = lines[lineNum].Substring(0, 9) + startAdd.ToString("X4") + lines[lineNum].Substring(13);
            //Modify E-Record
            int firstInstruction = Int32.Parse(lines[unmodified.Length - 1].Substring(3, 4), System.Globalization.NumberStyles.HexNumber);
            firstInstruction +=offset;
            Debug.WriteLine("New first executable instruction: " + firstInstruction.ToString("X4"));
            lines[lines.Length - 1] = "E00"+firstInstruction.ToString("X4")+"\n";
            Debug.WriteLine("New E record : " + lines[unmodified.Length - 1]);
            //Modify T-Records
            lineNum++; 
            linePos = TRecordOverhead(lineNum, ref lines,offset, ref curMemoryAddress);

            

            foreach(string rec in Mrec)
            {
                if(String.IsNullOrWhiteSpace(rec))
                {
                    continue;
                }
                //T001000
                //M00100104+COPY
                //getting the address that needs to be modified from the Mod records
                String addressSubstring = rec.Substring(3, 4);
                //converting to int and subtracting 1 to match the starting address in the corresponding T record
                int intAddress = Int32.Parse(addressSubstring, System.Globalization.NumberStyles.HexNumber);
                
                //String address = intAddress.ToString("X");
                //Debug.WriteLine(address);
                adjustString(ref lines, ref lineNum,ref linePos,ref curMemoryAddress,intAddress,offset);
                
            }
            //Make sure our lines looks like what we think it does
            this.txtModdedObjectCode.Text = String.Join("\n", lines);
            LoadObjectFile(lines);
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
        private void relocateCurrentProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Reload current object code into memory
            String[] lines = this.txtObjectCode.Text.Split('\n');
            String[] mods = this.txtModRecs.Text.Split('\n'); //ADJUST TO use function above


            //Warn user that Relocating Object code that has already been placed in memory will copy to a new location
            //It will not remove the existing copy of the code unless the new location overlaps.
            dlgRelocationWarning warn = new dlgRelocationWarning();

            if(warn.ShowDialog() == DialogResult.OK)
            {
                //Open relocation prompt on current object code
                dlgRelocateObjectFile rlcPrompt = new dlgRelocateObjectFile(lines,mods);
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

    }
}
