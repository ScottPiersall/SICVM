using System;
using System.Windows.Forms;

namespace SICVirtualMachine.View
{
    public partial class dlgSetMemoryWord : Form
    {
        public int MemoryAddress = 0;
        public int WordValue = 0;
        public dlgSetMemoryWord()
        {
            InitializeComponent();
        }

        public dlgSetMemoryWord(int MemorizedAddress)
        {
            InitializeComponent();
            txtAddressInHex.Text = MemorizedAddress.ToString("X6");
            txtWordValue.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int IntValue;
            string temp = txtAddressInHex.Text.Trim();

            if (temp.Length == 0)
            {
                MessageBox.Show("Please specify a memory address.", "No Address Specified");
                txtAddressInHex.Focus();
                return;
            }


            IntValue = int.Parse(temp, System.Globalization.NumberStyles.HexNumber);

            if (txtWordValue.Text.Length != 6)
            {
                MessageBox.Show("The word value must be six hexadecimal digits", "Invalid Word Value");
                txtWordValue.Focus();
                return;
            }


            if (IntValue > 32767)
            {
                MessageBox.Show("The memory address specified is outside of SIC Memory Range", "Invalid Memory Address");
                txtAddressInHex.Focus();
                return;

            }
            else { MemoryAddress = IntValue; }

            int TempW;
            temp = txtWordValue.Text.Trim();
            TempW = int.Parse(temp, System.Globalization.NumberStyles.HexNumber);

            WordValue = TempW;
            DialogResult = DialogResult.OK;
        }
    }
}
