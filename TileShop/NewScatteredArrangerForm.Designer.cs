namespace TileShop
{
    partial class NewScatteredArrangerForm
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
            this.TileHeightBox = new TileShop.IntegerTextBox();
            this.TileWidthBox = new TileShop.IntegerTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.TilesYBox = new TileShop.IntegerTextBox();
            this.TilesXBox = new TileShop.IntegerTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.CreateButton = new System.Windows.Forms.Button();
            this.CancelNewButton = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.TileHeightBox);
            this.groupBox2.Controls.Add(this.TileWidthBox);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(12, 103);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(272, 57);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tile Size";
            // 
            // TileHeightBox
            // 
            this.TileHeightBox.Location = new System.Drawing.Point(140, 19);
            this.TileHeightBox.Maximum = 2147483647;
            this.TileHeightBox.Minimum = -2147483648;
            this.TileHeightBox.Name = "TileHeightBox";
            this.TileHeightBox.Size = new System.Drawing.Size(62, 20);
            this.TileHeightBox.TabIndex = 4;
            // 
            // TileWidthBox
            // 
            this.TileWidthBox.Location = new System.Drawing.Point(9, 19);
            this.TileWidthBox.Maximum = 2147483647;
            this.TileWidthBox.Minimum = -2147483648;
            this.TileWidthBox.Name = "TileWidthBox";
            this.TileWidthBox.Size = new System.Drawing.Size(62, 20);
            this.TileWidthBox.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(208, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Tile Height";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(77, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Tile Width";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.NameTextBox);
            this.groupBox1.Controls.Add(this.TilesYBox);
            this.groupBox1.Controls.Add(this.TilesXBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(272, 85);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Arranger Properties";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(231, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Name";
            // 
            // NameTextBox
            // 
            this.NameTextBox.Location = new System.Drawing.Point(9, 20);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(216, 20);
            this.NameTextBox.TabIndex = 0;
            // 
            // TilesYBox
            // 
            this.TilesYBox.Location = new System.Drawing.Point(140, 50);
            this.TilesYBox.Maximum = 2147483647;
            this.TilesYBox.Minimum = -2147483648;
            this.TilesYBox.Name = "TilesYBox";
            this.TilesYBox.Size = new System.Drawing.Size(62, 20);
            this.TilesYBox.TabIndex = 2;
            // 
            // TilesXBox
            // 
            this.TilesXBox.Location = new System.Drawing.Point(9, 50);
            this.TilesXBox.Maximum = 2147483647;
            this.TilesXBox.Minimum = -2147483648;
            this.TilesXBox.Name = "TilesXBox";
            this.TilesXBox.Size = new System.Drawing.Size(62, 20);
            this.TilesXBox.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(208, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Tiles High";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(77, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Tiles Wide";
            // 
            // CreateButton
            // 
            this.CreateButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.CreateButton.Location = new System.Drawing.Point(135, 166);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(58, 23);
            this.CreateButton.TabIndex = 5;
            this.CreateButton.Text = "Create";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // CancelNewButton
            // 
            this.CancelNewButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelNewButton.Location = new System.Drawing.Point(216, 166);
            this.CancelNewButton.Name = "CancelNewButton";
            this.CancelNewButton.Size = new System.Drawing.Size(58, 23);
            this.CancelNewButton.TabIndex = 6;
            this.CancelNewButton.Text = "Cancel";
            this.CancelNewButton.UseVisualStyleBackColor = true;
            // 
            // NewScatteredArrangerForm
            // 
            this.AcceptButton = this.CreateButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 199);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CreateButton);
            this.Controls.Add(this.CancelNewButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewScatteredArrangerForm";
            this.Text = "New Scattered Arranger";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private IntegerTextBox TileHeightBox;
        private IntegerTextBox TileWidthBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private IntegerTextBox TilesYBox;
        private IntegerTextBox TilesXBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button CreateButton;
        private System.Windows.Forms.Button CancelNewButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox NameTextBox;
    }
}