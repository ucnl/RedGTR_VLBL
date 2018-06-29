namespace RedGTR_VLBL
{
    partial class MarinePlot
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MarinePlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Azure;
            this.DoubleBuffered = true;
            this.Name = "MarinePlot";
            this.Size = new System.Drawing.Size(541, 380);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MarinePlot_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MarinePlot_MouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MarinePlot_MouseDoubleClick);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MarinePlot_MouseMove);
            this.Resize += new System.EventHandler(this.MarinePlot_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
