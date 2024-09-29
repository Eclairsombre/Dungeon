using Dungeon.src.InterfaceClass;
using Dungeon.src.MapClass;
using Dungeon.src.MenuClass.BoutonClass;
using Dungeon.src.PlayerClass;
using Dungeon.src.PlayerClass.StatsClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dungeon.src.MenuClass
{
    public class Dungeon
    {
        private Player player;
        public Player Player { get { return player; } set { player = value; } }
        private Map map;
        private Interface gameInterface;

        private LevelUp levelUp;

        private readonly float pauseCooldown = 0.2f;
        private float pauseTimer = 0;

        private Rectangle pauseBackground;
        private Texture2D pauseBackgroundTexture;

        public Bouton resumeButton, quitButton, optionsButton, saveButton;

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            player = new Player(graphicsDevice);
            map = new Map();
            gameInterface = new Interface();

            int screenWidth = graphicsDevice.Viewport.Width;
            int screenHeight = graphicsDevice.Viewport.Height;
            int rectWidth = 400;
            int rectHeight = 500;
            pauseBackground = new Rectangle(
                (screenWidth - rectWidth) / 2,
                (screenHeight - rectHeight) / 2,
                rectWidth,
                rectHeight
            );

            int buttonWidth = 300;
            int buttonHeight = 100;

            int buttonX = (screenWidth - buttonWidth) / 2;

            resumeButton = new Bouton(buttonX, pauseBackground.Y + 50, buttonWidth, buttonHeight, GameState.Playing, "ResumeBouton-Sheet");
            optionsButton = new Bouton(buttonX, pauseBackground.Y + 50 + buttonHeight + 10, buttonWidth, buttonHeight, GameState.Options, "OptionsBouton-Sheet");
            saveButton = new Bouton(buttonX, pauseBackground.Y + 50 + 2 * (buttonHeight + 10), buttonWidth, buttonHeight, GameState.Menu, "SaveBouton-Sheet");
            quitButton = new Bouton(buttonX, pauseBackground.Y + 50 + 3 * (buttonHeight + 10), buttonWidth, buttonHeight, GameState.Menu, "ExitBouton-Sheet");

            levelUp = new LevelUp(graphicsDevice);



        }

        public void LoadContent(ContentManager content)
        {
            player.LoadContent(content);
            map.GenerateDungeon(content);
            gameInterface.LoadContent(content);
            pauseBackgroundTexture = content.Load<Texture2D>("Sprites/Background");
            resumeButton.LoadContent(content);
            optionsButton.LoadContent(content);
            saveButton.LoadContent(content);
            quitButton.LoadContent(content);

            levelUp.LoadContent(content);


        }

        public void UpdatePlaying(GameTime gameTime, ContentManager content, ref GameState gameState, ref GameState previousGameState)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && pauseTimer <= 0)
            {
                pauseTimer = pauseCooldown;
                if (gameState == GameState.Pause)
                {
                    gameState = GameState.Playing;
                }
                else
                {
                    gameState = GameState.Pause;
                }
            }
            else
            {
                pauseTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (gameState == GameState.LevelUp)
            {
                Stats playerStats = player.playerStats;
                levelUp.Update(gameTime, ref gameState, ref previousGameState, ref playerStats);
                player.playerStats = playerStats;
                return;
            }

            if (gameState == GameState.Pause)
            {
                var playerStats = player.playerStats;
                resumeButton.Update(gameTime);

                optionsButton.Update(gameTime);
                saveButton.Update(gameTime);
                quitButton.Update(gameTime);

                player.playerStats = playerStats;

                resumeButton.OnClick(ref gameState, ref previousGameState);
                optionsButton.OnClick(ref gameState, ref previousGameState);
                saveButton.OnClick(ref gameState, ref previousGameState);
                quitButton.OnClick(ref gameState, ref previousGameState);
                return;
            }
            player.Update(gameTime, map, content, ref gameState);
            map.Update(player.CenterPosition, gameTime);


            gameInterface.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameState gameState)
        {

            map.Draw(spriteBatch);
            player.Draw(spriteBatch);
            gameInterface.Draw(spriteBatch, player);
            if (gameState == GameState.LevelUp)
            {
                levelUp.Draw(spriteBatch);
            }
            if (gameState == GameState.Pause)
            {
                spriteBatch.Draw(pauseBackgroundTexture, pauseBackground, Color.White);
                resumeButton.Draw(spriteBatch);
                optionsButton.Draw(spriteBatch);
                saveButton.Draw(spriteBatch);
                quitButton.Draw(spriteBatch);
            }
        }
    }
}