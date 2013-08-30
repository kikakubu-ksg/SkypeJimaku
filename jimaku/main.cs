using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace jimaku
{
    public partial class main : Form
    {
        public jimaku myForm;
        public main()
        {
            InitializeComponent();
        }

        private void text_TextChanged(object sender, EventArgs e)
        {
            set_jimaku();
        }

        private void main_Load(object sender, EventArgs e)
        {
            // create Form1
            myForm = new jimaku();
            myForm.Visible = true;
            myForm.ShowInTaskbar = false;
            text.Text = Properties.Settings.Default.myString;
            text.Font = Properties.Settings.Default.myFont;
            text.ForeColor = Properties.Settings.Default.myColor;
            set_jimaku();
        }

        private void set_jimaku() 
        {
            System.Drawing.Drawing2D.GraphicsPath path =
            new System.Drawing.Drawing2D.GraphicsPath();
            
            int style = 0;
            style += (text.Font.Bold) ? (int)FontStyle.Bold : (int)FontStyle.Regular;
            style += (text.Font.Italic) ? (int)FontStyle.Italic : (int)FontStyle.Regular;
            style += (text.Font.Underline) ? (int)FontStyle.Underline : (int)FontStyle.Regular;
            style += (text.Font.Strikeout) ? (int)FontStyle.Strikeout : (int)FontStyle.Regular;
            path.AddString(text.Text, text.Font.FontFamily,
                style, 
                (float)(text.Font.SizeInPoints / 0.75), new Point(0, 0),
                StringFormat.GenericDefault);
            myForm.Region = new Region(path);
            myForm.BackColor = text.ForeColor;
            path.Dispose();
           
        }

        private void button_font_Click(object sender, EventArgs e)
        {
            //FontDialogクラスのインスタンスを作成
            FontDialog fd = new FontDialog();

            //初期のフォントを設定
            fd.Font = text.Font;
            //初期の色を設定
            fd.Color = text.ForeColor;
            //ユーザーが選択できるポイントサイズの最大値を設定する
            fd.MaxSize = 999;
            fd.MinSize = 2;
            //存在しないフォントやスタイルをユーザーが選択すると
            //エラーメッセージを表示する
            fd.FontMustExist = true;
            //横書きフォントだけを表示する
            fd.AllowVerticalFonts = false;
            //色を選択できるようにする
            fd.ShowColor = true;
            //取り消し線、下線、テキストの色などのオプションを指定可能にする
            //デフォルトがTrueのため必要はない
            fd.ShowEffects = true;
            //固定ピッチフォント以外も表示する
            //デフォルトがFalseのため必要はない
            fd.FixedPitchOnly = false;
            //ベクタ フォントを選択できるようにする
            //デフォルトがTrueのため必要はない
            fd.AllowVectorFonts = true;

            //ダイアログを表示する
            if (fd.ShowDialog() != DialogResult.Cancel)
            {
                //TextBox1のフォントと色を変える
                text.Font = fd.Font;
                text.ForeColor = fd.Color;
            }
            set_jimaku();
            fd.Dispose();
        }

        private void button_modify_Click(object sender, EventArgs e)
        {
            set_jimaku();
        }

        private void main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.myString = text.Text;
            Properties.Settings.Default.myFont = text.Font;
            Properties.Settings.Default.myColor = text.ForeColor;
            Properties.Settings.Default.Save();
        }



    }
}
