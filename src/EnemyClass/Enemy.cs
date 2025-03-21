using System;
using System.Collections.Generic;
using Dungeon.src.MapClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Dungeon.src.DropClass;
using Dungeon.src.CollisionClass;
using Microsoft.Xna.Framework.Content;

namespace Dungeon.src.EnemyClass
{
    public class Enemy
    {
        protected int damage = 1, speed = 3, xp = 100, maxHp = 3;
        protected float hp = 3;
        protected Vector2 position;
        protected Vector2 direction;
        protected Vector2 line;
        protected int width, height;
        protected bool inVision = false;
        private const float maxTimeWithoutSeeingPlayer = 3f;
        protected float VisionRadius { get; set; } = 150f;
        protected float VisionAngle { get; set; } = MathHelper.ToRadians(90f);
        protected float VisionRange { get; set; } = 500f;
        protected Rectangle hitbox;
        protected Rectangle healthBar;
        public Rectangle Hitbox { get { return hitbox; } }
        public float Hp { get { return hp; } set { hp = value; } }
        public int Damage { get { return damage; } set { damage = value; } }
        public Vector2 Position { get { return position; } set { position = value; } }
        public Vector2 Direction { get { return direction; } set { direction = value; } }
        public Enemy()
        {
            width = 50;
            height = 50;
            Position = new Vector2();
            Direction = new Vector2(1, 0);
            hitbox = new Rectangle((int)Position.X, (int)Position.Y, width, height);
            healthBar = new Rectangle((int)Position.X, (int)Position.Y - 10, width, 5);
        }

        public void LoadContent(ContentManager content)
        {
        }

        public void Update( GameTime gameTime)
        {
            hitbox = new Rectangle((int)position.X, (int)position.Y, width, height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(hitbox, Color.Blue);

            Rectangle healthBarBackground = new((int)position.X, (int)position.Y - 10, width, 5);
            spriteBatch.FillRectangle(healthBarBackground, Color.Red);

            float healthPercentage = hp / maxHp;
            int healthBarWidth = (int)(width * healthPercentage);

            Rectangle healthBarForeground = new((int)position.X, (int)position.Y - 10, healthBarWidth, 5);
            spriteBatch.FillRectangle(healthBarForeground, Color.Green);
        }

        public void UpdateVision(Room room, Vector2 playerPosition)
        {
            /*
            Vector2 centerPosition = new(position.X + width / 2, position.Y + height / 2);
            Vector2 toPlayer = playerPosition - centerPosition;
            float distanceToPlayer = toPlayer.Length();
            if (distanceToPlayer <= VisionRange)
            {
                toPlayer.Normalize();
                float angleToPlayer = (float)Math.Acos(Vector2.Dot(direction, toPlayer));
                if (angleToPlayer <= VisionAngle / 2)
                {
                    lastPlayerPosition = playerPosition;
                    line = toPlayer * VisionRange;
                    List<Vector2> visionVertices = [centerPosition, centerPosition + line];
                    for (int i = 1; i < visionVertices.Count; i++)
                    {
                        Vector2 start = visionVertices[0];
                        Vector2 end = visionVertices[i];
                        Vector2 d = end - start;
                        d.Normalize();
                        for (float j = 0; j < VisionRange; j += 1f)
                        {
                            Vector2 checkPosition = start + d * j;
                            if (Collision.CheckCollisionVision(room.Tiles, checkPosition))
                            {
                                break;
                            }
                            if (j == (VisionRange - 1f))
                            {
                                agroPlayer = true;
                                visionVertices[i] = checkPosition;
                                inVision = true;
                                lastPlayerPosition = playerPosition;
                                timeSinceLastSeenPlayer = 0f;
                                break;
                            }
                        }
                    }
                    line = visionVertices[1];
                }
                else
                {
                    inVision = false;
                    line = centerPosition;
                }
            }
            else
            {
                inVision = false;
                line = centerPosition;
            }
            */
        }

        public void DrawVision(SpriteBatch spriteBatch)
        {
            Vector2 centerPosition = new(position.X + width / 2, position.Y + height / 2);
            spriteBatch.DrawLine(centerPosition, line, Color.Yellow);
        }
    }
}