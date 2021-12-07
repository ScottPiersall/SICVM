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
    public partial class dlgSetDeviceContent : Form
    {
        public int MemoryAddress = 0;
        public dlgSetDeviceContent()
        {
            InitializeComponent();
        }

        public dlgSetDeviceContent(int MemorizedAddress)
        {
            InitializeComponent();
            this.textHex.Text = MemorizedAddress.ToString("X6");
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int IntValue;
            String temp = this.textHex.Text.Trim();

            if (temp.Length == 0)
            {
                MessageBox.Show("Please specify a memory address.", "No Address Specified");
                textHex.Focus();
                return;
            }
            IntValue = int.Parse(temp, System.Globalization.NumberStyles.HexNumber);

            if (IntValue > 32767)
            {
                MessageBox.Show("The memory address specified is outside of SIC Memory Range", "Invalid Memory Address");
                textHex.Focus();
                return;

            }
            else { this.MemoryAddress = IntValue; }

            int TempW;
            
            TempW = int.Parse(temp, System.Globalization.NumberStyles.HexNumber);

            this.DialogResult = DialogResult.OK;
        }
    }
    
}
