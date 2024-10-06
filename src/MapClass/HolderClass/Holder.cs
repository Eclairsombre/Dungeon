using System;
using Dungeon.src.DropClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        private Drop drop;
        public Drop DropHold { get { return drop; } set { drop = value; } }
        public Rectangle Hitbox { get { return hitbox; } set { hitbox = value; } }
        public Holder(int x, int y)
        {
            this.x = x;
            this.y = y;
            hitbox = new Rectangle(x + 20, y, width, height);
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(hitbox, Color.White);
        }

        public void LoadContent(ContentManager content)
        {
            drop.LoadContent(content);
        }


    }
}