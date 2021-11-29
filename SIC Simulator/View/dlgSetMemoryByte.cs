using System;
using System.Windows.Forms;

namespace SIC_Simulator
{
    public partial class dlgSetMemoryByte : Form
    {

        public int MemoryAddress = 0;
        public byte ByteValue = 0;

        public dlgSetMemoryByte()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void txtAddressInHex_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;

            if (c != '\b' && !((c <= 0x66 && c >= 61) || (c <= 0x46 && c >= 0x41) || (c >= 0x30 && c <= 0x39)))
            {
                e.Handled = true;
            }
        }

        private void txtByteValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;

            if (c != '\b' && !((c <= 0x66 && c >= 61) || (c <= 0x46 && c >= 0x41) || (c >= 0x30 && c <= 0x39)))
            {
                e.Handled = true;
            }
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

            if (txtByteValue.Text.Length != 2)
            {
                MessageBox.Show("The byte value must be two hexadecimal digits", "Invalid Byte Value");
                txtByteValue.Focus();
                return;
            }


            if (IntValue > 32767)
            {
                MessageBox.Show("The memory address specified is outside of SIC Memory Range", "Invalid Memory Address");
                txtAddressInHex.Focus();
                return;

            }
            else { MemoryAddress = IntValue; }

            byte TempB;
            temp = txtByteValue.Text.Trim();
            TempB = byte.Parse(temp, System.Globalization.NumberStyles.HexNumber);

            ByteValue = TempB;
            DialogResult = DialogResult.OK;

        }
    }
}
