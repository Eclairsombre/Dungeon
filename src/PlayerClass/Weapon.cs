using Dungeon.src.EnemyClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Dungeon.src.PlayerClass
{
    public class Weapon
    {
        public int damage = 1;
        public int range = 10;
        public Vector2 Position;

        public int width, height;

        public Weapon(Vector2 position)
        {
            this.Position = position;
        }

        public void Update(Vector2 playerPosition, Vector2 direction, Enemy[] enemies)
        {

            switch (direction)
            {
                case Vector2 v when v == new Vector2(1, 0):
                    this.width = 75;
                    this.height = range;
                    Position = new Vector2((int)playerPosition.X + 65, (int)playerPosition.Y + 5);

                    break;
                case Vector2 v when v == new Vector2(-1, 0):

                    this.width = 75;
                    this.height = range;
                    Position = new Vector2((int)playerPosition.X, (int)playerPosition.Y + 5);

                    break;
                case Vector2 v when v == new Vector2(0, 1):
                    this.width = range;
                    this.height = 65;
                    Position = new Vector2((int)playerPosition.X + 5, (int)playerPosition.Y - 5);

                    break;
                case Vector2 v when v == new Vector2(0, -1):
                    this.width = range;
                    this.height = 65;
                    Position = new Vector2((int)playerPosition.X + 5, (int)playerPosition.Y + 80);

                    break;
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Attack(enemies);
            }
        }


        public void Attack(Enemy[] enemies)
        {
            Rectangle hitbox = new Rectangle((int)Position.X, (int)Position.Y, 50, 50);
            foreach (var enemy in enemies)
            {
                if (hitbox.Intersects(enemy.hitbox))
                {
                    enemy.hp -= damage;
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(new Rectangle((int)Position.X, (int)Position.Y, height, width), Color.Red);
        }

    }
}