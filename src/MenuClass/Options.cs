using Dungeon.src.MenuClass.BoutonClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon.src.MenuClass
{
    public class Options
    {
        private readonly Bouton backButton;

        public Options(GraphicsDevice graphicsDevice)
        {
            int screenHeight = graphicsDevice.Viewport.Height;
            int buttonWidth = 100;
            int buttonHeight = 100;

            int buttonX = 10;
            int buttonY = screenHeight - buttonHeight * 2;

            backButton = new Bouton(buttonX, buttonY, buttonWidth, buttonHeight, GameState.Menu, "BackBouton-Sheet");
        }

        public void Update(GameTime gameTime, ref GameState gameState, ref GameState previousGameState)
        {
            backButton.Update(gameTime);
            GameState previousGameState1 = previousGameState;
            backButton.OnClick(ref gameState, ref previousGameState);
            if (previousGameState1 != previousGameState)
            {
                gameState = previousGameState1;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            backButton.Draw(spriteBatch);
        }

        public void LoadContent(ContentManager content)
        {
            backButton.LoadContent(content);
        }
    }
}