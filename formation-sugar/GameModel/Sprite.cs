using System.Drawing;

namespace formation_sugar.GameModel
{
    public class Sprite
    {
        private readonly int frameCount;
        private int currentFrame;
        
        public readonly Image Image;
        public Size FrameSize;
        public bool Flipped;

        public Sprite(int frameCount, Size frameSize, Image image)
        {
            this.frameCount = frameCount;
            FrameSize = frameSize;
            Image = image;
        }

        public Point CurrentFrameLocation
        {
            get
            {
                var framesPerRow = Image.Width / FrameSize.Width;
                var x = currentFrame % framesPerRow;
                var y = currentFrame / framesPerRow;

                return new Point(x * FrameSize.Width, y * FrameSize.Height);
            }
        }

        public void GotoNextFrame()
        {
            currentFrame = (currentFrame + 1) % frameCount;
        }

        public void Flip()
        {
            Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            Flipped = !Flipped;
        }
    }
}