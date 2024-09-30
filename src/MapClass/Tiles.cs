using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using MonoGame.Extended;
using Dungeon.src.MapClass.HolderClass;
using Dungeon.src.PlayerClass.WeaponClass;
using System.Diagnostics;

namespace Dungeon.src.MapClass
{
    public class Tiles
    {
        private Tuple<int, int> id;
        private Rectangle hitbox;
        private Door door = null;
        private Holder holder = null;

        private NextRoomRewardDisplay nextRoomRewardDisplay = null;
        public Tuple<int, int> Id { get { return id; } set { id = value; } }
        public Rectangle Hitbox { get { return hitbox; } set { hitbox = value; } }
        public Door Door { get { return door; } set { door = value; } }
        public Holder Holder { get { return holder; } set { holder = value; } }

        public NextRoomRewardDisplay NextRoomRewardDisplay { get { return nextRoomRewardDisplay; } set { nextRoomRewardDisplay = value; } }

        public Tiles(Tuple<int, int> id, int x, int y, int width, int height)
        {
            this.id = id;

            hitbox = new Rectangle(x, y, width, height);
            int random = new Random().Next(0, 4);
            switch (id.Item1)
            {

                case 2:
                    door = new Door(x, y, width, height, (RewardType)random);
                    break;
                case 4:
                    Vector2 position = new(x, y);
                    holder = new WeaponHolder(x, y, new Sword(position));
                    break;
                default:
                    break;
                case 5:
                    nextRoomRewardDisplay = new NextRoomRewardDisplay(x, y, width, height, (RewardType)random, ((RewardType)random).ToString() + "Display");
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
                    spriteBatch.Draw(texture[0], hitbox, Color.White);
                    if (finished)
                    {
                        if (holder is WeaponHolder weaponHolder)
                        {
                            weaponHolder.Draw(spriteBatch);
                        }
                        else
                        {
                            holder.Draw(spriteBatch);
                        }
                    }

                    break;
                case 5:
                    spriteBatch.FillRectangle(hitbox, Color.White);
                    if (finished)
                    {
                        nextRoomRewardDisplay.Draw(spriteBatch);
                    }
                    break;
                default:
                    break;

            }
        }
    }
}