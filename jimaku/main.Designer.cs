namespace jimaku
{
    partial class main
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
            this.text = new System.Windows.Forms.TextBox();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.button_font = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // text
            // 
            this.text.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.text.Font = new System.Drawing.Font("MS UI Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.text.Location = new System.Drawing.Point(0, 0);
            this.text.Multiline = true;
            this.text.Name = "text";
            this.text.Size = new System.Drawing.Size(373, 151);
            this.text.TabIndex = 1;
            this.text.Text = "てすと";
            this.text.TextChanged += new System.EventHandler(this.text_TextChanged);
            // 
            // button_font
            // 
            this.button_font.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button_font.Location = new System.Drawing.Point(0, 157);
            this.button_font.Name = "button_font";
            this.button_font.Size = new System.Drawing.Size(373, 33);
            this.button_font.TabIndex = 2;
            this.button_font.Text = "フォント変更";
            this.button_font.UseVisualStyleBackColor = true;
            this.button_font.Click += new System.EventHandler(this.button_font_Click);
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 190);
            this.Controls.Add(this.button_font);
            this.Controls.Add(this.text);
            this.Name = "main";
            this.Text = "非レイヤード字幕";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.main_FormClosing);
            this.Load += new System.EventHandler(this.main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox text;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.Button button_font;
    }
}