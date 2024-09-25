using Microsoft.Xna.Framework;

namespace Dungeon.src.AnimationClass
{
    public class CallBack
    {

        private Rectangle sourceRectangle;

        public Rectangle SourceRectangle { get { return sourceRectangle; } set { sourceRectangle = value; } }
        public void StaticMyCallback(Vector2 position, Vector2 size)
        {
            sourceRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        }
    }
}