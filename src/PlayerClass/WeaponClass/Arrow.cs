using System;
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

        private Rectangle sourceRectangle;

        private Texture2D texture;

        public Arrow(Vector2 position, Vector2 direction, float speed, ContentManager content)
        {
            Position = position;
            Direction = direction;
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
                    _animation = new Animation("ArrowSpriteSheet", MyCallback, 0, 0);
                    break;
                case var d when d == new Vector2(1, 0):
                    _animation = new Animation("ArrowSpriteSheet", MyCallback, 2, 0);
                    break;
                case var d when d == new Vector2(0, -1):
                    _animation = new Animation("ArrowSpriteSheet", MyCallback, 1, 0);
                    break;
                case var d when d == new Vector2(0, 1):
                    _animation = new Animation("ArrowSpriteSheet", MyCallback, 3, 0);
                    break;
            }

            texture = content.Load<Texture2D>("Sprites/ArrowSpriteSheet");


            //_animation = new Animation("ArrowSpriteSheet", MyCallback, 0, 0);

            _animation.ParseData();
        }
        void MyCallback(Vector2 position, Vector2 size)
        {
            sourceRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
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
            spriteBatch.Draw(this.texture, Position, sourceRectangle, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            //spriteBatch.FillRectangle(Hitbox, Color.Red);

        }
    }
}