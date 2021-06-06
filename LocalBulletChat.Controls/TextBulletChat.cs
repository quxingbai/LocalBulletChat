using LocalBulletChat.Controls.Forms;
using LocalBulletChat.Controls.Forms.BulletForms;
using LocalBulletChat.Controls.Tool;
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

namespace LocalBulletChat.Controls
{
    public class TextBulletChat : ContentControl
    {

        public enum Direction
        {
            Left = 1,
            Top = 2,
            Right = 3,
            Bottom = 4,
        }

        public Direction FromDirection
        {
            get { return (Direction)GetValue(FromDirectionProperty); }
            set { SetValue(FromDirectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FromDirection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FromDirectionProperty =
            DependencyProperty.Register("FromDirection", typeof(Direction), typeof(TextBulletChat), new PropertyMetadata(Direction.Right));



        public Direction ToDirection
        {
            get { return (Direction)GetValue(ToDirectionProperty); }
            set { SetValue(ToDirectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ToDirection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToDirectionProperty =
            DependencyProperty.Register("ToDirection", typeof(Direction), typeof(TextBulletChat), new PropertyMetadata(Direction.Left));

        //获取开始的位置
        public Double FromPosition
        {
            get
            {
                switch (FromDirection)
                {
                    case Direction.Left: return -StaticResource.ScreenWidth; break;
                    case Direction.Top: return -StaticResource.ScreenHeight; break;
                    //case Direction.Left: return -Width; break;
                    //case Direction.Top: return -Height; break;
                    //case Direction.Right: return StaticResource.ScreenWidth + Width; break;
                    //case Direction.Bottom: return StaticResource.ScreenHeight + Height; break;

                    case Direction.Right: return StaticResource.ScreenWidth; break;
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
                    case Direction.Left: return Canvas.LeftProperty; break;
                    case Direction.Top: return Canvas.TopProperty; break;
                    case Direction.Right: return Canvas.LeftProperty; break;
                    case Direction.Bottom: return Canvas.TopProperty; break;
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
                    case Direction.Left: return -StaticResource.ScreenWidth; break;
                    case Direction.Top: return -StaticResource.ScreenHeight; break;
                    case Direction.Right: return StaticResource.ScreenWidth; break;
                    case Direction.Bottom: return StaticResource.ScreenHeight; break;
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
                    case Direction.Left: return Canvas.LeftProperty; break;
                    case Direction.Top: return Canvas.TopProperty; break;
                    case Direction.Right: return Canvas.LeftProperty; break;
                    case Direction.Bottom: return Canvas.TopProperty; break;
                }
                return null;
            }
        }

        public bool MoveAnimaState
        {
            get { return (bool)GetValue(MoveAnimaStateProperty); }
            set { SetValue(MoveAnimaStateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MoveAnimaState.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MoveAnimaStateProperty =
            DependencyProperty.Register("MoveAnimaState", typeof(bool), typeof(TextBulletChat), new PropertyMetadata(true));



        public ICommand CopyContent
        {
            get { return (ICommand)GetValue(CopyContentProperty); }
            set { SetValue(CopyContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CopyContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CopyContentProperty =
            DependencyProperty.Register("CopyContent", typeof(ICommand), typeof(TextBulletChat), new PropertyMetadata(null));



        public static Double RightOffset { get; set; } = 100;
        public Double BulletSpeed { get; set; } = 17;//弹幕存在时间 秒数
        private Storyboard MoveAnima = new Storyboard();
        private static Random rd = new Random();
        private Action<TextBulletChat> RemoveCallBack;
        private BulletChatModel BulletSource { get; set; }
        private bool IsReSetRemoveTime { get; set; } = false;//表示是否延时删除
        static TextBulletChat()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBulletChat), new FrameworkPropertyMetadata(typeof(TextBulletChat)));
        }
        public TextBulletChat(Direction FromDirection, Direction ToDirection, BulletChatModel Bullet, Action<TextBulletChat> RemoveCallBack)
        {
            Visibility = Visibility.Collapsed;
            this.FromDirection = FromDirection;
            this.ToDirection = ToDirection;

            Content = $"{Bullet.SendUser}：{Bullet.Message}";
            Foreground = (Brush)new BrushConverter().ConvertFromString(Bullet.Foreground);
            FontSize = Bullet.FontSize;
            this.RemoveCallBack = RemoveCallBack;

            BulletSource = Bullet;

            MoveStart();
        }

        public void MoveStart()
        {

            if (FromDirection == Direction.Left || FromDirection == Direction.Right)
            {
                SetValue(Canvas.TopProperty, rd.Next(0, StaticResource.ScreenHeight - 50) * 1.0);
            }
            else
            {
                SetValue(Canvas.LeftProperty, rd.Next(0, StaticResource.ScreenWidth - 50) * 1.0);
            }
            ThreadPool.QueueUserWorkItem(ARG =>
            {
                Dispatcher.Invoke(() =>
                {
                    DoubleAnimation anima = new DoubleAnimation(FromPosition, ToPosition - (Encoding.UTF8.GetByteCount(Content.ToString()) * FontSize), new Duration(TimeSpan.FromSeconds(BulletSpeed)));
                    MoveAnima.Children.Add(anima);
                    MoveAnima.RepeatBehavior = new RepeatBehavior(1);
                    Storyboard.SetTargetProperty(anima, new PropertyPath(Canvas.LeftProperty));
                    MoveAnima.Begin(this, true);
                    Visibility = Visibility.Visible;
                });
                while (true)
                {
                    Thread.Sleep((int)BulletSpeed * 1000 + 4000);
                    if (IsReSetRemoveTime)
                    {
                        IsReSetRemoveTime = false;
                        continue;
                    }
                    else
                    {
                        RemoveCallBack?.Invoke(this);
                        break;
                    }
                }
            });

        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            CopyContent = new ActionCommand(() =>
              {
                  Clipboard.SetText(BulletSource.Message);
                  LBCMessageBox.Show(new TextBlock()
                  {
                      TextWrapping = TextWrapping.Wrap,
                      Text ="复制成功\n"+ Clipboard.GetText()
                  }) ;
              });
        }
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == MoveAnimaStateProperty)
            {
                if (MoveAnimaState)
                {
                    MoveAnima.Resume(this);
                }
                else
                {
                    MoveAnima.Pause(this);
                    IsReSetRemoveTime = true;
                }
            }
            base.OnPropertyChanged(e);
        }
    }
}
