using System;
using Dungeon.src.AnimationClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Dungeon.src.PlayerClass.WeaponClass
{
    public class Arrow
    {
        public Vector2 Position { get; private set; }
        public Vector2 Direction { get; private set; }
        public float Speed { get; private set; }
        private Rectangle _hitbox;
        public Rectangle Hitbox => _hitbox;

        private Animation _animation;

        //private Rectangle sourceRectangle;

        private CallBack callBack = new();


        public Arrow(Vector2 position, Vector2 direction, float speed, ContentManager content)
        {
            //Position = position;
            Direction = direction;

            this.Position = new Vector2(position.X - 15, position.Y - 15);
            Speed = speed;
            if (Direction == new Vector2(0, -1) || Direction == new Vector2(0, 1))
            {
                _hitbox = new Rectangle((int)Position.X, (int)Position.Y, 10, 30);
            }

            else if (Direction == new Vector2(-1, 0) || Direction == new Vector2(1, 0))
            {
                _hitbox = new Rectangle((int)Position.X, (int)Position.Y, 30, 10);
            }
            switch (Direction)
            {
                case var d when d == new Vector2(-1, 0):
                    _animation = new Animation("ArrowSpriteSheet", callBack.StaticMyCallback, 0, 0);
                    break;
                case var d when d == new Vector2(1, 0):
                    _animation = new Animation("ArrowSpriteSheet", callBack.StaticMyCallback, 2, 0);
                    break;
                case var d when d == new Vector2(0, 1):
                    _animation = new Animation("ArrowSpriteSheet", callBack.StaticMyCallback, 3, 0);
                    break;
                case var d when d == new Vector2(0, -1):
                    _animation = new Animation("ArrowSpriteSheet", callBack.StaticMyCallback, 1, 0);
                    break;

            }

            //texture = content.Load<Texture2D>("Sprites/ArrowSpriteSheet");


            //_animation = new Animation("ArrowSpriteSheet", MyCallback, 0, 0);

            _animation.ParseData();
        }



        public void LoadContent(ContentManager content)
        {
            _animation.LoadContent(content);
        }



        public void Update(GameTime gameTime)
        {
            Position += Direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _hitbox = new Rectangle((int)Position.X, (int)Position.Y, Hitbox.Width, Hitbox.Height);

            _animation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            float scale = 1.2f;
            spriteBatch.Draw(texture, Position, callBack.SourceRectangle, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            //spriteBatch.FillRectangle(Hitbox, Color.Red);

        }
    }
}