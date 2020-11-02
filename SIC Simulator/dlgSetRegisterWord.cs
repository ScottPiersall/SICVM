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
    public partial class dlgSetRegisterWord : Form
    {
        public int WordValue = 0;


        public dlgSetRegisterWord( String RegisterName)
        {
            InitializeComponent();

            this.Text = "Set " + RegisterName + " Word Value";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtWordValue.Text.Trim().Length != 6)
            {
                MessageBox.Show("The word value must be six hexadecimal digits", "Invalid Word Value");
                txtWordValue.Focus();
                return;
            }

            String temp;
            int TempW;
            temp = this.txtWordValue.Text.Trim();
            TempW = int.Parse(temp, System.Globalization.NumberStyles.HexNumber);

            this.WordValue = TempW;
            this.DialogResult = DialogResult.OK;
        }
    }
}
