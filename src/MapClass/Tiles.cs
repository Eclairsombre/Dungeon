using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using MonoGame.Extended;
using Dungeon.src.MapClass.HolderClass;

namespace Dungeon.src.MapClass
{
    public class Tiles
    {
        private Tuple<int, int> id;
        private Rectangle hitbox;

        private Door door = null;

        private Holder holder = null;

        public Tuple<int, int> Id { get { return id; } set { id = value; } }
        public Rectangle Hitbox { get { return hitbox; } set { hitbox = value; } }
        public Door Door { get { return door; } set { door = value; } }
        public Holder Holder { get { return holder; } set { holder = value; } }

        public Tiles(Tuple<int, int> id, int x, int y, int width, int height)
        {
            this.id = id;

            hitbox = new Rectangle(x, y, width, height);

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

        public void Draw(SpriteBatch spriteBatch, bool finished, Texture2D[] texture)
        {

            switch (id.Item1)
            {
                case 0:
                    spriteBatch.Draw(texture[0], hitbox, Color.White);
                    break;
                case 1:
                    spriteBatch.FillRectangle(hitbox, Color.White);
                    break;
                case 2:
                    if (!finished)
                    {
                        spriteBatch.FillRectangle(hitbox, Color.White);
                    }
                    else
                    {
                        door.Draw(spriteBatch);
                    }
                    break;
                case 3:
                    spriteBatch.Draw(texture[0], hitbox, Color.White);
                    break;
                case 4:
                    if (finished)
                    {
                        holder.Draw(spriteBatch);
                    }
                    spriteBatch.Draw(texture[0], hitbox, Color.White);

                    break;

                default:
                    break;

            }
        }
    }
}