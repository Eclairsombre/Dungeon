using System;
using System.Linq;
using Dungeon.src.MenuClass.BoutonClass;
using Dungeon.src.PlayerClass;
using Dungeon.src.TexteClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dungeon.src.MenuClass.OptionsClass
{
    public class KeyBindPage
    {
        private readonly Texte title;


        private readonly Texte[] keybindsTitle = new Texte[50];

        private readonly KeyBouton[] firstKey = new KeyBouton[50];
        private readonly KeyBouton[] secondKey = new KeyBouton[50];

        public KeyBindPage(GraphicsDevice graphicsDevice, ContentManager content, KeyBind keyBind)
        {
            title = new(content, "Key Bindings", new Vector2(graphicsDevice.Viewport.Width / 2 - 150, 50), Color.Black, 50);

            int x = 100;
            int y = 215;


            foreach (var var in keyBind.keyBindings)
            {
                keybindsTitle[keyBind.keyBindings.Keys.ToList().IndexOf(var.Key)] = new Texte(content, var.Key + " : ", new Vector2(x, y), Color.Black, 50);
                y += 100;
            }
            y = 200;
            foreach (var var in keyBind.keyBindings)
            {
                firstKey[keyBind.keyBindings.Keys.ToList().IndexOf(var.Key)] = new KeyBouton(x + 400, y + 5, 75, 75, var.Value[0], null, var.Key);
                if (var.Value.Length > 1)
                {
                    secondKey[keyBind.keyBindings.Keys.ToList().IndexOf(var.Key)] = new KeyBouton(x + 500, y + 5, 75, 75, var.Value[1], null, var.Key);
                }
                else
                {
                    secondKey[keyBind.keyBindings.Keys.ToList().IndexOf(var.Key)] = new KeyBouton(x + 500, y + 5, 75, 75, Keys.None, null, var.Key);
                }
                y += 100;
            }


        }

        public void LoadContent(ContentManager content)
        {

            for (int i = 0; i < keybindsTitle.Length; i++)
            {
                firstKey[i]?.LoadContent(content);
                secondKey[i]?.LoadContent(content);
            }
        }

        public void Update(GameTime gameTime, ref KeyBind keyBind, ContentManager content)
        {

            for (int i = 0; i < keybindsTitle.Length; i++)
            {
                firstKey[i]?.Update(gameTime, ref keyBind, content);
                secondKey[i]?.Update(gameTime, ref keyBind, content);
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            title.Draw(spriteBatch);

            for (int i = 0; i < keybindsTitle.Length; i++)
            {
                keybindsTitle[i]?.Draw(spriteBatch);
                firstKey[i]?.Draw(spriteBatch);
                secondKey[i]?.Draw(spriteBatch);
            }
        }
    }
}