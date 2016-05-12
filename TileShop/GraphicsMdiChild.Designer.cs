namespace TileShop
{
    partial class GraphicsMdiChild
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
            this.formatSelectBox = new System.Windows.Forms.ToolStripComboBox();
            this.vertScroll = new System.Windows.Forms.VScrollBar();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.formatSelectBox});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(564, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // formatSelectBox
            // 
            this.formatSelectBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.formatSelectBox.Items.AddRange(new object[] {
            "NES 2bpp",
            "GB/SNES 2bpp",
            "NES 1bpp"});
            this.formatSelectBox.Name = "formatSelectBox";
            this.formatSelectBox.Size = new System.Drawing.Size(121, 25);
            this.formatSelectBox.SelectedIndexChanged += new System.EventHandler(this.formatSelectBox_SelectedIndexChanged);
            // 
            // vertScroll
            // 
            this.vertScroll.Dock = System.Windows.Forms.DockStyle.Right;
            this.vertScroll.Location = new System.Drawing.Point(547, 25);
            this.vertScroll.Name = "vertScroll";
            this.vertScroll.Size = new System.Drawing.Size(17, 432);
            this.vertScroll.TabIndex = 2;
            // 
            // GraphicsMdiChild
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 457);
            this.Controls.Add(this.vertScroll);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.Name = "GraphicsMdiChild";
            this.Text = "GraphicsMdiChild";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GraphicsMdiChild_Paint);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox formatSelectBox;
        private System.Windows.Forms.VScrollBar vertScroll;
    }
}