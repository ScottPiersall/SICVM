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
    public partial class dlgWriteStringToDevice: Form
    {
        
        public string result = string.Empty;

        public dlgWriteStringToDevice()
        {
            InitializeComponent();
        }

        public dlgWriteStringToDevice(int MemorizedAddress)
        {
            InitializeComponent();
            this.textWord.Focus();
        }

        private void dlgWriteStringToDevice_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            String temp = this.textWord.Text.Trim();
            int TempW;
            temp = this.textWord.Text.Trim();
            TempW = int.Parse(temp);

            this.result = this.textWord.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
