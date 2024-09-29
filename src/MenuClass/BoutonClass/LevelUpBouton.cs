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
    public enum LevelUpChoice
    {
        Heal,
        Attack,
        Speed,
        Defense
    }
    public class LevelUpBouton
    {
        private readonly LevelUpChoice levelUpChoice;

        protected Rectangle hitbox;
        protected GameState gameState;
        protected CallBack callBack;
        protected Animation _animation;
        public bool isClicked;
        protected double clickDelay;
        protected double elapsedTime;

        public LevelUpBouton(int x, int y, int width, int height, GameState gameState, string file, LevelUpChoice levelUpChoice)
        {
            hitbox = new Rectangle(x, y, width, height);
            this.gameState = gameState;
            callBack = new CallBack();
            _animation = new Animation(file, callBack.StaticMyCallback, 0, 0);
            _animation.ParseData();

            this.levelUpChoice = levelUpChoice;

            isClicked = false;
            clickDelay = 500;
            elapsedTime = 0;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
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

        public void OnClick(ref GameState gameState, ref GameState previousGameState, ref Stats stats)
        {
            MouseState mouseState = Mouse.GetState();
            if (!isClicked && mouseState.LeftButton == ButtonState.Pressed && hitbox.Contains(mouseState.Position))
            {
                previousGameState = gameState;
                gameState = this.gameState;
                isClicked = true;

                switch (levelUpChoice)
                {
                    case LevelUpChoice.Heal:
                        stats.MaxHealth += 1;
                        stats.Health += 1;

                        break;
                    case LevelUpChoice.Attack:
                        stats.Attack *= 1.2f;
                        break;
                    case LevelUpChoice.Speed:
                        stats.Speed *= 1.1f;
                        break;
                    case LevelUpChoice.Defense:
                        stats.Defense += 1;
                        break;
                }
            }
        }
    }
}