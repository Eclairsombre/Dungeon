using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Dungeon.src.MapClass
{
    public class Wall(int x, int y, int width, int height)
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;
        public int Width { get; set; } = width;
        public int Height { get; set; } = height;

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(X, Y, Width, Height, Microsoft.Xna.Framework.Color.Black);
        }

    }
}