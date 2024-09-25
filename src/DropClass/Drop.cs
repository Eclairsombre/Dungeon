using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Dungeon.src.DropClass
{
    public class Drop(int x, int y, int height, int width)
    {
        private int x = x;
        private int y = y;
        private int height = height;
        private int width = width;
        protected Color color;

        private Rectangle hitbox = new(x, y, width, height);

        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        public Rectangle Hitbox { get { return hitbox; } set { hitbox = value; } }
        public int Height { get { return height; } set { height = value; } }
        public int Width { get { return width; } set { width = value; } }





        public void Draw(SpriteBatch spriteBatch, Texture2D texture = null)
        {
            if (texture == null)
            {
                spriteBatch.FillRectangle(hitbox, color);
                return;
            }
            else
            {
                spriteBatch.Draw(texture, hitbox, Color.White);
            }
        }



    }
}