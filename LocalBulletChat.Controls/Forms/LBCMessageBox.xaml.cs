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

namespace LocalBulletChat.Controls.Forms
{
    /// <summary>
    /// LBCMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class LBCMessageBox : Window
    {
        public LBCMessageBox()
        {
            InitializeComponent();
        }
        TimeSpan MoveSpeed = TimeSpan.FromSeconds(0.5);
        int ShowTime = 1500;
        public new void Show()
        {
            Width = StaticResource.ScreenWidth - 100;
            Height = 50;
            Top = -Height;
            Left = (StaticResource.ScreenWidth - Width) / 2;
            base.Show();
            DoubleAnimation animaA = new DoubleAnimation(0, new Duration(MoveSpeed));
            BeginAnimation(LBCMessageBox.TopProperty, animaA);

            //DoubleAnimation animaB = new DoubleAnimation(-Height, new Duration(MoveSpeed));
            //int showtime = MoveSpeed.Seconds * 1000 + ShowTime;
            //int mspeed = MoveSpeed.Seconds;
            //ThreadPool.QueueUserWorkItem(c =>
            //{
            //    Thread.Sleep(showtime);
            //    Dispatcher.Invoke(() =>
            //    {
            //        BeginAnimation(LBCMessageBox.TopProperty, animaB);
            //    });
            //    Thread.Sleep(mspeed + 1000);
            //    Dispatcher.Invoke(Close);
            //});
        }
        public new void Close()
        {
            ThreadPool.QueueUserWorkItem(c =>
            {
                int mspeed = MoveSpeed.Seconds;
                Dispatcher.Invoke(() =>
                {
                    DoubleAnimation animaB = new DoubleAnimation(-Height, new Duration(MoveSpeed));
                    BeginAnimation(LBCMessageBox.TopProperty, animaB);
                });
                Thread.Sleep(mspeed + 1000);
                Dispatcher.Invoke(base.Close);
            });
        }
        public void Close(int WaitTime)
        {
            ThreadPool.QueueUserWorkItem(q =>
            {
                Thread.Sleep(WaitTime);
                Close();
            });
        }
        public static void Show(Object Content)
        {
            LBCMessageBox box = new LBCMessageBox();
            box.CONTENT_Content.Content = Content;
            box.Show();
            box.Close(box.MoveSpeed.Seconds * 1000 + box.ShowTime);
        }
        public static bool? ShowDialog(Object Content)
        {
            LBCMessageBox box = new LBCMessageBox();
            box.Width = StaticResource.ScreenWidth - 100;
            box.Height = 70;
            box.Top = -box.Height;
            box.Left = (StaticResource.ScreenWidth - box.Width) / 2;
            DoubleAnimation animaA = new DoubleAnimation(0, new Duration(box.MoveSpeed));
            box.BeginAnimation(LBCMessageBox.TopProperty, animaA);

            box.CONTENT_Content.Content = Content;
            box.BT_Yes.Visibility = Visibility.Visible;
            box.BT_No.Visibility = Visibility.Visible;
            bool? result = box.ShowDialog();
            box.Close();
            return result;
        }

        private void BT_Yes_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void BT_No_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
