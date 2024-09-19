using Dungeon.src.PlayerClass;

namespace Dungeon.src.DropClass
{
    public class XpDrop : Drop
    {
        public int xp;

        public XpDrop(int x, int y, int height, int width, int xp) : base(x, y, height, width)
        {
            this.xp = xp;
        }



    }
}