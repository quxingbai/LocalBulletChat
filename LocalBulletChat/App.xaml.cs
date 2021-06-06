using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LocalBulletChat
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void StaticWindow_BD_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
           var ParentWindow =  Window.GetWindow(sender as Border);
            if(e.LeftButton== System.Windows.Input.MouseButtonState.Pressed)
            {
                if(ParentWindow.WindowState== WindowState.Maximized)
                {
                    ParentWindow.WindowState = WindowState.Normal;
                }
                ParentWindow.Dispatcher.Invoke(() =>
                {
                    ParentWindow.DragMove();
                });
            }
        }
    }
}
