namespace TileShop
{
    partial class NewPaletteForm
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
            this.projectFileBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.colorFormatBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.paletteNameBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.addPaletteButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.offsetBox = new TileShop.IntegerTextBox();
            this.entriesBox = new TileShop.IntegerTextBox();
            this.SuspendLayout();
            // 
            // projectFileBox
            // 
            this.projectFileBox.FormattingEnabled = true;
            this.projectFileBox.Location = new System.Drawing.Point(12, 41);
            this.projectFileBox.Name = "projectFileBox";
            this.projectFileBox.Size = new System.Drawing.Size(251, 21);
            this.projectFileBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(269, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Project File";
            // 
            // colorFormatBox
            // 
            this.colorFormatBox.FormattingEnabled = true;
            this.colorFormatBox.Location = new System.Drawing.Point(13, 69);
            this.colorFormatBox.Name = "colorFormatBox";
            this.colorFormatBox.Size = new System.Drawing.Size(177, 21);
            this.colorFormatBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(196, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Color Format";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(196, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Number of Entries";
            // 
            // paletteNameBox
            // 
            this.paletteNameBox.Location = new System.Drawing.Point(13, 15);
            this.paletteNameBox.Name = "paletteNameBox";
            this.paletteNameBox.Size = new System.Drawing.Size(177, 20);
            this.paletteNameBox.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(196, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Palette name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(196, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "File offset";
            // 
            // addPaletteButton
            // 
            this.addPaletteButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.addPaletteButton.Location = new System.Drawing.Point(172, 165);
            this.addPaletteButton.Name = "addPaletteButton";
            this.addPaletteButton.Size = new System.Drawing.Size(75, 23);
            this.addPaletteButton.TabIndex = 5;
            this.addPaletteButton.Text = "Add";
            this.addPaletteButton.UseVisualStyleBackColor = true;
            this.addPaletteButton.Click += new System.EventHandler(this.addPaletteButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(254, 165);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // offsetBox
            // 
            this.offsetBox.Location = new System.Drawing.Point(13, 123);
            this.offsetBox.Name = "offsetBox";
            this.offsetBox.Size = new System.Drawing.Size(177, 20);
            this.offsetBox.TabIndex = 4;
            // 
            // entriesBox
            // 
            this.entriesBox.Location = new System.Drawing.Point(13, 96);
            this.entriesBox.Name = "entriesBox";
            this.entriesBox.Size = new System.Drawing.Size(177, 20);
            this.entriesBox.TabIndex = 3;
            // 
            // NewPaletteForm
            // 
            this.AcceptButton = this.addPaletteButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(341, 201);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.addPaletteButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.offsetBox);
            this.Controls.Add(this.entriesBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.paletteNameBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.colorFormatBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.projectFileBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewPaletteForm";
            this.Text = "Add New Palette";
            this.Shown += new System.EventHandler(this.NewPaletteForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox projectFileBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox colorFormatBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox paletteNameBox;
        private System.Windows.Forms.Label label4;
        private IntegerTextBox entriesBox;
        private IntegerTextBox offsetBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button addPaletteButton;
        private System.Windows.Forms.Button cancelButton;
    }
}