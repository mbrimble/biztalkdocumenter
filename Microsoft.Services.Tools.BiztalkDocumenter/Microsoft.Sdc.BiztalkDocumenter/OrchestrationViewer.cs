using System.Drawing;
using Microsoft.Services.Tools.BizTalkOM;

namespace Microsoft.Services.Tools.BiztalkDocumenter
{
    /// <summary>
    /// Summary description for OrchestrationViewer.
    /// </summary>
    public class OrchestrationViewer : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private const int maxDisplaySize = 600;
        private const int additionalBorderSize = 60;

        public OrchestrationViewer(Orchestration orchestration)
        {
            InitializeComponent();

            if (orchestration != null)
            {
                this.Text = "Orchestration: " + orchestration.Name;
                Bitmap bmp = orchestration.GetImage();

                if (bmp != null)
                {
                    this.pictureBox1.Image = bmp;
                    this.panel1.AutoScrollMinSize = new Size(bmp.Width, bmp.Height);

                    if (bmp.Width < this.Width)
                    {
                        this.Width = bmp.Width + additionalBorderSize;
                    }
                    else
                    {
                        if (bmp.Width < maxDisplaySize)
                        {
                            this.Width = bmp.Width + additionalBorderSize;
                        }
                    }

                    if (bmp.Height < this.Height)
                    {
                        this.Height = bmp.Height + additionalBorderSize;
                    }
                    else
                    {
                        if (bmp.Height < maxDisplaySize)
                        {
                            this.Height = bmp.Height + additionalBorderSize;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(496, 430);
            this.panel1.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(496, 430);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // OrchestrationViewer
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(496, 430);
            this.Controls.Add(this.panel1);
            this.MinimizeBox = false;
            this.Name = "OrchestrationViewer";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Orchestration";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
    }
}
