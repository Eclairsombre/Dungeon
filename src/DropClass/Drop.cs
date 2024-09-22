using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Dungeon.src.DropClass
{
    public class Drop(int x, int y, int height, int width)
    {
        private int x = x;
        private int y = y;
        private readonly int height = height;
        private readonly int width = width;
        protected Color color;

        private Rectangle hitbox = new(x, y, width, height);

        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }



        public bool IsColliding(Rectangle player)
        {
            if (player.Intersects(hitbox))
            {
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture = null)
        {
            if (texture == null)
            {
                spriteBatch.FillRectangle(new Rectangle(x, y, width, height), color);
                return;
            }
            else
            {
                spriteBatch.Draw(texture, new Rectangle(x, y, width, height), Color.White);
            }
        }



    }
}