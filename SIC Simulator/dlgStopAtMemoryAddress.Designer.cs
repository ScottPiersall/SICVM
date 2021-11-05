
namespace SIC_Simulator
{
    partial class dlgStopAtMemoryAddress
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAddressInHex = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblLastLoadedFile = new System.Windows.Forms.Label();
            this.lblLastLoadedStartAddress = new System.Windows.Forms.Label();
            this.lblLastLoadedLength = new System.Windows.Forms.Label();
            this.lblCalculatedEndPoint = new System.Windows.Forms.Label();
            this.txtLastLoadedFile = new System.Windows.Forms.TextBox();
            this.txtLastLoadedStart = new System.Windows.Forms.TextBox();
            this.txtLastLoadedLength = new System.Windows.Forms.TextBox();
            this.txtCalculatedHaltingPoint = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOK.Location = new System.Drawing.Point(114, 185);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "&Ok";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(28, 152);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(278, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "HALT RUN When PC Reaches Address (in Hex)";
            // 
            // txtAddressInHex
            // 
            this.txtAddressInHex.Location = new System.Drawing.Point(312, 149);
            this.txtAddressInHex.MaxLength = 4;
            this.txtAddressInHex.Name = "txtAddressInHex";
            this.txtAddressInHex.Size = new System.Drawing.Size(100, 20);
            this.txtAddressInHex.TabIndex = 13;
            this.txtAddressInHex.Text = "0000";
            this.txtAddressInHex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.Location = new System.Drawing.Point(236, 185);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblLastLoadedFile
            // 
            this.lblLastLoadedFile.AutoSize = true;
            this.lblLastLoadedFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastLoadedFile.Location = new System.Drawing.Point(33, 18);
            this.lblLastLoadedFile.Name = "lblLastLoadedFile";
            this.lblLastLoadedFile.Size = new System.Drawing.Size(101, 13);
            this.lblLastLoadedFile.TabIndex = 15;
            this.lblLastLoadedFile.Text = "Last Loaded File";
            // 
            // lblLastLoadedStartAddress
            // 
            this.lblLastLoadedStartAddress.AutoSize = true;
            this.lblLastLoadedStartAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastLoadedStartAddress.Location = new System.Drawing.Point(33, 52);
            this.lblLastLoadedStartAddress.Name = "lblLastLoadedStartAddress";
            this.lblLastLoadedStartAddress.Size = new System.Drawing.Size(205, 13);
            this.lblLastLoadedStartAddress.TabIndex = 16;
            this.lblLastLoadedStartAddress.Text = "Last Loaded Start Address (in Hex)";
            // 
            // lblLastLoadedLength
            // 
            this.lblLastLoadedLength.AutoSize = true;
            this.lblLastLoadedLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastLoadedLength.Location = new System.Drawing.Point(33, 87);
            this.lblLastLoadedLength.Name = "lblLastLoadedLength";
            this.lblLastLoadedLength.Size = new System.Drawing.Size(218, 13);
            this.lblLastLoadedLength.TabIndex = 17;
            this.lblLastLoadedLength.Text = "Last Loaded Program Length (in Hex)";
            // 
            // lblCalculatedEndPoint
            // 
            this.lblCalculatedEndPoint.AutoSize = true;
            this.lblCalculatedEndPoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCalculatedEndPoint.Location = new System.Drawing.Point(33, 120);
            this.lblCalculatedEndPoint.Name = "lblCalculatedEndPoint";
            this.lblCalculatedEndPoint.Size = new System.Drawing.Size(192, 13);
            this.lblCalculatedEndPoint.TabIndex = 18;
            this.lblCalculatedEndPoint.Text = "Calculated Halting Point (in Hex)";
            // 
            // txtLastLoadedFile
            // 
            this.txtLastLoadedFile.Location = new System.Drawing.Point(207, 15);
            this.txtLastLoadedFile.MaxLength = 4;
            this.txtLastLoadedFile.Name = "txtLastLoadedFile";
            this.txtLastLoadedFile.ReadOnly = true;
            this.txtLastLoadedFile.Size = new System.Drawing.Size(205, 20);
            this.txtLastLoadedFile.TabIndex = 19;
            this.txtLastLoadedFile.Text = "0000";
            this.txtLastLoadedFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtLastLoadedStart
            // 
            this.txtLastLoadedStart.Location = new System.Drawing.Point(312, 49);
            this.txtLastLoadedStart.MaxLength = 4;
            this.txtLastLoadedStart.Name = "txtLastLoadedStart";
            this.txtLastLoadedStart.ReadOnly = true;
            this.txtLastLoadedStart.Size = new System.Drawing.Size(100, 20);
            this.txtLastLoadedStart.TabIndex = 20;
            this.txtLastLoadedStart.Text = "0000";
            this.txtLastLoadedStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtLastLoadedLength
            // 
            this.txtLastLoadedLength.Location = new System.Drawing.Point(312, 81);
            this.txtLastLoadedLength.MaxLength = 4;
            this.txtLastLoadedLength.Name = "txtLastLoadedLength";
            this.txtLastLoadedLength.ReadOnly = true;
            this.txtLastLoadedLength.Size = new System.Drawing.Size(100, 20);
            this.txtLastLoadedLength.TabIndex = 21;
            this.txtLastLoadedLength.Text = "0000";
            this.txtLastLoadedLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtCalculatedHaltingPoint
            // 
            this.txtCalculatedHaltingPoint.Location = new System.Drawing.Point(312, 113);
            this.txtCalculatedHaltingPoint.MaxLength = 4;
            this.txtCalculatedHaltingPoint.Name = "txtCalculatedHaltingPoint";
            this.txtCalculatedHaltingPoint.ReadOnly = true;
            this.txtCalculatedHaltingPoint.Size = new System.Drawing.Size(100, 20);
            this.txtCalculatedHaltingPoint.TabIndex = 22;
            this.txtCalculatedHaltingPoint.Text = "0000";
            this.txtCalculatedHaltingPoint.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // dlgStopAtMemoryAddress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 220);
            this.ControlBox = false;
            this.Controls.Add(this.txtCalculatedHaltingPoint);
            this.Controls.Add(this.txtLastLoadedLength);
            this.Controls.Add(this.txtLastLoadedStart);
            this.Controls.Add(this.txtLastLoadedFile);
            this.Controls.Add(this.lblCalculatedEndPoint);
            this.Controls.Add(this.lblLastLoadedLength);
            this.Controls.Add(this.lblLastLoadedStartAddress);
            this.Controls.Add(this.lblLastLoadedFile);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtAddressInHex);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.Name = "dlgStopAtMemoryAddress";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Run Until ";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAddressInHex;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblLastLoadedFile;
        private System.Windows.Forms.Label lblLastLoadedStartAddress;
        private System.Windows.Forms.Label lblLastLoadedLength;
        private System.Windows.Forms.Label lblCalculatedEndPoint;
        private System.Windows.Forms.TextBox txtLastLoadedFile;
        private System.Windows.Forms.TextBox txtLastLoadedStart;
        private System.Windows.Forms.TextBox txtLastLoadedLength;
        private System.Windows.Forms.TextBox txtCalculatedHaltingPoint;
    }
}