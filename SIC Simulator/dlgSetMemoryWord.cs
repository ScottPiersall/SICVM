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
    public partial class dlgSetMemoryWord : Form
    {
        public int MemoryAddress = 0;
        public int WordValue = 0;
        public dlgSetMemoryWord()
        {
            InitializeComponent();
        }

        public dlgSetMemoryWord( int MemorizedAddress)
        {
            InitializeComponent();
            this.txtAddressInHex.Text = MemorizedAddress.ToString("X6");
            this.txtWordValue1.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int IntValue;
            String temp = this.txtAddressInHex.Text.Trim();

            if (temp.Length == 0)
            {
                MessageBox.Show("Please specify a memory address.", "No Address Specified");
                txtAddressInHex.Focus();
                return;
            }


            IntValue = int.Parse(temp, System.Globalization.NumberStyles.HexNumber);

            if (txtWordValue1.Text.Length != 6)
            {
                MessageBox.Show("The word value must be six hexadecimal digits", "Invalid Word Value");
                txtWordValue1.Focus();
                return;
            }


            if (IntValue > 32767)
            {
                MessageBox.Show("The memory address specified is outside of SIC Memory Range", "Invalid Memory Address");
                txtAddressInHex.Focus();
                return;

            }
            else { this.MemoryAddress = IntValue; }

            int TempW;
            temp = this.txtWordValue1.Text.Trim();
            TempW = int.Parse(temp, System.Globalization.NumberStyles.HexNumber);

            this.WordValue = TempW;
            this.DialogResult = DialogResult.OK;
        }

        private void dlgSetMemoryWord_Load(object sender, EventArgs e)
        {

        }

        private void txtWordValue_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
