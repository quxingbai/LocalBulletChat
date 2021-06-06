using LocalBulletChat.Controls.Tool;
using LocalBulletChat.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LocalBulletChat.Server
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public CDSocketUDP Server = new CDSocketUDP(StaticResource.ServerPort);
        public Thread ClienTimeOutThread;
        public Dictionary<EndPoint, DateTime> OnLineUsers = new Dictionary<EndPoint, DateTime>();
        public List<EndPoint> BlackMembers = new List<EndPoint>();
        public MainWindow()
        {
            InitializeComponent();
            //版本确认
            if (DateTime.Now.Year != 2021) throw new Exception("Year Old 版本过旧");
            Server.GetNewMessage += Server_GetNewMessage;
            Server.StartGetMessage();
            (ClienTimeOutThread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(StaticResource.ClientTimeOut * 1000);
                    EndPoint[] keys = OnLineUsers.Keys.ToArray();
                    foreach (EndPoint k in keys)
                    {
                        if ((DateTime.Now.Ticks-OnLineUsers[k].Ticks) > 10000000 * (StaticResource.ClientTimeOut+1))
                        {
                            Server.SendTo(IsOnLine.Server_Send(false).ToByte(), k);
                            foreach (var item in LIST_OnlineUsers.Items)
                            {
                                if (item.GetType().GetProperty("IpAddress").GetValue(item).Equals(k))
                                {
                                    Dispatcher.Invoke(() =>
                                    {
                                        LIST_OnlineUsers.Items.Remove(item);
                                    });
                                    break;
                                }
                            }
                            OnLineUsers.Remove(k);
                        }
                    }
                }
            })).Start();
        }
        private void Server_GetNewMessage(byte[] Content, MessageBase Message, System.Net.EndPoint FromIP)
        {
            if (BlackMembers.Where(ip => FromIP == ip).Count() > 0) return;//如果是黑名单就拒收
            String TagMessage = "";
            if (Message.MessageType == SocketMessageType.IsOnLine)
            {
                IsOnLine onlin = IsOnLine.ToModel<IsOnLine>(Content);
                if (OnLineUsers.Keys.Where(k => k.Equals(FromIP)).Count() > 0)
                {
                    OnLineUsers[FromIP] = DateTime.Now;
                }
                else
                {
                    OnLineUsers.Add(FromIP, DateTime.Now);
                    Dispatcher.Invoke(() =>
                    {
                        LIST_OnlineUsers.Items.Add(new
                        {
                            IpAddress = FromIP,
                            Content = onlin,
                        });
                    });
                }
                TagMessage = "在线确认消息";
            }
            else if (Message.MessageType == SocketMessageType.SelectServer)
            {
                Server.SendTo(SelectServer.Server_Return(StaticResource.IPV4Address + ":" + StaticResource.ServerPort).ToByte(), FromIP);
            }
            else if (Message.MessageType == SocketMessageType.BulletChat)
            {
                BulletChatModel bullet = BulletChatModel.ToModel<BulletChatModel>(Content);
                TagMessage = $"{bullet.SendUser}发送的弹幕：{bullet.Message}";
                foreach (EndPoint user in OnLineUsers.Keys)
                {
                    if (BlackMembers.Where(ip => ip.Equals(user)).Count() > 0)
                    {
                        continue;
                    }
                    Server.SendTo(Content, user);
                }
            }
            Dispatcher.Invoke(() =>
            {
                if (LIST_Messages.Items.Count >= 10)
                {
                    LIST_Messages.Items.Clear();
                }
                LIST_Messages.Items.Add(new
                {
                    Tag = TagMessage,
                    IpAddress = FromIP,
                    MessageType = Message.MessageType,
                    Length = Content.Length,
                });
            });
        }
        protected override void OnClosed(EventArgs e)
        {
            try
            {
                Server.Dispose();
                ClienTimeOutThread.Abort();
                Process.GetProcessesByName("LocalBulletChat.Server").First().Kill();
            }
            catch
            {

            }
            base.OnClosed(e);
        }
    }
}
