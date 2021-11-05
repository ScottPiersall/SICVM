using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIC_Simulator
{
    public partial class dlgStopAtMemoryAddress : Form
    {

        public int HaltAtMemoryAddress = 0;

        public dlgStopAtMemoryAddress( String LastFile, int LastStartAddress, int LastLength)
        {
            InitializeComponent();
            this.txtLastLoadedFile.Text = LastFile;

            this.txtLastLoadedStart.Text = LastStartAddress.ToString("X4");
            this.txtLastLoadedLength.Text = LastLength.ToString("X4");
            int CalculatedEnd;
            CalculatedEnd = LastStartAddress + LastLength;
            this.txtCalculatedHaltingPoint.Text = CalculatedEnd.ToString("X4");
            this.txtAddressInHex.Text = CalculatedEnd.ToString("X4");


        }




        private void btnOK_Click(object sender, EventArgs e)
        {
            int IntValue =0;
            String temp = this.txtAddressInHex.Text.Trim();

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
            else { this.HaltAtMemoryAddress = IntValue;
                this.DialogResult = DialogResult.OK;
            }



        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
