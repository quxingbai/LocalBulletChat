using LocalBulletChat.Controls.Forms;
using LocalBulletChat.Controls.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LocalBulletChat.Controls
{
    public class CommandIcon : Button
    {




        public ImageSource IconSource
        {
            get { return (ImageSource)GetValue(IconSourceProperty); }
            set { SetValue(IconSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconSourceProperty =
            DependencyProperty.Register("IconSource", typeof(ImageSource), typeof(CommandIcon), new PropertyMetadata(null));





        public String CommandMark
        {
            get { return (String)GetValue(CommandMarkProperty); }
            set { SetValue(CommandMarkProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandMark.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandMarkProperty =
            DependencyProperty.Register("CommandMark", typeof(String), typeof(CommandIcon), new PropertyMetadata(null));


        public Window ParentWindow { get => Window.GetWindow(this); }
        static CommandIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CommandIcon), new FrameworkPropertyMetadata(typeof(CommandIcon)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == CommandMarkProperty)
            {
                switch (e.NewValue.ToString().ToUpper())
                {
                    case "PARENTWINDOW_CLOSE":
                        Command = new ActionCommand(() =>
                        {
                            if (LBCMessageBox.ShowDialog("关闭确认：关闭？") ?? false)
                            {
                                ParentWindow.Close();
                            }
                        }); break;
                    case "PARENTWINDOW_MIN": Command = new ActionCommand(() => { ParentWindow.WindowState = WindowState.Minimized; }); ; break;
                }
            }
            base.OnPropertyChanged(e);
        }
    }
}
