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
            gTrackBar.ColorPack colorPack1 = new gTrackBar.ColorPack();
            gTrackBar.ColorPack colorPack2 = new gTrackBar.ColorPack();
            gTrackBar.ColorPack colorPack3 = new gTrackBar.ColorPack();
            gTrackBar.ColorPack colorPack4 = new gTrackBar.ColorPack();
            gTrackBar.ColorLinearGradient colorLinearGradient1 = new gTrackBar.ColorLinearGradient();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaletteEditorForm));
            gTrackBar.ColorPack colorPack5 = new gTrackBar.ColorPack();
            gTrackBar.ColorPack colorPack6 = new gTrackBar.ColorPack();
            gTrackBar.ColorPack colorPack7 = new gTrackBar.ColorPack();
            gTrackBar.ColorPack colorPack8 = new gTrackBar.ColorPack();
            gTrackBar.ColorLinearGradient colorLinearGradient2 = new gTrackBar.ColorLinearGradient();
            gTrackBar.ColorLinearGradient colorLinearGradient3 = new gTrackBar.ColorLinearGradient();
            gTrackBar.ColorPack colorPack9 = new gTrackBar.ColorPack();
            gTrackBar.ColorPack colorPack10 = new gTrackBar.ColorPack();
            gTrackBar.ColorPack colorPack11 = new gTrackBar.ColorPack();
            gTrackBar.ColorPack colorPack12 = new gTrackBar.ColorPack();
            gTrackBar.ColorLinearGradient colorLinearGradient4 = new gTrackBar.ColorLinearGradient();
            gTrackBar.ColorLinearGradient colorLinearGradient5 = new gTrackBar.ColorLinearGradient();
            gTrackBar.ColorPack colorPack13 = new gTrackBar.ColorPack();
            gTrackBar.ColorPack colorPack14 = new gTrackBar.ColorPack();
            gTrackBar.ColorPack colorPack15 = new gTrackBar.ColorPack();
            gTrackBar.ColorPack colorPack16 = new gTrackBar.ColorPack();
            gTrackBar.ColorLinearGradient colorLinearGradient6 = new gTrackBar.ColorLinearGradient();
            gTrackBar.ColorLinearGradient colorLinearGradient7 = new gTrackBar.ColorLinearGradient();
            this.paletteNameBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.nudRed = new System.Windows.Forms.NumericUpDown();
            this.nudGreen = new System.Windows.Forms.NumericUpDown();
            this.nudBlue = new System.Windows.Forms.NumericUpDown();
            this.sliderRed = new gTrackBar.gTrackBar();
            this.sliderBlue = new gTrackBar.gTrackBar();
            this.sliderGreen = new gTrackBar.gTrackBar();
            this.label6 = new System.Windows.Forms.Label();
            this.nudAlpha = new System.Windows.Forms.NumericUpDown();
            this.sliderAlpha = new gTrackBar.gTrackBar();
            this.label7 = new System.Windows.Forms.Label();
            this.activeColorPanel = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.savePaletteButton = new System.Windows.Forms.Button();
            this.reloadPaletteButton = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.nudEntries = new System.Windows.Forms.NumericUpDown();
            this.projectFileBox = new System.Windows.Forms.TextBox();
            this.paletteOffsetBox = new TileShop.IntegerTextBox();
            this.colorFormatBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.saveColorButton = new System.Windows.Forms.Button();
            this.htmlBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.swatchControl = new TileShop.SwatchControl();
            this.paletteTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.nudRed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBlue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAlpha)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEntries)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // paletteNameBox
            // 
            this.paletteNameBox.Location = new System.Drawing.Point(50, 53);
            this.paletteNameBox.Name = "paletteNameBox";
            this.paletteNameBox.Size = new System.Drawing.Size(129, 20);
            this.paletteNameBox.TabIndex = 0;
            this.paletteNameBox.TextChanged += new System.EventHandler(this.paletteNameBox_TextChanged);
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
            // nudRed
            // 
            this.nudRed.Location = new System.Drawing.Point(50, 25);
            this.nudRed.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudRed.Name = "nudRed";
            this.nudRed.Size = new System.Drawing.Size(79, 20);
            this.nudRed.TabIndex = 14;
            this.nudRed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudRed.ValueChanged += new System.EventHandler(this.nudRed_ValueChanged);
            // 
            // nudGreen
            // 
            this.nudGreen.Location = new System.Drawing.Point(50, 53);
            this.nudGreen.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudGreen.Name = "nudGreen";
            this.nudGreen.Size = new System.Drawing.Size(77, 20);
            this.nudGreen.TabIndex = 15;
            this.nudGreen.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudGreen.ValueChanged += new System.EventHandler(this.nudGreen_ValueChanged);
            // 
            // nudBlue
            // 
            this.nudBlue.Location = new System.Drawing.Point(50, 81);
            this.nudBlue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudBlue.Name = "nudBlue";
            this.nudBlue.Size = new System.Drawing.Size(77, 20);
            this.nudBlue.TabIndex = 16;
            this.nudBlue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudBlue.ValueChanged += new System.EventHandler(this.nudBlue_ValueChanged);
            // 
            // sliderRed
            // 
            colorPack1.Border = System.Drawing.Color.Black;
            colorPack1.Face = System.Drawing.Color.Transparent;
            colorPack1.Highlight = System.Drawing.Color.Transparent;
            this.sliderRed.AButColor = colorPack1;
            this.sliderRed.ArrowColorDown = System.Drawing.Color.Black;
            this.sliderRed.ArrowColorHover = System.Drawing.Color.Black;
            this.sliderRed.ArrowColorUp = System.Drawing.Color.Black;
            this.sliderRed.BackColor = System.Drawing.SystemColors.Control;
            colorPack2.Border = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            colorPack2.Face = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            colorPack2.Highlight = System.Drawing.Color.White;
            this.sliderRed.ColorDown = colorPack2;
            colorPack3.Border = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            colorPack3.Face = System.Drawing.Color.Red;
            colorPack3.Highlight = System.Drawing.Color.White;
            this.sliderRed.ColorHover = colorPack3;
            colorPack4.Border = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            colorPack4.Face = System.Drawing.Color.Red;
            colorPack4.Highlight = System.Drawing.Color.White;
            this.sliderRed.ColorUp = colorPack4;
            this.sliderRed.FloatValue = false;
            this.sliderRed.Label = "";
            this.sliderRed.LabelPadding = new System.Windows.Forms.Padding(0);
            this.sliderRed.Location = new System.Drawing.Point(136, 25);
            this.sliderRed.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.sliderRed.MaxValue = 255;
            this.sliderRed.Name = "sliderRed";
            this.sliderRed.ShowFocus = false;
            this.sliderRed.Size = new System.Drawing.Size(305, 21);
            colorLinearGradient1.ColorA = System.Drawing.Color.Black;
            colorLinearGradient1.ColorB = System.Drawing.Color.Black;
            this.sliderRed.SliderColorHigh = colorLinearGradient1;
            this.sliderRed.SliderHighlightPt = ((System.Drawing.PointF)(resources.GetObject("sliderRed.SliderHighlightPt")));
            this.sliderRed.SliderShape = gTrackBar.gTrackBar.eShape.Rectangle;
            this.sliderRed.SliderSize = new System.Drawing.Size(10, 20);
            this.sliderRed.SliderWidthHigh = 2F;
            this.sliderRed.SliderWidthLow = 2F;
            this.sliderRed.TabIndex = 17;
            this.sliderRed.TickColor = System.Drawing.Color.Black;
            this.sliderRed.TickInterval = 5;
            this.sliderRed.TickThickness = 1F;
            this.sliderRed.TickType = gTrackBar.gTrackBar.eTickType.Middle;
            this.sliderRed.TickWidth = 8;
            this.sliderRed.UpDownShow = false;
            this.sliderRed.UpDownWidth = 15;
            this.sliderRed.Value = 0;
            this.sliderRed.ValueAdjusted = 0F;
            this.sliderRed.ValueDivisor = gTrackBar.gTrackBar.eValueDivisor.e1;
            this.sliderRed.ValueStrFormat = null;
            this.sliderRed.ValueChanged += new gTrackBar.gTrackBar.ValueChangedEventHandler(this.sliderRed_ValueChanged);
            // 
            // sliderBlue
            // 
            colorPack5.Border = System.Drawing.Color.Black;
            colorPack5.Face = System.Drawing.Color.Transparent;
            colorPack5.Highlight = System.Drawing.Color.Transparent;
            this.sliderBlue.AButColor = colorPack5;
            this.sliderBlue.ArrowColorDown = System.Drawing.Color.Black;
            this.sliderBlue.ArrowColorHover = System.Drawing.Color.Black;
            this.sliderBlue.ArrowColorUp = System.Drawing.Color.Black;
            this.sliderBlue.BackColor = System.Drawing.SystemColors.Control;
            colorPack6.Border = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            colorPack6.Face = System.Drawing.Color.Navy;
            colorPack6.Highlight = System.Drawing.Color.White;
            this.sliderBlue.ColorDown = colorPack6;
            colorPack7.Border = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            colorPack7.Face = System.Drawing.Color.Blue;
            colorPack7.Highlight = System.Drawing.Color.White;
            this.sliderBlue.ColorHover = colorPack7;
            colorPack8.Border = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            colorPack8.Face = System.Drawing.Color.RoyalBlue;
            colorPack8.Highlight = System.Drawing.Color.White;
            this.sliderBlue.ColorUp = colorPack8;
            this.sliderBlue.FloatValue = false;
            this.sliderBlue.Label = "";
            this.sliderBlue.LabelPadding = new System.Windows.Forms.Padding(0);
            this.sliderBlue.Location = new System.Drawing.Point(136, 81);
            this.sliderBlue.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.sliderBlue.MaxValue = 255;
            this.sliderBlue.Name = "sliderBlue";
            this.sliderBlue.ShowFocus = false;
            this.sliderBlue.Size = new System.Drawing.Size(305, 21);
            colorLinearGradient2.ColorA = System.Drawing.Color.Black;
            colorLinearGradient2.ColorB = System.Drawing.Color.Black;
            this.sliderBlue.SliderColorHigh = colorLinearGradient2;
            colorLinearGradient3.ColorA = System.Drawing.Color.Blue;
            colorLinearGradient3.ColorB = System.Drawing.Color.Blue;
            this.sliderBlue.SliderColorLow = colorLinearGradient3;
            this.sliderBlue.SliderHighlightPt = ((System.Drawing.PointF)(resources.GetObject("sliderBlue.SliderHighlightPt")));
            this.sliderBlue.SliderShape = gTrackBar.gTrackBar.eShape.Rectangle;
            this.sliderBlue.SliderSize = new System.Drawing.Size(10, 20);
            this.sliderBlue.SliderWidthHigh = 2F;
            this.sliderBlue.SliderWidthLow = 2F;
            this.sliderBlue.TabIndex = 18;
            this.sliderBlue.TickColor = System.Drawing.Color.Black;
            this.sliderBlue.TickInterval = 5;
            this.sliderBlue.TickThickness = 1F;
            this.sliderBlue.TickType = gTrackBar.gTrackBar.eTickType.Middle;
            this.sliderBlue.TickWidth = 8;
            this.sliderBlue.UpDownShow = false;
            this.sliderBlue.UpDownWidth = 15;
            this.sliderBlue.Value = 0;
            this.sliderBlue.ValueAdjusted = 0F;
            this.sliderBlue.ValueDivisor = gTrackBar.gTrackBar.eValueDivisor.e1;
            this.sliderBlue.ValueStrFormat = null;
            this.sliderBlue.ValueChanged += new gTrackBar.gTrackBar.ValueChangedEventHandler(this.sliderBlue_ValueChanged);
            // 
            // sliderGreen
            // 
            colorPack9.Border = System.Drawing.Color.Black;
            colorPack9.Face = System.Drawing.Color.Transparent;
            colorPack9.Highlight = System.Drawing.Color.Transparent;
            this.sliderGreen.AButColor = colorPack9;
            this.sliderGreen.ArrowColorDown = System.Drawing.Color.Black;
            this.sliderGreen.ArrowColorHover = System.Drawing.Color.Black;
            this.sliderGreen.ArrowColorUp = System.Drawing.Color.Black;
            this.sliderGreen.BackColor = System.Drawing.SystemColors.Control;
            colorPack10.Border = System.Drawing.Color.Green;
            colorPack10.Face = System.Drawing.Color.DarkGreen;
            colorPack10.Highlight = System.Drawing.Color.White;
            this.sliderGreen.ColorDown = colorPack10;
            colorPack11.Border = System.Drawing.Color.Green;
            colorPack11.Face = System.Drawing.Color.SeaGreen;
            colorPack11.Highlight = System.Drawing.Color.White;
            this.sliderGreen.ColorHover = colorPack11;
            colorPack12.Border = System.Drawing.Color.Green;
            colorPack12.Face = System.Drawing.Color.Lime;
            colorPack12.Highlight = System.Drawing.Color.White;
            this.sliderGreen.ColorUp = colorPack12;
            this.sliderGreen.FloatValue = false;
            this.sliderGreen.Label = "";
            this.sliderGreen.LabelPadding = new System.Windows.Forms.Padding(0);
            this.sliderGreen.Location = new System.Drawing.Point(136, 53);
            this.sliderGreen.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.sliderGreen.MaxValue = 255;
            this.sliderGreen.Name = "sliderGreen";
            this.sliderGreen.ShowFocus = false;
            this.sliderGreen.Size = new System.Drawing.Size(305, 21);
            colorLinearGradient4.ColorA = System.Drawing.Color.Black;
            colorLinearGradient4.ColorB = System.Drawing.Color.Black;
            this.sliderGreen.SliderColorHigh = colorLinearGradient4;
            colorLinearGradient5.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            colorLinearGradient5.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.sliderGreen.SliderColorLow = colorLinearGradient5;
            this.sliderGreen.SliderHighlightPt = ((System.Drawing.PointF)(resources.GetObject("sliderGreen.SliderHighlightPt")));
            this.sliderGreen.SliderShape = gTrackBar.gTrackBar.eShape.Rectangle;
            this.sliderGreen.SliderSize = new System.Drawing.Size(10, 20);
            this.sliderGreen.SliderWidthHigh = 2F;
            this.sliderGreen.SliderWidthLow = 2F;
            this.sliderGreen.TabIndex = 19;
            this.sliderGreen.TickColor = System.Drawing.Color.Black;
            this.sliderGreen.TickInterval = 5;
            this.sliderGreen.TickThickness = 1F;
            this.sliderGreen.TickType = gTrackBar.gTrackBar.eTickType.Middle;
            this.sliderGreen.TickWidth = 8;
            this.sliderGreen.UpDownShow = false;
            this.sliderGreen.UpDownWidth = 15;
            this.sliderGreen.Value = 0;
            this.sliderGreen.ValueAdjusted = 0F;
            this.sliderGreen.ValueDivisor = gTrackBar.gTrackBar.eValueDivisor.e1;
            this.sliderGreen.ValueStrFormat = null;
            this.sliderGreen.ValueChanged += new gTrackBar.gTrackBar.ValueChangedEventHandler(this.sliderGreen_ValueChanged);
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
            // nudAlpha
            // 
            this.nudAlpha.Location = new System.Drawing.Point(50, 109);
            this.nudAlpha.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudAlpha.Name = "nudAlpha";
            this.nudAlpha.Size = new System.Drawing.Size(77, 20);
            this.nudAlpha.TabIndex = 21;
            this.nudAlpha.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudAlpha.ValueChanged += new System.EventHandler(this.nudAlpha_ValueChanged);
            // 
            // sliderAlpha
            // 
            colorPack13.Border = System.Drawing.Color.Black;
            colorPack13.Face = System.Drawing.Color.Transparent;
            colorPack13.Highlight = System.Drawing.Color.Transparent;
            this.sliderAlpha.AButColor = colorPack13;
            this.sliderAlpha.ArrowColorDown = System.Drawing.Color.Black;
            this.sliderAlpha.ArrowColorHover = System.Drawing.Color.Black;
            this.sliderAlpha.ArrowColorUp = System.Drawing.Color.Black;
            this.sliderAlpha.BackColor = System.Drawing.SystemColors.Control;
            colorPack14.Border = System.Drawing.Color.Gray;
            colorPack14.Face = System.Drawing.Color.Gray;
            colorPack14.Highlight = System.Drawing.Color.Gainsboro;
            this.sliderAlpha.ColorDown = colorPack14;
            colorPack15.Border = System.Drawing.Color.DarkGray;
            colorPack15.Face = System.Drawing.Color.DarkGray;
            colorPack15.Highlight = System.Drawing.Color.White;
            this.sliderAlpha.ColorHover = colorPack15;
            colorPack16.Border = System.Drawing.Color.DarkGray;
            colorPack16.Face = System.Drawing.Color.Silver;
            colorPack16.Highlight = System.Drawing.Color.White;
            this.sliderAlpha.ColorUp = colorPack16;
            this.sliderAlpha.FloatValue = false;
            this.sliderAlpha.Label = "";
            this.sliderAlpha.LabelPadding = new System.Windows.Forms.Padding(0);
            this.sliderAlpha.Location = new System.Drawing.Point(136, 109);
            this.sliderAlpha.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.sliderAlpha.MaxValue = 255;
            this.sliderAlpha.Name = "sliderAlpha";
            this.sliderAlpha.ShowFocus = false;
            this.sliderAlpha.Size = new System.Drawing.Size(305, 21);
            colorLinearGradient6.ColorA = System.Drawing.Color.Black;
            colorLinearGradient6.ColorB = System.Drawing.Color.Black;
            this.sliderAlpha.SliderColorHigh = colorLinearGradient6;
            colorLinearGradient7.ColorA = System.Drawing.Color.Gray;
            colorLinearGradient7.ColorB = System.Drawing.Color.Gray;
            this.sliderAlpha.SliderColorLow = colorLinearGradient7;
            this.sliderAlpha.SliderHighlightPt = ((System.Drawing.PointF)(resources.GetObject("sliderAlpha.SliderHighlightPt")));
            this.sliderAlpha.SliderShape = gTrackBar.gTrackBar.eShape.Rectangle;
            this.sliderAlpha.SliderSize = new System.Drawing.Size(10, 20);
            this.sliderAlpha.SliderWidthHigh = 2F;
            this.sliderAlpha.SliderWidthLow = 2F;
            this.sliderAlpha.TabIndex = 22;
            this.sliderAlpha.TickColor = System.Drawing.Color.Black;
            this.sliderAlpha.TickInterval = 5;
            this.sliderAlpha.TickThickness = 1F;
            this.sliderAlpha.TickType = gTrackBar.gTrackBar.eTickType.Middle;
            this.sliderAlpha.TickWidth = 8;
            this.sliderAlpha.UpDownShow = false;
            this.sliderAlpha.UpDownWidth = 15;
            this.sliderAlpha.Value = 0;
            this.sliderAlpha.ValueAdjusted = 0F;
            this.sliderAlpha.ValueDivisor = gTrackBar.gTrackBar.eValueDivisor.e1;
            this.sliderAlpha.ValueStrFormat = null;
            this.sliderAlpha.ValueChanged += new gTrackBar.gTrackBar.ValueChangedEventHandler(this.sliderAlpha_ValueChanged);
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
            this.activeColorPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.activeColorPanel_Paint);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.savePaletteButton);
            this.groupBox1.Controls.Add(this.reloadPaletteButton);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.nudEntries);
            this.groupBox1.Controls.Add(this.paletteNameBox);
            this.groupBox1.Controls.Add(this.projectFileBox);
            this.groupBox1.Controls.Add(this.paletteOffsetBox);
            this.groupBox1.Controls.Add(this.colorFormatBox);
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
            // savePaletteButton
            // 
            this.savePaletteButton.Location = new System.Drawing.Point(297, 115);
            this.savePaletteButton.Name = "savePaletteButton";
            this.savePaletteButton.Size = new System.Drawing.Size(138, 23);
            this.savePaletteButton.TabIndex = 32;
            this.savePaletteButton.Text = "Save Palette To Source";
            this.savePaletteButton.UseVisualStyleBackColor = true;
            this.savePaletteButton.Click += new System.EventHandler(this.savePaletteButton_Click);
            // 
            // reloadPaletteButton
            // 
            this.reloadPaletteButton.Location = new System.Drawing.Point(9, 115);
            this.reloadPaletteButton.Name = "reloadPaletteButton";
            this.reloadPaletteButton.Size = new System.Drawing.Size(86, 23);
            this.reloadPaletteButton.TabIndex = 31;
            this.reloadPaletteButton.Text = "Reload Palette";
            this.reloadPaletteButton.UseVisualStyleBackColor = true;
            this.reloadPaletteButton.Click += new System.EventHandler(this.reloadPaletteButton_Click);
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
            // nudEntries
            // 
            this.nudEntries.Location = new System.Drawing.Point(255, 81);
            this.nudEntries.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.nudEntries.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudEntries.Name = "nudEntries";
            this.nudEntries.Size = new System.Drawing.Size(99, 20);
            this.nudEntries.TabIndex = 29;
            this.nudEntries.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudEntries.ValueChanged += new System.EventHandler(this.nudEntries_ValueChanged);
            // 
            // projectFileBox
            // 
            this.projectFileBox.Location = new System.Drawing.Point(50, 27);
            this.projectFileBox.Name = "projectFileBox";
            this.projectFileBox.ReadOnly = true;
            this.projectFileBox.Size = new System.Drawing.Size(304, 20);
            this.projectFileBox.TabIndex = 28;
            // 
            // paletteOffsetBox
            // 
            this.paletteOffsetBox.Location = new System.Drawing.Point(50, 81);
            this.paletteOffsetBox.Maximum = 2147483647;
            this.paletteOffsetBox.Minimum = 0;
            this.paletteOffsetBox.Name = "paletteOffsetBox";
            this.paletteOffsetBox.Size = new System.Drawing.Size(129, 20);
            this.paletteOffsetBox.TabIndex = 28;
            // 
            // colorFormatBox
            // 
            this.colorFormatBox.FormattingEnabled = true;
            this.colorFormatBox.Location = new System.Drawing.Point(255, 53);
            this.colorFormatBox.Name = "colorFormatBox";
            this.colorFormatBox.Size = new System.Drawing.Size(99, 21);
            this.colorFormatBox.TabIndex = 27;
            this.colorFormatBox.SelectedIndexChanged += new System.EventHandler(this.colorFormatBox_SelectedIndexChanged);
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
            this.groupBox2.Controls.Add(this.saveColorButton);
            this.groupBox2.Controls.Add(this.htmlBox);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.sliderAlpha);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.nudAlpha);
            this.groupBox2.Controls.Add(this.nudRed);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.nudGreen);
            this.groupBox2.Controls.Add(this.sliderGreen);
            this.groupBox2.Controls.Add(this.nudBlue);
            this.groupBox2.Controls.Add(this.sliderBlue);
            this.groupBox2.Controls.Add(this.sliderRed);
            this.groupBox2.Location = new System.Drawing.Point(12, 167);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(444, 171);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Color Editor";
            // 
            // saveColorButton
            // 
            this.saveColorButton.Location = new System.Drawing.Point(297, 139);
            this.saveColorButton.Name = "saveColorButton";
            this.saveColorButton.Size = new System.Drawing.Size(138, 23);
            this.saveColorButton.TabIndex = 25;
            this.saveColorButton.Text = "Save Color to Palette";
            this.saveColorButton.UseVisualStyleBackColor = true;
            this.saveColorButton.Click += new System.EventHandler(this.saveColorButton_Click);
            // 
            // htmlBox
            // 
            this.htmlBox.Location = new System.Drawing.Point(50, 135);
            this.htmlBox.MaxLength = 6;
            this.htmlBox.Name = "htmlBox";
            this.htmlBox.Size = new System.Drawing.Size(77, 20);
            this.htmlBox.TabIndex = 24;
            this.htmlBox.Text = "000000";
            this.htmlBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.htmlBox.TextChanged += new System.EventHandler(this.htmlBox_TextChanged);
            this.htmlBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.htmlBox_KeyDown);
            this.htmlBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.htmlBox_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 139);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 13);
            this.label9.TabIndex = 23;
            this.label9.Text = "HTML:";
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
            // swatchControl
            // 
            this.swatchControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.swatchControl.Location = new System.Drawing.Point(0, 348);
            this.swatchControl.Name = "swatchControl";
            this.swatchControl.SelectedIndex = 0;
            this.swatchControl.Size = new System.Drawing.Size(474, 310);
            this.swatchControl.TabIndex = 30;
            this.swatchControl.SelectedIndexChanged += new System.EventHandler<System.EventArgs>(this.swatchControl_SelectedIndexChanged);
            // 
            // PaletteEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 658);
            this.Controls.Add(this.swatchControl);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PaletteEditorForm";
            this.Text = "PaletteEditor";
            ((System.ComponentModel.ISupportInitialize)(this.nudRed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBlue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAlpha)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEntries)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox paletteNameBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudRed;
        private System.Windows.Forms.NumericUpDown nudGreen;
        private System.Windows.Forms.NumericUpDown nudBlue;
        private gTrackBar.gTrackBar sliderRed;
        private gTrackBar.gTrackBar sliderBlue;
        private gTrackBar.gTrackBar sliderGreen;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudAlpha;
        private gTrackBar.gTrackBar sliderAlpha;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel activeColorPanel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox colorFormatBox;
        private System.Windows.Forms.TextBox htmlBox;
        private System.Windows.Forms.Label label9;
        private IntegerTextBox paletteOffsetBox;
        private System.Windows.Forms.TextBox projectFileBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown nudEntries;
        private SwatchControl swatchControl;
        private System.Windows.Forms.Button savePaletteButton;
        private System.Windows.Forms.Button reloadPaletteButton;
        private System.Windows.Forms.Button saveColorButton;
        private System.Windows.Forms.ToolTip paletteTip;
    }
}