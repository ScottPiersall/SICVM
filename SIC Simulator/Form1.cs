using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;

namespace SIC_Simulator
{
    public partial class Form1 : Form
    {
        private SIC_CPU SICVirtualMachine;
        
        
        public Form1()
        {
            InitializeComponent();

            tsmAbout_About.Click += new EventHandler(tsmAbout_About_DropDownItemClicked);
            tsmzeroAllMemory.Click += new EventHandler(tsmzeroAllMemory_Click);
            this.SICVirtualMachine = new SIC_CPU(true);


            this.RefreshMemoryDisplay();


        }



        private void tsmAbout_About_DropDownItemClicked(object sender, EventArgs e)
        {
            var menuItem = sender as ToolStripMenuItem;
            var menuText = menuItem.Text;


            switch (menuText)
            {
                case "About":
                    frmAbout fa = new frmAbout();
                        fa.ShowDialog();
                    break;

                case "Check for Updates":

                    break;


            }



        }

        private void tsmzeroAllMemory_Click(object sender, EventArgs e)
        {
            this.SICVirtualMachine.ZeroizeMemory();

        }

        static string ByteArrayToHexStringViaBitConverter(byte[] bytes)
        {
            string hex = BitConverter.ToString(bytes);
            return hex.Replace("-", "");
        }


        private void RefreshMemoryDisplay()
        {
            if ( rbMemHex.Checked == true)
            {
                   String Blob = ByteArrayToHexStringViaBitConverter(this.SICVirtualMachine.MemoryBytes);
                
                    for ( int Add= 0; Add < 32768; Add++)
                    {
                    if ( (Add % 10 ) == 0 ){
                            txtMemory.Text += System.Environment.NewLine +  string.Format("{0:x4}: ", Add); 
                            }
                    else
                    {
                        txtMemory.Text += String.Format("{0:x2}", Blob.Substring(Add, 2)) + " ";
                    }



                    }


            } else
            {

            }


        }

        private void tsmSaveMachineState_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.AddExtension = true;

            DialogResult Result;

            Result = sfd.ShowDialog();


            if ( Result == DialogResult.OK)
            {
                using (var stream = File.Open(sfd.FileName, FileMode.Create))
                {
                    SoapFormatter sf = new SoapFormatter();
                    sf.Serialize(stream, this.SICVirtualMachine);
                }                
            }


            

        }

        private void tsmloadAndAssembleSICSourceFIle_Click(object sender, EventArgs e)
        {
            if (loadSICSourceFD.ShowDialog() == DialogResult.OK)
            {
                Assembler assembler = new Assembler(loadSICSourceFD.FileName);
            }
        }
    }
}
