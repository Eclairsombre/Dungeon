using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Dungeon.src.PlayerClass
{
    public class Arrow
    {
        public Vector2 Position { get; private set; }
        public Vector2 Direction { get; private set; }
        public float Speed { get; private set; }
        public Rectangle Hitbox => new Rectangle((int)Position.X, (int)Position.Y, 10, 10);

        public Arrow(Vector2 position, Vector2 direction, float speed)
        {
            Position = position;
            Direction = direction;
            Speed = speed;
        }

        public void Update(GameTime gameTime)
        {
            Console.WriteLine(Direction);
            Position += Direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {

            spriteBatch.FillRectangle(Hitbox, Color.Red);

        }
    }
}