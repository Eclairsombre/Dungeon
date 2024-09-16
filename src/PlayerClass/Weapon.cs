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
        public int range = 50;
        public Vector2 Position;

        public Weapon(Vector2 position)
        {
            this.Position = position;
        }

        public void Update(Vector2 playerPosition, Vector2 direction, Enemy[] enemies)
        {
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                Attack(enemies);
            }
            this.Position = playerPosition + direction * range;
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
            spriteBatch.FillRectangle(new Rectangle((int)Position.X, (int)Position.Y, 10, 10), Color.Red);
        }

    }
}