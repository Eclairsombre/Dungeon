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

        public int width, height;

        public Enemy()
        {
            this.width = 70;
            this.height = 70;
            this.Position = new Vector2(200, 200);
        }

        public void Update(Vector2 playerPosition, Room room)
        {
            Point playerTileIndex = PositionToTileIndex(playerPosition);
            FollowPlayer(playerTileIndex, room);
        }

        public void FollowPlayer(Point playerTileIndex, Room room)
        {
            Tiles[,] tiles = room.tiles;
            Point enemyTileIndex = PositionToTileIndex(Position);

            List<Point> path = FindPath(enemyTileIndex, playerTileIndex, tiles);

            if (path.Count > 1)
            {
                Point nextTile = path[1];
                Vector2 nextPosition = new Vector2(nextTile.X * 70 + 35, nextTile.Y * 70 + 35);
                Vector2 direction = nextPosition - Position;
                direction.Normalize();
                Position += direction * speed;
                Console.WriteLine($"Moving to {nextPosition}, new position: {Position}");
            }
            else
            {
                Console.WriteLine("No valid path to follow");
            }
        }

        public bool CheckCollision(Rectangle rect)
        {
            if (rect.Intersects(new Rectangle((int)Position.X, (int)Position.Y, width, height)))
            {
                return true;
            }
            return false;
        }

        public bool CheckCollisionWithPlayer(Player player)
        {
            Rectangle playerHitbox = player.GetHitbox();
            if (CheckCollision(playerHitbox))
            {
                return true;
            }
            return false;
        }

        public bool CheckCollisionWithRoom(Tiles[,] tiles)
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    if (tiles[i, y].id == 1)
                    {
                        if (CheckCollision(tiles[i, y].hitbox))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public List<Point> FindPath(Point start, Point goal, Tiles[,] tiles)
        {
            Queue<Node> queue = new Queue<Node>();
            HashSet<Point> visited = new HashSet<Point>();

            Node startNode = new Node(start);
            queue.Enqueue(startNode);
            visited.Add(start);

            while (queue.Count > 0)
            {
                Node currentNode = queue.Dequeue();

                if (currentNode.Position == goal)
                {
                    var path = RetracePath(startNode, currentNode);
                    Console.WriteLine("Path found: " + string.Join(" -> ", path));
                    return path;
                }

                foreach (Node neighbor in GetNeighbors(currentNode, tiles))
                {
                    if (!visited.Contains(neighbor.Position))
                    {
                        neighbor.Parent = currentNode;
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor.Position);
                    }
                }
            }

            Console.WriteLine("No path found");
            return new List<Point>();
        }

        private List<Node> GetNeighbors(Node node, Tiles[,] tiles)
        {
            List<Node> neighbors = new List<Node>();

            Point[] directions = new Point[]
            {
                new Point(0, -1),
                new Point(0, 1),
                new Point(-1, 0),
                new Point(1, 0)
            };

            foreach (Point direction in directions)
            {
                Point neighborPos = node.Position + direction;
                int x = neighborPos.X;
                int y = neighborPos.Y;

                Point oppositeDirection = node.Position - direction;


                if (x >= 0 && x < tiles.GetLength(0) && y >= 0 && y < tiles.GetLength(1) && tiles[x, y].id != 1)
                {
                    if (this.CheckCollisionWithRoom(tiles))
                    {
                        //Console.WriteLine(oppositeDirection);
                        neighbors.Add(new Node(node.Position));
                    }
                    else
                    {
                        //Console.WriteLine(neighborPos);
                        neighbors.Add(new Node(neighborPos));

                    }
                }
            }

            return neighbors;
        }

        private List<Point> RetracePath(Node startNode, Node endNode)
        {
            List<Point> path = new List<Point>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode.Position);
                currentNode = currentNode.Parent;
            }

            path.Reverse();
            return path;
        }

        private Point PositionToTileIndex(Vector2 position)
        {
            int x = (int)(position.X / 70);
            int y = (int)(position.Y / 70);
            return new Point(x, y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle rect = new Rectangle((int)Position.X, (int)Position.Y, width, height);
            spriteBatch.FillRectangle(rect, Color.Blue);
        }
    }
}