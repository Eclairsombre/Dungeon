using System;
using System.Collections.Generic;
using Dungeon.src.EnemyClass;
using Dungeon.src.MapClass;
using Dungeon.src.PlayerClass.StatsClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Dungeon.src.PlayerClass.WeaponClass
{
    public class Weapon(Vector2 position)
    {
        protected int damage = 1;
        protected int range = 10;
        protected Vector2 position = position;
        protected int width = 40, height = 40;

        protected bool attacking = false;
        protected float attackCooldown = 0.5f;
        protected float timeSinceLastAttack = 0f;

        public int Damage { get { return damage; } set { damage = value; } }
        public int Range { get { return range; } set { range = value; } }
        public Vector2 Position { get { return position; } set { position = value; } }
        public int Width { get { return width; } set { width = value; } }
        public int Height { get { return height; } set { height = value; } }


        public virtual void Update(Player player, GameTime gameTime, Map map, ContentManager content)
        {
            timeSinceLastAttack += (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (player.Direction)
            {
                case Vector2 v when v == new Vector2(1, 0):
                    //Console.WriteLine("Right");
                    width = range;
                    height = (int)(player.SpriteWidth * player.Scale);
                    Position = new Vector2((int)player.Position.X + player.SpriteWidth * player.Scale, (int)player.Position.Y + 5);

                    break;
                case Vector2 v when v == new Vector2(-1, 0):
                    //Console.WriteLine("Left");
                    width = range;
                    height = (int)(player.SpriteWidth * player.Scale);
                    Position = new Vector2((int)player.Position.X - range + 5, (int)player.Position.Y + 5);

                    break;
                case Vector2 v when v == new Vector2(0, -1):
                    //Console.WriteLine("up");
                    width = (int)(player.SpriteWidth * player.Scale);
                    height = range;
                    Position = new Vector2((int)player.Position.X + 5, (int)player.Position.Y - range + 5);

                    break;
                case Vector2 v when v == new Vector2(0, 1):
                    //Console.WriteLine("Down");
                    width = (int)(player.SpriteWidth * player.Scale) - 5;
                    height = range;
                    Position = new Vector2((int)player.Position.X + 5, (int)player.Position.Y + player.SpriteHeight * player.Scale - 5);

                    break;
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                attacking = true;

                Attack(map.ActualRoom.Enemies, player.Direction, player.playerStats);
            }
            if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                attacking = false;
            }
        }

        public virtual void LoadContent(ContentManager content)

        {

            // Implementation for loading content specific to the weapon

        }

        public virtual void Attack(List<Enemy> enemies, Vector2 direction, Stats playerStats)
        {
            if (timeSinceLastAttack >= attackCooldown)
            {
                timeSinceLastAttack = 0f;
                foreach (var enemy in enemies)
                {
                    if (enemy.Hitbox.Intersects(new Rectangle((int)position.X, (int)position.Y, width, height)))
                    {
                        enemy.Hp -= Damage * playerStats.Attack;
                    }
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

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (attacking)
            {
                spriteBatch.FillRectangle(new Rectangle((int)position.X, (int)position.Y, width, height), Color.Red);
            }
        }

    }
}