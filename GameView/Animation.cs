using System.Drawing;
using System.IO;
using System.Linq;

namespace formation_sugar.View
{
    public class Animation
    {
        private readonly Bitmap[] sprites;
        private int numberOfCurrentSprite;
        private int CountOfSprites { get; }
        
        public Bitmap Current => sprites[numberOfCurrentSprite];
        
        public Animation(DirectoryInfo pathToImages)
        {
            sprites = pathToImages.EnumerateFiles().Select(image => new Bitmap(image.FullName)).ToArray();
            CountOfSprites = sprites.Length;
            numberOfCurrentSprite = 0;
        }
        
        public void MoveNextSprite()
        {
            numberOfCurrentSprite = (numberOfCurrentSprite + 1) % CountOfSprites;
        }

        public void ResetAnimation()
        {
            numberOfCurrentSprite = 0;
        }
    }
}