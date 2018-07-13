namespace RedGTR_VLBL
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainToolStrip = new System.Windows.Forms.ToolStrip();
            this.connectionBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.isAutoQueryBtn = new System.Windows.Forms.ToolStripButton();
            this.isAutosnapshotBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tracksBtn = new System.Windows.Forms.ToolStripDropDownButton();
            this.exportTracksBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.clearTracksBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsBtn = new System.Windows.Forms.ToolStripButton();
            this.infoBtn = new System.Windows.Forms.ToolStripButton();
            this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.connectionStatusLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.marinePlot = new RedGTR_VLBL.MarinePlot();
            this.consoleToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.clearHistoryBtn = new System.Windows.Forms.ToolStripButton();
            this.historyTxb = new System.Windows.Forms.RichTextBox();
            this.logBtn = new System.Windows.Forms.ToolStripDropDownButton();
            this.analyzeBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.mainToolStrip.SuspendLayout();
            this.mainStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.consoleToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainToolStrip
            // 
            this.mainToolStrip.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectionBtn,
            this.toolStripSeparator1,
            this.isAutoQueryBtn,
            this.isAutosnapshotBtn,
            this.toolStripSeparator2,
            this.tracksBtn,
            this.logBtn,
            this.toolStripSeparator4,
            this.settingsBtn,
            this.infoBtn});
            this.mainToolStrip.Location = new System.Drawing.Point(0, 0);
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.Size = new System.Drawing.Size(899, 32);
            this.mainToolStrip.TabIndex = 0;
            this.mainToolStrip.Text = "mainToolStrip";
            // 
            // connectionBtn
            // 
            this.connectionBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.connectionBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.connectionBtn.Image = ((System.Drawing.Image)(resources.GetObject("connectionBtn.Image")));
            this.connectionBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.connectionBtn.Name = "connectionBtn";
            this.connectionBtn.Size = new System.Drawing.Size(125, 29);
            this.connectionBtn.Text = "CONNECTION";
            this.connectionBtn.Click += new System.EventHandler(this.connectionBtn_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 32);
            // 
            // isAutoQueryBtn
            // 
            this.isAutoQueryBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.isAutoQueryBtn.Enabled = false;
            this.isAutoQueryBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.isAutoQueryBtn.Image = ((System.Drawing.Image)(resources.GetObject("isAutoQueryBtn.Image")));
            this.isAutoQueryBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.isAutoQueryBtn.Name = "isAutoQueryBtn";
            this.isAutoQueryBtn.Size = new System.Drawing.Size(120, 29);
            this.isAutoQueryBtn.Text = "AUTO QUERY";
            this.isAutoQueryBtn.Click += new System.EventHandler(this.isAutoQueryBtn_Click);
            // 
            // isAutosnapshotBtn
            // 
            this.isAutosnapshotBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.isAutosnapshotBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.isAutosnapshotBtn.Image = ((System.Drawing.Image)(resources.GetObject("isAutosnapshotBtn.Image")));
            this.isAutosnapshotBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.isAutosnapshotBtn.Name = "isAutosnapshotBtn";
            this.isAutosnapshotBtn.Size = new System.Drawing.Size(150, 29);
            this.isAutosnapshotBtn.Text = "AUTOSNAPSHOT";
            this.isAutosnapshotBtn.Click += new System.EventHandler(this.isAutosnapshotBtn_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 32);
            // 
            // tracksBtn
            // 
            this.tracksBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tracksBtn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportTracksBtn,
            this.toolStripSeparator5,
            this.clearTracksBtn});
            this.tracksBtn.Enabled = false;
            this.tracksBtn.Font = new System.Drawing.Font("Segoe UI", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tracksBtn.Image = ((System.Drawing.Image)(resources.GetObject("tracksBtn.Image")));
            this.tracksBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tracksBtn.Name = "tracksBtn";
            this.tracksBtn.Size = new System.Drawing.Size(96, 29);
            this.tracksBtn.Text = "TRACKS";
            // 
            // exportTracksBtn
            // 
            this.exportTracksBtn.Name = "exportTracksBtn";
            this.exportTracksBtn.Size = new System.Drawing.Size(155, 30);
            this.exportTracksBtn.Text = "EXPORT";
            this.exportTracksBtn.Click += new System.EventHandler(this.exportTracksBtn_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(152, 6);
            // 
            // clearTracksBtn
            // 
            this.clearTracksBtn.Name = "clearTracksBtn";
            this.clearTracksBtn.Size = new System.Drawing.Size(155, 30);
            this.clearTracksBtn.Text = "CLEAR";
            this.clearTracksBtn.Click += new System.EventHandler(this.clearTracksBtn_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 32);
            // 
            // settingsBtn
            // 
            this.settingsBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.settingsBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.settingsBtn.Image = ((System.Drawing.Image)(resources.GetObject("settingsBtn.Image")));
            this.settingsBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.settingsBtn.Name = "settingsBtn";
            this.settingsBtn.Size = new System.Drawing.Size(93, 29);
            this.settingsBtn.Text = "SETTINGS";
            this.settingsBtn.Click += new System.EventHandler(this.settingsBtn_Click);
            // 
            // infoBtn
            // 
            this.infoBtn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.infoBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.infoBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.infoBtn.Image = ((System.Drawing.Image)(resources.GetObject("infoBtn.Image")));
            this.infoBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.infoBtn.Name = "infoBtn";
            this.infoBtn.Size = new System.Drawing.Size(54, 29);
            this.infoBtn.Text = "INFO";
            this.infoBtn.Click += new System.EventHandler(this.infoBtn_Click);
            // 
            // mainStatusStrip
            // 
            this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.connectionStatusLbl});
            this.mainStatusStrip.Location = new System.Drawing.Point(0, 596);
            this.mainStatusStrip.Name = "mainStatusStrip";
            this.mainStatusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 13, 0);
            this.mainStatusStrip.Size = new System.Drawing.Size(899, 28);
            this.mainStatusStrip.TabIndex = 1;
            this.mainStatusStrip.Text = "mainStatusStrip";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(124, 23);
            this.toolStripStatusLabel1.Text = "CONNECTION:";
            // 
            // connectionStatusLbl
            // 
            this.connectionStatusLbl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.connectionStatusLbl.Name = "connectionStatusLbl";
            this.connectionStatusLbl.Size = new System.Drawing.Size(41, 23);
            this.connectionStatusLbl.Text = "- - -";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 32);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.marinePlot);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.consoleToolStrip);
            this.splitContainer1.Panel2.Controls.Add(this.historyTxb);
            this.splitContainer1.Size = new System.Drawing.Size(899, 564);
            this.splitContainer1.SplitterDistance = 367;
            this.splitContainer1.TabIndex = 2;
            // 
            // marinePlot
            // 
            this.marinePlot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.marinePlot.BackColor = System.Drawing.Color.Azure;
            this.marinePlot.LeftBottomLine = "";
            this.marinePlot.LeftUpperCornerText = "";
            this.marinePlot.Location = new System.Drawing.Point(3, 2);
            this.marinePlot.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.marinePlot.Name = "marinePlot";
            this.marinePlot.Size = new System.Drawing.Size(893, 362);
            this.marinePlot.TabIndex = 0;
            // 
            // consoleToolStrip
            // 
            this.consoleToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripSeparator3,
            this.clearHistoryBtn});
            this.consoleToolStrip.Location = new System.Drawing.Point(0, 0);
            this.consoleToolStrip.Name = "consoleToolStrip";
            this.consoleToolStrip.Size = new System.Drawing.Size(899, 30);
            this.consoleToolStrip.TabIndex = 1;
            this.consoleToolStrip.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(88, 27);
            this.toolStripLabel1.Text = "CONSOLE";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 30);
            // 
            // clearHistoryBtn
            // 
            this.clearHistoryBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.clearHistoryBtn.Font = new System.Drawing.Font("Segoe UI", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.clearHistoryBtn.Image = ((System.Drawing.Image)(resources.GetObject("clearHistoryBtn.Image")));
            this.clearHistoryBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.clearHistoryBtn.Name = "clearHistoryBtn";
            this.clearHistoryBtn.Size = new System.Drawing.Size(66, 27);
            this.clearHistoryBtn.Text = "CLEAR";
            this.clearHistoryBtn.Click += new System.EventHandler(this.clearHistoryBtn_Click);
            // 
            // historyTxb
            // 
            this.historyTxb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.historyTxb.Location = new System.Drawing.Point(3, 33);
            this.historyTxb.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.historyTxb.Name = "historyTxb";
            this.historyTxb.ReadOnly = true;
            this.historyTxb.Size = new System.Drawing.Size(893, 161);
            this.historyTxb.TabIndex = 0;
            this.historyTxb.Text = "";
            this.historyTxb.TextChanged += new System.EventHandler(this.historyTxb_TextChanged);
            // 
            // logBtn
            // 
            this.logBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.logBtn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.analyzeBtn});
            this.logBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.logBtn.Image = ((System.Drawing.Image)(resources.GetObject("logBtn.Image")));
            this.logBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.logBtn.Name = "logBtn";
            this.logBtn.Size = new System.Drawing.Size(57, 29);
            this.logBtn.Text = "LOG";
            // 
            // analyzeBtn
            // 
            this.analyzeBtn.Name = "analyzeBtn";
            this.analyzeBtn.Size = new System.Drawing.Size(169, 28);
            this.analyzeBtn.Text = "ANALYZE...";
            this.analyzeBtn.Click += new System.EventHandler(this.analyzeBtn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 624);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.mainStatusStrip);
            this.Controls.Add(this.mainToolStrip);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "RedGTR VLBL navigation demo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.mainToolStrip.ResumeLayout(false);
            this.mainToolStrip.PerformLayout();
            this.mainStatusStrip.ResumeLayout(false);
            this.mainStatusStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.consoleToolStrip.ResumeLayout(false);
            this.consoleToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip mainToolStrip;
        private System.Windows.Forms.ToolStripButton connectionBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton isAutoQueryBtn;
        private System.Windows.Forms.ToolStripButton isAutosnapshotBtn;
        private System.Windows.Forms.ToolStripButton settingsBtn;
        private System.Windows.Forms.ToolStripButton infoBtn;
        private System.Windows.Forms.StatusStrip mainStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel connectionStatusLbl;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox historyTxb;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private MarinePlot marinePlot;
        private System.Windows.Forms.ToolStrip consoleToolStrip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton clearHistoryBtn;
        private System.Windows.Forms.ToolStripDropDownButton tracksBtn;
        private System.Windows.Forms.ToolStripMenuItem exportTracksBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem clearTracksBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripDropDownButton logBtn;
        private System.Windows.Forms.ToolStripMenuItem analyzeBtn;
    }
}

