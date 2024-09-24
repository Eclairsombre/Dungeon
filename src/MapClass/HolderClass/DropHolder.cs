using Dungeon.src.DropClass;

namespace Dungeon.src.MapClass.HolderClass
{
    public class DropHolder(int x, int y, Drop drop) : Holder(x, y)
    {

        private Drop drop = drop;

        public Drop DropHold { get { return drop; } set { drop = value; } }

    }
}