using System;
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

        private readonly Animation heartAnimation, heartEmptyAnimation;

        private readonly CallBack callBack = new(), callBackEmpty = new();


        public Interface()
        {
            heartAnimation = new Animation("Coeur-Sheet", callBack.StaticMyCallback, 1, 0);
            heartAnimation.ParseData();

            heartEmptyAnimation = new Animation("CoeurVide", callBackEmpty.StaticMyCallback, 0, 0);
            heartEmptyAnimation.ParseData();
        }

        public void LoadContent(ContentManager content)
        {
            heartAnimation.LoadContent(content);
            heartEmptyAnimation.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            heartAnimation.Update(gameTime);
            heartEmptyAnimation.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch, Player player)
        {
            float xpPercentage = (float)player.PlayerStats.Xp / player.PlayerStats.XpToLevelUp;
            Rectangle filledXpBar = new(xpBar.X, xpBar.Y, (int)(xpBar.Width * xpPercentage), xpBar.Height);
            spriteBatch.FillRectangle(filledXpBar, Color.Green);
            spriteBatch.DrawRectangle(xpBar, Color.Black);
            for (int i = 0; i < (int)player.PlayerStats.MaxHealth; i++)
            {
                if (i < player.PlayerStats.Health)
                {
                    spriteBatch.Draw(heartAnimation.texture, new Vector2(heart.X + i * 60, heart.Y), callBack.SourceRectangle, Color.White);
                }
                else
                {
                    spriteBatch.Draw(heartEmptyAnimation.texture, new Vector2(heart.X + i * 60, heart.Y), callBackEmpty.SourceRectangle, Color.Gray);
                }
            }
        }
    }
}