using Dungeon.src.MenuClass.BoutonClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon.src.MenuClass.OptionsClass
{
    public enum OptionsState
    {
        KeyBindOptions,
        SoundOptions,
        WindowSettingOption

    }
    public class Options
    {
        private readonly Bouton backButton;

        private OptionsState optionsState = OptionsState.KeyBindOptions;

        private KeyBindPage keyBindPage;
        public Options(GraphicsDevice graphicsDevice, ContentManager content)
        {
            int buttonWidth = 100;
            int buttonHeight = 100;
            int buttonX = 10;
            int buttonY = 10;
            backButton = new Bouton(buttonX, buttonY, buttonWidth, buttonHeight, GameState.Menu, "BackBouton-Sheet");

            keyBindPage = new KeyBindPage(graphicsDevice, content);
        }

        public void Update(GameTime gameTime, ref GameState gameState, ref GameState previousGameState)
        {
            backButton.Update(gameTime);
            GameState previousGameState1 = previousGameState;
            backButton.OnClick(ref gameState, ref previousGameState);
            if (backButton.isClicked)
            {
                optionsState = OptionsState.KeyBindOptions;
            }
            if (previousGameState1 != previousGameState)
            {
                gameState = previousGameState1;
            }

            switch (optionsState)
            {
                case OptionsState.KeyBindOptions:
                    break;
                case OptionsState.SoundOptions:
                    break;
                case OptionsState.WindowSettingOption:
                    break;
            }
        }

        public void OnWindowResize(int screenWidth, int screenHeight)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            backButton.Draw(spriteBatch);
            switch (optionsState)
            {
                case OptionsState.KeyBindOptions:
                    keyBindPage.Draw(spriteBatch);
                    break;
                case OptionsState.SoundOptions:
                    break;
                case OptionsState.WindowSettingOption:
                    break;
            }
        }

        public void LoadContent(ContentManager content)
        {
            backButton.LoadContent(content);
        }
    }
}