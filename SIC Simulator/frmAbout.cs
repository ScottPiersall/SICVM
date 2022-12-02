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
            this.lvAuthors.Columns.Clear();
            this.lvAuthors.GridLines = true;
            this.lvAuthors.View = View.Details;
            this.lvAuthors.Columns.Add("Author(s)", 350, HorizontalAlignment.Left);
            this.lvAuthors.Columns.Add("Contribution(s)", 1600, HorizontalAlignment.Left);

            this.lvAuthors.Items.Clear();


            String[] ScottPLvItems = { "Scott Piersall", "Chief Architect & Lead Developer VM" };
            this.lvAuthors.Items.Add(new ListViewItem(ScottPLvItems ));


            String[]  RileySLvItems = { "Riley Strickland", "Pass 1 & 2 of SIC Assembler"};
            this.lvAuthors.Items.Add(new ListViewItem(RileySLvItems));

            String[] EllisLLvItems = { "Ellis Levine", "Pass 1 & 2 of SIC Assembler" };
            this.lvAuthors.Items.Add(new ListViewItem(EllisLLvItems));

            ListViewItem WDanielHiromoto;
            WDanielHiromoto = new ListViewItem("W. Daniel Hiromoto", "Relocating Loader");
            this.lvAuthors.Items.Add(WDanielHiromoto);

            ListViewItem BenDeBruin;
            BenDeBruin = new ListViewItem("Ben DeBruin", "Relocating Loader");
            this.lvAuthors.Items.Add(BenDeBruin);

            ListViewItem AhmadOsmani;
            AhmadOsmani = new ListViewItem("Ahmad Osmani", "Relocating Loader");
            this.lvAuthors.Items.Add(AhmadOsmani);

            String[] KrisWLvItems = { "Kris Wieben", "GUI & VM Testing" };
            this.lvAuthors.Items.Add(new ListViewItem(KrisWLvItems));

            String[] BrandonWLvItems = { "Brandon Woodrum", "Absolute Loader" };
            this.lvAuthors.Items.Add(new ListViewItem(BrandonWLvItems));

            String[] FransiscoLvItems = { "Francisco Romero", "Decimal Memory View" } ;
            this.lvAuthors.Items.Add( new ListViewItem(FransiscoLvItems));

            String[] JosselynMLvItems = { "Josselyn Munoz", "Binary Memory View" };
            this.lvAuthors.Items.Add(new ListViewItem(JosselynMLvItems));

            String[] JacobMLvItems = { "Jacob McGee", "ASCII Memory View" };
            this.lvAuthors.Items.Add(new ListViewItem(JacobMLvItems));

            String[] CarlosGLvItems = { "Carlos Garciagomez", "ASCII Memory View" };
            this.lvAuthors.Items.Add(new ListViewItem(CarlosGLvItems));

            String[] BrittanySLvItems = { "Brittany Santos", "Next Instruction Display" };
            this.lvAuthors.Items.Add(new ListViewItem(BrittanySLvItems));
            String[] DylanSLvItems = { "Dylan Strickley", "Bug Fixing & Device Features" };
            this.lvAuthors.Items.Add(new ListViewItem(DylanSLvItems));

            String[] BryceStremmelLvItems = { "Bryce Stremmel", "Bug Fixing & Device Features" };
            this.lvAuthors.Items.Add(new ListViewItem(BryceStremmelLvItems));

            this.lblVersion.Text = "Version: " + Application.ProductVersion.ToString();
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
