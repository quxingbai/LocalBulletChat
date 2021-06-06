using LocalBulletChat.Controls.Forms;
using LocalBulletChat.Controls.Tool;
using LocalBulletChat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LocalBulletChat
{
    /// <summary>
    /// ServerSelect.xaml 的交互逻辑
    /// </summary>
    public partial class ServerSelect : Window
    {
        private CDSocketUDP SocketUDP = new CDSocketUDP(StaticResource.ClientPort);
        private Thread SelectServerThread;
        public ServerSelect()
        {
            InitializeComponent();
            TEXT_Name.Text = StaticResource.IPV4Address.ToString();
            SocketUDP.GetNewMessage += SocketUDP_GetNewMessage;
            SocketUDP.StartGetMessage();
            StartSelectServer();
        }
        private void SocketUDP_GetNewMessage(byte[] Content, MessageBase Message, EndPoint FromIP)
        {
            bool IsIt = false;
            Dispatcher.Invoke(() =>
            {
                foreach (var item in LIST_Servers.Items)
                {
                    if ((item as EndPoint).ToString() == FromIP.ToString())
                    {
                        IsIt = true;
                        break;
                    }
                }
                if (!IsIt)
                {
                    LIST_Servers.Items.Add(FromIP);
                    GRID_Loading.Visibility = Visibility.Collapsed;
                    LBCMessageBox.Show($"查找到服务器{FromIP}");
                }
            });
        }
        public void StartSelectServer()
        {

            (SelectServerThread = new Thread(() =>
            {
                int[] IpVal = StaticResource.IPV4Address.ToString().Split('.').Select(val => Convert.ToInt32(val)).ToArray();
                if (IpVal.First() == 10)//A
                {
                    //IpVal = new int[] { 10, 0, 0, 0 };

                }
                else if (IpVal.First() == 172)//B
                {
                    IpVal = new int[] { 172, 16, 0, 0 };
                    while (IpVal[1] <= 31)
                    {
                        SendServerSelect($"{IpVal[0]}.{IpVal[1]}.{IpVal[2]}.{IpVal[3]}", StaticResource.ServerPort);
                        if (IpVal[3] < 255)
                        {
                            IpVal[3] += 1;
                        }
                        else
                        {
                            IpVal[3] = 0;
                            if (IpVal[2] < 255)
                            {
                                IpVal[2] += 1;
                            }
                            else
                            {
                                IpVal[1] += 1;
                            }
                        }
                    }
                }
                else if (IpVal.First() == 192)//C
                {
                    IpVal = new int[] { 192, 168, 0, 0 };
                    while (IpVal[2] <= 255)
                    {
                        //LIST_Servers.Items.Add(IpVal[0] + "." + IpVal[1] + "." + IpVal[2] + "." + IpVal[3]);
                        SendServerSelect($"{IpVal[0]}.{IpVal[1]}.{IpVal[2]}.{IpVal[3]}", StaticResource.ServerPort);
                        if (IpVal[3] < 255)
                        {
                            IpVal[3] += 1;
                        }
                        else
                        {
                            IpVal[3] = 0;
                            if (IpVal[2] <= 255)
                            {
                                IpVal[2] += 1;
                            }
                        }
                    }
                }

            })).Start();
        }
        void SendServerSelect(String Ip, int Port)
        {
            SocketUDP.SendTo(SelectServer.Client_Submit().ToByte(), new IPEndPoint(IPAddress.Parse(Ip), Port));
        }
        private void BT_ServerItem_Click(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            //if (MessageBox.Show($"连接服务器->{bt.DataContext}", "连接确认", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            //{
            //    SelectServerThread.Abort();
            //    SocketUDP.Dispose();
            //    Hide();
            //    StaticResource.ServerIpAddress = (bt.DataContext as EndPoint);
            //    StaticResource.UserName = TEXT_Name.Text;
            //    new MainWindow().Show();
            //    Close();
            //    LBCMessageBox.Show($"加入服务器{StaticResource.ServerIpAddress}");
            //}

            if (LBCMessageBox.ShowDialog($"连接确认：连接服务器->{bt.DataContext}\t用户名：{TEXT_Name.Text}") ??false)
            {
                SelectServerThread.Abort();
                SocketUDP.Dispose();
                Hide();
                StaticResource.ServerIpAddress = (bt.DataContext as EndPoint);
                StaticResource.UserName = TEXT_Name.Text;
                new MainWindow().Show();
                Close();
                LBCMessageBox.Show($"加入服务器{StaticResource.ServerIpAddress}");
            }

        }
        protected override void OnClosed(EventArgs e)
        {
            SocketUDP.Dispose();
            SelectServerThread.Abort();
            base.OnClosed(e);
        }
    }
}
