using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
using System.Linq;
using System.Collections.Generic;

namespace Dungeon.src.MapClass
{
    public class Tiles
    {
        public int id;
        public int x, y, width, height;

        public Rectangle hitbox;

        Door door = null;

        public Tiles(int id, int x, int y, int width, int height)
        {
            this.id = id;

            this.hitbox = new Rectangle(x, y, width, height);

            if (id == 2)
            {
                door = new Door(x, y, width, height);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (id == 0)
            {
                spriteBatch.DrawRectangle(this.hitbox, Color.White);
            }
            else if (id == 1)
            {
                spriteBatch.FillRectangle(this.hitbox, Color.White);
            }
            else if (id == 2)
            {
                door.Draw(spriteBatch);
            }
        }
    }
}