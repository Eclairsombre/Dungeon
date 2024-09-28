using Dungeon.src.AnimationClass;
using Dungeon.src.PlayerClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Dungeon.src.InterfaceClass
{
    public class Interface
    {
        private Rectangle xpBar = new(50, 70, 250, 20);

        private Rectangle heart = new(50, 10, 50, 50);

        private readonly Animation heartAnimation;

        private readonly CallBack callBack = new();


        public Interface()
        {
            heartAnimation = new Animation("Coeur-Sheet", callBack.StaticMyCallback, 1, 0);
            heartAnimation.ParseData();
        }

        public void LoadContent(ContentManager content)
        {
            heartAnimation.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            heartAnimation.Update(gameTime);
        }


        public void Draw(SpriteBatch spriteBatch, Player player)
        {
            float xpPercentage = (float)player.Stats.Xp / player.Stats.XpToLevelUp;
            Rectangle filledXpBar = new(xpBar.X, xpBar.Y, (int)(xpBar.Width * xpPercentage), xpBar.Height);
            spriteBatch.FillRectangle(filledXpBar, Color.Green);
            spriteBatch.DrawRectangle(xpBar, Color.Black);
            for (int i = 0; i < player.Stats.Health; i++)
            {
                //spriteBatch.FillRectangle(new Rectangle(heart.X + i * 60, heart.Y, heart.Width, heart.Height), Color.Red);
                spriteBatch.Draw(heartAnimation.texture, new Vector2(heart.X + i * 60, heart.Y), callBack.SourceRectangle, Color.White);
            }
        }
    }
}