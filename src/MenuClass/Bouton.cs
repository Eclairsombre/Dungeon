using System;
using Dungeon.src.AnimationClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;

namespace Dungeon.src.MenuClass
{
    public class Bouton
    {
        private Rectangle hitbox;
        private GameState gameState;

        private CallBack callBack;
        private Animation _animation;

        public Bouton(int x, int y, int width, int height, GameState gameState, string file)
        {
            hitbox = new Rectangle(x, y, width, height);
            this.gameState = gameState;
            callBack = new CallBack();
            _animation = new Animation(file, callBack.StaticMyCallback, 0, 0);
            _animation.ParseData();
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.FillRectangle(hitbox, Color.Black);
            spriteBatch.Draw(_animation.texture, new Vector2(hitbox.X, hitbox.Y), callBack.SourceRectangle, Color.White);
        }

        public void LoadContent(ContentManager content)
        {
            _animation.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            if (hitbox.Contains(mouseState.Position) && _animation.GetTimeline() == 0)
            {
                Console.WriteLine("Mouse is over the button");
                _animation.SetTimeLine(1);
                Console.WriteLine(_animation.GetTimeline());
            }
            else if (_animation.GetTimeline() == 1 && !hitbox.Contains(mouseState.Position))
            {
                _animation.SetTimeLine(0);
            }
            _animation.Update(gameTime);
        }

        public GameState OnClick(Menu menu)
        {
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed && hitbox.Contains(mouseState.Position))
            {
                return gameState;
            }
            return menu.GameState;
        }

    }
}