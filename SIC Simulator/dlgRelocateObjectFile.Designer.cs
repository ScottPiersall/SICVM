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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAssembledStartPoint = new System.Windows.Forms.TextBox();
            this.txtRelocationAddress = new System.Windows.Forms.TextBox();
            this.lblProgramLength = new System.Windows.Forms.Label();
            this.lblRelocationRecords = new System.Windows.Forms.Label();
            this.lblNote = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.Location = new System.Drawing.Point(285, 243);
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
            this.btnOK.Location = new System.Drawing.Point(139, 243);
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
            this.lblProgramName.Location = new System.Drawing.Point(153, 13);
            this.lblProgramName.Name = "lblProgramName";
            this.lblProgramName.Size = new System.Drawing.Size(123, 20);
            this.lblProgramName.TabIndex = 8;
            this.lblProgramName.Text = "Program Name: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(97, 130);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "Assembled Start Point";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(97, 167);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "Relocate To";
            // 
            // txtAssembledStartPoint
            // 
            this.txtAssembledStartPoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAssembledStartPoint.Location = new System.Drawing.Point(301, 127);
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
            this.txtRelocationAddress.Location = new System.Drawing.Point(301, 164);
            this.txtRelocationAddress.MaxLength = 4;
            this.txtRelocationAddress.Name = "txtRelocationAddress";
            this.txtRelocationAddress.Size = new System.Drawing.Size(100, 26);
            this.txtRelocationAddress.TabIndex = 12;
            this.txtRelocationAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblProgramLength
            // 
            this.lblProgramLength.AutoSize = true;
            this.lblProgramLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgramLength.Location = new System.Drawing.Point(153, 47);
            this.lblProgramLength.Name = "lblProgramLength";
            this.lblProgramLength.Size = new System.Drawing.Size(127, 20);
            this.lblProgramLength.TabIndex = 13;
            this.lblProgramLength.Text = "Program Length:";
            // 
            // lblRelocationRecords
            // 
            this.lblRelocationRecords.AutoSize = true;
            this.lblRelocationRecords.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRelocationRecords.Location = new System.Drawing.Point(153, 80);
            this.lblRelocationRecords.Name = "lblRelocationRecords";
            this.lblRelocationRecords.Size = new System.Drawing.Size(153, 20);
            this.lblRelocationRecords.TabIndex = 14;
            this.lblRelocationRecords.Text = "Relocation Records:";
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.BackColor = System.Drawing.Color.Yellow;
            this.lblNote.Location = new System.Drawing.Point(75, 205);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(35, 13);
            this.lblNote.TabIndex = 15;
            this.lblNote.Text = "label3";
            // 
            // dlgRelocateObjectFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 278);
            this.Controls.Add(this.lblNote);
            this.Controls.Add(this.lblRelocationRecords);
            this.Controls.Add(this.lblProgramLength);
            this.Controls.Add(this.txtRelocationAddress);
            this.Controls.Add(this.txtAssembledStartPoint);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblProgramName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "dlgRelocateObjectFile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Object File Details / Relocation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblProgramName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAssembledStartPoint;
        private System.Windows.Forms.TextBox txtRelocationAddress;
        private System.Windows.Forms.Label lblProgramLength;
        private System.Windows.Forms.Label lblRelocationRecords;
        private System.Windows.Forms.Label lblNote;
    }
}