using System;
using Dungeon.src.AnimationClass;
using Dungeon.src.PlayerClass.StatsClass;
using Dungeon.src.TexteClass;
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

        Texte description;

        public LevelUpBouton(int x, int y, int width, int height, GameState gameState, string file, LevelUpChoice levelUpChoice, ContentManager content)
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

            string texteBouton = "";
            switch (levelUpChoice)
            {
                case LevelUpChoice.Heal:
                    texteBouton = "Add +1 to your max health";
                    break;
                case LevelUpChoice.Attack:
                    texteBouton = "Multiply your attack by 1.2";
                    break;
                case LevelUpChoice.Speed:
                    texteBouton = "Multiply your speed by 1.1";
                    break;
                case LevelUpChoice.Defense:
                    texteBouton = "Add +1 to your defense";
                    break;
            }

            description = new Texte(content, texteBouton, new Vector2(x + width / 2, y + height + 10), Color.Black, 40);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_animation.texture, new Vector2(hitbox.X, hitbox.Y), callBack.SourceRectangle, Color.White);
            description.Draw(spriteBatch);
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