using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocalBulletChat.Model
{
    public enum MessageResultType
    {
        Submit=1,
        Return=2,
    }
    public enum SocketMessageType
    {
        BulletChat=1,
        IsOnLine=2,
        SelectServer=3,
    }
    public class MessageBase
    {
        public SocketMessageType MessageType { get; set; }
        public MessageResultType MessageResultType { get; set; }
        public static byte[] ToByte(MessageBase Msg)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Msg));
        }
        public byte[] ToByte()
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
        }
        public static MessageBase ToModel(Byte[] bs)
        {
            return JsonConvert.DeserializeObject<MessageBase>(Encoding.UTF8.GetString(bs));
        }
        public static T ToModel<T>(Byte[] bs)
        {
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bs));
        }
    }
}
