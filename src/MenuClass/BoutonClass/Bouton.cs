using System;
using Dungeon.src.AnimationClass;
using Dungeon.src.PlayerClass.StatsClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;

namespace Dungeon.src.MenuClass.BoutonClass
{
    public class Bouton
    {
        protected Rectangle hitbox;
        protected GameState gameState;

        protected CallBack callBack;
        protected Animation _animation;

        public bool isClicked;
        protected double clickDelay;
        protected double elapsedTime;

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

        public virtual void Update(GameTime gameTime)
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

        public void OnClick(ref GameState gameState, ref GameState previousGameState)
        {
            MouseState mouseState = Mouse.GetState();
            if (!isClicked && mouseState.LeftButton == ButtonState.Pressed && hitbox.Contains(mouseState.Position))
            {
                previousGameState = gameState;
                gameState = this.gameState;
                isClicked = true;
            }
        }
    }
}