using LocalBulletChat.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LocalBulletChat.Controls.Tool
{
    public delegate void GetClientMessage(byte[] Content, MessageBase Message, EndPoint FromIP);
    public class CDSocketUDP : IDisposable
    {
        //是否已经释放内存
        private Thread GetMessageThread = null;
        public Socket TheSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        public event GetClientMessage GetNewMessage;
        public bool IsDispose { get; private set; }
        public CDSocketUDP(int Port)
        {
            TheSocket.Bind(new IPEndPoint(IPAddress.Parse(StaticResource.IPV4Address.ToString()), Port));
        }
        public void StartGetMessage()
        {
            EndPoint FromIP = new IPEndPoint(0, 0);
            byte[] bs = new byte[StaticResource.SocketMaxByte];
            int blen = 0;
            (GetMessageThread = new Thread(() =>
            {
                try
                {

                    while ((blen = TheSocket.ReceiveFrom(bs, ref FromIP)) > 0)
                    {
                        try
                        {

                            byte[] bsbuf = new MemoryStream(bs, 0, blen).ToArray();
                            MessageBase msg = MessageBase.ToModel(bsbuf);
                            GetNewMessage?.Invoke(bsbuf, msg, FromIP);
                        }
                        catch (Exception error)
                        {
                            Debug.WriteLine("CDSocketUDP----" + error.Message);
                        }
                    }
                }
                catch(Exception error)
                {
                    //如果已经释放内存就不继续重新启动了
                    if (!IsDispose)
                    {
                        StartGetMessage();
                    }
                }
            })).Start();
        }
        public int SendTo(byte[] Msg,EndPoint Target)
        {
           return TheSocket.SendTo(Msg, Target);
        }
        public void Dispose()
        {
            try
            {
                IsDispose = true;
                GetMessageThread.Abort();
                TheSocket.Dispose();
            }
            catch
            {

            }
        }
    }
}
