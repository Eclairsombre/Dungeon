using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Dungeon.src.MapClass.HolderClass
{
    public class Holder
    {
        protected readonly int width = 40;
        protected readonly int height = 70;
        protected readonly int x;
        protected readonly int y;

        protected Rectangle hitbox;

        public Rectangle Hitbox { get { return hitbox; } }

        public Holder(int x, int y)
        {
            this.x = x;
            this.y = y;
            hitbox = new Rectangle(x + 15, y, width, height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(hitbox, Color.White);
        }


    }
}