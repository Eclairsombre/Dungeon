using Dungeon.src.MapClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Dungeon.src.EnemyClass
{
    public class Enemy
    {
        public int hp = 1, damage = 0, speed = 3;
        public Vector2 Position;

        public int width, height;

        public Enemy()
        {
            this.width = 50;
            this.height = 50;
            this.Position = new Vector2(200, 200);

        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle rect = new Rectangle((int)Position.X, (int)Position.Y, width, height);
            spriteBatch.FillRectangle(rect, Color.Blue);
        }

    }
}