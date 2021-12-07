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
        public List<Byte> result = new List<byte>();
        public dlgSetDeviceContent(string currentHex)
        {
            InitializeComponent();
            this.textHex.Text = currentHex;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void textHex_Validating(object sender, CancelEventArgs e)
        {
            string input = this.textHex.Text;
            int length = input.Length;
            char[] hexCharacters = new char[] {'0','1','2','3','4','5','6','7','8','9','A','B','C','D','E','F','a','b','c','d','e','f'};

            string oneHex = string.Empty;

            result.Clear();

            //loop to validate input
            for (int i=0;i<length;i++)
            {
                if (input[i] == ' ')
                {
                    //ignore any spaces
                    continue;
                }
                else if (!hexCharacters.Contains(input[i]))
                {
                    MessageBox.Show("Invalid Character is not a Hex Character: '" + input[i] + "'");
                    e.Cancel = true;
                    return;
                }
                
                if (oneHex.Length < 2)
                {
                    oneHex += input[i];
                }

                if (oneHex.Length == 2)
                {
                    result.Add(byte.Parse(oneHex, System.Globalization.NumberStyles.HexNumber));
                    oneHex = string.Empty;
                }
            }

            if (oneHex.Length > 0)
            {
                MessageBox.Show("Invalid Number of Hex Characters!\nMake sure to have two hex characters for every byte.");
                e.Cancel = true;
                return;
            }
        }
    }
}
