using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace LocalBulletChat.Model
{
    public class SelectServer:MessageBase
    {
        public String SendDate { get; set; } = DateTime.Now.ToString();
        public String IpAddress { get; set; }
        public SelectServer()
        {
        }
        /// <summary>
        /// 服务器的返回确认消息 
        /// </summary>
        /// <param name="IpAddress">服务器地址</param>
        /// <returns></returns>
        public static SelectServer Server_Return(String IpAddress)
        {
            return new SelectServer()
            {
                MessageResultType = MessageResultType.Return,
                MessageType = SocketMessageType.SelectServer,
                IpAddress=IpAddress,
            };
        }

        public static SelectServer Client_Submit()
        {
            return new SelectServer()
            {
                MessageResultType = MessageResultType.Submit,
                MessageType = SocketMessageType.SelectServer,
            };
        }

    }
}
