using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace LocalBulletChat.Controls.Tool
{
    public class IoTool
    {
        public static byte[] ReadByteBlock(String Path, int Size, int Count)
        {
            FileStream file = new FileStream(Path, FileMode.Open);
            byte[] bs = new byte[Size];
            try
            {
                file.Position = Size * (Count - 1);
                if (Size * Count >= file.Length)
                {
                    int NewByteLen = (int)(file.Length - file.Position);
                    if (NewByteLen < 0) return new byte[0];
                    file.Read(bs, 0, NewByteLen);
                    bs = new MemoryStream(bs, 0, NewByteLen).ToArray();
                }
                else
                {
                    file.Read(bs, 0, Size);
                }
            }
            catch
            {

            }
            file.Dispose();
            return bs;
        }
    }
}
