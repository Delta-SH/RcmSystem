using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Rcm.Core {
    public partial class CommonHelper {
        public static string CaptchaId {
            get { return "rcms.login.captcha"; }
        }

        public static string CaptchaSalt {
            get { return "fn0yU7AvamI3MYwGZSLRRg=="; }
        }

        public static string GenerateCode(int length) {
            var chars = "abcdefghjkmnpqrstuvwxyz23456789ABCDEFGHJKLMNPQRSTUVWXYZ23456789".ToCharArray();

            var code = "";
            var index = 0;
            for(var i = 0; i < length; i++) {
                index = GenerateRandomInteger(0, chars.Length - 1);
                code += chars[index].ToString();
            }
            return code;
        }

        public static byte[] CreateCaptcha(string code, int width = 100, int height = 30) {
            if(string.IsNullOrWhiteSpace(code)) throw new ArgumentNullException("code");

            Graphics g = null;
            Font font = null;
            HatchBrush hatchBrush = null;
            try {
                var image = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                g = Graphics.FromImage(image);
                g.SmoothingMode = SmoothingMode.AntiAlias;

                var rect = new Rectangle(0, 0, width, height);
                hatchBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White);
                g.FillRectangle(hatchBrush, rect);

                SizeF size;
                var fontSize = rect.Height + 1f;
                do {
                    fontSize--;
                    font = new Font("Arial", fontSize, FontStyle.Bold);
                    size = g.MeasureString(code, font);
                } while(size.Width > rect.Width);

                var format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;

                var path = new GraphicsPath();
                path.AddString(code, font.FontFamily, (int)font.Style, font.Size + 5, rect, format);

                var v = 4f;
                var random = new Random();
                var points = new PointF[] {
                    new PointF(random.Next(rect.Width) / v, random.Next(rect.Height) / v),
                    new PointF(rect.Width - random.Next(rect.Width) / v, random.Next(rect.Height) / v),
                    new PointF(random.Next(rect.Width) / v, rect.Height - random.Next(rect.Height) / v),
                    new PointF(rect.Width - random.Next(rect.Width) / v, rect.Height - random.Next(rect.Height) / v)
                };

                var matrix = new Matrix(); matrix.Translate(0F, 0F);
                path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);

                hatchBrush.Dispose();
                hatchBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.DarkGray);
                g.FillPath(hatchBrush, path);

                var m = Math.Max(rect.Width, rect.Height);
                var ct = (int)(rect.Width * rect.Height / 8F);
                for(var i = 0; i < ct; i++) {
                    var x = random.Next(rect.Width);
                    var y = random.Next(rect.Height);
                    var w = random.Next(m / 30);
                    var h = random.Next(m / 30);
                    g.FillEllipse(hatchBrush, x, y, w, h);
                }

                var bzct = random.Next(0, 3);
                for(int i = 0; i < bzct; i++) {
                    var p1 = new PointF(random.Next(rect.Width / 8), random.Next(rect.Height));
                    var p2 = new PointF(random.Next(rect.Width / 8), random.Next(rect.Height / 4));
                    var p3 = new PointF(random.Next(rect.Width - rect.Width / 2, rect.Width), random.Next(rect.Height));
                    var p4 = new PointF(random.Next(rect.Width - rect.Width / 2, rect.Width), random.Next(rect.Height));
                    g.DrawBezier(new Pen(Color.DarkGray, 1.6f), p1, p2, p3, p4);
                }

                using(var ms = new MemoryStream()) {
                    image.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }
            } finally {
                if(font != null) font.Dispose();
                if(hatchBrush != null) hatchBrush.Dispose();
                if(g != null) g.Dispose();
            }
        }

        public static string GenerateRandomDigitCode(int length) {
            var random = new Random();
            string str = string.Empty;
            for(int i = 0; i < length; i++)
                str = String.Concat(str, random.Next(10).ToString());
            return str;
        }

        public static int GenerateRandomInteger(int min = 0, int max = int.MaxValue) {
            var randomNumberBuffer = new byte[10];
            new RNGCryptoServiceProvider().GetBytes(randomNumberBuffer);
            return new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(min, max);
        }

        public static string CreateHash(string plainText, string saltkey = "", string format = "SHA1") {
            var text = String.Concat(plainText, saltkey);

            if(String.IsNullOrWhiteSpace(format))
                format = "SHA1";

            //return FormsAuthentication.HashPasswordForStoringInConfigFile(text, format);
            var algorithm = HashAlgorithm.Create(format);
            if(algorithm == null)
                throw new ArgumentException("Unrecognized hash name");

            var hashByteArray = algorithm.ComputeHash(Encoding.UTF8.GetBytes(text));
            return BitConverter.ToString(hashByteArray).Replace("-", "");
        }

        public static string DateTimeConverter(DateTime val) {
            if(!IsValidDateTime(val))
                return string.Empty;

            return val.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string DateConverter(DateTime val) {
            if(!IsValidDateTime(val))
                return string.Empty;

            return val.ToString("yyyy-MM-dd");
        }

        public static string TimeConverter(DateTime val) {
            if(!IsValidDateTime(val))
                return string.Empty;

            return val.ToString("HH:mm:ss");
        }

        public static string ShortTimeConverter(DateTime val) {
            if(!IsValidDateTime(val))
                return string.Empty;

            return val.ToString("mm′ss″");
        }

        public static string IntervalConverter(DateTime start, DateTime? end = null) {
            if(start == default(DateTime)) { return String.Empty; }
            if(!end.HasValue) { end = DateTime.Now; }
            var ts = end.Value.Subtract(start);
            return String.Format("{0:0000}.{1:00}:{2:00}:{3:00}", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
        }

        public static bool IsValidDateTime(DateTime val) {
            if(val == default(DateTime))
                return false;

            /*
             * 解决JsonSerializer.DeserializeFromString方法转换默认时间[default(DateTime)]会自动加上时区(+8H)
             * 在使用Redis缓存时，NServiceKit默认会使用JsonSerializer.SerializeToString方法对要缓存的数据进行Json序列化
             * 在获取缓存时，NServiceKit默认会使用JsonSerializer.DeserializeFromString方法对已缓存的数据进行Json反序列化
             * 它会将0001-01-01 00:00:00 反序列号为 0001-01-01 08:00:00
             */
            if(val == new DateTime(1, 1, 1, 8, 0, 0))
                return false;

            return true;
        }
    }
}
