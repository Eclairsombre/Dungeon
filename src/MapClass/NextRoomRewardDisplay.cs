using Dungeon.src.AnimationClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


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
        private readonly Animation animation;
        private readonly CallBack callBack = new();
        private Rectangle hitbox;
        public Rectangle Hitbox { get { return hitbox; } set { hitbox = value; } }
        private readonly RewardType rewardType;


        public NextRoomRewardDisplay(int x, int y, int width, int height, RewardType rewardType, string file)
        {

            animation = new(file, callBack.StaticMyCallback, 0, 0);
            hitbox = new Rectangle(x + width / 4, y, width / 2, 3 * height / 4);

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
            Vector2 position = new Vector2(hitbox.X, hitbox.Y);
            Vector2 scale = new Vector2(1.2f, 1.2f); // Scale by 1.2f
            spriteBatch.Draw(animation.texture, position, callBack.SourceRectangle, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }



    }
}