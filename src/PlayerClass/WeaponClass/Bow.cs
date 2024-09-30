using System;
using System.Collections.Generic;
using Dungeon.src.EnemyClass;
using Dungeon.src.MapClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dungeon.src.PlayerClass.WeaponClass
{
    public class Bow : Weapon
    {
        private readonly List<Arrow> arrows = [];
        public Bow(Vector2 position) : base(position)
        {
            attackCooldown = 0.5f;
        }


        public void Attack(Vector2 direction, ContentManager content)
        {
            if (timeSinceLastAttack >= attackCooldown)
            {
                timeSinceLastAttack = 0f;
                Arrow arrow = new(Position, direction, 400f, content);
                arrow.LoadContent(content);
                arrows.Add(arrow);
            }
        }

        public override void Update(Player player, GameTime gameTime, Map map, ContentManager content)
        {
            timeSinceLastAttack += (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position = player.CenterPosition;
            Tiles[,] tiles = map.ActualRoom.Tiles;
            List<Enemy> enemies = map.ActualRoom.Enemies;

            List<Arrow> arrowsToRemove = [];

            foreach (var arrow in arrows)
            {
                arrow.Update(gameTime);

                for (int i = 0; i < enemies.Count; i++)
                {
                    if (arrow.Hitbox.Intersects(enemies[i].Hitbox))
                    {
                        enemies[i].Hp -= Damage * player.PlayerStats.Attack;
                        arrowsToRemove.Add(arrow);
                    }
                }
            }

            foreach (var arrow in arrows)
            {
                foreach (var tile in tiles)
                {
                    if ((tile.Id.Item1 == 1 || tile.Id.Item1 == 2) && tile.Hitbox.Intersects(arrow.Hitbox))
                    {
                        arrowsToRemove.Add(arrow);
                    }
                }
            }

            foreach (var arrow in arrowsToRemove)
            {
                arrows.Remove(arrow);
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Attack(player.Direction, content);
            }
            if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
            }

        }

        public override void LoadContent(ContentManager content)
        {
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < arrows.Count; i++)
            {
                arrows[i].Draw(spriteBatch);
            }
        }
    }
}