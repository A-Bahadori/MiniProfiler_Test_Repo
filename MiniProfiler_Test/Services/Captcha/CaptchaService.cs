using System.Drawing;
using System.Drawing.Imaging;
using MiniProfiler_Test.Interfaces.Captcha;

namespace MiniProfiler_Test.Services.Captcha;

 public class CaptchaService : ICaptchaService
 {
     public (string Code, byte[] Image) GenerateImageCaptcha()
     {
         var random = new Random();
         const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
         string captchaCode = new string(Enumerable.Repeat(chars, 6)
             .Select(s => s[random.Next(s.Length)]).ToArray());

         using var bitmap = new Bitmap(250, 80);
         using var graphics = Graphics.FromImage(bitmap);
         graphics.Clear(Color.LightGray);

         AddNoise(graphics, bitmap.Width, bitmap.Height, random);

         AddRandomLines(graphics, bitmap.Width, bitmap.Height, random);

         using var font = new Font("Arial", 28, FontStyle.Bold | FontStyle.Italic);
         DrawRotatedText(graphics, captchaCode, font, random, bitmap.Width, bitmap.Height);

         using var ms = new MemoryStream();
         bitmap.Save(ms, ImageFormat.Png);
         var captchaImage = ms.ToArray();

         return (captchaCode, captchaImage);
     }

     #region Utilities

     private void AddNoise(Graphics graphics, int width, int height, Random random)
     {
         for (int i = 0; i < 150; i++)
         {
             int x = random.Next(width);
             int y = random.Next(height);
             int r = random.Next(256);
             int g = random.Next(256);
             int b = random.Next(256);
             graphics.FillEllipse(new SolidBrush(Color.FromArgb(r, g, b)), x, y, 3, 3);
         }
     }

     private void AddRandomLines(Graphics graphics, int width, int height, Random random)
     {
         using var pen = new Pen(Color.Gray, 3);
         for (int i = 0; i < 8; i++)
         {
             int x1 = random.Next(width);
             int y1 = random.Next(height);
             int x2 = random.Next(width);
             int y2 = random.Next(height);
             graphics.DrawLine(pen, x1, y1, x2, y2);
         }
     }

     private void DrawRotatedText(Graphics graphics, string text, Font font, Random random, int width, int height)
     {
         for (int i = 0; i < text.Length; i++)
         {
             using var brush = new SolidBrush(Color.FromArgb(random.Next(256), random.Next(256), random.Next(256)));

             float xPosition = 20 + (i * 35);
             float yPosition = random.Next(10, 30);
             float angle = random.Next(-30, 30);

             graphics.TranslateTransform(xPosition, yPosition);
             graphics.RotateTransform(angle);

             graphics.DrawString(text[i].ToString(), font, brush, 0, 0);

             graphics.RotateTransform(-angle);
             graphics.TranslateTransform(-xPosition, -yPosition);
         }
     }

     #endregion
 }