using System;
using System.Windows.Forms;

namespace SICVirtualMachine.View
{
    public partial class dlgSetRegisterWord : Form
    {
        public int WordValue = 0;


        public dlgSetRegisterWord(string RegisterName)
        {
            InitializeComponent();

            Text = "Set " + RegisterName + " Word Value";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtWordValue.Text.Trim().Length != 6)
            {
                MessageBox.Show("The word value must be six hexadecimal digits", "Invalid Word Value");
                txtWordValue.Focus();
                return;
            }

            string temp;
            int TempW;
            temp = txtWordValue.Text.Trim();
            TempW = int.Parse(temp, System.Globalization.NumberStyles.HexNumber);

            WordValue = TempW;
            DialogResult = DialogResult.OK;
        }
    }
}
