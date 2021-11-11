
namespace SIC_Simulator
{
    partial class dlgRelocatePrompt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgRelocatePrompt));
            this.InfoPromptRelocate = new System.Windows.Forms.Label();
            this.AbsoluteLoadBtn = new System.Windows.Forms.Button();
            this.RelocateLoaderBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // InfoPromptRelocate
            // 
            this.InfoPromptRelocate.AutoSize = true;
            this.InfoPromptRelocate.Location = new System.Drawing.Point(12, 9);
            this.InfoPromptRelocate.Name = "InfoPromptRelocate";
            this.InfoPromptRelocate.Size = new System.Drawing.Size(392, 52);
            this.InfoPromptRelocate.TabIndex = 0;
            this.InfoPromptRelocate.Text = resources.GetString("InfoPromptRelocate.Text");
            // 
            // AbsoluteLoadBtn
            // 
            this.AbsoluteLoadBtn.Location = new System.Drawing.Point(40, 93);
            this.AbsoluteLoadBtn.Name = "AbsoluteLoadBtn";
            this.AbsoluteLoadBtn.Size = new System.Drawing.Size(166, 23);
            this.AbsoluteLoadBtn.TabIndex = 1;
            this.AbsoluteLoadBtn.Text = "Absolute Loader (Default)";
            this.AbsoluteLoadBtn.UseVisualStyleBackColor = true;
            this.AbsoluteLoadBtn.Click += new System.EventHandler(this.AbsoluteLoadBtn_Click);
            // 
            // RelocateLoaderBtn
            // 
            this.RelocateLoaderBtn.Location = new System.Drawing.Point(212, 93);
            this.RelocateLoaderBtn.Name = "RelocateLoaderBtn";
            this.RelocateLoaderBtn.Size = new System.Drawing.Size(135, 23);
            this.RelocateLoaderBtn.TabIndex = 2;
            this.RelocateLoaderBtn.Text = "Relocating Loader ";
            this.RelocateLoaderBtn.UseVisualStyleBackColor = true;
            this.RelocateLoaderBtn.Click += new System.EventHandler(this.RelocateLoaderBtn_Click);
            // 
            // dlgRelocatePrompt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 143);
            this.Controls.Add(this.RelocateLoaderBtn);
            this.Controls.Add(this.AbsoluteLoadBtn);
            this.Controls.Add(this.InfoPromptRelocate);
            this.Name = "dlgRelocatePrompt";
            this.Text = "Relocate Assembled Code?";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label InfoPromptRelocate;
        private System.Windows.Forms.Button AbsoluteLoadBtn;
        private System.Windows.Forms.Button RelocateLoaderBtn;
    }
}