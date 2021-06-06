using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LocalBulletChat.Model
{
    public class StaticResource
    {
        public static int ScreenWidth { get => Screen.AllScreens[0].Bounds.Width; }
        public static int ScreenHeight { get => Screen.AllScreens[0].Bounds.Height; }
        public static String UserName { get; set; } = "用户";
        public const int SocketMaxByte = 1024 * 1024 * 50;
        public const int ClientPort = 5002;
        public const int ServerPort = 5003;
        public const int ClientTimeOut = 5;//连接超时 /秒
        public static EndPoint ServerIpAddress { get; set; }
        public static IPAddress IPV4Address
        {
            get
            {
                return Dns.GetHostAddresses(Dns.GetHostName()).Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).Last();
            }
        }
    }
}
