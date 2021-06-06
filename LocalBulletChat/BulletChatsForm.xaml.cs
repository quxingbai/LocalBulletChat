using LocalBulletChat.Controls;
using LocalBulletChat.Model;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using static LocalBulletChat.Controls.TextBulletChat;

namespace LocalBulletChat
{


    public partial class BulletChatsForm : Window
    {


        public static BulletChatsForm MainBulletChatsForm { get; set; }

        public Thread SetTopmostThread;
        public BulletChatsForm()
        {
            InitializeComponent();
            MainBulletChatsForm = this;
        }
        /// <summary>
        /// 显示一条弹幕
        /// </summary>
        public void ShowBulletChat(BulletChatModel Bullet,Direction FromDirection,Direction ToDirection)
        {
            if (!Topmost) Topmost = true;
            TextBulletChat text = new TextBulletChat(FromDirection, ToDirection, Bullet,(sss)=>{ Dispatcher.Invoke(()=> { CANVAS_Map.Children.Remove(sss); }); });
            CANVAS_Map.Children.Add(text);
        }
        //public void ShowBulletChat(BulletChatModel Bullet)
        //{
        //    if (!Topmost) Topmost = true;
        //    TextBlock text = new TextBlock()
        //    {
        //        Text = $"{Bullet.SendUser}：{Bullet.Message}",
        //        Foreground = (Brush)new BrushConverter().ConvertFromString(Bullet.Foreground),
        //        FontSize = Bullet.FontSize,
        //        Visibility = Visibility.Collapsed,
        //    };
        //    TextBulletChat textt = new TextBulletChat(this.FromDirection, ToDirection)
        //    text.SetValue(Canvas.TopProperty, rd.Next(0, StaticResource.ScreenHeight - 50) * 1.0);
        //    CANVAS_Map.Children.Add(text);
        //    ThreadPool.QueueUserWorkItem(ARG =>
        //    {
        //        Thread.Sleep(100);
        //        Dispatcher.Invoke(() =>
        //        {
        //            text.BeginAnimation(FromProperty, new DoubleAnimation(FromPosition, ToPosition - (Encoding.UTF8.GetByteCount(text.Text) * text.FontSize), new Duration(TimeSpan.FromSeconds(BulletSpeed))));
        //            text.Visibility = Visibility.Visible;
        //        });
        //        Thread.Sleep((int)BulletSpeed * 1000 + 4000);
        //        Dispatcher.Invoke(() =>
        //        {
        //            CANVAS_Map.Children.Remove(text);
        //        });
        //    });
        //}
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == TopmostProperty)
            {
                Topmost = true;
            }else if (e.Property == WindowStateProperty)
            {
                if (WindowState != WindowState.Maximized)
                {
                    ThreadPool.QueueUserWorkItem(arg =>
                    {
                        Thread.Sleep(1);
                        Dispatcher.Invoke(() =>
                        {
                            WindowState = WindowState.Maximized;
                            Topmost = true;
                        });
                    });
                }
            }
            base.OnPropertyChanged(e);
        }
    }
}
