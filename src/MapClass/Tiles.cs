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
        public Tuple<int, int> id;
        public int x, y, width, height;

        public Rectangle hitbox;

        public Door door = null;

        public Tiles(Tuple<int, int> id, int x, int y, int width, int height)
        {
            this.id = id;

            this.hitbox = new Rectangle(x, y, width, height);

            if (id.Item1 == 2)
            {
                door = new Door(x, y, width, height);
            }
        }

        public void Draw(SpriteBatch spriteBatch, bool finished)
        {
            if (id.Item1 == 0)
            {
                spriteBatch.DrawRectangle(this.hitbox, Color.White);
            }
            else if (id.Item1 == 1)
            {
                spriteBatch.FillRectangle(this.hitbox, Color.White);
            }
            else if (id.Item1 == 2)
            {
                if (!finished)
                {
                    spriteBatch.FillRectangle(this.hitbox, Color.White);
                }
                else
                {
                    door.Draw(spriteBatch);
                }
            }
        }
    }
}