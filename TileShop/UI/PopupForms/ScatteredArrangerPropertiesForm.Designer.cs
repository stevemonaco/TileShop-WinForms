namespace TileShop
{
    partial class ScatteredArrangerPropertiesForm
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ElementHeightBox = new System.Windows.Forms.NumericUpDown();
            this.ElementWidthBox = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.LinearLayoutButton = new System.Windows.Forms.RadioButton();
            this.TiledLayoutButton = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.ArrangerHeightBox = new System.Windows.Forms.NumericUpDown();
            this.ArrangerWidthBox = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ConfirmButton = new System.Windows.Forms.Button();
            this.CancelNewButton = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ElementHeightBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ElementWidthBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ArrangerHeightBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ArrangerWidthBox)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ElementHeightBox);
            this.groupBox2.Controls.Add(this.ElementWidthBox);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(12, 131);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(307, 57);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Element Pixel Size";
            // 
            // ElementHeightBox
            // 
            this.ElementHeightBox.Location = new System.Drawing.Point(159, 20);
            this.ElementHeightBox.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.ElementHeightBox.Name = "ElementHeightBox";
            this.ElementHeightBox.Size = new System.Drawing.Size(60, 20);
            this.ElementHeightBox.TabIndex = 12;
            this.ElementHeightBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ElementWidthBox
            // 
            this.ElementWidthBox.Location = new System.Drawing.Point(9, 22);
            this.ElementWidthBox.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.ElementWidthBox.Name = "ElementWidthBox";
            this.ElementWidthBox.Size = new System.Drawing.Size(62, 20);
            this.ElementWidthBox.TabIndex = 11;
            this.ElementWidthBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(225, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Element Height";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(77, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Element Width";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.LinearLayoutButton);
            this.groupBox1.Controls.Add(this.TiledLayoutButton);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.ArrangerHeightBox);
            this.groupBox1.Controls.Add(this.ArrangerWidthBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.NameTextBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(307, 113);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Arranger Properties";
            // 
            // LinearLayoutButton
            // 
            this.LinearLayoutButton.AutoSize = true;
            this.LinearLayoutButton.Location = new System.Drawing.Point(109, 81);
            this.LinearLayoutButton.Name = "LinearLayoutButton";
            this.LinearLayoutButton.Size = new System.Drawing.Size(54, 17);
            this.LinearLayoutButton.TabIndex = 20;
            this.LinearLayoutButton.Text = "Linear";
            this.LinearLayoutButton.UseVisualStyleBackColor = true;
            // 
            // TiledLayoutButton
            // 
            this.TiledLayoutButton.AutoSize = true;
            this.TiledLayoutButton.Checked = true;
            this.TiledLayoutButton.Location = new System.Drawing.Point(55, 81);
            this.TiledLayoutButton.Name = "TiledLayoutButton";
            this.TiledLayoutButton.Size = new System.Drawing.Size(48, 17);
            this.TiledLayoutButton.TabIndex = 19;
            this.TiledLayoutButton.TabStop = true;
            this.TiledLayoutButton.Text = "Tiled";
            this.TiledLayoutButton.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 83);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Layout";
            // 
            // ArrangerHeightBox
            // 
            this.ArrangerHeightBox.Location = new System.Drawing.Point(157, 51);
            this.ArrangerHeightBox.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.ArrangerHeightBox.Name = "ArrangerHeightBox";
            this.ArrangerHeightBox.Size = new System.Drawing.Size(62, 20);
            this.ArrangerHeightBox.TabIndex = 17;
            this.ArrangerHeightBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ArrangerWidthBox
            // 
            this.ArrangerWidthBox.Location = new System.Drawing.Point(9, 51);
            this.ArrangerWidthBox.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.ArrangerWidthBox.Name = "ArrangerWidthBox";
            this.ArrangerWidthBox.Size = new System.Drawing.Size(62, 20);
            this.ArrangerWidthBox.TabIndex = 16;
            this.ArrangerWidthBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(265, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Name";
            // 
            // NameTextBox
            // 
            this.NameTextBox.Location = new System.Drawing.Point(9, 20);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(253, 20);
            this.NameTextBox.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(225, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Elements High";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(75, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Elements Wide";
            // 
            // ConfirmButton
            // 
            this.ConfirmButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ConfirmButton.Location = new System.Drawing.Point(197, 194);
            this.ConfirmButton.Name = "ConfirmButton";
            this.ConfirmButton.Size = new System.Drawing.Size(58, 23);
            this.ConfirmButton.TabIndex = 5;
            this.ConfirmButton.Text = "Confirm";
            this.ConfirmButton.UseVisualStyleBackColor = true;
            this.ConfirmButton.Click += new System.EventHandler(this.ConfirmButton_Click);
            // 
            // CancelNewButton
            // 
            this.CancelNewButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelNewButton.Location = new System.Drawing.Point(261, 194);
            this.CancelNewButton.Name = "CancelNewButton";
            this.CancelNewButton.Size = new System.Drawing.Size(58, 23);
            this.CancelNewButton.TabIndex = 6;
            this.CancelNewButton.Text = "Cancel";
            this.CancelNewButton.UseVisualStyleBackColor = true;
            // 
            // ScatteredArrangerPropertiesForm
            // 
            this.AcceptButton = this.ConfirmButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 228);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ConfirmButton);
            this.Controls.Add(this.CancelNewButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScatteredArrangerPropertiesForm";
            this.Text = "Scattered Arranger Properties";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ElementHeightBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ElementWidthBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ArrangerHeightBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ArrangerWidthBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button ConfirmButton;
        private System.Windows.Forms.Button CancelNewButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.NumericUpDown ElementHeightBox;
        private System.Windows.Forms.NumericUpDown ElementWidthBox;
        private System.Windows.Forms.NumericUpDown ArrangerHeightBox;
        private System.Windows.Forms.NumericUpDown ArrangerWidthBox;
        private System.Windows.Forms.RadioButton LinearLayoutButton;
        private System.Windows.Forms.RadioButton TiledLayoutButton;
        private System.Windows.Forms.Label label6;
    }
}