using Dungeon.src.DropClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Dungeon.src.MapClass.HolderClass
{
    public class DropHolder(int x, int y, Drop drop) : Holder(x, y)
    {
        private Drop drop = drop;
        public Drop DropHold { get { return drop; } set { drop = value; } }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle dropRectangle = new(hitbox.X + (hitbox.Width - drop.Width) / 2, hitbox.Y - drop.Height - 10, drop.Width, drop.Height);
            spriteBatch.FillRectangle(hitbox, Color.White);
            drop.Draw(spriteBatch);
        }
    }


}