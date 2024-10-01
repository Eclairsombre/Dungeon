using System;
using Dungeon.src.AnimationClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;

namespace Dungeon.src.MapClass
{
    public enum RewardType
    {
        Weapon,
        Health,
        Gold,
        Xp,
        None
    }
    public class NextRoomRewardDisplay
    {
        private Animation animation;
        private readonly CallBack callBack = new();
        private Rectangle hitbox;
        public Rectangle Hitbox { get { return hitbox; } set { hitbox = value; } }
        private readonly RewardType rewardType;


        public NextRoomRewardDisplay(int x, int y, int width, int height, RewardType rewardType, string file)
        {
            animation = new(file, callBack.StaticMyCallback, 0, 0);
            hitbox = new Rectangle(x + width / 4, y, width / 2, 3 * height / 4);
            Console.WriteLine(hitbox);

            animation.ParseData();


            this.rewardType = rewardType;
        }

        public void LoadContent(ContentManager content)
        {
            animation.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            animation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (rewardType)
            {
                case RewardType.Weapon:
                    //spriteBatch.FillRectangle(hitbox, Color.Black);
                    spriteBatch.Draw(animation.texture, new Vector2(hitbox.X, hitbox.Y), callBack.SourceRectangle, Color.White);
                    break;
                case RewardType.Health:
                    spriteBatch.Draw(animation.texture, new Vector2(hitbox.X, hitbox.Y), callBack.SourceRectangle, Color.White);
                    //spriteBatch.FillRectangle(hitbox, Color.Red);
                    break;
                case RewardType.Gold:
                    //spriteBatch.FillRectangle(hitbox, Color.Yellow);
                    spriteBatch.Draw(animation.texture, new Vector2(hitbox.X, hitbox.Y), callBack.SourceRectangle, Color.White);
                    break;
                case RewardType.Xp:
                    //spriteBatch.FillRectangle(hitbox, Color.Blue);
                    spriteBatch.Draw(animation.texture, new Vector2(hitbox.X, hitbox.Y), callBack.SourceRectangle, Color.White);
                    break;
                default:
                    break;
            }
        }



    }
}