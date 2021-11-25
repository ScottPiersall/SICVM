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
    public partial class dlgRelocatePrompt : Form
    {
        public dlgRelocatePrompt()
        {
            InitializeComponent();
        }

        private void AbsoluteLoadBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            return;
        }

        private void RelocateLoaderBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            return;
        }
    }
}
