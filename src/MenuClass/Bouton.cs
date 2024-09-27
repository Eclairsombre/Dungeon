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

        public bool isClicked;
        private double clickDelay;
        private double elapsedTime;

        public Bouton(int x, int y, int width, int height, GameState gameState, string file)
        {
            hitbox = new Rectangle(x, y, width, height);
            this.gameState = gameState;
            callBack = new CallBack();
            _animation = new Animation(file, callBack.StaticMyCallback, 0, 0);
            _animation.ParseData();

            isClicked = false;
            clickDelay = 500; // Délai en millisecondes
            elapsedTime = 0;
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

            if (isClicked)
            {
                elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (elapsedTime >= clickDelay)
                {
                    isClicked = false;
                    elapsedTime = 0;
                }
            }

            if (!isClicked)
            {
                if (hitbox.Contains(mouseState.Position) && _animation.GetTimeline() == 0)
                {
                    _animation.SetTimeLine(1);
                }
                else if (_animation.GetTimeline() == 1 && !hitbox.Contains(mouseState.Position))
                {
                    _animation.SetTimeLine(0);
                }
            }

            _animation.Update(gameTime);
        }

        public void OnClick(ref GameState gameState)
        {
            MouseState mouseState = Mouse.GetState();
            if (!isClicked && mouseState.LeftButton == ButtonState.Pressed && hitbox.Contains(mouseState.Position))
            {
                Console.WriteLine("Click");
                Console.WriteLine(gameState);
                gameState = this.gameState;
                isClicked = true;
            }
        }
    }
}