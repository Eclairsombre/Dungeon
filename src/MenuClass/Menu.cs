using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dungeon.src.MenuClass
{
    public enum GameState
    {
        Menu,
        Playing,
        Options,
        Pause
    }
    public class Menu
    {
        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;

        private ContentManager _content;


        private Dungeon dungeon;
        private GameState gameState = GameState.Menu;

        public GameState GameState { get { return gameState; } set { gameState = value; } }

        private Bouton playButton;


        public Menu(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ContentManager content)
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = spriteBatch;
            _content = content;

            int screenWidth = graphicsDevice.Viewport.Width;
            int screenHeight = graphicsDevice.Viewport.Height;
            int buttonWidth = 300;
            int buttonHeight = 100;

            int buttonX = (screenWidth - buttonWidth) / 2;
            int buttonY = (screenHeight - buttonHeight) / 2;

            playButton = new Bouton(buttonX, buttonY, buttonWidth, buttonHeight, GameState.Playing, "PlayBouton-Sheet");
        }
        public void Initialize()
        {
            dungeon = new Dungeon();
            dungeon.Initialize(_graphicsDevice);

        }
        public void LoadContent()
        {
            _spriteBatch = new SpriteBatch(_graphicsDevice);
            dungeon.LoadContent(_content);
            playButton.LoadContent(_content);

        }
        public void Update(GameTime gameTime)
        {

            switch (gameState)
            {
                case GameState.Menu:
                    playButton.Update(gameTime);
                    gameState = playButton.OnClick(this);
                    break;
                case GameState.Playing:
                case GameState.Pause:
                    dungeon.UpdatePlaying(gameTime, _content, ref gameState);
                    break;
                case GameState.Options:
                    break;
            }
        }

        public void Draw()
        {
            _spriteBatch.Begin();

            switch (gameState)
            {
                case GameState.Menu:
                    playButton.Draw(_spriteBatch);
                    break;
                case GameState.Playing:
                case GameState.Pause:
                    dungeon.Draw(_spriteBatch);
                    break;
                case GameState.Options:
                    break;

            }

            _spriteBatch.End();
        }
    }
}