using System;
using System.Windows.Forms;

namespace SICVirtualMachine.View
{
    public partial class dlgStopAtMemoryAddress : Form
    {

        public int HaltAtMemoryAddress = 0;

        public dlgStopAtMemoryAddress(string LastFile, int LastStartAddress, int LastLength)
        {
            InitializeComponent();
            txtLastLoadedFile.Text = LastFile;

            txtLastLoadedStart.Text = LastStartAddress.ToString("X4");
            txtLastLoadedLength.Text = LastLength.ToString("X4");
            int CalculatedEnd;
            CalculatedEnd = LastStartAddress + LastLength;
            txtCalculatedHaltingPoint.Text = CalculatedEnd.ToString("X4");
            txtAddressInHex.Text = CalculatedEnd.ToString("X4");


        }




        private void btnOK_Click(object sender, EventArgs e)
        {
            int IntValue = 0;
            string temp = txtAddressInHex.Text.Trim();

            if (temp.Length == 0)
            {
                MessageBox.Show("Please specify a memory address.", "No Address Specified");
                txtAddressInHex.Focus();
                return;
            }


            IntValue = int.Parse(temp, System.Globalization.NumberStyles.HexNumber);

            if (IntValue > 32767)
            {
                MessageBox.Show("The memory address specified for the halting point is outside of SIC Memory Range", "Invalid Memory Address");
                txtAddressInHex.Focus();
                return;

            }
            else
            {
                HaltAtMemoryAddress = IntValue;
                DialogResult = DialogResult.OK;
            }



        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
