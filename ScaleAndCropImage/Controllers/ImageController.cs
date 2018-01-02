using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScaleAndCropImage.Controllers
{
    public class ImageController : Controller
    {
        // GET: Image
        public ActionResult Index(string FileName)
        {
            var filePath = Path.GetDirectoryName(Path.GetDirectoryName(Server.MapPath("~\\Images\\")));
            Image image = Image.FromFile(filePath + FileName);
            Image newImage = ScaleAndCropImage(image, 800, 300); 
            image.Dispose();
            using (MemoryStream ms = new MemoryStream())
            {
                newImage.Save(ms, ImageFormat.Png);
                return File(ms.ToArray(), "image/png");
            }           
        }

        public ActionResult Size(string FileName, int Width, int Height )
        {
            var filePath = Path.GetDirectoryName(Path.GetDirectoryName(Server.MapPath("~\\Images\\")));
            Image image = Image.FromFile(filePath + FileName);
            Image newImage = ScaleAndCropImage(image, Width, Height);
            image.Dispose();
            using (MemoryStream ms = new MemoryStream())
            {
                newImage.Save(ms, ImageFormat.Png);
                return File(ms.ToArray(), "image/png");
            }
        }

        private Image ScaleAndCropImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Max(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);


            double extraWidth = newImage.Width - maxWidth;
            double extraHeight = newImage.Height - maxHeight;

            double cropStartFromX = (newImage.Width - maxWidth) / 2;
            double cropStartFromY = (newImage.Height - maxHeight) / 2;

            Bitmap bmp = new Bitmap((int)(newImage.Width - extraWidth), (int)(newImage.Height - extraHeight));
            Graphics grp = Graphics.FromImage(bmp);
            grp.DrawImage(newImage, new Rectangle(0, 0, (int)(newImage.Width - extraWidth), (int)(newImage.Height - extraHeight)), new Rectangle((int)cropStartFromX, (int)cropStartFromY, (int)(newImage.Width - extraWidth), (int)(newImage.Height - extraHeight)), GraphicsUnit.Pixel);
            return bmp;

        }
    }
}