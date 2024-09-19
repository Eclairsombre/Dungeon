using Dungeon.src.PlayerClass;

namespace Dungeon.src.DropClass
{
    public class XpDrop(int x, int y, int height, int width, int xp) : Drop(x, y, height, width)
    {
        public int xp = xp;
    }
}