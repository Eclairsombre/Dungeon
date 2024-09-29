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


        private Bouton choice1, choice2, choice3;

        public LevelUp(GraphicsDevice graphicsDevice)
        {
            callBack = new CallBack();
            animation = new Animation("LevelUpBackground-Sheet", callBack.StaticMyCallback, 1, 0);
            animation.ParseData();

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


            choice1 = new Bouton((screenWidth - hitboxChoiceWidth) / 2, hitbox.Y + 200, hitboxChoiceWidth, hitboxChoiceHeight, GameState.Playing, "LevelUpHealBouton-Sheet");
            choice2 = new Bouton((screenWidth - hitboxChoiceWidth) / 2, hitbox.Y + 350, hitboxChoiceWidth, hitboxChoiceHeight, GameState.Playing, "LevelUpAttackBouton-Sheet");
            choice3 = new Bouton((screenWidth - hitboxChoiceWidth) / 2, hitbox.Y + 500, hitboxChoiceWidth, hitboxChoiceHeight, GameState.Playing, "LevelUpSpeedBouton-Sheet");






        }

        public void LoadContent(ContentManager content)
        {
            animation.LoadContent(content);

            choice1.LoadContent(content);
            choice2.LoadContent(content);
            choice3.LoadContent(content);
        }

        public void Update(GameTime gameTime, ref GameState gameState, ref GameState previousGameState)
        {
            animation.Update(gameTime);
            choice1.Update(gameTime);
            choice2.Update(gameTime);
            choice3.Update(gameTime);

            choice1.OnClick(ref gameState, ref previousGameState);
            choice2.OnClick(ref gameState, ref previousGameState);
            choice3.OnClick(ref gameState, ref previousGameState);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(animation.texture, new Vector2(hitbox.X, hitbox.Y), callBack.SourceRectangle, Color.White);

            choice1.Draw(spriteBatch);
            choice2.Draw(spriteBatch);
            choice3.Draw(spriteBatch);

        }
    }
}