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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LocalBulletChat.Controls.Forms.BulletForms
{
    public enum Direction
    {
        Left = 1,
        Top = 2,
        Right = 3,
        Bottom = 4,
    }

    //废弃的
    public class BulletBase : Window
    {


        public Direction FromDirection
        {
            get { return (Direction)GetValue(FromDirectionProperty); }
            set { SetValue(FromDirectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FromDirection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FromDirectionProperty =
            DependencyProperty.Register("FromDirection", typeof(Direction), typeof(BulletBase), new PropertyMetadata(Direction.Right));



        public Direction ToDirection
        {
            get { return (Direction)GetValue(ToDirectionProperty); }
            set { SetValue(ToDirectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ToDirection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToDirectionProperty =
            DependencyProperty.Register("ToDirection", typeof(Direction), typeof(BulletBase), new PropertyMetadata(Direction.Left));

        //获取开始的位置
        public Double FromPosition
        {
            get
            {
                switch (FromDirection)
                {
                    case Direction.Left: return -Width; break;
                    case Direction.Top: return -Height; break;
                    case Direction.Right: return StaticResource.ScreenWidth + RightOffset; break;
                    case Direction.Bottom: return StaticResource.ScreenHeight; break;
                }
                return 0;
            }
        }
        //获取开始移动的Property
        public DependencyProperty FromProperty
        {
            get
            {
                switch (FromDirection)
                {
                    case Direction.Left: return LeftProperty; break;
                    case Direction.Top: return TopProperty; break;
                    case Direction.Right: return LeftProperty; break;
                    case Direction.Bottom: return TopProperty; break;
                }
                return null;
            }
        }
        //获取结束点位置
        public Double ToPosition
        {
            get
            {
                switch (ToDirection)
                {
                    case Direction.Left: return -Width; break;
                    case Direction.Top: return -Height; break;
                    case Direction.Right: return StaticResource.ScreenWidth + Width; break;
                    case Direction.Bottom: return StaticResource.ScreenHeight + Height; break;
                }
                return 0;
            }
        }
        //获取结束点Property
        public DependencyProperty ToProperty
        {
            get
            {
                switch (ToDirection)
                {
                    case Direction.Left: return LeftProperty; break;
                    case Direction.Top: return TopProperty; break;
                    case Direction.Right: return LeftProperty; break;
                    case Direction.Bottom: return TopProperty; break;
                }
                return null;
            }
        }
        public Double BulletSpeed { get; set; } = 17;//弹幕存在时间 秒数
        public static Double RightOffset { get; set; } = 100;
        static BulletBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BulletBase), new FrameworkPropertyMetadata(typeof(BulletBase)));
        }
        public BulletBase() { }
        public BulletBase(BulletChatModel msg, Direction From = Direction.Right, Direction To = Direction.Left, bool IsShowAuto = true)
        {
            String msgContent = msg.SendUser + "：" + msg.Message;
            this.Foreground = new BrushConverter().ConvertFromString(msg.Foreground) as Brush;
            this.FontSize = msg.FontSize;
            this.Content = msgContent;
            this.FromDirection = From;
            this.ToDirection = To;
            //Width = FontSize * (Encoding.UTF8.GetBytes(msg.Message).Length);
            ShowAuto();
        }
        public void MoveStart()
        {
            CreateAnima(FromProperty, FromPosition, ToPosition, BulletSpeed);
            ThreadPool.QueueUserWorkItem(c =>
            {
                Thread.Sleep((int)BulletSpeed * 1000);
                Dispatcher.Invoke(Close);
            });
        }
        public void CreateAnima(DependencyProperty Property, Double From, Double To, Double Second)
        {
            BeginAnimation(Property, new DoubleAnimation(From, To, new Duration(TimeSpan.FromSeconds(Second))));
        }
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == FromDirectionProperty)
            {
                SetValue(FromProperty, FromPosition);
            }
            base.OnPropertyChanged(e);
        }
        //自动设置启动弹幕
        public void ShowAuto()
        {
            SetValue(FromProperty, FromPosition);
            this.Show();
            MoveStart();
        }

    }
}
