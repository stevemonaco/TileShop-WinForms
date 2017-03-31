namespace TileShop
{
    partial class GraphicsViewerChild
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphicsViewerChild));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.formatSelectBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.zoomSelectBox = new System.Windows.Forms.ToolStripComboBox();
            this.showGridlinesButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.offsetLabel = new System.Windows.Forms.ToolStripLabel();
            this.editModeButton = new System.Windows.Forms.ToolStripButton();
            this.RenderPanel = new System.Windows.Forms.Panel();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.formatSelectBox,
            this.toolStripSeparator1,
            this.zoomSelectBox,
            this.showGridlinesButton,
            this.toolStripSeparator2,
            this.offsetLabel,
            this.editModeButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(564, 25);
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
            this.showGridlinesButton.Image = global::TileShop.Properties.Resources.gridlines;
            this.showGridlinesButton.ImageTransparentColor = System.Drawing.Color.White;
            this.showGridlinesButton.Name = "showGridlinesButton";
            this.showGridlinesButton.Size = new System.Drawing.Size(23, 22);
            this.showGridlinesButton.Text = "Show Gridlines";
            this.showGridlinesButton.ToolTipText = "Show Gridlines";
            this.showGridlinesButton.Click += new System.EventHandler(this.showGridlinesButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // offsetLabel
            // 
            this.offsetLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.offsetLabel.Name = "offsetLabel";
            this.offsetLabel.Size = new System.Drawing.Size(0, 22);
            // 
            // editModeButton
            // 
            this.editModeButton.Checked = true;
            this.editModeButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.editModeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.editModeButton.Image = global::TileShop.Properties.Resources.EditModeTemp;
            this.editModeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editModeButton.Name = "editModeButton";
            this.editModeButton.Size = new System.Drawing.Size(23, 22);
            this.editModeButton.Text = "toolStripButton1";
            this.editModeButton.Click += new System.EventHandler(this.editModeButton_Click);
            // 
            // RenderPanel
            // 
            this.RenderPanel.AllowDrop = true;
            this.RenderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RenderPanel.Location = new System.Drawing.Point(0, 25);
            this.RenderPanel.Name = "RenderPanel";
            this.RenderPanel.Size = new System.Drawing.Size(564, 432);
            this.RenderPanel.TabIndex = 5;
            this.RenderPanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.RenderPanel_DragDrop);
            this.RenderPanel.DragEnter += new System.Windows.Forms.DragEventHandler(this.RenderPanel_DragEnter);
            this.RenderPanel.DragOver += new System.Windows.Forms.DragEventHandler(this.RenderPanel_DragOver);
            this.RenderPanel.DragLeave += new System.EventHandler(this.RenderPanel_DragLeave);
            this.RenderPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.RenderPanel_Paint);
            this.RenderPanel.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.RenderPanel_QueryContinueDrag);
            this.RenderPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RenderPanel_MouseDown);
            this.RenderPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RenderPanel_MouseMove);
            this.RenderPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.RenderPanel_MouseUp);
            // 
            // GraphicsViewerMdiChild
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(564, 457);
            this.Controls.Add(this.RenderPanel);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "GraphicsViewerMdiChild";
            this.Text = "GraphicsMdiChild";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GraphicsViewerMdiChild_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox formatSelectBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox zoomSelectBox;
        private System.Windows.Forms.ToolStripButton showGridlinesButton;
        private System.Windows.Forms.Panel RenderPanel;
        private System.Windows.Forms.ToolStripLabel offsetLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton editModeButton;
    }
}