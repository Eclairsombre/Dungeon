using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;

namespace Dungeon.src.MenuClass
{
    public class Bouton(int x, int y, int width, int height, GameState gameState)
    {
        Rectangle hitbox = new(x, y, width, height);
        GameState gameState = gameState;

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(hitbox, Color.Black);
        }

        public GameState OnClick(Menu menu)
        {
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed && hitbox.Contains(mouseState.Position))
            {
                return gameState;
            }
            return menu.GameState;
        }

    }
}