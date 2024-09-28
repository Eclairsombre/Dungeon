using System;
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
        Pause,
        Exit,
        LevelUp
    }
    public class Menu
    {
        private readonly GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;

        private readonly ContentManager _content;


        private Dungeon dungeon;

        private readonly Options options;
        private GameState gameState = GameState.Menu;
        private GameState previousGameState = GameState.Menu;

        public GameState GameState { get { return gameState; } set { gameState = value; } }

        private readonly Bouton playButton, optionsButton, exitButton;


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
            optionsButton = new Bouton(buttonX, buttonY + buttonHeight + 10, buttonWidth, buttonHeight, GameState.Options, "OptionsBouton-Sheet");
            exitButton = new Bouton(buttonX, buttonY + 2 * (buttonHeight + 10), buttonWidth, buttonHeight, GameState.Exit, "ExitBouton-Sheet");

            options = new Options(graphicsDevice);
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
            optionsButton.LoadContent(_content);
            exitButton.LoadContent(_content);

            options.LoadContent(_content);

        }
        public void Update(GameTime gameTime)
        {


            switch (gameState)
            {
                case GameState.Menu:
                    playButton.Update(gameTime);
                    optionsButton.Update(gameTime);
                    exitButton.Update(gameTime);

                    if (!dungeon.quitButton.isClicked)
                    {
                        playButton.OnClick(ref gameState, ref previousGameState);
                        optionsButton.OnClick(ref gameState, ref previousGameState);
                        exitButton.OnClick(ref gameState, ref previousGameState);
                    }
                    else
                    {
                        dungeon.quitButton.Update(gameTime);
                    }

                    break;
                case GameState.Playing:
                case GameState.Pause:
                case GameState.LevelUp:
                    dungeon.UpdatePlaying(gameTime, _content, ref gameState, ref previousGameState);
                    break;
                case GameState.Options:
                    options.Update(gameTime, ref gameState, ref previousGameState);
                    break;
                case GameState.Exit:
                    Environment.Exit(0);
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
                    optionsButton.Draw(_spriteBatch);
                    exitButton.Draw(_spriteBatch);
                    break;
                case GameState.Playing:
                case GameState.Pause:
                case GameState.LevelUp:
                    dungeon.Draw(_spriteBatch, gameState);
                    break;
                case GameState.Options:
                    options.Draw(_spriteBatch);
                    break;

            }

            _spriteBatch.End();
        }
    }
}