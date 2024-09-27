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
    public class Menu(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ContentManager content)
    {
        private GraphicsDevice _graphicsDevice = graphicsDevice;
        private SpriteBatch _spriteBatch = spriteBatch;

        private ContentManager _content = content;


        private Dungeon dungeon;
        private GameState gameState = GameState.Menu;

        private Bouton playButton = new Bouton(100, 100, 300, 100, GameState.Playing, "PlayBouton-Sheet");


        public GameState GameState { get { return gameState; } set { gameState = value; } }
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