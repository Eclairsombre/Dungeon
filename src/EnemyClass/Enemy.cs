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

        public int width, height;

        public float VisionRadius { get; set; } = 150f; // Rayon du champ de vision
        public float VisionAngle { get; set; } = MathHelper.ToRadians(90f); // Angle du champ de vision en radians
        public float VisionRange { get; set; } = 1000f; // Portée du champ de vision

        public Enemy()
        {
            this.width = 50;
            this.height = 50;
            this.Position = new Vector2(300, 200);
            this.Direction = new Vector2(0, 1); // Direction initiale (vers le bas)
        }

        public void Update(Vector2 playerPosition, Room room)
        {
            this.Position.X += speed;
        }

        public void FollowPlayer(Point playerTileIndex, Room room)
        {
            Tiles[,] tiles = room.tiles;
            // Logique pour suivre le joueur
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
        public void Draw(SpriteBatch spriteBatch, Room room)
        {
            Rectangle rect = new Rectangle((int)Position.X, (int)Position.Y, width, height);
            spriteBatch.FillRectangle(rect, Color.Blue);

            DrawVision(spriteBatch, room);
        }

        public void DrawVision(SpriteBatch spriteBatch, Room room)
        {
            Vector2 centerPosition = new Vector2(Position.X + width / 2, Position.Y + height / 2);

            Vector2 leftBoundary = Vector2.Transform(Direction, Matrix.CreateRotationZ(-VisionAngle / 2));
            Vector2 rightBoundary = Vector2.Transform(Direction, Matrix.CreateRotationZ(VisionAngle / 2));

            Vector2 leftPoint = centerPosition + leftBoundary * VisionRange;
            Vector2 rightPoint = centerPosition + rightBoundary * VisionRange;

            List<Vector2> visionVertices = new List<Vector2> { centerPosition, leftPoint, rightPoint };

            for (int i = 1; i < visionVertices.Count; i++)
            {
                Vector2 start = visionVertices[0];
                Vector2 end = visionVertices[i];
                Vector2 direction = end - start;
                direction.Normalize();

                for (float j = 0; j < VisionRange; j += 1f)
                {
                    Vector2 checkPosition = start + direction * j;
                    if (CheckCollisionVision(room.tiles, checkPosition))
                    {
                        visionVertices[i] = checkPosition;
                        break;
                    }
                }
            }

            spriteBatch.DrawLine(visionVertices[0], visionVertices[1], Color.Yellow);
            spriteBatch.DrawLine(visionVertices[0], visionVertices[2], Color.Yellow);
            spriteBatch.DrawLine(visionVertices[1], visionVertices[2], Color.Yellow * 0.5f);
        }
    }
}