using System.Drawing;

namespace GameLibWeb.Controllers;

public static class ImageConverter
{
    public static Image LoadBase64(string base64)
    {
        byte[] bytes = Convert.FromBase64String(base64);
        Image image;
        using MemoryStream ms = new MemoryStream(bytes);
        image = Image.FromStream(ms);
        return image;
    }

    public static string ToBase64(IFormFile? image)
    {
        using MemoryStream ms = new MemoryStream();
        image!.CopyTo(ms);
        byte[] imageBytes = ms.ToArray();

        string base64String = Convert.ToBase64String(imageBytes);
        return base64String;
    }

    public static string ToBase64(string imagePath)
    {
        return Convert.ToBase64String(File.ReadAllBytes(imagePath));
    }
}