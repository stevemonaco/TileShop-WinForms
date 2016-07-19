namespace TileShop
{
    partial class GameDescriptorControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.filesTabPage = new System.Windows.Forms.TabPage();
            this.filesTreeView = new System.Windows.Forms.TreeView();
            this.arrangersTabPage = new System.Windows.Forms.TabPage();
            this.arrangersTreeView = new System.Windows.Forms.TreeView();
            this.palettesTabPage = new System.Windows.Forms.TabPage();
            this.palettesTreeView = new System.Windows.Forms.TreeView();
            this.tabControl1.SuspendLayout();
            this.filesTabPage.SuspendLayout();
            this.arrangersTabPage.SuspendLayout();
            this.palettesTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Controls.Add(this.filesTabPage);
            this.tabControl1.Controls.Add(this.arrangersTabPage);
            this.tabControl1.Controls.Add(this.palettesTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(269, 373);
            this.tabControl1.TabIndex = 0;
            // 
            // filesTabPage
            // 
            this.filesTabPage.Controls.Add(this.filesTreeView);
            this.filesTabPage.Location = new System.Drawing.Point(4, 4);
            this.filesTabPage.Margin = new System.Windows.Forms.Padding(0);
            this.filesTabPage.Name = "filesTabPage";
            this.filesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.filesTabPage.Size = new System.Drawing.Size(261, 347);
            this.filesTabPage.TabIndex = 1;
            this.filesTabPage.Text = "Files";
            this.filesTabPage.UseVisualStyleBackColor = true;
            // 
            // filesTreeView
            // 
            this.filesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filesTreeView.Location = new System.Drawing.Point(3, 3);
            this.filesTreeView.Margin = new System.Windows.Forms.Padding(0);
            this.filesTreeView.Name = "filesTreeView";
            this.filesTreeView.Size = new System.Drawing.Size(255, 341);
            this.filesTreeView.TabIndex = 0;
            // 
            // arrangersTabPage
            // 
            this.arrangersTabPage.Controls.Add(this.arrangersTreeView);
            this.arrangersTabPage.Location = new System.Drawing.Point(4, 4);
            this.arrangersTabPage.Name = "arrangersTabPage";
            this.arrangersTabPage.Size = new System.Drawing.Size(261, 347);
            this.arrangersTabPage.TabIndex = 2;
            this.arrangersTabPage.Text = "Arrangers";
            this.arrangersTabPage.UseVisualStyleBackColor = true;
            // 
            // arrangersTreeView
            // 
            this.arrangersTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.arrangersTreeView.Location = new System.Drawing.Point(0, 0);
            this.arrangersTreeView.Name = "arrangersTreeView";
            this.arrangersTreeView.Size = new System.Drawing.Size(261, 347);
            this.arrangersTreeView.TabIndex = 0;
            // 
            // palettesTabPage
            // 
            this.palettesTabPage.Controls.Add(this.palettesTreeView);
            this.palettesTabPage.Location = new System.Drawing.Point(4, 4);
            this.palettesTabPage.Name = "palettesTabPage";
            this.palettesTabPage.Size = new System.Drawing.Size(261, 347);
            this.palettesTabPage.TabIndex = 3;
            this.palettesTabPage.Text = "Palettes";
            this.palettesTabPage.UseVisualStyleBackColor = true;
            // 
            // palettesTreeView
            // 
            this.palettesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.palettesTreeView.Location = new System.Drawing.Point(0, 0);
            this.palettesTreeView.Name = "palettesTreeView";
            this.palettesTreeView.Size = new System.Drawing.Size(261, 347);
            this.palettesTreeView.TabIndex = 0;
            // 
            // GameDescriptorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(269, 373);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "GameDescriptorControl";
            this.Text = "Project Explorer";
            this.tabControl1.ResumeLayout(false);
            this.filesTabPage.ResumeLayout(false);
            this.arrangersTabPage.ResumeLayout(false);
            this.palettesTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage filesTabPage;
        private System.Windows.Forms.TabPage arrangersTabPage;
        private System.Windows.Forms.TabPage palettesTabPage;
        private System.Windows.Forms.TreeView filesTreeView;
        private System.Windows.Forms.TreeView arrangersTreeView;
        private System.Windows.Forms.TreeView palettesTreeView;
    }
}
