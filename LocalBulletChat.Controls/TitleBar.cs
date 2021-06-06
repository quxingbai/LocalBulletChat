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
    public class TitleBar : HeaderedContentControl
    {

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(TitleBar), new PropertyMetadata(null));





        private Window ParentWindow { get => Window.GetWindow(this); }
        static TitleBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TitleBar), new FrameworkPropertyMetadata(typeof(TitleBar)));
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if(e.LeftButton== MouseButtonState.Pressed)
            {
                ParentWindow.DragMove();
            }
            base.OnMouseMove(e);
        }
        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            ParentWindow.WindowState =ParentWindow.WindowState== WindowState.Maximized? WindowState.Normal: WindowState.Maximized;
            base.OnMouseDoubleClick(e);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
        }
    }
}
