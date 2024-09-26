using System;
using System.Collections.Generic;
using Dungeon.src.EnemyClass;
using Dungeon.src.MapClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
            this.attackCooldown = 0.5f;
        }


        public void Attack(Vector2 direction, Vector2 position, ContentManager content)
        {
            if (timeSinceLastAttack >= attackCooldown)
            {

                timeSinceLastAttack = 0f;


                arrows.Add(new Arrow(Position, direction, 400f, content));
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

                for (int i = 0; i < enemies.Count; i++)
                {
                    if (arrow.Hitbox.Intersects(enemies[i].Hitbox))
                    {
                        enemies[i].Hp -= Damage;
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

        }

        public override void LoadContent(ContentManager content)
        {
            arrowTexture = (content.Load<Texture2D>("Sprites/ArrowSpriteSheet"));

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < arrows.Count; i++)
            {
                arrows[i].Draw(spriteBatch, arrowTexture);
            }
        }
    }
}