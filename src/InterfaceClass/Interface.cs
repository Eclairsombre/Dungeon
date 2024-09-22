using Dungeon.src.PlayerClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Dungeon.src.InterfaceClass
{
    public class Interface
    {
        private Rectangle xpBar = new(30, 70, 200, 20);

        private Rectangle heart = new(30, 10, 50, 50);


        public Interface()
        {
        }
        public void Draw(SpriteBatch spriteBatch, Player player)
        {
            float xpPercentage = (float)player.Xp / player.XpToLevelUp;
            Rectangle filledXpBar = new(xpBar.X, xpBar.Y, (int)(xpBar.Width * xpPercentage), xpBar.Height);
            spriteBatch.FillRectangle(filledXpBar, Color.Green);
            spriteBatch.DrawRectangle(xpBar, Color.Black);
            for (int i = 0; i < player.NbHeart; i++)
            {
                spriteBatch.FillRectangle(new Rectangle(heart.X + i * 60, heart.Y, heart.Width, heart.Height), Color.Red);
            }
        }
    }
}