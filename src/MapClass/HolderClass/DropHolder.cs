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
            spriteBatch.FillRectangle(hitbox, Color.White);
            DropHold?.Draw(spriteBatch);
        }
    }
}