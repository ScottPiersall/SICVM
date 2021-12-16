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
            this.lvAuthors.Items.Clear();

            ListViewItem ScottPLvItem;
            ScottPLvItem = new ListViewItem("Scott Piersall", "Chief Architect & Lead Developer VM");
            this.lvAuthors.Items.Add(ScottPLvItem);

            ListViewItem RileySLvItem;
            RileySLvItem = new ListViewItem("Riley Strickland", "Pass 1 & 2 of SIC Assembler");
            this.lvAuthors.Items.Add(RileySLvItem);

            ListViewItem EllisLLvItem;
            EllisLLvItem = new ListViewItem("Ellis Levine", "Pass 1 & 2 of SIC Assembler");
            this.lvAuthors.Items.Add(EllisLLvItem);


            ListViewItem KrisWLvItem;
            KrisWLvItem = new ListViewItem("Kris Wieben", "GUI & VM Testing");
            this.lvAuthors.Items.Add(KrisWLvItem);

            ListViewItem BrandonWLvItem;
            BrandonWLvItem = new ListViewItem("Brandon Woodrum", "Absolute & Relocating Loader");
            this.lvAuthors.Items.Add(BrandonWLvItem);

            ListViewItem WDanielHiromoto;
            WDanielHiromoto = new ListViewItem("W. Daniel Hiromoto", "Relocating Loader");
            this.lvAuthors.Items.Add(WDanielHiromoto);

            ListViewItem BenDeBruin;
            BenDeBruin = new ListViewItem("Ben DeBruin", "Relocating Loader");
            this.lvAuthors.Items.Add(BenDeBruin);

            ListViewItem AhmadOsmani;
            AhmadOsmani = new ListViewItem("Ahmad Osmani", "Relocating Loader");
            this.lvAuthors.Items.Add(AhmadOsmani);

            this.lblVersion.Text = "Version: " + Application.ProductVersion.ToString();
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
