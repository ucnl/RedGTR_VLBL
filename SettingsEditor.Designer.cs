namespace RedGTR_VLBL
{
    partial class SettingsEditor
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
            this.cancelBtn = new System.Windows.Forms.Button();
            this.okBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.gtrPortNameCbx = new System.Windows.Forms.ComboBox();
            this.isGNSSEmulationChb = new System.Windows.Forms.CheckBox();
            this.gnssEmuPortNameCbx = new System.Windows.Forms.ComboBox();
            this.gnssEmulatorPortNameLbl = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.maxDistanceEdit = new System.Windows.Forms.NumericUpDown();
            this.salinityEdit = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.fifoSizeEdit = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.baseSizeEdit = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.targetAddrEdit = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.rERrThresholdEdit = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.maxDistanceEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.salinityEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fifoSizeEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseSizeEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.targetAddrEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rERrThresholdEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cancelBtn.Location = new System.Drawing.Point(375, 361);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(96, 39);
            this.cancelBtn.TabIndex = 0;
            this.cancelBtn.Text = "CANCEL";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // okBtn
            // 
            this.okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBtn.Enabled = false;
            this.okBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.okBtn.Location = new System.Drawing.Point(245, 361);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(96, 39);
            this.okBtn.TabIndex = 1;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "RedGTR port name";
            // 
            // gtrPortNameCbx
            // 
            this.gtrPortNameCbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gtrPortNameCbx.FormattingEnabled = true;
            this.gtrPortNameCbx.Location = new System.Drawing.Point(16, 64);
            this.gtrPortNameCbx.Name = "gtrPortNameCbx";
            this.gtrPortNameCbx.Size = new System.Drawing.Size(183, 31);
            this.gtrPortNameCbx.TabIndex = 3;
            this.gtrPortNameCbx.SelectedIndexChanged += new System.EventHandler(this.gtrPortNameCbx_SelectedIndexChanged);
            // 
            // isGNSSEmulationChb
            // 
            this.isGNSSEmulationChb.AutoSize = true;
            this.isGNSSEmulationChb.Location = new System.Drawing.Point(254, 12);
            this.isGNSSEmulationChb.Name = "isGNSSEmulationChb";
            this.isGNSSEmulationChb.Size = new System.Drawing.Size(142, 27);
            this.isGNSSEmulationChb.TabIndex = 4;
            this.isGNSSEmulationChb.Text = "Emulate GNSS";
            this.isGNSSEmulationChb.UseVisualStyleBackColor = true;
            this.isGNSSEmulationChb.CheckedChanged += new System.EventHandler(this.isGNSSEmulationChb_CheckedChanged);
            // 
            // gnssEmuPortNameCbx
            // 
            this.gnssEmuPortNameCbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gnssEmuPortNameCbx.Enabled = false;
            this.gnssEmuPortNameCbx.FormattingEnabled = true;
            this.gnssEmuPortNameCbx.Location = new System.Drawing.Point(254, 64);
            this.gnssEmuPortNameCbx.Name = "gnssEmuPortNameCbx";
            this.gnssEmuPortNameCbx.Size = new System.Drawing.Size(183, 31);
            this.gnssEmuPortNameCbx.TabIndex = 6;
            this.gnssEmuPortNameCbx.SelectedIndexChanged += new System.EventHandler(this.gnssEmuPortNameCbx_SelectedIndexChanged);
            // 
            // gnssEmulatorPortNameLbl
            // 
            this.gnssEmulatorPortNameLbl.AutoSize = true;
            this.gnssEmulatorPortNameLbl.Enabled = false;
            this.gnssEmulatorPortNameLbl.Location = new System.Drawing.Point(250, 38);
            this.gnssEmulatorPortNameLbl.Name = "gnssEmulatorPortNameLbl";
            this.gnssEmulatorPortNameLbl.Size = new System.Drawing.Size(212, 23);
            this.gnssEmulatorPortNameLbl.TabIndex = 5;
            this.gnssEmulatorPortNameLbl.Text = "GNSS emulator port name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 23);
            this.label2.TabIndex = 7;
            this.label2.Text = "Max distance, m";
            // 
            // maxDistanceEdit
            // 
            this.maxDistanceEdit.Location = new System.Drawing.Point(16, 148);
            this.maxDistanceEdit.Maximum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.maxDistanceEdit.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.maxDistanceEdit.Name = "maxDistanceEdit";
            this.maxDistanceEdit.Size = new System.Drawing.Size(130, 30);
            this.maxDistanceEdit.TabIndex = 8;
            this.maxDistanceEdit.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // salinityEdit
            // 
            this.salinityEdit.DecimalPlaces = 1;
            this.salinityEdit.Location = new System.Drawing.Point(254, 148);
            this.salinityEdit.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.salinityEdit.Name = "salinityEdit";
            this.salinityEdit.Size = new System.Drawing.Size(130, 30);
            this.salinityEdit.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(250, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 23);
            this.label3.TabIndex = 9;
            this.label3.Text = "Salinity, PSU";
            // 
            // fifoSizeEdit
            // 
            this.fifoSizeEdit.Location = new System.Drawing.Point(254, 217);
            this.fifoSizeEdit.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.fifoSizeEdit.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.fifoSizeEdit.Name = "fifoSizeEdit";
            this.fifoSizeEdit.Size = new System.Drawing.Size(130, 30);
            this.fifoSizeEdit.TabIndex = 12;
            this.fifoSizeEdit.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(250, 195);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 23);
            this.label4.TabIndex = 11;
            this.label4.Text = "FIFO size";
            // 
            // baseSizeEdit
            // 
            this.baseSizeEdit.Location = new System.Drawing.Point(16, 217);
            this.baseSizeEdit.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.baseSizeEdit.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.baseSizeEdit.Name = "baseSizeEdit";
            this.baseSizeEdit.Size = new System.Drawing.Size(130, 30);
            this.baseSizeEdit.TabIndex = 14;
            this.baseSizeEdit.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 190);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 23);
            this.label5.TabIndex = 13;
            this.label5.Text = "Base size";
            // 
            // targetAddrEdit
            // 
            this.targetAddrEdit.Location = new System.Drawing.Point(254, 294);
            this.targetAddrEdit.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.targetAddrEdit.Name = "targetAddrEdit";
            this.targetAddrEdit.Size = new System.Drawing.Size(130, 30);
            this.targetAddrEdit.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(250, 267);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 23);
            this.label6.TabIndex = 15;
            this.label6.Text = "Target address";
            // 
            // rERrThresholdEdit
            // 
            this.rERrThresholdEdit.Location = new System.Drawing.Point(16, 294);
            this.rERrThresholdEdit.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.rERrThresholdEdit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.rERrThresholdEdit.Name = "rERrThresholdEdit";
            this.rERrThresholdEdit.Size = new System.Drawing.Size(130, 30);
            this.rERrThresholdEdit.TabIndex = 18;
            this.rERrThresholdEdit.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 267);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(199, 23);
            this.label7.TabIndex = 17;
            this.label7.Text = "Radial error threshold, m";
            // 
            // SettingsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 412);
            this.Controls.Add(this.rERrThresholdEdit);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.targetAddrEdit);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.baseSizeEdit);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.fifoSizeEdit);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.salinityEdit);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.maxDistanceEdit);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.gnssEmuPortNameCbx);
            this.Controls.Add(this.gnssEmulatorPortNameLbl);
            this.Controls.Add(this.isGNSSEmulationChb);
            this.Controls.Add(this.gtrPortNameCbx);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.cancelBtn);
            this.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SettingsEditor";
            this.Text = "SettingsEditor";
            ((System.ComponentModel.ISupportInitialize)(this.maxDistanceEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.salinityEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fifoSizeEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseSizeEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.targetAddrEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rERrThresholdEdit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox gtrPortNameCbx;
        private System.Windows.Forms.CheckBox isGNSSEmulationChb;
        private System.Windows.Forms.ComboBox gnssEmuPortNameCbx;
        private System.Windows.Forms.Label gnssEmulatorPortNameLbl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown maxDistanceEdit;
        private System.Windows.Forms.NumericUpDown salinityEdit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown fifoSizeEdit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown baseSizeEdit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown targetAddrEdit;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown rERrThresholdEdit;
        private System.Windows.Forms.Label label7;
    }
}