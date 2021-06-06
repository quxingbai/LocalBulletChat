using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalBulletChat.Model
{
    public class IsOnLine:MessageBase
    {
        public bool Online { get; set; }
        public static IsOnLine Client_Send()
        {
            return new IsOnLine() { Online = true ,MessageResultType= MessageResultType.Submit,MessageType= SocketMessageType.IsOnLine};
        }
        public static IsOnLine Server_Send(bool Online)
        {
            return new IsOnLine() { Online = Online,MessageResultType= MessageResultType.Return,MessageType= SocketMessageType.IsOnLine };
        }
    }
}
