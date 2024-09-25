using Dungeon.src.InterfaceClass;
using Dungeon.src.MapClass;
using Dungeon.src.PlayerClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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
        private GameState gameState = GameState.Playing;

        public void Initialize()
        {
            dungeon = new Dungeon();
            dungeon.Initialize(_graphicsDevice);

        }

        public void LoadContent()
        {
            _spriteBatch = new SpriteBatch(_graphicsDevice);
            dungeon.LoadContent(_content);

        }

        public void Update(GameTime gameTime)
        {

            switch (gameState)
            {
                case GameState.Menu:
                    break;
                case GameState.Playing:
                    dungeon.UpdatePlaying(gameTime, content);
                    break;
                case GameState.Options:
                    break;
                case GameState.Pause:
                    break;
            }

        }

        public void Draw()
        {
            _spriteBatch.Begin();

            switch (gameState)
            {
                case GameState.Menu:
                    break;
                case GameState.Playing:
                    dungeon.Draw(_spriteBatch);
                    break;
                case GameState.Options:
                    break;
                case GameState.Pause:
                    break;
            }

            _spriteBatch.End();
        }
    }
}