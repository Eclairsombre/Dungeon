using Dungeon.src.PlayerClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Dungeon.src.DropClass
{
    public class XpDrop : Drop
    {
        public int xp;

        public XpDrop(int x, int y, int height, int width, int xp) : base(x, y, height, width)
        {
            this.xp = xp;
            this.color = Color.Green;
        }
    }


}
