namespace TileShop
{
    partial class PaletteEditorForm
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
            this.components = new System.ComponentModel.Container();
            gTrackBar.ColorPack colorPack17 = new gTrackBar.ColorPack();
            gTrackBar.ColorPack colorPack18 = new gTrackBar.ColorPack();
            gTrackBar.ColorPack colorPack19 = new gTrackBar.ColorPack();
            gTrackBar.ColorPack colorPack20 = new gTrackBar.ColorPack();
            gTrackBar.ColorLinearGradient colorLinearGradient8 = new gTrackBar.ColorLinearGradient();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaletteEditorForm));
            gTrackBar.ColorPack colorPack21 = new gTrackBar.ColorPack();
            gTrackBar.ColorPack colorPack22 = new gTrackBar.ColorPack();
            gTrackBar.ColorPack colorPack23 = new gTrackBar.ColorPack();
            gTrackBar.ColorPack colorPack24 = new gTrackBar.ColorPack();
            gTrackBar.ColorLinearGradient colorLinearGradient9 = new gTrackBar.ColorLinearGradient();
            gTrackBar.ColorLinearGradient colorLinearGradient10 = new gTrackBar.ColorLinearGradient();
            gTrackBar.ColorPack colorPack25 = new gTrackBar.ColorPack();
            gTrackBar.ColorPack colorPack26 = new gTrackBar.ColorPack();
            gTrackBar.ColorPack colorPack27 = new gTrackBar.ColorPack();
            gTrackBar.ColorPack colorPack28 = new gTrackBar.ColorPack();
            gTrackBar.ColorLinearGradient colorLinearGradient11 = new gTrackBar.ColorLinearGradient();
            gTrackBar.ColorLinearGradient colorLinearGradient12 = new gTrackBar.ColorLinearGradient();
            gTrackBar.ColorPack colorPack29 = new gTrackBar.ColorPack();
            gTrackBar.ColorPack colorPack30 = new gTrackBar.ColorPack();
            gTrackBar.ColorPack colorPack31 = new gTrackBar.ColorPack();
            gTrackBar.ColorPack colorPack32 = new gTrackBar.ColorPack();
            gTrackBar.ColorLinearGradient colorLinearGradient13 = new gTrackBar.ColorLinearGradient();
            gTrackBar.ColorLinearGradient colorLinearGradient14 = new gTrackBar.ColorLinearGradient();
            this.PaletteNameBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.NudRed = new System.Windows.Forms.NumericUpDown();
            this.NudGreen = new System.Windows.Forms.NumericUpDown();
            this.NudBlue = new System.Windows.Forms.NumericUpDown();
            this.SliderRed = new gTrackBar.gTrackBar();
            this.SliderBlue = new gTrackBar.gTrackBar();
            this.SliderGreen = new gTrackBar.gTrackBar();
            this.label6 = new System.Windows.Forms.Label();
            this.NudAlpha = new System.Windows.Forms.NumericUpDown();
            this.SliderAlpha = new gTrackBar.gTrackBar();
            this.label7 = new System.Windows.Forms.Label();
            this.activeColorPanel = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SavePaletteButton = new System.Windows.Forms.Button();
            this.ReloadPaletteButton = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.NudEntries = new System.Windows.Forms.NumericUpDown();
            this.ProjectFileBox = new System.Windows.Forms.TextBox();
            this.PaletteOffsetBox = new TileShop.IntegerTextBox();
            this.ColorFormatBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SwatchControl = new TileShop.SwatchControl();
            this.paletteTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.NudRed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudBlue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudAlpha)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NudEntries)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PaletteNameBox
            // 
            this.PaletteNameBox.Location = new System.Drawing.Point(50, 53);
            this.PaletteNameBox.Name = "PaletteNameBox";
            this.PaletteNameBox.Size = new System.Drawing.Size(129, 20);
            this.PaletteNameBox.TabIndex = 0;
            this.PaletteNameBox.TextChanged += new System.EventHandler(this.PaletteNameBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Source:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Red:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Green:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Blue:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Name:";
            // 
            // NudRed
            // 
            this.NudRed.Location = new System.Drawing.Point(50, 25);
            this.NudRed.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NudRed.Name = "NudRed";
            this.NudRed.Size = new System.Drawing.Size(79, 20);
            this.NudRed.TabIndex = 14;
            this.NudRed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.NudRed.ValueChanged += new System.EventHandler(this.NudRed_ValueChanged);
            // 
            // NudGreen
            // 
            this.NudGreen.Location = new System.Drawing.Point(50, 53);
            this.NudGreen.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NudGreen.Name = "NudGreen";
            this.NudGreen.Size = new System.Drawing.Size(77, 20);
            this.NudGreen.TabIndex = 15;
            this.NudGreen.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.NudGreen.ValueChanged += new System.EventHandler(this.NudGreen_ValueChanged);
            // 
            // NudBlue
            // 
            this.NudBlue.Location = new System.Drawing.Point(50, 81);
            this.NudBlue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NudBlue.Name = "NudBlue";
            this.NudBlue.Size = new System.Drawing.Size(77, 20);
            this.NudBlue.TabIndex = 16;
            this.NudBlue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.NudBlue.ValueChanged += new System.EventHandler(this.NudBlue_ValueChanged);
            // 
            // SliderRed
            // 
            colorPack17.Border = System.Drawing.Color.Black;
            colorPack17.Face = System.Drawing.Color.Transparent;
            colorPack17.Highlight = System.Drawing.Color.Transparent;
            this.SliderRed.AButColor = colorPack17;
            this.SliderRed.ArrowColorDown = System.Drawing.Color.Black;
            this.SliderRed.ArrowColorHover = System.Drawing.Color.Black;
            this.SliderRed.ArrowColorUp = System.Drawing.Color.Black;
            this.SliderRed.BackColor = System.Drawing.SystemColors.Control;
            colorPack18.Border = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            colorPack18.Face = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            colorPack18.Highlight = System.Drawing.Color.White;
            this.SliderRed.ColorDown = colorPack18;
            colorPack19.Border = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            colorPack19.Face = System.Drawing.Color.Red;
            colorPack19.Highlight = System.Drawing.Color.White;
            this.SliderRed.ColorHover = colorPack19;
            colorPack20.Border = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            colorPack20.Face = System.Drawing.Color.Red;
            colorPack20.Highlight = System.Drawing.Color.White;
            this.SliderRed.ColorUp = colorPack20;
            this.SliderRed.FloatValue = false;
            this.SliderRed.Label = "";
            this.SliderRed.LabelPadding = new System.Windows.Forms.Padding(0);
            this.SliderRed.Location = new System.Drawing.Point(136, 25);
            this.SliderRed.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.SliderRed.MaxValue = 255;
            this.SliderRed.Name = "SliderRed";
            this.SliderRed.ShowFocus = false;
            this.SliderRed.Size = new System.Drawing.Size(305, 21);
            colorLinearGradient8.ColorA = System.Drawing.Color.Black;
            colorLinearGradient8.ColorB = System.Drawing.Color.Black;
            this.SliderRed.SliderColorHigh = colorLinearGradient8;
            this.SliderRed.SliderHighlightPt = ((System.Drawing.PointF)(resources.GetObject("SliderRed.SliderHighlightPt")));
            this.SliderRed.SliderShape = gTrackBar.gTrackBar.eShape.Rectangle;
            this.SliderRed.SliderSize = new System.Drawing.Size(10, 20);
            this.SliderRed.SliderWidthHigh = 2F;
            this.SliderRed.SliderWidthLow = 2F;
            this.SliderRed.TabIndex = 17;
            this.SliderRed.TickColor = System.Drawing.Color.Black;
            this.SliderRed.TickInterval = 5;
            this.SliderRed.TickThickness = 1F;
            this.SliderRed.TickType = gTrackBar.gTrackBar.eTickType.Middle;
            this.SliderRed.TickWidth = 8;
            this.SliderRed.UpDownShow = false;
            this.SliderRed.UpDownWidth = 15;
            this.SliderRed.Value = 0;
            this.SliderRed.ValueAdjusted = 0F;
            this.SliderRed.ValueDivisor = gTrackBar.gTrackBar.eValueDivisor.e1;
            this.SliderRed.ValueStrFormat = null;
            this.SliderRed.ValueChanged += new gTrackBar.gTrackBar.ValueChangedEventHandler(this.SliderRed_ValueChanged);
            // 
            // SliderBlue
            // 
            colorPack21.Border = System.Drawing.Color.Black;
            colorPack21.Face = System.Drawing.Color.Transparent;
            colorPack21.Highlight = System.Drawing.Color.Transparent;
            this.SliderBlue.AButColor = colorPack21;
            this.SliderBlue.ArrowColorDown = System.Drawing.Color.Black;
            this.SliderBlue.ArrowColorHover = System.Drawing.Color.Black;
            this.SliderBlue.ArrowColorUp = System.Drawing.Color.Black;
            this.SliderBlue.BackColor = System.Drawing.SystemColors.Control;
            colorPack22.Border = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            colorPack22.Face = System.Drawing.Color.Navy;
            colorPack22.Highlight = System.Drawing.Color.White;
            this.SliderBlue.ColorDown = colorPack22;
            colorPack23.Border = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            colorPack23.Face = System.Drawing.Color.Blue;
            colorPack23.Highlight = System.Drawing.Color.White;
            this.SliderBlue.ColorHover = colorPack23;
            colorPack24.Border = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            colorPack24.Face = System.Drawing.Color.RoyalBlue;
            colorPack24.Highlight = System.Drawing.Color.White;
            this.SliderBlue.ColorUp = colorPack24;
            this.SliderBlue.FloatValue = false;
            this.SliderBlue.Label = "";
            this.SliderBlue.LabelPadding = new System.Windows.Forms.Padding(0);
            this.SliderBlue.Location = new System.Drawing.Point(136, 81);
            this.SliderBlue.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.SliderBlue.MaxValue = 255;
            this.SliderBlue.Name = "SliderBlue";
            this.SliderBlue.ShowFocus = false;
            this.SliderBlue.Size = new System.Drawing.Size(305, 21);
            colorLinearGradient9.ColorA = System.Drawing.Color.Black;
            colorLinearGradient9.ColorB = System.Drawing.Color.Black;
            this.SliderBlue.SliderColorHigh = colorLinearGradient9;
            colorLinearGradient10.ColorA = System.Drawing.Color.Blue;
            colorLinearGradient10.ColorB = System.Drawing.Color.Blue;
            this.SliderBlue.SliderColorLow = colorLinearGradient10;
            this.SliderBlue.SliderHighlightPt = ((System.Drawing.PointF)(resources.GetObject("SliderBlue.SliderHighlightPt")));
            this.SliderBlue.SliderShape = gTrackBar.gTrackBar.eShape.Rectangle;
            this.SliderBlue.SliderSize = new System.Drawing.Size(10, 20);
            this.SliderBlue.SliderWidthHigh = 2F;
            this.SliderBlue.SliderWidthLow = 2F;
            this.SliderBlue.TabIndex = 18;
            this.SliderBlue.TickColor = System.Drawing.Color.Black;
            this.SliderBlue.TickInterval = 5;
            this.SliderBlue.TickThickness = 1F;
            this.SliderBlue.TickType = gTrackBar.gTrackBar.eTickType.Middle;
            this.SliderBlue.TickWidth = 8;
            this.SliderBlue.UpDownShow = false;
            this.SliderBlue.UpDownWidth = 15;
            this.SliderBlue.Value = 0;
            this.SliderBlue.ValueAdjusted = 0F;
            this.SliderBlue.ValueDivisor = gTrackBar.gTrackBar.eValueDivisor.e1;
            this.SliderBlue.ValueStrFormat = null;
            this.SliderBlue.ValueChanged += new gTrackBar.gTrackBar.ValueChangedEventHandler(this.SliderBlue_ValueChanged);
            // 
            // SliderGreen
            // 
            colorPack25.Border = System.Drawing.Color.Black;
            colorPack25.Face = System.Drawing.Color.Transparent;
            colorPack25.Highlight = System.Drawing.Color.Transparent;
            this.SliderGreen.AButColor = colorPack25;
            this.SliderGreen.ArrowColorDown = System.Drawing.Color.Black;
            this.SliderGreen.ArrowColorHover = System.Drawing.Color.Black;
            this.SliderGreen.ArrowColorUp = System.Drawing.Color.Black;
            this.SliderGreen.BackColor = System.Drawing.SystemColors.Control;
            colorPack26.Border = System.Drawing.Color.Green;
            colorPack26.Face = System.Drawing.Color.DarkGreen;
            colorPack26.Highlight = System.Drawing.Color.White;
            this.SliderGreen.ColorDown = colorPack26;
            colorPack27.Border = System.Drawing.Color.Green;
            colorPack27.Face = System.Drawing.Color.SeaGreen;
            colorPack27.Highlight = System.Drawing.Color.White;
            this.SliderGreen.ColorHover = colorPack27;
            colorPack28.Border = System.Drawing.Color.Green;
            colorPack28.Face = System.Drawing.Color.Lime;
            colorPack28.Highlight = System.Drawing.Color.White;
            this.SliderGreen.ColorUp = colorPack28;
            this.SliderGreen.FloatValue = false;
            this.SliderGreen.Label = "";
            this.SliderGreen.LabelPadding = new System.Windows.Forms.Padding(0);
            this.SliderGreen.Location = new System.Drawing.Point(136, 53);
            this.SliderGreen.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.SliderGreen.MaxValue = 255;
            this.SliderGreen.Name = "SliderGreen";
            this.SliderGreen.ShowFocus = false;
            this.SliderGreen.Size = new System.Drawing.Size(305, 21);
            colorLinearGradient11.ColorA = System.Drawing.Color.Black;
            colorLinearGradient11.ColorB = System.Drawing.Color.Black;
            this.SliderGreen.SliderColorHigh = colorLinearGradient11;
            colorLinearGradient12.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            colorLinearGradient12.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.SliderGreen.SliderColorLow = colorLinearGradient12;
            this.SliderGreen.SliderHighlightPt = ((System.Drawing.PointF)(resources.GetObject("SliderGreen.SliderHighlightPt")));
            this.SliderGreen.SliderShape = gTrackBar.gTrackBar.eShape.Rectangle;
            this.SliderGreen.SliderSize = new System.Drawing.Size(10, 20);
            this.SliderGreen.SliderWidthHigh = 2F;
            this.SliderGreen.SliderWidthLow = 2F;
            this.SliderGreen.TabIndex = 19;
            this.SliderGreen.TickColor = System.Drawing.Color.Black;
            this.SliderGreen.TickInterval = 5;
            this.SliderGreen.TickThickness = 1F;
            this.SliderGreen.TickType = gTrackBar.gTrackBar.eTickType.Middle;
            this.SliderGreen.TickWidth = 8;
            this.SliderGreen.UpDownShow = false;
            this.SliderGreen.UpDownWidth = 15;
            this.SliderGreen.Value = 0;
            this.SliderGreen.ValueAdjusted = 0F;
            this.SliderGreen.ValueDivisor = gTrackBar.gTrackBar.eValueDivisor.e1;
            this.SliderGreen.ValueStrFormat = null;
            this.SliderGreen.ValueChanged += new gTrackBar.gTrackBar.ValueChangedEventHandler(this.SliderGreen_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 111);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Alpha:";
            // 
            // NudAlpha
            // 
            this.NudAlpha.Location = new System.Drawing.Point(50, 109);
            this.NudAlpha.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NudAlpha.Name = "NudAlpha";
            this.NudAlpha.Size = new System.Drawing.Size(77, 20);
            this.NudAlpha.TabIndex = 21;
            this.NudAlpha.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.NudAlpha.ValueChanged += new System.EventHandler(this.NudAlpha_ValueChanged);
            // 
            // SliderAlpha
            // 
            colorPack29.Border = System.Drawing.Color.Black;
            colorPack29.Face = System.Drawing.Color.Transparent;
            colorPack29.Highlight = System.Drawing.Color.Transparent;
            this.SliderAlpha.AButColor = colorPack29;
            this.SliderAlpha.ArrowColorDown = System.Drawing.Color.Black;
            this.SliderAlpha.ArrowColorHover = System.Drawing.Color.Black;
            this.SliderAlpha.ArrowColorUp = System.Drawing.Color.Black;
            this.SliderAlpha.BackColor = System.Drawing.SystemColors.Control;
            colorPack30.Border = System.Drawing.Color.Gray;
            colorPack30.Face = System.Drawing.Color.Gray;
            colorPack30.Highlight = System.Drawing.Color.Gainsboro;
            this.SliderAlpha.ColorDown = colorPack30;
            colorPack31.Border = System.Drawing.Color.DarkGray;
            colorPack31.Face = System.Drawing.Color.DarkGray;
            colorPack31.Highlight = System.Drawing.Color.White;
            this.SliderAlpha.ColorHover = colorPack31;
            colorPack32.Border = System.Drawing.Color.DarkGray;
            colorPack32.Face = System.Drawing.Color.Silver;
            colorPack32.Highlight = System.Drawing.Color.White;
            this.SliderAlpha.ColorUp = colorPack32;
            this.SliderAlpha.FloatValue = false;
            this.SliderAlpha.Label = "";
            this.SliderAlpha.LabelPadding = new System.Windows.Forms.Padding(0);
            this.SliderAlpha.Location = new System.Drawing.Point(136, 109);
            this.SliderAlpha.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.SliderAlpha.MaxValue = 255;
            this.SliderAlpha.Name = "SliderAlpha";
            this.SliderAlpha.ShowFocus = false;
            this.SliderAlpha.Size = new System.Drawing.Size(305, 21);
            colorLinearGradient13.ColorA = System.Drawing.Color.Black;
            colorLinearGradient13.ColorB = System.Drawing.Color.Black;
            this.SliderAlpha.SliderColorHigh = colorLinearGradient13;
            colorLinearGradient14.ColorA = System.Drawing.Color.Gray;
            colorLinearGradient14.ColorB = System.Drawing.Color.Gray;
            this.SliderAlpha.SliderColorLow = colorLinearGradient14;
            this.SliderAlpha.SliderHighlightPt = ((System.Drawing.PointF)(resources.GetObject("SliderAlpha.SliderHighlightPt")));
            this.SliderAlpha.SliderShape = gTrackBar.gTrackBar.eShape.Rectangle;
            this.SliderAlpha.SliderSize = new System.Drawing.Size(10, 20);
            this.SliderAlpha.SliderWidthHigh = 2F;
            this.SliderAlpha.SliderWidthLow = 2F;
            this.SliderAlpha.TabIndex = 22;
            this.SliderAlpha.TickColor = System.Drawing.Color.Black;
            this.SliderAlpha.TickInterval = 5;
            this.SliderAlpha.TickThickness = 1F;
            this.SliderAlpha.TickType = gTrackBar.gTrackBar.eTickType.Middle;
            this.SliderAlpha.TickWidth = 8;
            this.SliderAlpha.UpDownShow = false;
            this.SliderAlpha.UpDownWidth = 15;
            this.SliderAlpha.Value = 0;
            this.SliderAlpha.ValueAdjusted = 0F;
            this.SliderAlpha.ValueDivisor = gTrackBar.gTrackBar.eValueDivisor.e1;
            this.SliderAlpha.ValueStrFormat = null;
            this.SliderAlpha.ValueChanged += new gTrackBar.gTrackBar.ValueChangedEventHandler(this.SliderAlpha_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 84);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "Offset:";
            // 
            // activeColorPanel
            // 
            this.activeColorPanel.BackColor = System.Drawing.Color.White;
            this.activeColorPanel.BackgroundImage = global::TileShop.Properties.Resources.TransparentBrushPattern;
            this.activeColorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.activeColorPanel.Location = new System.Drawing.Point(360, 27);
            this.activeColorPanel.Name = "activeColorPanel";
            this.activeColorPanel.Size = new System.Drawing.Size(75, 75);
            this.activeColorPanel.TabIndex = 25;
            this.activeColorPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.ActiveColorPanel_Paint);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SavePaletteButton);
            this.groupBox1.Controls.Add(this.ReloadPaletteButton);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.NudEntries);
            this.groupBox1.Controls.Add(this.PaletteNameBox);
            this.groupBox1.Controls.Add(this.ProjectFileBox);
            this.groupBox1.Controls.Add(this.PaletteOffsetBox);
            this.groupBox1.Controls.Add(this.ColorFormatBox);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.activeColorPanel);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(444, 149);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Palette Configuration";
            // 
            // SavePaletteButton
            // 
            this.SavePaletteButton.Location = new System.Drawing.Point(9, 115);
            this.SavePaletteButton.Name = "SavePaletteButton";
            this.SavePaletteButton.Size = new System.Drawing.Size(91, 23);
            this.SavePaletteButton.TabIndex = 32;
            this.SavePaletteButton.Text = "Save";
            this.SavePaletteButton.UseVisualStyleBackColor = true;
            this.SavePaletteButton.Click += new System.EventHandler(this.SavePaletteButton_Click);
            // 
            // ReloadPaletteButton
            // 
            this.ReloadPaletteButton.Location = new System.Drawing.Point(106, 115);
            this.ReloadPaletteButton.Name = "ReloadPaletteButton";
            this.ReloadPaletteButton.Size = new System.Drawing.Size(93, 23);
            this.ReloadPaletteButton.TabIndex = 31;
            this.ReloadPaletteButton.Text = "Reload";
            this.ReloadPaletteButton.UseVisualStyleBackColor = true;
            this.ReloadPaletteButton.Click += new System.EventHandler(this.ReloadPaletteButton_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(212, 84);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(42, 13);
            this.label10.TabIndex = 30;
            this.label10.Text = "Entries:";
            // 
            // NudEntries
            // 
            this.NudEntries.Location = new System.Drawing.Point(255, 81);
            this.NudEntries.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.NudEntries.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.NudEntries.Name = "NudEntries";
            this.NudEntries.Size = new System.Drawing.Size(99, 20);
            this.NudEntries.TabIndex = 29;
            this.NudEntries.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.NudEntries.ValueChanged += new System.EventHandler(this.NudEntries_ValueChanged);
            // 
            // ProjectFileBox
            // 
            this.ProjectFileBox.Location = new System.Drawing.Point(50, 27);
            this.ProjectFileBox.Name = "ProjectFileBox";
            this.ProjectFileBox.ReadOnly = true;
            this.ProjectFileBox.Size = new System.Drawing.Size(304, 20);
            this.ProjectFileBox.TabIndex = 28;
            // 
            // PaletteOffsetBox
            // 
            this.PaletteOffsetBox.Location = new System.Drawing.Point(50, 81);
            this.PaletteOffsetBox.Maximum = 2147483647;
            this.PaletteOffsetBox.Minimum = 0;
            this.PaletteOffsetBox.Name = "PaletteOffsetBox";
            this.PaletteOffsetBox.Size = new System.Drawing.Size(129, 20);
            this.PaletteOffsetBox.TabIndex = 28;
            // 
            // ColorFormatBox
            // 
            this.ColorFormatBox.FormattingEnabled = true;
            this.ColorFormatBox.Location = new System.Drawing.Point(255, 53);
            this.ColorFormatBox.Name = "ColorFormatBox";
            this.ColorFormatBox.Size = new System.Drawing.Size(99, 21);
            this.ColorFormatBox.TabIndex = 27;
            this.ColorFormatBox.SelectedIndexChanged += new System.EventHandler(this.ColorFormatBox_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(185, 56);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "Color Format:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.SliderAlpha);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.NudAlpha);
            this.groupBox2.Controls.Add(this.NudRed);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.NudGreen);
            this.groupBox2.Controls.Add(this.SliderGreen);
            this.groupBox2.Controls.Add(this.NudBlue);
            this.groupBox2.Controls.Add(this.SliderBlue);
            this.groupBox2.Controls.Add(this.SliderRed);
            this.groupBox2.Location = new System.Drawing.Point(12, 167);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(444, 139);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Color Editor";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(474, 348);
            this.panel1.TabIndex = 28;
            // 
            // SwatchControl
            // 
            this.SwatchControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SwatchControl.Location = new System.Drawing.Point(0, 348);
            this.SwatchControl.Name = "SwatchControl";
            this.SwatchControl.SelectedIndex = 0;
            this.SwatchControl.Size = new System.Drawing.Size(474, 310);
            this.SwatchControl.TabIndex = 30;
            this.SwatchControl.SelectedIndexChanged += new System.EventHandler<System.EventArgs>(this.SwatchControl_SelectedIndexChanged);
            // 
            // PaletteEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 658);
            this.Controls.Add(this.SwatchControl);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PaletteEditorForm";
            this.Text = "PaletteEditor";
            ((System.ComponentModel.ISupportInitialize)(this.NudRed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudGreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudBlue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudAlpha)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NudEntries)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox PaletteNameBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown NudRed;
        private System.Windows.Forms.NumericUpDown NudGreen;
        private System.Windows.Forms.NumericUpDown NudBlue;
        private gTrackBar.gTrackBar SliderRed;
        private gTrackBar.gTrackBar SliderBlue;
        private gTrackBar.gTrackBar SliderGreen;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown NudAlpha;
        private gTrackBar.gTrackBar SliderAlpha;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel activeColorPanel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox ColorFormatBox;
        private IntegerTextBox PaletteOffsetBox;
        private System.Windows.Forms.TextBox ProjectFileBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown NudEntries;
        private SwatchControl SwatchControl;
        private System.Windows.Forms.Button SavePaletteButton;
        private System.Windows.Forms.Button ReloadPaletteButton;
        private System.Windows.Forms.ToolTip paletteTip;
    }
}