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
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();

     
        }


        private void frmAbout_Load(object sender, EventArgs e)
        {
            ListViewItem ScottPLvItem;
            ScottPLvItem = new ListViewItem("Scott Piersall", "Chief Architect & Lead Developer VM");
            this.lvAuthors.Items.Add(ScottPLvItem);

            ListViewItem RileySLvItem;
            RileySLvItem = new ListViewItem("Riley Strickland", "Pass 1 & 2 of SIC Assembler");
            this.lvAuthors.Items.Add(RileySLvItem);

            ListViewItem EllisLLvItem;
            EllisLLvItem = new ListViewItem("Ellis Levine", "Pass 1 & 2 of SIC Assembler");
            this.lvAuthors.Items.Add(EllisLLvItem);

            ListViewItem BrandonWLvItem;
            BrandonWLvItem = new ListViewItem("Brandon Woodrum", "Asbolute & Relocating Loader");
            this.lvAuthors.Items.Add(BrandonWLvItem);


            this.lblVersion.Text = "Version: " + Application.ProductVersion.ToString();
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
