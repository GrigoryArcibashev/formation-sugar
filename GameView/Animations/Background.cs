using System;
using System.Drawing;
using System.IO;

namespace View.Animations
{
    public static class Background
    {
        public static readonly Bitmap BackGroundForTheGame = new Bitmap(
            Path.Combine(
                new DirectoryInfo(Directory.GetCurrentDirectory()).Parent?.Parent?.FullName ??
                throw new Exception("failed to load background"),
                @"Sprites\gameBackground\b.png"));
    }
}