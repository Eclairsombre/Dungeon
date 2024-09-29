using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;


namespace Dungeon.src.DropClass
{
    public class XpDrop : Drop
    {
        private int xp;
        public int Xp { get { return xp; } set { xp = value; } }
        public XpDrop(int x, int y, int height, int width, int xp) : base(x, y, height, width)
        {
            this.xp = xp;
            color = Color.Green;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(Hitbox, color);
        }
    }
}
