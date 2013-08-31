using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using SKYPE4COMLib;

namespace jimaku
{
    public partial class Skype : Form
    {
        private SKYPE4COMLib.Skype _skype;
        private string _key;//Chat検索key
        private string BR = Environment.NewLine;//改行文字

        public Skype()
        {
            InitializeComponent();

            //Skypeオブジェクト生成
            _skype = new SKYPE4COMLib.Skype();
            _skype.Attach();
        }

        private void Skype_Load_1(object sender, EventArgs e)
        {
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

        /// <summary>
        /// チャットメッセージ受信イベントハンドラ
        /// </summary>
        /// <param name="pMessage">メッセージ</param>
        /// <param name="Status">送受信Status</param>
        void _skype_MessageStatus(ChatMessage pMessage, TChatMessageStatus Status)
        {
            textBox1.Text = "event\r\n" + textBox1.Text;
            try
            {
                //このメッセージの属しているチャットがkeyに合致するかチェック
                if (checkKey(_key, pMessage.Chat) == false)
                {
                    return;
                }
                string str = getMessageString(pMessage);

                //表示
                textBox1.Text = str + textBox1.Text;
            }
            catch (Exception)
            {
                textBox1.Text = "Exception" + BR + textBox1.Text;
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
            textBox1.Text += "getChat\r\n";
            try
            {
                //foreach (SKYPE4COMLib.Chat chat in _skype.Chats) //全てのチャットっぽい
                foreach (SKYPE4COMLib.Chat chat in _skype.ActiveChats) //アクティブなチャットウィンドウ順っぽい
                {
                    textBox1.Text = "chat.Name: " + chat.Name + "\r\n" + textBox1.Text;
                    textBox1.Text = "chat.DialogPartner:" + chat.DialogPartner + "\r\n" + textBox1.Text;
                    textBox1.Text = "chat.Topic:" + chat.Topic + "\r\n" + textBox1.Text;
                    textBox1.Text = "chat.TopicXML:" + chat.TopicXML + "\r\n" + textBox1.Text;
                    textBox1.Text = "key :" + key +"\r\n" + textBox1.Text;
                    if (checkKey(key, chat) == true)
                    {
                        textBox1.Text += "checkKey\r\n";
                        return chat;
                    }
                    else {

                        textBox1.Text = "checkKey fault\r\n" + textBox1.Text;
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
                textBox1.Text = "chat.Status: " + chat.Status + "\r\n" + textBox1.Text;
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
