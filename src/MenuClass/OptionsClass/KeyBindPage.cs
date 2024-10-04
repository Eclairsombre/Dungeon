using Dungeon.src.TexteClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon.src.MenuClass.OptionsClass
{
    public class KeyBindPage(GraphicsDevice graphicsDevice, ContentManager content)
    {
        private readonly Texte title = new(content, "Key Bindings", new Vector2(graphicsDevice.Viewport.Width / 2 - 150, 50), Color.Black, 50);


        public void LoadContent(ContentManager content)
        {
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            title.Draw(spriteBatch);
        }
    }
}