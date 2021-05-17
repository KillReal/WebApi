using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi
{
    public static class Tools
    {
        public static IConfiguration Configuration { get; private set; }

        public static void Init(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static byte[] CorrectResolution(byte[] imageBytes)
        {
            Stream stream = new MemoryStream(imageBytes);
            Image image = new Bitmap(stream);
            Size size = image.Size;
            int width = Convert.ToInt32(Configuration["MaxPictureSize:Width"]);
            int height = Convert.ToInt32(Configuration["MaxPictureSize:Height"]);
            float[] div = new float[] { (float)size.Width / (float)width, (float)size.Height / (float)height };
            float maxDiv = Math.Max(div[0], div[1]);
            if (maxDiv > 1.0)
            {
                Bitmap bitmap = new Bitmap(image, new Size(Convert.ToInt32(size.Width / maxDiv), Convert.ToInt32(size.Height / maxDiv)));
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.Jpeg);
                imageBytes = ms.ToArray();
            }
            return imageBytes;
        }

        public static string UpperLetter(string text)
        {
            return Char.ToUpper(text[0]) + text[1..];
        }
    }
}
