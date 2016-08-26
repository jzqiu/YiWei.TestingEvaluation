using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using TCBase.Component.QRCode;

namespace YiWei.Utility
{
    public class QrCodeHelper
    {
        public static byte[] GetQrCode(string text, Bitmap logo)
        {   
            var stream = QRCodeGenerate.BuildQrCode(text, QrCodeLevel.H);
            Image code = Bitmap.FromStream(stream);
            Graphics g = Graphics.FromImage(code);
            if (logo!=null)
            {
                logo = new Bitmap(logo, 66, 66);
                g.DrawImage(logo, code.Width / 2 - 33, code.Height / 2 - 33);
            }
            g.Dispose();

            MemoryStream ms = new MemoryStream();
            code.Save(ms, ImageFormat.Jpeg);
            return ms.ToArray();
        }
    }
}
