namespace SIC_Simulator
{
    partial class dlgRelocateObjectFile
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblProgramName = new System.Windows.Forms.Label();
            this.StartAddressField = new System.Windows.Forms.Label();
            this.RelocateAddress = new System.Windows.Forms.Label();
            this.txtAssembledStartPoint = new System.Windows.Forms.TextBox();
            this.txtRelocationAddress = new System.Windows.Forms.TextBox();
            this.lblProgramLength = new System.Windows.Forms.Label();
            this.lblRelocationRecords = new System.Windows.Forms.Label();
            this.lblRelocateNote = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.Location = new System.Drawing.Point(272, 267);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOK.Location = new System.Drawing.Point(126, 267);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "&Ok";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblProgramName
            // 
            this.lblProgramName.AutoSize = true;
            this.lblProgramName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgramName.Location = new System.Drawing.Point(34, 9);
            this.lblProgramName.Name = "lblProgramName";
            this.lblProgramName.Size = new System.Drawing.Size(123, 20);
            this.lblProgramName.TabIndex = 8;
            this.lblProgramName.Text = "Program Name: ";
            this.lblProgramName.Click += new System.EventHandler(this.lblProgramName_Click);
            // 
            // StartAddressField
            // 
            this.StartAddressField.AutoSize = true;
            this.StartAddressField.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartAddressField.Location = new System.Drawing.Point(34, 109);
            this.StartAddressField.Name = "StartAddressField";
            this.StartAddressField.Size = new System.Drawing.Size(216, 20);
            this.StartAddressField.TabIndex = 9;
            this.StartAddressField.Text = "Absolute Start Address (Hex)";
            // 
            // RelocateAddress
            // 
            this.RelocateAddress.AutoSize = true;
            this.RelocateAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RelocateAddress.Location = new System.Drawing.Point(34, 143);
            this.RelocateAddress.Name = "RelocateAddress";
            this.RelocateAddress.Size = new System.Drawing.Size(226, 20);
            this.RelocateAddress.TabIndex = 10;
            this.RelocateAddress.Text = "Relocated Start Address (Hex)";
            // 
            // txtAssembledStartPoint
            // 
            this.txtAssembledStartPoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAssembledStartPoint.Location = new System.Drawing.Point(275, 106);
            this.txtAssembledStartPoint.MaxLength = 4;
            this.txtAssembledStartPoint.Name = "txtAssembledStartPoint";
            this.txtAssembledStartPoint.ReadOnly = true;
            this.txtAssembledStartPoint.Size = new System.Drawing.Size(100, 26);
            this.txtAssembledStartPoint.TabIndex = 11;
            this.txtAssembledStartPoint.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtRelocationAddress
            // 
            this.txtRelocationAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRelocationAddress.Location = new System.Drawing.Point(275, 140);
            this.txtRelocationAddress.MaxLength = 6;
            this.txtRelocationAddress.Name = "txtRelocationAddress";
            this.txtRelocationAddress.Size = new System.Drawing.Size(100, 26);
            this.txtRelocationAddress.TabIndex = 12;
            this.txtRelocationAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblProgramLength
            // 
            this.lblProgramLength.AutoSize = true;
            this.lblProgramLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgramLength.Location = new System.Drawing.Point(34, 43);
            this.lblProgramLength.Name = "lblProgramLength";
            this.lblProgramLength.Size = new System.Drawing.Size(127, 20);
            this.lblProgramLength.TabIndex = 13;
            this.lblProgramLength.Text = "Program Length:";
            // 
            // lblRelocationRecords
            // 
            this.lblRelocationRecords.AutoSize = true;
            this.lblRelocationRecords.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRelocationRecords.Location = new System.Drawing.Point(34, 78);
            this.lblRelocationRecords.Name = "lblRelocationRecords";
            this.lblRelocationRecords.Size = new System.Drawing.Size(175, 20);
            this.lblRelocationRecords.TabIndex = 14;
            this.lblRelocationRecords.Text = "# Modification Records:";
            // 
            // lblRelocateNote
            // 
            this.lblRelocateNote.AutoSize = true;
            this.lblRelocateNote.BackColor = System.Drawing.Color.Yellow;
            this.lblRelocateNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRelocateNote.Location = new System.Drawing.Point(37, 182);
            this.lblRelocateNote.Name = "lblRelocateNote";
            this.lblRelocateNote.Size = new System.Drawing.Size(124, 16);
            this.lblRelocateNote.TabIndex = 15;
            this.lblRelocateNote.Text = "Relocation Note:";
            this.lblRelocateNote.Click += new System.EventHandler(this.lblNote_Click);
            // 
            // dlgRelocateObjectFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 302);
            this.Controls.Add(this.lblRelocateNote);
            this.Controls.Add(this.lblRelocationRecords);
            this.Controls.Add(this.lblProgramLength);
            this.Controls.Add(this.txtRelocationAddress);
            this.Controls.Add(this.txtAssembledStartPoint);
            this.Controls.Add(this.RelocateAddress);
            this.Controls.Add(this.StartAddressField);
            this.Controls.Add(this.lblProgramName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "dlgRelocateObjectFile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Object File Details / Relocation";
            this.Load += new System.EventHandler(this.dlgRelocateObjectFile_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblProgramName;
        private System.Windows.Forms.Label StartAddressField;
        private System.Windows.Forms.Label RelocateAddress;
        private System.Windows.Forms.TextBox txtAssembledStartPoint;
        private System.Windows.Forms.TextBox txtRelocationAddress;
        private System.Windows.Forms.Label lblProgramLength;
        private System.Windows.Forms.Label lblRelocationRecords;
        private System.Windows.Forms.Label lblRelocateNote;
    }
}