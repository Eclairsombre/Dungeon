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
            spriteBatch.Draw(animation.texture, new Vector2(hitbox.X, hitbox.Y), callBack.SourceRectangle, Color.White);
        }
    }
}