using Microsoft.Xna.Framework;

namespace Dungeon.src.EnemyClass
{
    public class Node
    {
        public Point Position { get; set; }
        public Node Parent { get; set; }

        public Node(Point position)
        {
            Position = position;
        }
    }
}