using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SKYPE4COMLib;

namespace jimaku
{
    public partial class main : Form
    {
        public jimaku myForm;
        private SKYPE4COMLib.Skype _skype;
        private string _key;//Chat検索key
        private string BR = Environment.NewLine;//改行文字

        private List<string> myList = new List<string>();

        public main()
        {
            InitializeComponent();
            //Skypeオブジェクト生成
            _skype = new SKYPE4COMLib.Skype();
            try
            {
                _skype.Attach();
            }
            catch (Exception)
            {
                
                throw;
            }
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

            //このアプリからメッセージ送受信したいChatオブジェクトを抽出
            //以下を検索keyとしてマッチするChatオブジェクトを抽出する
            //  個人チャットの場合 : 相手のskype名(loftkunとか) 注)表示名じゃない
            //  グループチャットの場合 : 会話のタイトル(～～会議室とか)
            _key = "test";
            SKYPE4COMLib.Chat chat = getChat(_key);
            if (chat == null)
            {
                return;
            }

            //既存メッセージ表示
            //textBox1.Text = getMessages(chat);

            //メッセージ受信ハンドラ登録
            _skype.MessageStatus += new _ISkypeEvents_MessageStatusEventHandler(_skype_MessageStatus);

            //メッセージ送信
            //chat.SendMessage("ほげ～");

            //なお以下のように個人チャットの相手名を指定してメッセージ送信もできるようだ
            //グループチャットの場合、SendMessage()の第一引数が不明なので、
            //上記(chat.SendMessage())を採用している。
            //_skype.SendMessage("loftkun", "ほげほげ"); //loftkun個人宛に送信
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

        /// <summary>
        /// チャットメッセージ受信イベントハンドラ
        /// </summary>
        /// <param name="pMessage">メッセージ</param>
        /// <param name="Status">送受信Status</param>
        void _skype_MessageStatus(ChatMessage pMessage, TChatMessageStatus Status)
        {
            //textBox1.Text = "event\r\n" + textBox1.Text;
            try
            {
                //このメッセージの属しているチャットがkeyに合致するかチェック
                if (!(Status == TChatMessageStatus.cmsReceived || Status == TChatMessageStatus.cmsSent) ||
                    checkKey(_key, pMessage.Chat) == false)
                //if (checkKey(_key, pMessage.Chat) == false)
                {
                    return;
                }
                // 文字列でメッセージ内容を取得
                string str = getMessageString(pMessage);

                // リストにぶち込む
                myList.Add(str);

                //表示
                //textBox1.Text = str + textBox1.Text;
                // リストに入ってるデータの最新n個を表示させせます
                text.Text = "";
                for (int i = 0; i < int.Parse(textNum.Text) && i <= myList.Count - 1 ; i++) {
                    text.Text = myList[myList.Count - 1 - i] + "\r\n" + text.Text;
                }

                //text.Text = str;
            }
            catch (Exception ex)
            {
                text.Text = ex.ToString();
            }
        }

        /// <summary>
        /// チャットの既存メッセージを取得
        /// </summary>
        private String getMessages(SKYPE4COMLib.Chat chat)
        {

            try
            {
                String str = "";
                foreach (SKYPE4COMLib.ChatMessage msg in chat.Messages)
                {
                    str += getMessageString(msg);//適当に整形
                }
                return str;
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// チャットメッセージを表示用に整形
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private string getMessageString(ChatMessage msg)
        {
            //適当に整形
            return msg.Timestamp.ToString()
                + " [" + msg.Status.ToString() + "]"
                + "[" + msg.FromDisplayName + "] "
                + msg.Body + BR;
        }

        /// <summary>
        /// 検索keyにHITするChatオブジェクトを取得する
        /// </summary>
        /// <param name="key">検索key</param>
        /// <returns></returns>
        private SKYPE4COMLib.Chat getChat(String key)
        {
            //textBox1.Text += "getChat\r\n";
            try
            {
                //foreach (SKYPE4COMLib.Chat chat in _skype.Chats) //全てのチャットっぽい
                foreach (SKYPE4COMLib.Chat chat in _skype.ActiveChats) //アクティブなチャットウィンドウ順っぽい
                {
                    //textBox1.Text = "chat.Name: " + chat.Name + "\r\n" + textBox1.Text;
                    //textBox1.Text = "chat.DialogPartner:" + chat.DialogPartner + "\r\n" + textBox1.Text;
                    //textBox1.Text = "chat.Topic:" + chat.Topic + "\r\n" + textBox1.Text;
                    //textBox1.Text = "chat.TopicXML:" + chat.TopicXML + "\r\n" + textBox1.Text;
                    //textBox1.Text = "key :" + key + "\r\n" + textBox1.Text;
                    if (checkKey(key, chat) == true)
                    {
                        //textBox1.Text += "checkKey\r\n";
                        return chat;
                    }
                    else
                    {

                        //textBox1.Text = "checkKey fault\r\n" + textBox1.Text;
                    }
                }
                return null;
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
                return null;
            }
        }

        /// <summary>
        /// Chatオブジェクトが検索keyにHITするかどうかを判定する
        /// 検索keyは以下
        /// 　個人チャットの場合     : 相手のskype名
        /// 　グループチャットの場合 : 会話のタイトル
        /// </summary>
        /// <param name="key"></param>
        /// <param name="chat"></param>
        /// <returns></returns>
        private bool checkKey(string key, Chat chat)
        {
            try
            {
                //textBox1.Text = "chat.Status: " + chat.Status + "\r\n" + textBox1.Text;
                //けっこうあいまいな判定
                switch (chat.Status)
                {
                    case TChatStatus.chsDialog: //個人チャットかな～
                        //個人チャットの相手のskype名
                        if (key == chat.DialogPartner)
                        {
                            return true;
                        }
                        break;
                    case TChatStatus.chsMultiSubscribed://グループチャットかな～
                        //グループチャットの会話のタイトル
                        if (key == chat.Topic)
                        {
                            return true;
                        }
                        break;
                    default://分からん
                        break;
                }
                return false;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

    }
}
