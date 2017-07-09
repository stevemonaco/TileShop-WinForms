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
            this.FormatSelectBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ShowGridlinesButton = new System.Windows.Forms.ToolStripButton();
            this.offsetLabel = new System.Windows.Forms.ToolStripLabel();
            this.EditModeButton = new System.Windows.Forms.ToolStripButton();
            this.ReloadButton = new System.Windows.Forms.ToolStripButton();
            this.SaveLoadSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.SaveButton = new System.Windows.Forms.ToolStripButton();
            this.JumpButton = new System.Windows.Forms.ToolStripButton();
            this.RenderPanel = new System.Windows.Forms.Panel();
            this.ArrangerPropertiesButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FormatSelectBox,
            this.toolStripSeparator1,
            this.ShowGridlinesButton,
            this.offsetLabel,
            this.EditModeButton,
            this.ReloadButton,
            this.SaveLoadSeparator,
            this.SaveButton,
            this.JumpButton,
            this.ArrangerPropertiesButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(564, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // FormatSelectBox
            // 
            this.FormatSelectBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FormatSelectBox.Name = "FormatSelectBox";
            this.FormatSelectBox.Size = new System.Drawing.Size(121, 25);
            this.FormatSelectBox.DropDownClosed += new System.EventHandler(this.FormatSelectBox_DropDownClosed);
            this.FormatSelectBox.SelectedIndexChanged += new System.EventHandler(this.FormatSelectBox_SelectedIndexChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // ShowGridlinesButton
            // 
            this.ShowGridlinesButton.Checked = true;
            this.ShowGridlinesButton.CheckOnClick = true;
            this.ShowGridlinesButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowGridlinesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ShowGridlinesButton.Image = global::TileShop.Properties.Resources.gridlines;
            this.ShowGridlinesButton.ImageTransparentColor = System.Drawing.Color.White;
            this.ShowGridlinesButton.Name = "ShowGridlinesButton";
            this.ShowGridlinesButton.Size = new System.Drawing.Size(23, 22);
            this.ShowGridlinesButton.Text = "Show Gridlines";
            this.ShowGridlinesButton.ToolTipText = "Show Gridlines";
            this.ShowGridlinesButton.Click += new System.EventHandler(this.ShowGridlinesButton_Click);
            // 
            // offsetLabel
            // 
            this.offsetLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.offsetLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.offsetLabel.Name = "offsetLabel";
            this.offsetLabel.Size = new System.Drawing.Size(0, 22);
            // 
            // EditModeButton
            // 
            this.EditModeButton.Checked = true;
            this.EditModeButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EditModeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.EditModeButton.Image = global::TileShop.Properties.Resources.EditModeTemp;
            this.EditModeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.EditModeButton.Name = "EditModeButton";
            this.EditModeButton.Size = new System.Drawing.Size(23, 22);
            this.EditModeButton.Text = "Enable/Disable Edit Mode";
            this.EditModeButton.ToolTipText = "Enable/Disable Edit Mode";
            this.EditModeButton.Click += new System.EventHandler(this.EditModeButton_Click);
            // 
            // ReloadButton
            // 
            this.ReloadButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ReloadButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ReloadButton.Image = ((System.Drawing.Image)(resources.GetObject("ReloadButton.Image")));
            this.ReloadButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ReloadButton.Name = "ReloadButton";
            this.ReloadButton.Size = new System.Drawing.Size(47, 22);
            this.ReloadButton.Text = "Reload";
            this.ReloadButton.ToolTipText = "Reload arranger from underlying source";
            this.ReloadButton.Click += new System.EventHandler(this.ReloadButton_Click);
            // 
            // SaveLoadSeparator
            // 
            this.SaveLoadSeparator.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.SaveLoadSeparator.Name = "SaveLoadSeparator";
            this.SaveLoadSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // SaveButton
            // 
            this.SaveButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.SaveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SaveButton.Image = ((System.Drawing.Image)(resources.GetObject("SaveButton.Image")));
            this.SaveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(35, 22);
            this.SaveButton.Text = "Save";
            this.SaveButton.ToolTipText = "Save arranger to underlying source";
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // JumpButton
            // 
            this.JumpButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.JumpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.JumpButton.Image = ((System.Drawing.Image)(resources.GetObject("JumpButton.Image")));
            this.JumpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.JumpButton.Name = "JumpButton";
            this.JumpButton.Size = new System.Drawing.Size(40, 22);
            this.JumpButton.Text = "Jump";
            this.JumpButton.Click += new System.EventHandler(this.JumpButton_Click);
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
            // ArrangerPropertiesButton
            // 
            this.ArrangerPropertiesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ArrangerPropertiesButton.Image = ((System.Drawing.Image)(resources.GetObject("ArrangerPropertiesButton.Image")));
            this.ArrangerPropertiesButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ArrangerPropertiesButton.Name = "ArrangerPropertiesButton";
            this.ArrangerPropertiesButton.Size = new System.Drawing.Size(64, 22);
            this.ArrangerPropertiesButton.Text = "Properties";
            this.ArrangerPropertiesButton.ToolTipText = "Arranger Properties";
            this.ArrangerPropertiesButton.Click += new System.EventHandler(this.ArrangerPropertiesButton_Click);
            // 
            // GraphicsViewerChild
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
            this.Name = "GraphicsViewerChild";
            this.Text = "GraphicsMdiChild";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox FormatSelectBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton ShowGridlinesButton;
        private System.Windows.Forms.Panel RenderPanel;
        private System.Windows.Forms.ToolStripLabel offsetLabel;
        private System.Windows.Forms.ToolStripButton EditModeButton;
        private System.Windows.Forms.ToolStripButton SaveButton;
        private System.Windows.Forms.ToolStripButton ReloadButton;
        private System.Windows.Forms.ToolStripSeparator SaveLoadSeparator;
        private System.Windows.Forms.ToolStripButton JumpButton;
        private System.Windows.Forms.ToolStripButton ArrangerPropertiesButton;
    }
}