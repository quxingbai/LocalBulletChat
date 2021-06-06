using LocalBulletChat.Controls;
using LocalBulletChat.Controls.Forms.BulletForms;
using LocalBulletChat.Controls.Tool;
using LocalBulletChat.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static LocalBulletChat.Controls.WinAPI;

namespace LocalBulletChat
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        CDSocketUDP Server = new CDSocketUDP(StaticResource.ClientPort);
        Thread IsOnlineThread;
        int KeyBoardHookID = 0;
        HookProc KeyboardHookProc;
        public MainWindow()
        {
            InitializeComponent();
            //设置窗体位置
            Double FormMargin = 50;
            Left = FormMargin;
            //下角
            //Top = StaticResource.ScreenHeight - Height - FormMargin - 40;
            //中间
            Top = (StaticResource.ScreenHeight - Height) / 2-Height;
            Width = StaticResource.ScreenWidth-FormMargin*2;

            String ForegroundColors = "Black,White,Red,Orange,Yellow,Green,Aqua,Blue,Purple,Gray,Lime,Pink,#FF0068FF,#FFFF0097,#FF00FFA2,#FF8000FF,#FFFF0068,#FFF959E3,";
            foreach(var v in typeof(Brushes).GetProperties())
            {
                ForegroundColors += v.Name + ",";
                var a = v.GetMethod;
                var b = v.Name;
                var c = v.GetValue(typeof(Brushes));
                //ForegroundColors += v.GetMethod.Invoke(,) + ",";
            }
            ForegroundColors = ForegroundColors.Remove(ForegroundColors.Length - 1);
            COLORSELECT_Foreground.Colors = ForegroundColors;
            COLORSELECT_Foreground.SelectedItem = COLORSELECT_Foreground.Items[0];
            //设置 键盘钩子
            KeyboardHookProc = (c, p, ip) =>
             {
                //110是 小键盘的 . 
                if (ip.First().ToString() == "110")
                 {
                     if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                     {
                         WindowState = WindowState.Normal;
                         this.Show();
                         if (!BulletChatsForm.MainBulletChatsForm.Topmost)
                         {
                             BulletChatsForm.MainBulletChatsForm.Topmost = true;
                         }
                     }
                     else
                     {
                         Directory.CreateDirectory(DateTime.Now.ToString("yyyy-MM-dd"));
                         String fileName = ".\\"+DateTime.Now.ToString("yyyy-MM-dd") + "\\"+DateTime.Now.ToString("HH：mm：ss")+".png";
                         GDITool.SaveScreenImage(fileName);
                     }
                 }
                 else if(ip.First().ToString()=="27")
                 {
                     Hide();
                 }
                 return 0;
             };
            KeyBoardHookID = SetWindowsHookEx(13, KeyboardHookProc, IntPtr.Zero, 0);

            Server.StartGetMessage();
            new BulletChatsForm().Show();
            Server.GetNewMessage += Server_GetNewMessage;
            (IsOnlineThread = new Thread(() =>
              {
                  while (true)
                  {
                      //Server.SendTo(IsOnLine.Client_Send().ToByte(),)
                      SendToServer(IsOnLine.Client_Send());
                      Thread.Sleep((StaticResource.ClientTimeOut - 1) * 1000);
                  }
              })).Start();
        }
        private void Server_GetNewMessage(byte[] Content, MessageBase Message, System.Net.EndPoint FromIP)
        {
            if (Message.MessageType == SocketMessageType.BulletChat)
            {
                BulletChatModel msg = BulletChatModel.ToModel<BulletChatModel>(Content);
                ThreadPool.QueueUserWorkItem(a =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        //BulletBase form = new BulletBase(msg);
                        BulletChatsForm.MainBulletChatsForm.ShowBulletChat(msg, TextBulletChat.Direction.Right, TextBulletChat.Direction.Left);
                    });
                });
            }
        }

        private void ColorSelectBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ColorSelectBox s = sender as ColorSelectBox;
            Brush b = s.SelectedBrush;
        }
        //发送消息 以变量为基准
        public void SendToServer(MessageBase Msg)
        {
            Server.SendTo(Msg.ToByte(), StaticResource.ServerIpAddress);
        }
        //发送消息 以输入为基准
        public void SendToServer()
        {
            Double FontSize = 15;
            String text = TEXT_Message.Text;
            text = TextPrintRouter(text);
            try
            {
                FontSize = Convert.ToDouble(TEXT_FontSize.Text);
            }
            catch
            {

            }
            Server.SendTo(new BulletChatModel()
            {
                Message = text,
                SendUser = StaticResource.UserName,
                FontSize = FontSize,
                Foreground = COLORSELECT_Foreground.SelectedBrush.ToString(),
                MessageType = SocketMessageType.BulletChat
            }.ToByte(), StaticResource.ServerIpAddress);
            TEXT_Message.Text = "";
        }
        public String TextPrintRouter(String Text)
        {
            String Result = Text;
            Result=Text.Replace("[Time]",DateTime.Now.ToString());
            return Result;
        }
        protected override void OnClosed(EventArgs e)
        {
            IsOnlineThread.Abort();
            Server.Dispose();
            Process.GetProcessesByName("LocalBulletChat").First().Kill();
            UnhookWindowsHookEx(KeyBoardHookID);
            base.OnClosed(e);
        }

        private void TEXT_Message_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Keyboard.IsKeyDown(Key.LeftShift))
                {
                    int sele = TEXT_Message.SelectionStart;
                    TEXT_Message.Text = TEXT_Message.Text.Insert(sele, "\n");
                    TEXT_Message.SelectionStart = sele+1;
                }
                else if(TEXT_Message.Text.Trim()!="")
                {
                    SendToServer();
                }
            }
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape) WindowState= WindowState.Minimized;
            base.OnKeyDown(e);
        }
        public new void Show()
        {
            base.Show();
            Topmost = true;
            Focus();
            TEXT_Message.Focus();
            TEXT_Message.Focusable = true;
        }
        public new void Hide()
        {
            base.Hide();
        }
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == TopmostProperty)
            {
                BulletChatsForm.MainBulletChatsForm.Topmost = true;
            }
            base.OnPropertyChanged(e);
        }
    }
}
