using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Dungeon.src.DropClass
{
    public class Drop(int x, int y, int height, int width)
    {
        protected int x = x;
        protected int y = y;
        protected int height = height;
        protected int width = width;
        protected Color color;

        private Rectangle hitbox = new(x, y, width, height);

        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        public Rectangle Hitbox { get { return hitbox; } set { hitbox = value; } }
        public int Height { get { return height; } set { height = value; } }
        public int Width { get { return width; } set { width = value; } }

        public virtual void Draw(SpriteBatch spriteBatch) { }

        public virtual void Update(GameTime gameTime) { }

        public virtual void LoadContent(ContentManager content) { }
    }
}