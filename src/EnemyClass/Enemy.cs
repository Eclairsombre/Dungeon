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

        public Enemy()
        {
            this.width = 50;
            this.height = 50;
            this.Position = new Vector2(200, 200);
            this.Direction = new Vector2(1, 0); // Direction initiale (vers la droite)
        }

        public void Update(Vector2 playerPosition, Room room)
        {
        }

        public void FollowPlayer(Point playerTileIndex, Room room)
        {
            Tiles[,] tiles = room.tiles;

        }


        public bool CheckCollisionWithRoom(Tiles[,] tiles, Vector2 potentialPosition)
        {
            Rectangle enemyHitbox = new Rectangle((int)potentialPosition.X, (int)potentialPosition.Y, width, height);

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



        public void Draw(SpriteBatch spriteBatch, Room room)
        {
            Rectangle rect = new Rectangle((int)Position.X, (int)Position.Y, width, height);
            spriteBatch.FillRectangle(rect, Color.Blue);

            // Dessiner le champ de vision
            DrawVision(spriteBatch, room);
        }

        public void DrawVision(SpriteBatch spriteBatch, Room room)
        {
            // Dessiner un cône pour représenter l'angle de vision
            Vector2 leftBoundary = Vector2.Transform(Direction, Matrix.CreateRotationZ(-VisionAngle / 2));
            Vector2 rightBoundary = Vector2.Transform(Direction, Matrix.CreateRotationZ(VisionAngle / 2));

            List<Vector2> visionVertices = new List<Vector2> { Position };
            visionVertices.Add(Position + leftBoundary * VisionRadius);
            visionVertices.Add(Position + rightBoundary * VisionRadius);

            // Vérifier les collisions avec les murs pour bloquer la vision
            for (int i = 0; i < visionVertices.Count - 1; i++)
            {
                Vector2 start = visionVertices[i];
                Vector2 end = visionVertices[i + 1];
                Vector2 direction = end - start;
                direction.Normalize();

                for (float j = 0; j < VisionRadius; j += 1f)
                {
                    Vector2 checkPosition = start + direction * j;
                    if (CheckCollisionWithRoom(room.tiles, checkPosition))
                    {
                        visionVertices[i + 1] = checkPosition;
                        break;
                    }
                }
            }

            // Dessiner le cône de vision
            for (int i = 0; i < visionVertices.Count - 1; i++)
            {
                spriteBatch.DrawLine(visionVertices[i], visionVertices[i + 1], Color.Yellow);
            }
        }
    }
}