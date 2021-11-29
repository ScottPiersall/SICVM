using System;
using System.Windows.Forms;

namespace SIC_Simulator
{






    public partial class dlgRelocateObjectFile : Form
    {
        public dlgRelocateObjectFile(string ProgramName, int AssembledAddress, int ProgramLength, int MRecords)
        {
            InitializeComponent();

            RelocatedToAddress = AssembledAddress;
            this.ProgramName = ProgramName;
            ProgramLengthInBytes = ProgramLength;
            lblProgramName.Text = "Program Name: " + this.ProgramName;
            lblProgramLength.Text = "Program Length :" + ProgramLengthInBytes.ToString() + "(hex) bytes";
            txtAssembledStartPoint.Text = AssembledAddress.ToString("X6");
            txtRelocationAddress.Text = AssembledAddress.ToString("X6");
            lblRelocationRecords.Text = "Relocation Records : " + MRecords.ToString();


            MaxAddress = 32767 - ProgramLength;

            lblNote.Text = "NOTE: This program cannot be relocated to an address higher than " + MaxAddress.ToString("X6");

        }
        private readonly int MaxAddress = 0;
        public int RelocatedToAddress;
        public string ProgramName = string.Empty;
        public int ProgramLengthInBytes;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Validate Entries
            string temp = txtRelocationAddress.Text.Trim();
            int IntValue;

            if (temp.Length == 0)
            {
                MessageBox.Show("Please specify a relocation address.", "No Relocation Address Specified");
                txtRelocationAddress.Focus();
                return;
            }

            IntValue = int.Parse(temp, System.Globalization.NumberStyles.HexNumber);


            if (IntValue > 32767)
            {
                MessageBox.Show("The memory address specified is outside of SIC Memory Range", "Invalid Memory Address");
                txtRelocationAddress.Focus();
                return;
            }

            if (IntValue > MaxAddress)
            {
                MessageBox.Show("Invalid relocation. Not enough memory available to relocate this program to " + IntValue.ToString("X6"), "Invalid Memory Address");
                txtRelocationAddress.Focus();
                return;

            }



            // If we get here...we are good

            RelocatedToAddress = IntValue;

            DialogResult = DialogResult.OK;
        }
    }
}
