namespace TileShop
{
    partial class PixelEditorForm
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.paletteBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.pencilButton = new System.Windows.Forms.ToolStripButton();
            this.pickerButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.gridlinesButton = new System.Windows.Forms.ToolStripButton();
            this.PixelPanel = new System.Windows.Forms.Panel();
            this.swatchControl = new TileShop.SwatchControl();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.paletteBox,
            this.toolStripSeparator1,
            this.pencilButton,
            this.pickerButton,
            this.toolStripSeparator2,
            this.gridlinesButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(533, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // paletteBox
            // 
            this.paletteBox.Name = "paletteBox";
            this.paletteBox.Size = new System.Drawing.Size(121, 25);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // pencilButton
            // 
            this.pencilButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pencilButton.Image = global::TileShop.Properties.Resources.PencilTool;
            this.pencilButton.ImageTransparentColor = System.Drawing.Color.White;
            this.pencilButton.Name = "pencilButton";
            this.pencilButton.Size = new System.Drawing.Size(23, 22);
            this.pencilButton.Click += new System.EventHandler(this.pencilButton_Click);
            // 
            // pickerButton
            // 
            this.pickerButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pickerButton.Image = global::TileShop.Properties.Resources.ColorPickerTool;
            this.pickerButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pickerButton.Name = "pickerButton";
            this.pickerButton.Size = new System.Drawing.Size(23, 22);
            this.pickerButton.Text = "toolStripButton3";
            this.pickerButton.Click += new System.EventHandler(this.pickerButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // gridlinesButton
            // 
            this.gridlinesButton.CheckOnClick = true;
            this.gridlinesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.gridlinesButton.Image = global::TileShop.Properties.Resources.gridlines;
            this.gridlinesButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.gridlinesButton.Name = "gridlinesButton";
            this.gridlinesButton.Size = new System.Drawing.Size(23, 22);
            this.gridlinesButton.Text = "toolStripButton3";
            this.gridlinesButton.Click += new System.EventHandler(this.gridlinesButton_Click);
            // 
            // PixelPanel
            // 
            this.PixelPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.PixelPanel.Location = new System.Drawing.Point(0, 25);
            this.PixelPanel.Name = "PixelPanel";
            this.PixelPanel.Size = new System.Drawing.Size(533, 201);
            this.PixelPanel.TabIndex = 1;
            this.PixelPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.PixelPanel_Paint);
            this.PixelPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PixelPanel_MouseDown);
            this.PixelPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PixelPanel_MouseMove);
            // 
            // swatchControl
            // 
            this.swatchControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.swatchControl.Location = new System.Drawing.Point(0, 226);
            this.swatchControl.Name = "swatchControl";
            this.swatchControl.SelectedIndex = 0;
            this.swatchControl.Size = new System.Drawing.Size(533, 250);
            this.swatchControl.TabIndex = 2;
            // 
            // PixelEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 476);
            this.Controls.Add(this.swatchControl);
            this.Controls.Add(this.PixelPanel);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PixelEditorForm";
            this.Text = "Pixel Editor";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox paletteBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Panel PixelPanel;
        private SwatchControl swatchControl;
        private System.Windows.Forms.ToolStripButton pencilButton;
        private System.Windows.Forms.ToolStripButton pickerButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton gridlinesButton;
    }
}