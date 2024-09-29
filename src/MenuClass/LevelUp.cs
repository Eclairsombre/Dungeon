using System;
using Dungeon.src.AnimationClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dungeon.src.MenuClass
{
    public class LevelUp
    {
        private Animation animation;
        private CallBack callBack;

        private Rectangle hitbox;

        private Animation animationChoice1, animationChoice2, animationChoice3;
        private CallBack callBackChoice1, callBackChoice2, callBackChoice3;
        private Rectangle hitboxChoice1, hitboxChoice2, hitboxChoice3;

        public LevelUp(GraphicsDevice graphicsDevice)
        {
            callBack = new CallBack();
            animation = new Animation("LevelUpBackground-Sheet", callBack.StaticMyCallback, 1, 0);
            animation.ParseData();
            callBackChoice1 = new CallBack();
            callBackChoice2 = new CallBack();
            callBackChoice3 = new CallBack();
            animationChoice1 = new Animation("LevelUpBouton-Sheet", callBackChoice1.StaticMyCallback, 0, 0);
            animationChoice2 = new Animation("LevelUpBouton-Sheet", callBackChoice2.StaticMyCallback, 1, 0);
            animationChoice3 = new Animation("LevelUpBouton-Sheet", callBackChoice3.StaticMyCallback, 2, 0);

            animationChoice1.ParseData();
            animationChoice2.ParseData();
            animationChoice3.ParseData();








            int screenWidth = graphicsDevice.Viewport.Width;
            int screenHeight = graphicsDevice.Viewport.Height;
            int hitboxWidth = 800;
            int hitboxHeight = 800;

            hitbox = new Rectangle(
                (screenWidth - hitboxWidth) / 2,
                (screenHeight - hitboxHeight) / 2,
                hitboxWidth,
                hitboxHeight
            );

            int hitboxChoiceWidth = 400;
            int hitboxChoiceHeight = 150;

            hitboxChoice1 = new Rectangle(
                (screenWidth - hitboxChoiceWidth) / 2,
                hitbox.Y + 200,
                hitboxChoiceWidth,
                hitboxChoiceHeight
            );

            hitboxChoice2 = new Rectangle(
                (screenWidth - hitboxChoiceWidth) / 2,
                hitbox.Y + 350,
                hitboxChoiceWidth,
                hitboxChoiceHeight
            );


            hitboxChoice3 = new Rectangle(
                (screenWidth - hitboxChoiceWidth) / 2,
                hitbox.Y + 500,
                hitboxChoiceWidth,
                hitboxChoiceHeight
            );

        }

        public void LoadContent(ContentManager content)
        {
            animation.LoadContent(content);

            animationChoice1.LoadContent(content);
            animationChoice2.LoadContent(content);
            animationChoice3.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            animation.Update(gameTime);
            animationChoice1.Update(gameTime);
            animationChoice2.Update(gameTime);
            animationChoice3.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(animation.texture, new Vector2(hitbox.X, hitbox.Y), callBack.SourceRectangle, Color.White);
            spriteBatch.Draw(animationChoice1.texture, new Vector2(hitboxChoice1.X, hitboxChoice1.Y), callBackChoice1.SourceRectangle, Color.White);
            spriteBatch.Draw(animationChoice2.texture, new Vector2(hitboxChoice2.X, hitboxChoice2.Y), callBackChoice2.SourceRectangle, Color.White);
            spriteBatch.Draw(animationChoice3.texture, new Vector2(hitboxChoice3.X, hitboxChoice3.Y), callBackChoice3.SourceRectangle, Color.White);
        }
    }
}