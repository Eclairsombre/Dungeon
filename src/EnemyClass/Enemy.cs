using System;
using System.Collections.Generic;
using Dungeon.src.MapClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Dungeon.src.DropClass;

namespace Dungeon.src.EnemyClass
{
    public class Enemy
    {
        protected int hp = 1, damage = 1, speed = 3, xp = 10;
        protected Vector2 position;
        protected Vector2 direction;
        protected Vector2 line;
        protected int width, height;
        protected bool inVision = false;
        protected bool agroPlayer = false;
        protected float timeSinceLastSeenPlayer = 0f;
        private const float maxTimeWithoutSeeingPlayer = 3f;
        protected float VisionRadius { get; set; } = 150f;
        protected float VisionAngle { get; set; } = MathHelper.ToRadians(90f);
        protected float VisionRange { get; set; } = 500f;
        protected Drop[] loot = new Drop[1];
        protected Rectangle hitbox;
        protected Vector2 lastPlayerPosition;

        public Rectangle Hitbox { get { return hitbox; } }
        public int Hp { get { return hp; } set { hp = value; } }
        public int Damage { get { return damage; } set { damage = value; } }
        public Vector2 Position { get { return position; } set { position = value; } }
        public Vector2 Direction { get { return direction; } set { direction = value; } }

        public Drop[] Loot { get { return loot; } set { loot = value; } }
        public Enemy()
        {
            width = 50;
            height = 50;
            Position = new Vector2(300, 200);
            Direction = new Vector2(1, 0);
            hitbox = new Rectangle((int)Position.X, (int)Position.Y, width, height);
            loot[0] = new XpDrop((int)Position.X, (int)Position.Y, 10, 10, xp);
        }

        public void Update(Vector2 playerPosition, Room room, GameTime gameTime)
        {
            hitbox = new Rectangle((int)position.X, (int)position.Y, width, height);

            Tiles[,] tiles = room.Tiles;

            if (agroPlayer)
            {
                FollowPlayer(playerPosition, room);

                if (!inVision)
                {
                    timeSinceLastSeenPlayer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (timeSinceLastSeenPlayer >= maxTimeWithoutSeeingPlayer)
                    {
                        agroPlayer = false;
                        timeSinceLastSeenPlayer = 0f;
                    }
                }
                else
                {
                    timeSinceLastSeenPlayer = 0f;
                }
            }
            else
            {
                if (CheckCollisionWithRoom(tiles))
                {
                    direction = -direction;
                }
                position += direction * speed;
            }

            UpdateVision(room, playerPosition);
        }



        public void FollowPlayer(Vector2 playerPosition, Room room)
        {
            GoToo(lastPlayerPosition);
        }





        public void GoToo(Vector2 end)
        {

            Vector2 d = end - position;
            d.Normalize();
            position += d * speed;
            direction = d;


        }

        public bool CheckCollisionWithRoom(Tiles[,] tiles)
        {
            Rectangle enemyHitbox = new((int)position.X, (int)position.Y, width, height);

            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    if (tiles[i, y].Id.Item1 == 1)
                    {
                        if (enemyHitbox.Intersects(tiles[i, y].Hitbox))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool CheckCollisionVision(Tiles[,] tiles, Vector2 vision)
        {
            Rectangle view = new((int)vision.X, (int)vision.Y, 1, 1);

            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    if (tiles[i, y].Id.Item1 == 1)
                    {
                        if (view.Intersects(tiles[i, y].Hitbox))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle rect = new((int)position.X, (int)position.Y, width, height);
            spriteBatch.FillRectangle(rect, Color.Blue);
            DrawVision(spriteBatch);
        }

        public void UpdateVision(Room room, Vector2 playerPosition)
        {
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
                            if (CheckCollisionVision(room.Tiles, checkPosition))
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
        }

        public void DrawVision(SpriteBatch spriteBatch)
        {
            Vector2 centerPosition = new(position.X + width / 2, position.Y + height / 2);
            spriteBatch.DrawLine(centerPosition, line, Color.Yellow);
        }
    }
}