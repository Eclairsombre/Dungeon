using Dungeon.src.DropClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Dungeon.src.MapClass.HolderClass
{
    public class DropHolder : Holder
    {

        public DropHolder(int x, int y, Drop drop) : base(x, y)
        {
            DropHold = drop;


        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle dropRectangle = new(hitbox.X + (hitbox.Width - DropHold.Width) / 2, hitbox.Y - DropHold.Height - 10, DropHold.Width, DropHold.Height);
            spriteBatch.FillRectangle(hitbox, Color.White);
            DropHold.Draw(spriteBatch);
        }
    }
}