namespace TileShop
{
    partial class TileShopForm
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.SaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.SaveProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveProjectAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewPaletteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewScatteredArrangerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DebugXmlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainToolStrip = new System.Windows.Forms.ToolStrip();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.SelectionLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.FileOffsetLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.dockTheme = new WeifenLuo.WinFormsUI.Docking.VS2013BlueTheme();
            this.DockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.addToolStripMenuItem,
            this.pluginsToolStripMenuItem,
            this.DebugXmlToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip.Size = new System.Drawing.Size(963, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewToolStripMenuItem,
            this.OpenToolStripMenuItem,
            this.CloseProjectToolStripMenuItem,
            this.toolStripMenuItem1,
            this.SaveToolStripMenuItem,
            this.SaveAsToolStripMenuItem,
            this.toolStripMenuItem2,
            this.SaveProjectToolStripMenuItem,
            this.SaveProjectAsToolStripMenuItem,
            this.toolStripMenuItem3,
            this.ExitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // NewToolStripMenuItem
            // 
            this.NewToolStripMenuItem.Name = "NewToolStripMenuItem";
            this.NewToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.NewToolStripMenuItem.Text = "New";
            // 
            // OpenToolStripMenuItem
            // 
            this.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem";
            this.OpenToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.OpenToolStripMenuItem.Text = "Open";
            this.OpenToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // CloseProjectToolStripMenuItem
            // 
            this.CloseProjectToolStripMenuItem.Name = "CloseProjectToolStripMenuItem";
            this.CloseProjectToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.CloseProjectToolStripMenuItem.Text = "Close Project";
            this.CloseProjectToolStripMenuItem.Click += new System.EventHandler(this.CloseProjectToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(160, 6);
            // 
            // SaveToolStripMenuItem
            // 
            this.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
            this.SaveToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.SaveToolStripMenuItem.Text = "&Save";
            // 
            // SaveAsToolStripMenuItem
            // 
            this.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem";
            this.SaveAsToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.SaveAsToolStripMenuItem.Text = "Save &As...";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(160, 6);
            // 
            // SaveProjectToolStripMenuItem
            // 
            this.SaveProjectToolStripMenuItem.Name = "SaveProjectToolStripMenuItem";
            this.SaveProjectToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.SaveProjectToolStripMenuItem.Text = "Save Project";
            this.SaveProjectToolStripMenuItem.Click += new System.EventHandler(this.SaveProjectToolStripMenuItem_Click);
            // 
            // SaveProjectAsToolStripMenuItem
            // 
            this.SaveProjectAsToolStripMenuItem.Name = "SaveProjectAsToolStripMenuItem";
            this.SaveProjectAsToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.SaveProjectAsToolStripMenuItem.Text = "Save Project As...";
            this.SaveProjectAsToolStripMenuItem.Click += new System.EventHandler(this.SaveProjectAsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(160, 6);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.ExitToolStripMenuItem.Text = "E&xit";
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewPaletteToolStripMenuItem,
            this.NewScatteredArrangerToolStripMenuItem});
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.addToolStripMenuItem.Text = "&Add";
            // 
            // NewPaletteToolStripMenuItem
            // 
            this.NewPaletteToolStripMenuItem.Name = "NewPaletteToolStripMenuItem";
            this.NewPaletteToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.NewPaletteToolStripMenuItem.Text = "New Palette...";
            this.NewPaletteToolStripMenuItem.Click += new System.EventHandler(this.NewPaletteToolStripMenuItem_Click);
            // 
            // NewScatteredArrangerToolStripMenuItem
            // 
            this.NewScatteredArrangerToolStripMenuItem.Name = "NewScatteredArrangerToolStripMenuItem";
            this.NewScatteredArrangerToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.NewScatteredArrangerToolStripMenuItem.Text = "New Scattered Arranger...";
            this.NewScatteredArrangerToolStripMenuItem.Click += new System.EventHandler(this.NewScatteredArrangerToolStripMenuItem_Click);
            // 
            // pluginsToolStripMenuItem
            // 
            this.pluginsToolStripMenuItem.Name = "pluginsToolStripMenuItem";
            this.pluginsToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.pluginsToolStripMenuItem.Text = "&Plugins";
            // 
            // DebugXmlToolStripMenuItem
            // 
            this.DebugXmlToolStripMenuItem.Name = "DebugXmlToolStripMenuItem";
            this.DebugXmlToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.DebugXmlToolStripMenuItem.Text = "Debug Xml";
            this.DebugXmlToolStripMenuItem.Click += new System.EventHandler(this.DebugXmlToolStripMenuItem_Click);
            // 
            // MainToolStrip
            // 
            this.MainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.MainToolStrip.Location = new System.Drawing.Point(0, 24);
            this.MainToolStrip.Name = "MainToolStrip";
            this.MainToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.MainToolStrip.Size = new System.Drawing.Size(963, 25);
            this.MainToolStrip.TabIndex = 2;
            this.MainToolStrip.Text = "toolStrip1";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelectionLabel,
            this.FileOffsetLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 603);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(963, 22);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip1";
            // 
            // SelectionLabel
            // 
            this.SelectionLabel.BackColor = System.Drawing.SystemColors.Control;
            this.SelectionLabel.Name = "SelectionLabel";
            this.SelectionLabel.Size = new System.Drawing.Size(22, 17);
            this.SelectionLabel.Text = "     ";
            // 
            // FileOffsetLabel
            // 
            this.FileOffsetLabel.BackColor = System.Drawing.SystemColors.Control;
            this.FileOffsetLabel.Name = "FileOffsetLabel";
            this.FileOffsetLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            this.FileOffsetLabel.Size = new System.Drawing.Size(926, 17);
            this.FileOffsetLabel.Spring = true;
            this.FileOffsetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // DockPanel
            // 
            this.DockPanel.BackColor = System.Drawing.SystemColors.Control;
            this.DockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DockPanel.Location = new System.Drawing.Point(0, 49);
            this.DockPanel.Name = "DockPanel";
            this.DockPanel.Size = new System.Drawing.Size(963, 554);
            this.DockPanel.TabIndex = 4;
            this.DockPanel.Theme = this.dockTheme;
            // 
            // TileShopForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(963, 625);
            this.Controls.Add(this.DockPanel);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.MainToolStrip);
            this.Controls.Add(this.menuStrip);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "TileShopForm";
            this.Text = "TileShop";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStrip MainToolStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel FileOffsetLabel;
        private System.Windows.Forms.ToolStripMenuItem NewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem SaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel SelectionLabel;
        private WeifenLuo.WinFormsUI.Docking.VS2013BlueTheme dockTheme;
        private System.Windows.Forms.ToolStripMenuItem DebugXmlToolStripMenuItem;
        public WeifenLuo.WinFormsUI.Docking.DockPanel DockPanel;
        private System.Windows.Forms.ToolStripMenuItem SaveProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveProjectAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NewPaletteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NewScatteredArrangerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CloseProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pluginsToolStripMenuItem;
    }
}

