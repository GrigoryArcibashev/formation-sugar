using System.IO;

namespace View.AnimationsForCreatures
{
    public static class PlayerHealthAnimation
    {
        public static Animation HearthAnimation = new Animation(new DirectoryInfo(
            Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent?.Parent?.FullName,
                @"Sprites\heart")));
    }
}