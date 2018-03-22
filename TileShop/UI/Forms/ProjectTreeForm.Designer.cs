namespace TileShop
{
    partial class ProjectTreeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectTreeForm));
            this.nodeImageList = new System.Windows.Forms.ImageList(this.components);
            this.ProjectTreeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
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
            // ProjectTreeView
            // 
            this.ProjectTreeView.AllowDrop = true;
            this.ProjectTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProjectTreeView.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProjectTreeView.ImageIndex = 0;
            this.ProjectTreeView.ImageList = this.nodeImageList;
            this.ProjectTreeView.LabelEdit = true;
            this.ProjectTreeView.Location = new System.Drawing.Point(0, 0);
            this.ProjectTreeView.Name = "ProjectTreeView";
            this.ProjectTreeView.SelectedImageIndex = 0;
            this.ProjectTreeView.Size = new System.Drawing.Size(269, 373);
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
            // ProjectTreeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(269, 373);
            this.Controls.Add(this.ProjectTreeView);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ProjectTreeForm";
            this.Text = "Project Tree";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ImageList nodeImageList;
        private System.Windows.Forms.TreeView ProjectTreeView;
    }
}
