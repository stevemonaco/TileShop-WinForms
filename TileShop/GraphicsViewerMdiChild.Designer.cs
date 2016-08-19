namespace TileShop
{
    partial class GraphicsViewerMdiChild
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphicsViewerMdiChild));
            this.vertScroll = new System.Windows.Forms.VScrollBar();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.formatSelectBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.zoomSelectBox = new System.Windows.Forms.ToolStripComboBox();
            this.showGridlinesButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // vertScroll
            // 
            this.vertScroll.Dock = System.Windows.Forms.DockStyle.Right;
            this.vertScroll.Location = new System.Drawing.Point(547, 0);
            this.vertScroll.Name = "vertScroll";
            this.vertScroll.Size = new System.Drawing.Size(17, 457);
            this.vertScroll.TabIndex = 2;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.formatSelectBox,
            this.toolStripSeparator1,
            this.zoomSelectBox,
            this.showGridlinesButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(547, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // formatSelectBox
            // 
            this.formatSelectBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.formatSelectBox.Name = "formatSelectBox";
            this.formatSelectBox.Size = new System.Drawing.Size(121, 25);
            this.formatSelectBox.SelectedIndexChanged += new System.EventHandler(this.formatSelectBox_SelectedIndexChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // zoomSelectBox
            // 
            this.zoomSelectBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.zoomSelectBox.Items.AddRange(new object[] {
            "Zoom 100%",
            "Zoom 200%",
            "Zoom 300%",
            "Zoom 400%",
            "Zoom 500%",
            "Zoom 600%",
            "Zoom 700%",
            "Zoom 800%"});
            this.zoomSelectBox.Name = "zoomSelectBox";
            this.zoomSelectBox.Size = new System.Drawing.Size(121, 25);
            this.zoomSelectBox.SelectedIndexChanged += new System.EventHandler(this.zoomSelectBox_SelectedIndexChanged);
            // 
            // showGridlinesButton
            // 
            this.showGridlinesButton.Checked = true;
            this.showGridlinesButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showGridlinesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.showGridlinesButton.Image = ((System.Drawing.Image)(resources.GetObject("showGridlinesButton.Image")));
            this.showGridlinesButton.ImageTransparentColor = System.Drawing.Color.White;
            this.showGridlinesButton.Name = "showGridlinesButton";
            this.showGridlinesButton.Size = new System.Drawing.Size(23, 22);
            this.showGridlinesButton.Text = "Show Gridlines";
            this.showGridlinesButton.ToolTipText = "Show Gridlines";
            this.showGridlinesButton.Click += new System.EventHandler(this.showGridlinesButton_Click);
            // 
            // GraphicsViewerMdiChild
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(564, 457);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.vertScroll);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "GraphicsViewerMdiChild";
            this.Text = "GraphicsMdiChild";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GraphicsViewerMdiChild_FormClosing);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GraphicsMdiChild_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GraphicsViewerMdiChild_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GraphicsMdiChild_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GraphicsViewerMdiChild_MouseUp);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.VScrollBar vertScroll;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox formatSelectBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox zoomSelectBox;
        private System.Windows.Forms.ToolStripButton showGridlinesButton;
    }
}