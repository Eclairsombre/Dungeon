using System;
using System.Collections.Generic;
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
        //TODO : Fix range hitbox
        public Vector2 Position;

        public int width, height;

        public Weapon(Vector2 position)
        {
            this.Position = position;
        }

        public void Update(Player player, Vector2 direction, List<Enemy> enemies)
        {
            switch (direction)
            {
                case Vector2 v when v == new Vector2(1, 0):
                    //Console.WriteLine("Right");
                    this.width = (int)(player.spriteWidth * player.scale);
                    this.height = range;
                    Position = new Vector2((int)player.Position.X + player.spriteWidth * player.scale, (int)player.Position.Y + 5);

                    break;
                case Vector2 v when v == new Vector2(-1, 0):
                    //Console.WriteLine("Left");
                    this.width = (int)(player.spriteWidth * player.scale);
                    this.height = range;
                    Position = new Vector2((int)player.Position.X - this.range + 5, (int)player.Position.Y + 5);

                    break;
                case Vector2 v when v == new Vector2(0, 1):
                    //Console.WriteLine("up");
                    this.width = range;
                    this.height = (int)(player.spriteWidth * player.scale) - 5;
                    Position = new Vector2((int)player.Position.X + 5, (int)player.Position.Y - this.range + 5);

                    break;
                case Vector2 v when v == new Vector2(0, -1):
                    //Console.WriteLine("Down");
                    this.width = range;
                    this.height = (int)(player.spriteWidth * player.scale) - 5; ;
                    Position = new Vector2((int)player.Position.X + 5, (int)player.Position.Y + player.spriteHeight * player.scale - 5);

                    break;
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Attack(enemies);
            }
        }


        public void Attack(List<Enemy> enemies)
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