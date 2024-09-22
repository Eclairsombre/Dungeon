using System.Collections.Generic;
using Dungeon.src.EnemyClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Dungeon.src.PlayerClass
{
    public class Weapon(Vector2 position)
    {
        protected int damage = 1;
        protected int range = 10;
        protected Vector2 position = position;
        protected int width, height;

        public int Damage { get { return damage; } set { damage = value; } }
        public int Range { get { return range; } set { range = value; } }
        public Vector2 Position { get { return position; } set { position = value; } }


        public void Update(Player player, Vector2 direction, List<Enemy> enemies)
        {
            switch (direction)
            {
                case Vector2 v when v == new Vector2(1, 0):
                    //Console.WriteLine("Right");
                    width = (int)(player.SpriteWidth * player.Scale);
                    height = range;
                    Position = new Vector2((int)player.Position.X + player.SpriteWidth * player.Scale, (int)player.Position.Y + 5);

                    break;
                case Vector2 v when v == new Vector2(-1, 0):
                    //Console.WriteLine("Left");
                    width = (int)(player.SpriteWidth * player.Scale);
                    height = range;
                    Position = new Vector2((int)player.Position.X - range + 5, (int)player.Position.Y + 5);

                    break;
                case Vector2 v when v == new Vector2(0, 1):
                    //Console.WriteLine("up");
                    width = range;
                    height = (int)(player.SpriteWidth * player.Scale) - 5;
                    Position = new Vector2((int)player.Position.X + 5, (int)player.Position.Y - range + 5);

                    break;
                case Vector2 v when v == new Vector2(0, -1):
                    //Console.WriteLine("Down");
                    width = range;
                    height = (int)(player.SpriteWidth * player.Scale) - 5; ;
                    Position = new Vector2((int)player.Position.X + 5, (int)player.Position.Y + player.SpriteHeight * player.Scale - 5);

                    break;
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Attack(enemies);
            }
        }


        public void Attack(List<Enemy> enemies)
        {
            Rectangle hitbox = new((int)Position.X, (int)Position.Y, 50, 50);
            foreach (var enemy in enemies)
            {
                if (hitbox.Intersects(enemy.Hitbox))
                {
                    enemy.Hp -= damage;
                }
            }
        }

        public void SetRange(int newRange)
        {
            range = newRange;
        }

        public void SetDamage(int newDamage)
        {
            damage = newDamage;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(new Rectangle((int)Position.X, (int)Position.Y, height, width), Color.Red);
        }

    }
}