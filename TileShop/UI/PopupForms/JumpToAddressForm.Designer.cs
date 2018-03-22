namespace TileShop.PopupForms
{
    partial class JumpToAddressForm
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
            this.NumberBox = new System.Windows.Forms.TextBox();
            this.HexButton = new System.Windows.Forms.RadioButton();
            this.DecimalButton = new System.Windows.Forms.RadioButton();
            this.JumpButton = new System.Windows.Forms.Button();
            this.CancelFormButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // NumberBox
            // 
            this.NumberBox.Location = new System.Drawing.Point(12, 37);
            this.NumberBox.Name = "NumberBox";
            this.NumberBox.Size = new System.Drawing.Size(156, 20);
            this.NumberBox.TabIndex = 0;
            // 
            // HexButton
            // 
            this.HexButton.AutoSize = true;
            this.HexButton.Checked = true;
            this.HexButton.Location = new System.Drawing.Point(12, 14);
            this.HexButton.Name = "HexButton";
            this.HexButton.Size = new System.Drawing.Size(44, 17);
            this.HexButton.TabIndex = 1;
            this.HexButton.TabStop = true;
            this.HexButton.Text = "Hex";
            this.HexButton.UseVisualStyleBackColor = true;
            // 
            // DecimalButton
            // 
            this.DecimalButton.AutoSize = true;
            this.DecimalButton.Location = new System.Drawing.Point(62, 14);
            this.DecimalButton.Name = "DecimalButton";
            this.DecimalButton.Size = new System.Drawing.Size(63, 17);
            this.DecimalButton.TabIndex = 2;
            this.DecimalButton.Text = "Decimal";
            this.DecimalButton.UseVisualStyleBackColor = true;
            // 
            // JumpButton
            // 
            this.JumpButton.Location = new System.Drawing.Point(12, 64);
            this.JumpButton.Name = "JumpButton";
            this.JumpButton.Size = new System.Drawing.Size(75, 23);
            this.JumpButton.TabIndex = 3;
            this.JumpButton.Text = "Jump";
            this.JumpButton.UseVisualStyleBackColor = true;
            this.JumpButton.Click += new System.EventHandler(this.JumpButton_Click);
            // 
            // CancelFormButton
            // 
            this.CancelFormButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelFormButton.Location = new System.Drawing.Point(93, 63);
            this.CancelFormButton.Name = "CancelFormButton";
            this.CancelFormButton.Size = new System.Drawing.Size(75, 23);
            this.CancelFormButton.TabIndex = 4;
            this.CancelFormButton.Text = "Cancel";
            this.CancelFormButton.UseVisualStyleBackColor = true;
            // 
            // JumpToAddressForm
            // 
            this.AcceptButton = this.JumpButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(180, 108);
            this.ControlBox = false;
            this.Controls.Add(this.CancelFormButton);
            this.Controls.Add(this.JumpButton);
            this.Controls.Add(this.DecimalButton);
            this.Controls.Add(this.HexButton);
            this.Controls.Add(this.NumberBox);
            this.Name = "JumpToAddressForm";
            this.Text = "Jump To Address";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox NumberBox;
        private System.Windows.Forms.RadioButton HexButton;
        private System.Windows.Forms.RadioButton DecimalButton;
        private System.Windows.Forms.Button JumpButton;
        private System.Windows.Forms.Button CancelFormButton;
    }
}