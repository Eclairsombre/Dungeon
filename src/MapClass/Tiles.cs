using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
using System.Linq;
using System.Collections.Generic;
using Dungeon.src.MapClass.HolderClass;

namespace Dungeon.src.MapClass
{
    public class Tiles
    {
        public Tuple<int, int> id;
        public int x, y, width, height;

        public Rectangle hitbox;

        public Door door = null;

        public Holder holder = null;

        public Tiles(Tuple<int, int> id, int x, int y, int width, int height)
        {
            this.id = id;

            this.hitbox = new Rectangle(x, y, width, height);

            switch (id.Item1)
            {
                case 2:
                    door = new Door(x, y, width, height);
                    break;
                case 4:
                    holder = new Holder(x, y);
                    break;
                default:
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch, bool finished)
        {

            switch (id.Item1)
            {
                case 0:
                    spriteBatch.DrawRectangle(this.hitbox, Color.White);
                    break;
                case 1:
                    spriteBatch.FillRectangle(this.hitbox, Color.White);
                    break;
                case 2:
                    if (!finished)
                    {
                        spriteBatch.FillRectangle(this.hitbox, Color.White);
                    }
                    else
                    {
                        door.Draw(spriteBatch);
                    }
                    break;
                case 4:
                    if (finished)
                    {
                        holder.Draw(spriteBatch);
                    }
                    break;

                default:
                    break;

            }
        }
    }
}