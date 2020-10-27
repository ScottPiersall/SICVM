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
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

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
            if (rbMemHex.Checked == true)
            {
                String Blob = ByteArrayToHexStringViaBitConverter(this.SICVirtualMachine.MemoryBytes);

                for (int Add = 0; Add < 32768; Add++)
                {
                    if ((Add % 10) == 0) {
                        txtMemory.Text += System.Environment.NewLine + string.Format("{0:x4}: ", Add);
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
            sfd.Filter = "SIC VM State Files|*.sicstate";

            DialogResult Result;

            Result = sfd.ShowDialog();


            if (Result == DialogResult.OK)
            {
                using (var stream = File.Open(sfd.FileName, FileMode.Create))
                {
                    SoapFormatter sf = new SoapFormatter();
                    sf.Serialize(stream, this.SICVirtualMachine);
                }
            }




        }

        private void btnStep_Click(object sender, EventArgs e)
        {
            this.SICVirtualMachine.PerformStep();

            this.RefreshCPUDisplays();
        }


        private void RefreshCPUDisplays(){
            var RegThread = new System.Threading.Thread(RegRefreshAsync);
            RegThread.Start();

            var MemoryThread = new System.Threading.Thread(MemoryRefreshAsync);
            MemoryThread.Start();


        }

        /// <summary>
        /// Refreshes Memory Display on background thread. Calls are marshalled to UI thread
        /// </summary>
        private void MemoryRefreshAsync()
        {
            if (rbMemHex.Checked == true)
            {
                String Blob = ByteArrayToHexStringViaBitConverter(this.SICVirtualMachine.MemoryBytes);

                StringBuilder sb = new StringBuilder((32768 * 2) + 512);

                for (int Add = 0; Add < 32768; Add++)
                {
                    if ((Add % 10) == 0)
                    {
                        sb.Append(System.Environment.NewLine + string.Format("{0:x4}: ", Add));
                    }
                    else
                    {
                        sb.Append(String.Format("{0:x2}", Blob.Substring(Add, 2)) + " ");   
                    }
                }
                txtMemory.Invoke(new Action(() => this.txtMemory.Text = sb.ToString())  );

            }
            else
            {

            }



        }

        /// <summary>
        /// Refreshes Register Displays on background thread. Calls are marshalled to UI thread
        /// </summary>
        private void RegRefreshAsync()
        {
            txtPC_Hex.Invoke( new Action (() =>    this.txtX_Hex.Text = this.SICVirtualMachine.X.ToString("X6") ));
            txtPC_Hex.Invoke(new Action(() => this.txtA_Hex.Text = this.SICVirtualMachine.A.ToString("X6") ));
            txtPC_Hex.Invoke(new Action(() => this.txtL_Hex.Text = this.SICVirtualMachine.L.ToString("X6")));
            txtPC_Hex.Invoke(new Action(() => this.txtPC_Hex.Text = this.SICVirtualMachine.PC.ToString("X6")));
            txtPC_Hex.Invoke(new Action(() => this.txtSW_Hex.Text = this.SICVirtualMachine.SW.ToString("X6")));

            txtPC_Hex.Invoke(new Action(() => this.txtX_Dec.Text = this.SICVirtualMachine.X.ToString()));
            txtPC_Hex.Invoke(new Action(() => this.txtA_Dec.Text = this.SICVirtualMachine.A.ToString()));
            txtPC_Hex.Invoke(new Action(() => this.txtL_Dec.Text = this.SICVirtualMachine.L.ToString()));
            txtPC_Hex.Invoke(new Action(() => this.txtPC_Dec.Text = this.SICVirtualMachine.PC.ToString() ));
            txtPC_Hex.Invoke(new Action(() => this.txtSW_Dec.Text = this.SICVirtualMachine.SW.ToString()));

            // Now do the binary bytes for each register and Status Word



        }

        private void loadSavedSICMachineStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            DialogResult Res;
            ofd.Filter = "SIC VM State Files|*.sicstate";
            ofd.Multiselect = false;

            Res = ofd.ShowDialog();

            if ( Res == DialogResult.OK)
            {
                using (var stream = File.Open(ofd.FileName, FileMode.Open))
                {
                    SoapFormatter osf = new SoapFormatter();
                    this.SICVirtualMachine = (SIC_CPU) osf.Deserialize(stream); 
                }
                // Refresh Memory and Register Displays to Show Saved State
                this.RefreshCPUDisplays();


            }


        }
    }
}
