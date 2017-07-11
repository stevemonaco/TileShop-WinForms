using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;

namespace TileShop
{
    public class EditorDockContent : DockContent
    {
        public event EventHandler<EventArgs> ContentModified = null;
        public event EventHandler<EventArgs> ContentSaved = null;

        public EditorDockContent()
        {
            this.FormClosing += EditorDockContent_FormClosing;
        }

        private void EditorDockContent_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if(ContainsModifiedContent)
            {
                DialogResult dr = MessageBox.Show("Content has been modified. Save?", "Save", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes)
                    SaveContent();
                else if (dr == DialogResult.No)
                    return;
                else if(dr == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        protected virtual void OnContentModified(EventArgs e)
        {
            ContentModified?.Invoke(this, e);
        }

        protected virtual void OnContentSaved(EventArgs e)
        {
            ContentSaved?.Invoke(this, e);
        }

        /// <summary>
        /// Returns true if the content has been modified since last save or false if it has not been modified
        /// </summary>
        public bool ContainsModifiedContent
        {
            get => containsModifiedContent;
            protected set
            {
                containsModifiedContent = value;
                if (containsModifiedContent)
                    Text = ContentSourceName + "*";
                else
                    Text = ContentSourceName;
            }
        }
        protected bool containsModifiedContent;

        /// <summary>
        /// Returns the name of the content source
        /// </summary>
        public string ContentSourceName
        {
            get => contentSourceName;
            protected set => contentSourceName = value;
        }
        protected string contentSourceName;

        /// <summary>
        /// Returns the key of the content source
        /// </summary>
        public string ContentSourceKey
        {
            get => contentSourceKey;
            protected set
            {
                contentSourceKey = value;
                if (containsModifiedContent)
                    Text = contentSourceName + "*";
                else
                    Text = contentSourceName;
            }
        }
        protected string contentSourceKey;

        /// <summary>
        /// Saves content to underlying source
        /// </summary>
        /// <returns></returns>
        public virtual bool SaveContent()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Loads content from underlying source
        /// </summary>
        /// <returns></returns>
        public virtual bool ReloadContent()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Refreshes content displayed within the DockContent without reloading from underlying source
        /// </summary>
        /// <returns></returns>
        public virtual bool RefreshContent()
        {
            throw new NotImplementedException();
        }


    }
}
