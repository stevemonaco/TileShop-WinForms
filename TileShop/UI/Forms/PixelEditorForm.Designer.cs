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
            this.components = new System.ComponentModel.Container();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.PaletteBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.PencilButton = new System.Windows.Forms.ToolStripButton();
            this.PickerButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.GridlinesButton = new System.Windows.Forms.ToolStripButton();
            this.ReloadButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.SaveButton = new System.Windows.Forms.ToolStripButton();
            this.PixelPanel = new System.Windows.Forms.Panel();
            this.SwatchControl = new TileShop.SwatchControl();
            this.pixelTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PaletteBox,
            this.toolStripSeparator1,
            this.PencilButton,
            this.PickerButton,
            this.toolStripSeparator2,
            this.GridlinesButton,
            this.ReloadButton,
            this.toolStripSeparator3,
            this.SaveButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(3);
            this.toolStrip1.Size = new System.Drawing.Size(533, 38);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // PaletteBox
            // 
            this.PaletteBox.Name = "PaletteBox";
            this.PaletteBox.Size = new System.Drawing.Size(121, 32);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 32);
            // 
            // PencilButton
            // 
            this.PencilButton.AutoSize = false;
            this.PencilButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.PencilButton.Image = global::TileShop.Properties.Resources.pencil;
            this.PencilButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.PencilButton.ImageTransparentColor = System.Drawing.Color.White;
            this.PencilButton.Margin = new System.Windows.Forms.Padding(0);
            this.PencilButton.Name = "PencilButton";
            this.PencilButton.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.PencilButton.Size = new System.Drawing.Size(36, 36);
            this.PencilButton.Click += new System.EventHandler(this.PencilButton_Click);
            // 
            // PickerButton
            // 
            this.PickerButton.AutoSize = false;
            this.PickerButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.PickerButton.Image = global::TileShop.Properties.Resources.color_picker;
            this.PickerButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.PickerButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PickerButton.Margin = new System.Windows.Forms.Padding(0);
            this.PickerButton.Name = "PickerButton";
            this.PickerButton.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.PickerButton.Size = new System.Drawing.Size(36, 36);
            this.PickerButton.Text = "toolStripButton3";
            this.PickerButton.Click += new System.EventHandler(this.PickerButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 32);
            // 
            // GridlinesButton
            // 
            this.GridlinesButton.AutoSize = false;
            this.GridlinesButton.CheckOnClick = true;
            this.GridlinesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.GridlinesButton.Image = global::TileShop.Properties.Resources.grid;
            this.GridlinesButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.GridlinesButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.GridlinesButton.Margin = new System.Windows.Forms.Padding(0);
            this.GridlinesButton.Name = "GridlinesButton";
            this.GridlinesButton.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.GridlinesButton.Size = new System.Drawing.Size(36, 36);
            this.GridlinesButton.Text = "toolStripButton3";
            this.GridlinesButton.Click += new System.EventHandler(this.GridlinesButton_Click);
            // 
            // ReloadButton
            // 
            this.ReloadButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ReloadButton.AutoSize = false;
            this.ReloadButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ReloadButton.Image = global::TileShop.Properties.Resources.arrow_refresh_small;
            this.ReloadButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ReloadButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ReloadButton.Margin = new System.Windows.Forms.Padding(0);
            this.ReloadButton.Name = "ReloadButton";
            this.ReloadButton.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.ReloadButton.Size = new System.Drawing.Size(36, 36);
            this.ReloadButton.Text = "Reload";
            this.ReloadButton.Click += new System.EventHandler(this.ReloadButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 32);
            // 
            // SaveButton
            // 
            this.SaveButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.SaveButton.AutoSize = false;
            this.SaveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveButton.Image = global::TileShop.Properties.Resources.disk;
            this.SaveButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.SaveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveButton.Margin = new System.Windows.Forms.Padding(0);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.SaveButton.Size = new System.Drawing.Size(36, 36);
            this.SaveButton.Text = "Save";
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // PixelPanel
            // 
            this.PixelPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.PixelPanel.Location = new System.Drawing.Point(0, 38);
            this.PixelPanel.Name = "PixelPanel";
            this.PixelPanel.Size = new System.Drawing.Size(533, 201);
            this.PixelPanel.TabIndex = 1;
            this.PixelPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.PixelPanel_Paint);
            this.PixelPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PixelPanel_MouseDown);
            this.PixelPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PixelPanel_MouseMove);
            this.PixelPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PixelPanel_MouseUp);
            // 
            // SwatchControl
            // 
            this.SwatchControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SwatchControl.Location = new System.Drawing.Point(0, 239);
            this.SwatchControl.Name = "SwatchControl";
            this.SwatchControl.SelectedIndex = 0;
            this.SwatchControl.Size = new System.Drawing.Size(533, 237);
            this.SwatchControl.TabIndex = 2;
            // 
            // PixelEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 476);
            this.Controls.Add(this.SwatchControl);
            this.Controls.Add(this.PixelPanel);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PixelEditorForm";
            this.Text = "Pixel Editor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PixelEditorForm_FormClosed);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox PaletteBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Panel PixelPanel;
        private SwatchControl SwatchControl;
        private System.Windows.Forms.ToolStripButton PencilButton;
        private System.Windows.Forms.ToolStripButton PickerButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton GridlinesButton;
        private System.Windows.Forms.ToolStripButton SaveButton;
        private System.Windows.Forms.ToolStripButton ReloadButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolTip pixelTip;
    }
}