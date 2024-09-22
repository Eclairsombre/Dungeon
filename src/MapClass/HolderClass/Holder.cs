using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Dungeon.src.MapClass.HolderClass
{
    public class Holder(int x, int y)
    {
        private readonly int width = 40;
        private readonly int height = 70;
        private readonly int x = x;
        private readonly int y = y;

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle rectangle = new(x + 15, y, width, height);
            spriteBatch.FillRectangle(rectangle, Color.White);
        }


    }
}