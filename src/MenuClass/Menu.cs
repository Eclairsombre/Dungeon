using System;
using Dungeon.src.MenuClass.BoutonClass;
using Dungeon.src.MenuClass.OptionsClass;
using Dungeon.src.PlayerClass;
using Dungeon.src.TexteClass;
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

        private readonly Texte titre;

        private readonly GameWindow gameWindow;


        private KeyBind keyBind = new();

        public Menu(GraphicsDevice graphicsDevice, ContentManager content, GameWindow window)
        {
            _graphicsDevice = graphicsDevice;
            _content = content;
            gameWindow = window;

            keyBind.ParseData();


            gameWindow.ClientSizeChanged += OnWindowResize;

            int screenWidth = graphicsDevice.Viewport.Width;
            int screenHeight = graphicsDevice.Viewport.Height;
            int buttonWidth = 300;
            int buttonHeight = 100;

            int buttonX = screenWidth / 2 - buttonWidth / 2;
            int buttonY = screenHeight / 2 - buttonHeight / 2;

            playButton = new Bouton(buttonX, buttonY, buttonWidth, buttonHeight, GameState.Playing, "PlayBouton-Sheet");
            optionsButton = new Bouton(buttonX, buttonY + buttonHeight + 10, buttonWidth, buttonHeight, GameState.Options, "OptionsBouton-Sheet");
            exitButton = new Bouton(buttonX, buttonY + 2 * (buttonHeight + 10), buttonWidth, buttonHeight, GameState.Exit, "ExitBouton-Sheet");

            options = new Options(graphicsDevice, content, keyBind);

            titre = new Texte(content, "Dungeon", new Vector2(screenWidth / 2 - buttonWidth / 2, screenHeight / 4), Color.Black, 50);

        }
        public void Initialize()
        {
            dungeon = new Dungeon();
            dungeon.Initialize(_graphicsDevice, _content, keyBind);
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

        private void OnWindowResize(object sender, EventArgs e)
        {
            int screenWidth = _graphicsDevice.Viewport.Width;
            int screenHeight = _graphicsDevice.Viewport.Height;
            int buttonWidth = 300;
            int buttonHeight = 100;

            int buttonX = screenWidth / 2 - buttonWidth / 2;
            int buttonY = screenHeight / 2 - buttonHeight / 2;

            playButton.SetPosition(buttonX, buttonY);

            optionsButton.SetPosition(buttonX, buttonY + buttonHeight + 10);

            exitButton.SetPosition(buttonX, buttonY + 2 * (buttonHeight + 10));

            titre.SetPosition(screenWidth / 2 - buttonWidth / 2, screenHeight / 4);

            options.OnWindowResize(screenWidth, screenHeight);



        }
        public void Update(GameTime gameTime)
        {

            switch (gameState)
            {
                case GameState.Menu:

                    if (!dungeon.quitButton.isClicked)
                    {
                        playButton.Update(gameTime, ref gameState, ref previousGameState);
                        optionsButton.Update(gameTime, ref gameState, ref previousGameState);
                        exitButton.Update(gameTime, ref gameState, ref previousGameState);

                    }
                    else
                    {
                        dungeon.quitButton.Update(gameTime, ref gameState, ref previousGameState);
                    }

                    break;
                case GameState.Playing:
                case GameState.Pause:
                case GameState.LevelUp:
                    dungeon.UpdatePlaying(gameTime, _content, ref gameState, ref previousGameState, ref keyBind);
                    break;
                case GameState.Options:
                    options.Update(gameTime, ref gameState, ref previousGameState, ref keyBind, _content);
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
                    titre.Draw(_spriteBatch);

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