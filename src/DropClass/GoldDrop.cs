using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Dungeon.src.DropClass
{
    public class GoldDrop(int x, int y, int width, int height, int amount) : Drop(x, y, width, height)
    {

        private int amount = amount;




        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(Hitbox, Color.Gold);
        }
        public override void Update(GameTime gameTime)
        {

        }
    }
}