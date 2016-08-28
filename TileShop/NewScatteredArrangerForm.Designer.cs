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
            this.tileHeightBox = new TileShop.IntegerTextBox();
            this.tileWidthBox = new TileShop.IntegerTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.tilesYBox = new TileShop.IntegerTextBox();
            this.tilesXBox = new TileShop.IntegerTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.createButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tileHeightBox);
            this.groupBox2.Controls.Add(this.tileWidthBox);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(12, 103);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(272, 57);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tile Size";
            // 
            // tileHeightBox
            // 
            this.tileHeightBox.Location = new System.Drawing.Point(140, 19);
            this.tileHeightBox.Name = "tileHeightBox";
            this.tileHeightBox.Size = new System.Drawing.Size(62, 20);
            this.tileHeightBox.TabIndex = 4;
            // 
            // tileWidthBox
            // 
            this.tileWidthBox.Location = new System.Drawing.Point(9, 19);
            this.tileWidthBox.Name = "tileWidthBox";
            this.tileWidthBox.Size = new System.Drawing.Size(62, 20);
            this.tileWidthBox.TabIndex = 3;
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
            this.groupBox1.Controls.Add(this.nameTextBox);
            this.groupBox1.Controls.Add(this.tilesYBox);
            this.groupBox1.Controls.Add(this.tilesXBox);
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
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(9, 20);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(216, 20);
            this.nameTextBox.TabIndex = 0;
            // 
            // tilesYBox
            // 
            this.tilesYBox.Location = new System.Drawing.Point(140, 50);
            this.tilesYBox.Name = "tilesYBox";
            this.tilesYBox.Size = new System.Drawing.Size(62, 20);
            this.tilesYBox.TabIndex = 2;
            // 
            // tilesXBox
            // 
            this.tilesXBox.Location = new System.Drawing.Point(9, 50);
            this.tilesXBox.Name = "tilesXBox";
            this.tilesXBox.Size = new System.Drawing.Size(62, 20);
            this.tilesXBox.TabIndex = 1;
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
            // createButton
            // 
            this.createButton.Location = new System.Drawing.Point(135, 166);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(58, 23);
            this.createButton.TabIndex = 5;
            this.createButton.Text = "Create";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(216, 166);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(58, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // NewScatteredArrangerForm
            // 
            this.AcceptButton = this.createButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(295, 199);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.cancelButton);
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
        private IntegerTextBox tileHeightBox;
        private IntegerTextBox tileWidthBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private IntegerTextBox tilesYBox;
        private IntegerTextBox tilesXBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nameTextBox;
    }
}