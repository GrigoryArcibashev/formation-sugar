using System.Drawing;
using System.IO;

namespace View.Animations
{
    public static class Background
    {
        public static readonly Bitmap BackGroundForTheGame = new Bitmap(
            Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent?.Parent?.FullName ?? string.Empty,
            @"Sprites\gameBackground\b.png"));
    }
}