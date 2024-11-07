using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using MonoGame.Extended;
using Dungeon.src.MapClass.HolderClass;
using Dungeon.src.PlayerClass.WeaponClass;
using Dungeon.src.DropClass;
using Microsoft.Xna.Framework.Content;

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

        public Tiles(Tuple<int, int> id, int x, int y, int width, int height, RewardType rewardType, ContentManager content)
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

                    holder = rewardType switch
                    {
                        RewardType.Weapon => new WeaponHolder(x, y, new Sword(position)),
                        RewardType.Health => new DropHolder(x, y, new HeartDrop(x + 20 + (40 - 50) / 2, y - 50 - 10, 50, 50, 1f, "Coeur-Sheet")),
                        RewardType.Gold => new DropHolder(x, y, new GoldDrop(x + 20 + (40 - 50) / 2, y - 50 - 10, 50, 50, 50)),
                        RewardType.Xp => new DropHolder(x, y, new XpDrop(x + 20 + (40 - 50) / 2, y - 50 - 10, 50, 50, 50, 5f)),
                        _ => new WeaponHolder(x, y, new Sword(position)),
                    };

                    LoadContent(content);

                    break;


                default:
                    break;

            }
        }

        public void Update(GameTime gameTime)
        {
            nextRoomRewardDisplay?.Update(gameTime);
        }

        public void LoadContent(ContentManager content)
        {
            holder?.DropHold?.LoadContent(content);
            nextRoomRewardDisplay?.LoadContent(content);
        }

        public void Draw(SpriteBatch spriteBatch, bool finished, Texture2D[] texture)
        {

            switch (id.Item1)
            {
                case 0:
                    //spriteBatch.Draw(texture[0], hitbox, Color.White);
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
                    //spriteBatch.Draw(texture[0], hitbox, Color.White);
                    break;
                case 4:
                    //spriteBatch.Draw(texture[0], hitbox, Color.White);
                    if (finished)
                    {
                        holder?.Draw(spriteBatch);
                    }

                    break;
                case 5:
                    spriteBatch.FillRectangle(hitbox, Color.White);
                    if (finished)
                    {
                        nextRoomRewardDisplay?.Draw(spriteBatch);
                    }
                    break;
                default:
                    break;

            }
        }
    }
}