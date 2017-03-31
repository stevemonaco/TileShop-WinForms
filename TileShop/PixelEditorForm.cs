using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Runtime.InteropServices;
using System.Reflection;

namespace TileShop
{
    public enum PixelDrawState { PencilState = 0, PickerState };

    public partial class PixelEditorForm : DockContent
    {
        Arranger EditArranger = null;
        RenderManager rm = null;
        Size PixelMargin;
        Rectangle DisplayRect;
        TextureBrush TransparentBrush;

        int Zoom = 24;
        PixelDrawState DrawState = PixelDrawState.PencilState;
        bool RenderTransparency = true;
        bool showGridlines = false;

        public PixelEditorForm()
        {
            InitializeComponent();

            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, PixelPanel, new object[] { true }); // Enable double buffering of the PixelPanel

            PixelMargin = new Size(12, 4);
            DisplayRect = new Rectangle(new Point(PixelMargin.Width, PixelMargin.Height), new Size(0, 0));
            TransparentBrush = new TextureBrush(Properties.Resources.TransparentBrushPattern);

            SetDrawState(PixelDrawState.PencilState);
        }

        public void SetEditArranger(Arranger arr)
        {
            EditArranger = arr;
            rm = new RenderManager();

            DisplayRect.Size = new Size(EditArranger.ArrangerPixelSize.Width * Zoom, EditArranger.ArrangerPixelSize.Height * Zoom);
            PixelPanel.Height = arr.ArrangerPixelSize.Height * Zoom + PixelMargin.Height;
            PixelPanel.Invalidate();

            swatchControl.SetPaletteName(EditArranger.GetElement(0, 0).Palette);
            swatchControl.SelectedIndex = 0;
        }

        /// <summary>
        /// Reloads arranger data from underlying source
        /// </summary>
        public void ReloadArranger()
        {
            if (EditArranger == null)
                return;

            // Forces the render manager to do a full redraw
            rm.Invalidate();
            // Redraw the viewer graphics
            PixelPanel.Invalidate();
            // Update palette colors in the swatch
            swatchControl.Invalidate();
        }

        private void PixelPanel_Paint(object sender, PaintEventArgs e)
        {
            if (EditArranger == null)
                return;

            rm.Render(EditArranger);

            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            if(RenderTransparency)
                e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            else
                e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;

            Rectangle src = new Rectangle(0, 0, EditArranger.ArrangerPixelSize.Width, EditArranger.ArrangerPixelSize.Height);
            Rectangle dest = new Rectangle(PixelMargin.Width, PixelMargin.Height,
                EditArranger.ArrangerPixelSize.Width * Zoom, EditArranger.ArrangerPixelSize.Height * Zoom);

            e.Graphics.FillRectangle(TransparentBrush, DisplayRect);
            e.Graphics.DrawImage(rm.Image, dest, src, GraphicsUnit.Pixel);

            if(showGridlines)
                DrawPixelGridlines(e.Graphics);
        }

        private void DrawPixelGridlines(Graphics g)
        {
            int dx = Zoom;
            int dy = Zoom;

            for (int y = PixelMargin.Height; y < EditArranger.ArrangerPixelSize.Height * Zoom + PixelMargin.Height; y += dy) // Draw horizontal lines
                g.DrawLine(Pens.White, PixelMargin.Width, y, EditArranger.ArrangerPixelSize.Width * Zoom + PixelMargin.Width, y);

            for (int x = PixelMargin.Width; x < EditArranger.ArrangerPixelSize.Width * Zoom + PixelMargin.Width; x += dx) // Draw vertical lines
                g.DrawLine(Pens.White, x, PixelMargin.Height, x, EditArranger.ArrangerPixelSize.Height * Zoom + PixelMargin.Height);
        }

        private void SetDrawState(PixelDrawState state)
        {
            // Reset buttons
            pencilButton.Checked = false;
            pickerButton.Checked = false;

            DrawState = state;

            switch(state)
            {
                case PixelDrawState.PencilState:
                    pencilButton.Checked = true;
                    PixelPanel.Cursor = FileManager.Instance.GetCursor("PencilCursor");
                    break;
                case PixelDrawState.PickerState:
                    pickerButton.Checked = true;
                    PixelPanel.Cursor = FileManager.Instance.GetCursor("PickerCursor");
                    break;
            }
        }

        private void pencilButton_Click(object sender, EventArgs e)
        {
            SetDrawState(PixelDrawState.PencilState);
        }

        private void pickerButton_Click(object sender, EventArgs e)
        {
            SetDrawState(PixelDrawState.PickerState);
        }

        private void gridlinesButton_Click(object sender, EventArgs e)
        {
            showGridlines ^= true;
            PixelPanel.Invalidate();
        }

        private void PixelPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (!DisplayRect.Contains(e.Location) || EditArranger == null)
                return;

            Point unscaledLoc = new Point(); // Location in pixels
            unscaledLoc.X = (e.Location.X - PixelMargin.Width) / Zoom;
            unscaledLoc.Y = (e.Location.Y - PixelMargin.Height) / Zoom;

            if(DrawState == PixelDrawState.PencilState)
            {
                Palette pal = FileManager.Instance.GetPalette(EditArranger.GetElement(0, 0).Palette);
                rm.SetPixel(unscaledLoc.X, unscaledLoc.Y, Color.FromArgb((int)pal[swatchControl.SelectedIndex]));
                PixelPanel.Invalidate();
            }
            else if(DrawState == PixelDrawState.PickerState)
            {
                Color c = rm.GetPixel(unscaledLoc.X, unscaledLoc.Y);
                Palette pal = FileManager.Instance.GetPalette(EditArranger.GetElement(0, 0).Palette);
                swatchControl.SelectedIndex = pal.GetIndexByLocalColor(c, true);
            }
        }

        private void PixelPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (DisplayRect.Contains(e.Location) || EditArranger == null)
            {
                if (DrawState == PixelDrawState.PencilState)
                    PixelPanel.Cursor = FileManager.Instance.GetCursor("PencilCursor");
                else if (DrawState == PixelDrawState.PickerState)
                    PixelPanel.Cursor = FileManager.Instance.GetCursor("PickerCursor");
            }
            else
                PixelPanel.Cursor = Cursors.Arrow;
        }
    }
}
