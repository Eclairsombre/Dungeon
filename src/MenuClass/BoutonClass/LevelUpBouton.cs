using Dungeon.src.AnimationClass;
using Dungeon.src.PlayerClass.StatsClass;
using Dungeon.src.TexteClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


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

        private Rectangle hitbox;
        private readonly GameState gameState;
        private readonly CallBack callBack;
        private readonly Animation _animation;
        public bool isClicked;
        private readonly double clickDelay;
        private double elapsedTime;

        private double nbBoost;
        private readonly Texte description;

        public LevelUpBouton(int x, int y, int width, int height, GameState gameState, string file, LevelUpChoice levelUpChoice, double nbBoost, ContentManager content)
        {
            hitbox = new Rectangle(x, y, width, height);
            this.gameState = gameState;
            callBack = new CallBack();
            _animation = new Animation(file, callBack.StaticMyCallback, 0, 0);
            _animation.ParseData();

            this.nbBoost = nbBoost;

            this.levelUpChoice = levelUpChoice;

            isClicked = false;
            clickDelay = 500;
            elapsedTime = 0;

            string texteBouton = "";
            switch (levelUpChoice)
            {
                case LevelUpChoice.Heal:
                    texteBouton = $"Add +{nbBoost} to your max health";
                    break;
                case LevelUpChoice.Attack:
                    texteBouton = $"Multiply your attack by {nbBoost}";
                    break;
                case LevelUpChoice.Speed:
                    texteBouton = $"Multiply your speed by {nbBoost}";
                    break;
                case LevelUpChoice.Defense:
                    texteBouton = $"Add +{nbBoost} to your defense";
                    break;
            }

            description = new Texte(content, texteBouton, new Vector2(x + width / 4 + 10, y + height / 2), Color.Black, 12);
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
                        stats.MaxHealth += (int)nbBoost;
                        stats.Health += (int)nbBoost;

                        break;
                    case LevelUpChoice.Attack:
                        stats.Attack *= (float)nbBoost;
                        break;
                    case LevelUpChoice.Speed:
                        stats.Speed *= (float)nbBoost;
                        break;
                    case LevelUpChoice.Defense:
                        stats.Defense += (float)nbBoost;
                        break;
                }
            }
        }
    }
}