using Dungeon.src.InterfaceClass;
using Dungeon.src.MapClass;
using Dungeon.src.PlayerClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon.src.MenuClass
{
    public class Dungeon
    {
        private Player player;
        private Map map;
        private Interface gameInterface;

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
        }

        public void UpdatePlaying(GameTime gameTime, ContentManager content)
        {
            player.Update(gameTime, map, content);
            map.Update(player.Position, gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            map.Draw(spriteBatch);
            player.Draw(spriteBatch);
            gameInterface.Draw(spriteBatch, player);
        }
    }
}