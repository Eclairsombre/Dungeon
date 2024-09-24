using System;
using System.Collections.Generic;
using Dungeon.src.EnemyClass;
using Dungeon.src.MapClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon.src.PlayerClass.WeaponClass
{
    public class Bow : Weapon
    {
        private List<Arrow> arrows = [];
        private Texture2D arrowTexture;

        public Bow(Vector2 position) : base(position)
        {
            //this.arrowTexture = arrowTexture;
            this.attackCooldown = 0.1f;
        }

        public void Attack(Vector2 direction, Vector2 position)
        {
            if (timeSinceLastAttack >= attackCooldown)
            {

                timeSinceLastAttack = 0f;
                Vector2 newDir;
                if (direction == new Vector2(0, -1))
                {
                    newDir = new Vector2(0, 1);
                }
                else if (direction == new Vector2(0, 1))
                {
                    newDir = new Vector2(0, -1);
                }
                else
                {
                    newDir = direction;
                }
                arrows.Add(new Arrow(Position, newDir, 200f));
            }
        }

        public void Update(GameTime gameTime, List<Enemy> enemies, Vector2 position, Tiles[,] tiles)
        {
            timeSinceLastAttack += (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.Position = position;

            List<Arrow> arrowsToRemove = [];

            foreach (var arrow in arrows)
            {
                arrow.Update(gameTime);

                foreach (var enemy in enemies)
                {
                    if (arrow.Hitbox.Intersects(enemy.Hitbox))
                    {
                        enemy.Hp -= Damage;
                        arrowsToRemove.Add(arrow);
                    }
                }
            }

            foreach (var arrow in arrows)
            {
                foreach (var tile in tiles)
                {
                    if (tile.Id.Item1 == 1 || tile.Id.Item1 == 2 && tile.Hitbox.Intersects(arrow.Hitbox))
                    {
                        arrowsToRemove.Add(arrow);
                    }
                }
            }

            foreach (var arrow in arrowsToRemove)
            {
                arrows.Remove(arrow);
            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var arrow in arrows)
            {
                arrow.Draw(spriteBatch, arrowTexture);
            }
        }
    }
}