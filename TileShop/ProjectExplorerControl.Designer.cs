namespace TileShop
{
    partial class ProjectExplorerControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectExplorerControl));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.projectTabPage = new System.Windows.Forms.TabPage();
            this.ProjectTreeView = new System.Windows.Forms.TreeView();
            this.nodeImageList = new System.Windows.Forms.ImageList(this.components);
            this.tabControl1.SuspendLayout();
            this.projectTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Controls.Add(this.projectTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(269, 373);
            this.tabControl1.TabIndex = 0;
            // 
            // projectTabPage
            // 
            this.projectTabPage.Controls.Add(this.ProjectTreeView);
            this.projectTabPage.Location = new System.Drawing.Point(4, 4);
            this.projectTabPage.Name = "projectTabPage";
            this.projectTabPage.Size = new System.Drawing.Size(261, 347);
            this.projectTabPage.TabIndex = 1;
            this.projectTabPage.Text = "Project";
            this.projectTabPage.UseVisualStyleBackColor = true;
            // 
            // ProjectTreeView
            // 
            this.ProjectTreeView.AllowDrop = true;
            this.ProjectTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProjectTreeView.ImageIndex = 0;
            this.ProjectTreeView.ImageList = this.nodeImageList;
            this.ProjectTreeView.LabelEdit = true;
            this.ProjectTreeView.Location = new System.Drawing.Point(0, 0);
            this.ProjectTreeView.Name = "ProjectTreeView";
            this.ProjectTreeView.SelectedImageIndex = 0;
            this.ProjectTreeView.Size = new System.Drawing.Size(261, 347);
            this.ProjectTreeView.TabIndex = 0;
            this.ProjectTreeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.ProjectTreeView_AfterLabelEdit);
            this.ProjectTreeView.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.ProjectTreeView_BeforeCollapse);
            this.ProjectTreeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.ProjectTreeView_BeforeExpand);
            this.ProjectTreeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.ProjectTreeView_ItemDrag);
            this.ProjectTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.ProjectTreeView_NodeMouseClick);
            this.ProjectTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.ProjectTreeView_NodeMouseDoubleClick);
            this.ProjectTreeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.ProjectTreeView_DragDrop);
            this.ProjectTreeView.DragEnter += new System.Windows.Forms.DragEventHandler(this.ProjectTreeView_DragEnter);
            // 
            // nodeImageList
            // 
            this.nodeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("nodeImageList.ImageStream")));
            this.nodeImageList.TransparentColor = System.Drawing.Color.Black;
            this.nodeImageList.Images.SetKeyName(0, "folder_Closed_16xLG.png");
            this.nodeImageList.Images.SetKeyName(1, "folder_Open_16xLG.png");
            this.nodeImageList.Images.SetKeyName(2, "CodeSnippet_6225_32.bmp");
            this.nodeImageList.Images.SetKeyName(3, "ImageButton_735_32.bmp");
            this.nodeImageList.Images.SetKeyName(4, "ChooseColorHS.bmp");
            // 
            // ProjectExplorerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(269, 373);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ProjectExplorerControl";
            this.Text = "Project Explorer";
            this.tabControl1.ResumeLayout(false);
            this.projectTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage projectTabPage;
        private System.Windows.Forms.TreeView ProjectTreeView;
        private System.Windows.Forms.ImageList nodeImageList;
    }
}
