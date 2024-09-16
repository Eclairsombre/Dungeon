using System;
using System.Collections.Generic;
using System.Linq;
using Dungeon.src.MapClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Dungeon.src.PlayerClass;

namespace Dungeon.src.EnemyClass
{
    public class Enemy
    {
        public int hp = 1, damage = 0, speed = 3;
        public Vector2 Position;
        public Vector2 Direction;
        public Vector2 line;
        public int width, height;

        public float VisionRadius { get; set; } = 150f;
        public float VisionAngle { get; set; } = MathHelper.ToRadians(90f);
        public float VisionRange { get; set; } = 250f;

        public Enemy()
        {
            this.width = 50;
            this.height = 50;
            this.Position = new Vector2(300, 200);
            this.Direction = new Vector2(0, 1);
        }

        public void Update(Vector2 playerPosition, Room room)
        {
            Tiles[,] tiles = room.tiles;

            if (CheckCollisionWithRoom(tiles))
            {
                this.speed = -this.speed;

            }
            this.Position.X += speed;

            UpdateVision(room, playerPosition);

        }

        public void FollowPlayer(Point playerTileIndex, Room room)
        {
            Tiles[,] tiles = room.tiles;
        }

        public bool CheckCollisionWithRoom(Tiles[,] tiles)
        {
            Rectangle enemyHitbox = new Rectangle((int)Position.X, (int)Position.Y, width, height);

            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    if (tiles[i, y].id == 1)
                    {
                        if (enemyHitbox.Intersects(tiles[i, y].hitbox))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool CheckCollisionVision(Tiles[,] tiles, Vector2 vision)
        {
            Rectangle view = new Rectangle((int)vision.X, (int)vision.Y, 1, 1);

            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    if (tiles[i, y].id == 1)
                    {
                        if (view.Intersects(tiles[i, y].hitbox))
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
            Rectangle rect = new Rectangle((int)Position.X, (int)Position.Y, width, height);
            spriteBatch.FillRectangle(rect, Color.Blue);

            DrawVision(spriteBatch);
        }

        public void UpdateVision(Room room, Vector2 playerPosition)
        {

            Vector2 centerPosition = new Vector2(Position.X + width / 2, Position.Y + height / 2);
            line = playerPosition - centerPosition;
            line.Normalize();
            line *= VisionRange;

            List<Vector2> visionVertices = new List<Vector2> { centerPosition, centerPosition + line };
            for (int i = 1; i < visionVertices.Count; i++)
            {
                Vector2 start = visionVertices[0];
                Vector2 end = visionVertices[i];
                Vector2 direction = end - start;
                direction.Normalize();

                for (float j = 0; j < VisionRange; j += 1f)
                {
                    Vector2 checkPosition = start + direction * j;
                    if (CheckCollisionVision(room.tiles, checkPosition) || j == (VisionRange - 1f))
                    {
                        visionVertices[i] = checkPosition;
                        break;
                    }
                }
            }

            line = visionVertices[1];

        }

        public void DrawVision(SpriteBatch spriteBatch)
        {
            Vector2 centerPosition = new Vector2(Position.X + width / 2, Position.Y + height / 2);

            spriteBatch.DrawLine(centerPosition, line, Color.Yellow);
        }
    }
}