using Dungeon.src.AnimationClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Dungeon.src.MenuClass.BoutonClass
{
    public class Bouton
    {
        private Rectangle hitbox;
        private readonly GameState gameState;
        private readonly CallBack callBack;
        private readonly Animation _animation;
        public bool isClicked;
        private readonly double clickDelay;
        private double elapsedTime;

        private readonly float _scale = 1f;
        public Bouton(int x, int y, int width, int height, GameState gameState, string file)
        {
            hitbox = new Rectangle(x, y, width, height);
            this.gameState = gameState;
            callBack = new CallBack();
            _animation = new Animation(file, callBack.StaticMyCallback, 0, 0);
            _animation.ParseData();

            isClicked = false;
            clickDelay = 500;
            elapsedTime = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_animation.texture, new Vector2(hitbox.X, hitbox.Y), callBack.SourceRectangle, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        }

        public void LoadContent(ContentManager content)
        {
            _animation.LoadContent(content);
        }

        public void SetPosition(int x, int y)
        {
            hitbox.X = x;
            hitbox.Y = y;
        }

        public void SetSize(int width, int height)
        {
            hitbox.Width = width;
            hitbox.Height = height;
        }

        public virtual void Update(GameTime gameTime, ref GameState gameState, ref GameState previousGameState)
        {
            MouseState mouseState = Mouse.GetState();

            OnClick(ref gameState, ref previousGameState);

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