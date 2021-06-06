using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalBulletChat.Model
{
    public class BulletChatModel:MessageBase
    {
        public String SendUser { get; set; }
        public String CreateDate { get; set; }
        public String Message { get; set; }
        public String Foreground { get; set; }
        public Double FontSize { get; set; }
    }
}
