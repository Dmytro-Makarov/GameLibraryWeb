using System;
using System.Drawing;
using System.Collections.Generic;

namespace GameLibWeb
{
    public partial class Librarymedium
    {
        public uint Id { get; set; }
        public string? Media { get; set; }
        public static Image LoadBase64(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }
            return image;
        }

        public static string ToBase64(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                byte[] imageBytes = ms.ToArray();

                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public virtual Developer? Developer { get; set; }
        public virtual Game? Game { get; set; }
        public virtual Publisher? Publisher { get; set; }
    }
}
