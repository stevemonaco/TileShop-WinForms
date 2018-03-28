namespace TileShop
{
    partial class ArrangerViewerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArrangerViewerForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.FormatSelectBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.offsetLabel = new System.Windows.Forms.ToolStripLabel();
            this.ArrangeModeButton = new System.Windows.Forms.ToolStripButton();
            this.ReloadButton = new System.Windows.Forms.ToolStripButton();
            this.SaveLoadSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.SaveButton = new System.Windows.Forms.ToolStripButton();
            this.JumpButton = new System.Windows.Forms.ToolStripButton();
            this.EditModeButton = new System.Windows.Forms.ToolStripButton();
            this.ArrangerPropertiesButton = new System.Windows.Forms.ToolStripButton();
            this.PaletteDropDownButton = new TileShop.Controls.CheckedDropDownButton();
            this.ShowGridlinesButton = new System.Windows.Forms.ToolStripButton();
            this.RenderPanel = new System.Windows.Forms.Panel();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FormatSelectBox,
            this.toolStripSeparator1,
            this.offsetLabel,
            this.ArrangeModeButton,
            this.ReloadButton,
            this.SaveLoadSeparator,
            this.SaveButton,
            this.JumpButton,
            this.EditModeButton,
            this.ArrangerPropertiesButton,
            this.PaletteDropDownButton,
            this.ShowGridlinesButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(3);
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(564, 40);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // FormatSelectBox
            // 
            this.FormatSelectBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FormatSelectBox.Name = "FormatSelectBox";
            this.FormatSelectBox.Size = new System.Drawing.Size(121, 34);
            this.FormatSelectBox.DropDownClosed += new System.EventHandler(this.FormatSelectBox_DropDownClosed);
            this.FormatSelectBox.SelectedIndexChanged += new System.EventHandler(this.FormatSelectBox_SelectedIndexChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 34);
            // 
            // offsetLabel
            // 
            this.offsetLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.offsetLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.offsetLabel.Name = "offsetLabel";
            this.offsetLabel.Size = new System.Drawing.Size(0, 31);
            // 
            // ArrangeModeButton
            // 
            this.ArrangeModeButton.AutoSize = false;
            this.ArrangeModeButton.BackColor = System.Drawing.SystemColors.Control;
            this.ArrangeModeButton.Checked = true;
            this.ArrangeModeButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ArrangeModeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ArrangeModeButton.Image = global::TileShop.Properties.Resources.cursor;
            this.ArrangeModeButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ArrangeModeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ArrangeModeButton.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.ArrangeModeButton.Name = "ArrangeModeButton";
            this.ArrangeModeButton.Size = new System.Drawing.Size(36, 36);
            this.ArrangeModeButton.Text = "Arrange Elements Mode";
            this.ArrangeModeButton.ToolTipText = "Select Elements to Arrange";
            this.ArrangeModeButton.Click += new System.EventHandler(this.ArrangeModeButton_Click);
            // 
            // ReloadButton
            // 
            this.ReloadButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ReloadButton.AutoSize = false;
            this.ReloadButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ReloadButton.Image = global::TileShop.Properties.Resources.arrow_refresh_small;
            this.ReloadButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ReloadButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ReloadButton.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.ReloadButton.Name = "ReloadButton";
            this.ReloadButton.Size = new System.Drawing.Size(36, 36);
            this.ReloadButton.Text = "Reload";
            this.ReloadButton.ToolTipText = "Reload arranger from underlying source";
            this.ReloadButton.Click += new System.EventHandler(this.ReloadButton_Click);
            // 
            // SaveLoadSeparator
            // 
            this.SaveLoadSeparator.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.SaveLoadSeparator.Name = "SaveLoadSeparator";
            this.SaveLoadSeparator.Size = new System.Drawing.Size(6, 34);
            // 
            // SaveButton
            // 
            this.SaveButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.SaveButton.AutoSize = false;
            this.SaveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveButton.Image = global::TileShop.Properties.Resources.disk;
            this.SaveButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.SaveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveButton.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(36, 36);
            this.SaveButton.Text = "Save";
            this.SaveButton.ToolTipText = "Save arranger to underlying source";
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // JumpButton
            // 
            this.JumpButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.JumpButton.AutoSize = false;
            this.JumpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.JumpButton.Image = global::TileShop.Properties.Resources.document_redirect;
            this.JumpButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.JumpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.JumpButton.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.JumpButton.Name = "JumpButton";
            this.JumpButton.Size = new System.Drawing.Size(36, 36);
            this.JumpButton.Text = "Jump";
            this.JumpButton.ToolTipText = "Jump to File Offset";
            this.JumpButton.Click += new System.EventHandler(this.JumpButton_Click);
            // 
            // EditModeButton
            // 
            this.EditModeButton.AutoSize = false;
            this.EditModeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.EditModeButton.Image = global::TileShop.Properties.Resources.pencil;
            this.EditModeButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.EditModeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.EditModeButton.Margin = new System.Windows.Forms.Padding(0);
            this.EditModeButton.Name = "EditModeButton";
            this.EditModeButton.Size = new System.Drawing.Size(36, 36);
            this.EditModeButton.Text = "Edit Elements";
            this.EditModeButton.ToolTipText = "Select Elements to Edit";
            this.EditModeButton.Click += new System.EventHandler(this.EditModeButton_Click);
            // 
            // ArrangerPropertiesButton
            // 
            this.ArrangerPropertiesButton.AutoSize = false;
            this.ArrangerPropertiesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ArrangerPropertiesButton.Image = global::TileShop.Properties.Resources.application_form_edit;
            this.ArrangerPropertiesButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ArrangerPropertiesButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ArrangerPropertiesButton.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.ArrangerPropertiesButton.Name = "ArrangerPropertiesButton";
            this.ArrangerPropertiesButton.Size = new System.Drawing.Size(36, 36);
            this.ArrangerPropertiesButton.Text = "Properties";
            this.ArrangerPropertiesButton.ToolTipText = "Arranger Properties";
            this.ArrangerPropertiesButton.Click += new System.EventHandler(this.ArrangerPropertiesButton_Click);
            // 
            // PaletteDropDownButton
            // 
            this.PaletteDropDownButton.AutoSize = false;
            this.PaletteDropDownButton.Checked = false;
            this.PaletteDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.PaletteDropDownButton.Image = global::TileShop.Properties.Resources.palette;
            this.PaletteDropDownButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.PaletteDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PaletteDropDownButton.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.PaletteDropDownButton.Name = "PaletteDropDownButton";
            this.PaletteDropDownButton.Size = new System.Drawing.Size(46, 36);
            this.PaletteDropDownButton.Text = "Choose Palette to Apply";
            this.PaletteDropDownButton.ToolTipText = "Choose Palette to Apply";
            // 
            // ShowGridlinesButton
            // 
            this.ShowGridlinesButton.AutoSize = false;
            this.ShowGridlinesButton.Checked = true;
            this.ShowGridlinesButton.CheckOnClick = true;
            this.ShowGridlinesButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowGridlinesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ShowGridlinesButton.Image = global::TileShop.Properties.Resources.grid;
            this.ShowGridlinesButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ShowGridlinesButton.ImageTransparentColor = System.Drawing.Color.White;
            this.ShowGridlinesButton.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.ShowGridlinesButton.Name = "ShowGridlinesButton";
            this.ShowGridlinesButton.Size = new System.Drawing.Size(36, 36);
            this.ShowGridlinesButton.Text = "Show Gridlines";
            this.ShowGridlinesButton.ToolTipText = "Show Gridlines";
            this.ShowGridlinesButton.Click += new System.EventHandler(this.ShowGridlinesButton_Click);
            // 
            // RenderPanel
            // 
            this.RenderPanel.AllowDrop = true;
            this.RenderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RenderPanel.Location = new System.Drawing.Point(0, 40);
            this.RenderPanel.Name = "RenderPanel";
            this.RenderPanel.Size = new System.Drawing.Size(564, 417);
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
            // ArrangerViewerForm
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
            this.Name = "ArrangerViewerForm";
            this.Text = "GraphicsViewerForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ArrangerViewerForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ArrangerViewerForm_FormClosed);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox FormatSelectBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton ShowGridlinesButton;
        private System.Windows.Forms.Panel RenderPanel;
        private System.Windows.Forms.ToolStripLabel offsetLabel;
        private System.Windows.Forms.ToolStripButton ArrangeModeButton;
        private System.Windows.Forms.ToolStripButton SaveButton;
        private System.Windows.Forms.ToolStripButton ReloadButton;
        private System.Windows.Forms.ToolStripSeparator SaveLoadSeparator;
        private System.Windows.Forms.ToolStripButton JumpButton;
        private System.Windows.Forms.ToolStripButton ArrangerPropertiesButton;
        private System.Windows.Forms.ToolStripButton EditModeButton;
        private Controls.CheckedDropDownButton PaletteDropDownButton;
    }
}