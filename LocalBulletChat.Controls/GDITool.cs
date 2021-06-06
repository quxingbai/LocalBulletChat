using LocalBulletChat.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalBulletChat.Controls
{
    public class GDITool
    {
        public static byte[] GetScreenImage()
        {
            Bitmap map = new Bitmap(StaticResource.ScreenWidth, StaticResource.ScreenHeight);
            Graphics g = Graphics.FromImage(map);
            g.CopyFromScreen(0, 0, 0, 0, map.Size) ;
            g.Dispose();
            MemoryStream memory = new MemoryStream();
            map.Save(memory,System.Drawing.Imaging.ImageFormat.Png);
            map.Dispose();
            return memory.ToArray();
        }
        public static void SaveScreenImage(String FilePath)
        {
            Bitmap map = new Bitmap(StaticResource.ScreenWidth, StaticResource.ScreenHeight);
            Graphics g = Graphics.FromImage(map);
            g.CopyFromScreen(0, 0, 0, 0, map.Size);
            g.Dispose();
            map.Save(FilePath);
        }
    }
}
