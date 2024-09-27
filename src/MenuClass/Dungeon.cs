using Dungeon.src.InterfaceClass;
using Dungeon.src.MapClass;
using Dungeon.src.PlayerClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dungeon.src.MenuClass
{
    public class Dungeon
    {
        private Player player;
        private Map map;
        private Interface gameInterface;

        private float pauseCooldown = 0.2f;
        private float pauseTimer = 0;

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            player = new Player(graphicsDevice);
            map = new Map();
            gameInterface = new Interface();
        }

        public void LoadContent(ContentManager content)
        {
            player.LoadContent(content);
            map.GenerateDungeon(content);
            gameInterface.LoadContent(content);
        }

        public void UpdatePlaying(GameTime gameTime, ContentManager content, ref GameState gameState)
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


            if (gameState == GameState.Pause)
            {
                return;
            }
            player.Update(gameTime, map, content);
            map.Update(player.CenterPosition, gameTime);
            gameInterface.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            map.Draw(spriteBatch);
            player.Draw(spriteBatch);
            gameInterface.Draw(spriteBatch, player);
        }
    }
}