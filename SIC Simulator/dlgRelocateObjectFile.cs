using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace SIC_Simulator
{




  

    public partial class dlgRelocateObjectFile : Form
    {
        public dlgRelocateObjectFile(String[] lines,String[] mods)//Object code and mod records are seperated
        {
            InitializeComponent();
            int NewAddress = 0;
            int StartAddress = 0;
            int PLength = 0;
            int ModRecordCount = mods.Length;
            String ProgramName = string.Empty;

            try
            {

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
                        PLength = int.Parse(line.Substring(13, 6), System.Globalization.NumberStyles.HexNumber);
                        ProgramName = line.Substring(1, 6).TrimEnd();
                        //this.SICVirtualMachine.CurrentProgramEndAddress = Int32.Parse(firstAddress, System.Globalization.NumberStyles.HexNumber) + Int32.Parse(programSize, System.Globalization.NumberStyles.HexNumber);
                        break;
                    }
                    
                }
                

            }
            catch (Exception Ex)
            {
                MessageBox.Show("There was an error reading the object file you specified: " + Ex.ToString(), "Error Opening Object File");
                return;

            }

            //this.RelocatedToAddress = StartAddress;
            this.ProgramName = ProgramName;
            this.ProgramLengthInBytes = PLength;
            this.lblProgramName.Text = "Program Name: " + this.ProgramName;
            this.lblProgramLength.Text = "Program Length :" + this.ProgramLengthInBytes.ToString() + "(hex) bytes";
            this.txtAssembledStartPoint.Text = StartAddress.ToString("X6");
            this.txtRelocationAddress.Text = StartAddress.ToString("X6");
            this.lblRelocationRecords.Text = "#Modification Records : " + ModRecordCount.ToString();
            this.modrecords = ModRecordCount;
            this.objectcodes = lines.Length;
            //Tell them the highest address they can relocate to.
            MaxAddress = 32767 - PLength; 

            this.lblRelocateNote.Text = "Relocation Note:\nAttempting to relocate to an address larger than: " + MaxAddress.ToString("X6") + "\nwill exceed available memory.";

        }
        private int MaxAddress = 0;
        public int RelocatedToAddress;
        public String ProgramName = string.Empty;
        public int ProgramLengthInBytes;
        private int modrecords;
        private int objectcodes;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Validate Entries
            String temp = this.txtRelocationAddress.Text;
            int IntValue;

            

            if (temp.Length == 0)
            {
                MessageBox.Show("Please specify a relocation address.", "No Relocation Address Specified");
                this.txtRelocationAddress.Focus();
                return;
            }
            try
            {
                IntValue = Int32.Parse(temp, System.Globalization.NumberStyles.HexNumber);
                //Debug.WriteLine("I just read in the new address: It's " + IntValue.ToString("X"));
            }
            catch (FormatException err)
            {
                MessageBox.Show(temp + " is not a valid Hex value");
                return;
            }
            

            if (IntValue > 32767)
            {
                MessageBox.Show("The memory address specified is outside of SIC Memory Range", "Invalid Memory Address");
                this.txtRelocationAddress.Focus();
                return;
            }

            if ( IntValue > this.MaxAddress )
            {
                MessageBox.Show("Invalid relocation. Not enough memory available to relocate this program to " + IntValue.ToString("X6"), "Invalid Memory Address");
                this.txtRelocationAddress.Focus();
                return;

            }
            if(ProgramLengthInBytes ==0)
            {
                MessageBox.Show("The program loaded has no records to modify");        
                return;
            }



            // If we get here...we are good

            this.RelocatedToAddress = IntValue;
           

            this.DialogResult = DialogResult.OK;

            return;
        }

        private void lblProgramName_Click(object sender, EventArgs e)
        {

        }

        private void lblNote_Click(object sender, EventArgs e)
        {

        }

        private void dlgRelocateObjectFile_Load(object sender, EventArgs e)
        {

        }
    }
}
