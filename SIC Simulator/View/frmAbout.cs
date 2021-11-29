using System;
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
            lvAuthors.Columns.Clear();
            lvAuthors.GridLines = true;
            lvAuthors.View = View.Details;
            lvAuthors.Columns.Add("Author(s)", 350, HorizontalAlignment.Left);
            lvAuthors.Columns.Add("Contribution(s)", 1600, HorizontalAlignment.Left);

            lvAuthors.Items.Clear();


            string[] ScottPLvItems = { "Scott Piersall", "Chief Architect & Lead Developer VM" };
            lvAuthors.Items.Add(new ListViewItem(ScottPLvItems));


            string[] RileySLvItems = { "Riley Strickland", "Pass 1 & 2 of SIC Assembler" };
            lvAuthors.Items.Add(new ListViewItem(RileySLvItems));

            string[] EllisLLvItems = { "Ellis Levine", "Pass 1 & 2 of SIC Assembler" };
            lvAuthors.Items.Add(new ListViewItem(EllisLLvItems));


            string[] KrisWLvItems = { "Kris Wieben", "GUI & VM Testing" };
            lvAuthors.Items.Add(new ListViewItem(KrisWLvItems));

            string[] BrandonWLvItems = { "Brandon Woodrum", "Absolute Loader" };
            lvAuthors.Items.Add(new ListViewItem(BrandonWLvItems));


            lblVersion.Text = "Version: " + Application.ProductVersion.ToString();
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
