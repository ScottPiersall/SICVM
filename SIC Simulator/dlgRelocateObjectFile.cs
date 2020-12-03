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




  

    public partial class dlgRelocateObjectFile : Form
    {
        public dlgRelocateObjectFile( String ProgramName, int AssembledAddress, int ProgramLength, int MRecords)
        {
            InitializeComponent();

            this.RelocatedToAddress = AssembledAddress;
            this.ProgramName = ProgramName;
            this.ProgramLengthInBytes = ProgramLength;
            this.lblProgramName.Text = "Program Name: " + this.ProgramName;
            this.lblProgramLength.Text = "Program Length :" + this.ProgramLengthInBytes.ToString() + "(hex) bytes";
            this.txtAssembledStartPoint.Text = AssembledAddress.ToString("X6");
            this.txtRelocationAddress.Text = AssembledAddress.ToString("X6");
            this.lblRelocationRecords.Text = "Relocation Records : " + MRecords.ToString();
        }
    
        public int RelocatedToAddress;
        public String ProgramName = string.Empty;
        public int ProgramLengthInBytes;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Validate Entries
            String temp = this.txtRelocationAddress.Text.Trim();
            int IntValue;

            if (temp.Length == 0)
            {
                MessageBox.Show("Please specify a relocation address.", "No Relocation Address Specified");
                this.txtRelocationAddress.Focus();
                return;
            }

            IntValue = int.Parse(temp, System.Globalization.NumberStyles.HexNumber);


            if (IntValue > 32767)
            {
                MessageBox.Show("The memory address specified is outside of SIC Memory Range", "Invalid Memory Address");
                this.txtRelocationAddress.Focus();
                return;
            }

            // If we get here...we are good

            this.RelocatedToAddress = IntValue;

            this.DialogResult = DialogResult.OK;
        }
    }
}
