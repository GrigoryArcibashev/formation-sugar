using System.IO;

namespace View.Animations
{
    public static class PlayerHealthAnimation
    {
        public static readonly Animation HearthAnimation = new Animation(new DirectoryInfo(
            Path.Combine(
                new DirectoryInfo(Directory.GetCurrentDirectory()).Parent?.Parent?.FullName!,
                @"Sprites\heartAnimations")));
    }
}