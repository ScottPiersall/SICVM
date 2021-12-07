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

        public string result = string.Empty;

        public dlgWriteStringToDevice()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            String temp = this.textWord.Text.Trim();
            int TempW;
            temp = this.textWord.Text.Trim();
            TempW = int.Parse(temp);

            this.result = this.textWord.Text;
            this.DialogResult = DialogResult.OK;
            this.result = this.textBox.Text;
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
