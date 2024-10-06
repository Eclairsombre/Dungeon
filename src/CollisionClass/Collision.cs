
using Dungeon.src.MapClass;
using Microsoft.Xna.Framework;

namespace Dungeon.src.CollisionClass
{
    public static class Collision
    {
        public static bool CheckCollisionTwoRect(Rectangle rect1, Rectangle rect2)
        {
            if (rect1.Intersects(rect2))
            {
                return true;
            }
            return false;
        }

        public static bool CheckCollisionWithRoom(Rectangle rect, Room room)
        {
            for (int i = 0; i < room.Tiles.GetLength(0); i++)
            {
                for (int y = 0; y < room.Tiles.GetLength(1); y++)
                {
                    if (room.Tiles[i, y].Id.Item1 == 1 || room.Tiles[i, y].Id.Item1 == 5)
                    {

                        if (CheckCollisionTwoRect(rect, room.Tiles[i, y].Hitbox))
                        {
                            return true;
                        }
                    }
                    else if (room.Tiles[i, y].Id.Item1 == 4 && room.Finished)
                    {
                        if (CheckCollisionTwoRect(rect, room.Tiles[i, y].Holder.Hitbox))
                        {
                            return true;
                        }
                    }

                }
            }
            return false;
        }

        public static (bool, Door) CheckCollisionWithDoor(Rectangle rectangle, Room room)
        {
            if (room.Finished)
            {
                Door door1 = room.Tiles[8, 0].Door;
                Door door2 = room.Tiles[12, 0].Door;
                if (door1 != null)
                {
                    if (rectangle.Intersects(door1.Hitbox))
                    {
                        return (true, door1);
                    }
                }
                if (door2 != null)
                {
                    if (rectangle.Intersects(door2.Hitbox))
                    {
                        return (true, door2);
                    }
                }

            }
            return (false, null);
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
    }
}