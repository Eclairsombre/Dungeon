using Dungeon.src.PlayerClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Dungeon.src.DropClass
{
    public class XpDrop(int x, int y, int height, int width, int xp) : Drop(x, y, height, width)
    {
        public int xp = xp;

        new Color color = Color.Green;
    }


}
