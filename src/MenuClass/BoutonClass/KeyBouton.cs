using System;
using Dungeon.src.AnimationClass;
using Dungeon.src.PlayerClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Dungeon.src.MenuClass.OptionsClass
{
    public class KeyBouton
    {
        private Keys keys;

        private Rectangle hitbox;
        private readonly CallBack callBack;
        private readonly Animation _animation;
        public bool isClicked;

        private readonly float _scale = 1f;

        private string name;

        private Keys pressedKey = Keys.None;

        private Texture2D texture, noKeyTexture;



        public KeyBouton(int x, int y, int width, int height, Keys keys, string file, string name)
        {
            hitbox = new Rectangle(x, y, width, height);

            this.keys = keys;
            this.name = name;
            if (file != null)
            {
                callBack = new CallBack();
                _animation = new Animation(file, callBack.StaticMyCallback, 0, 0);
                _animation.ParseData();
            }

            isClicked = false;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.FillRectangle(hitbox, Color.Red);
            spriteBatch.Draw(texture, new Vector2(hitbox.X, hitbox.Y), Color.White);
            //spriteBatch.Draw(_animation.texture, new Vector2(hitbox.X, hitbox.Y), callBack.SourceRectangle, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Sprites/KeyBindSprite/" + keys.ToString() + "KeyBouton");
            noKeyTexture = content.Load<Texture2D>("Sprites/KeyBindSprite/NoneKeyBouton");
            //_animation.LoadContent(content);
        }

        public void SetPosition(int x, int y)
        {
            hitbox.X = x;
            hitbox.Y = y;
        }

        public void SetSize(int width, int height)
        {
            hitbox.Width = width;
            hitbox.Height = height;
        }

        public virtual void Update(GameTime gameTime, ref KeyBind keybind, ContentManager content)
        {
            MouseState mouseState = Mouse.GetState();

            if (!isClicked && mouseState.LeftButton == ButtonState.Pressed && hitbox.Contains(mouseState.Position))
            {
                isClicked = true;
            }

            if (isClicked)
            {

                Keys[] pressedKeys;
                texture = noKeyTexture;

                if (pressedKey == Keys.None)
                {
                    pressedKeys = Keyboard.GetState().GetPressedKeys();
                    pressedKey = pressedKeys.Length > 0 ? pressedKeys[0] : Keys.None;

                }
                if (pressedKey == Keys.Escape)
                {
                    pressedKey = Keys.None;
                    isClicked = false;
                    return;
                }

                if (pressedKey != Keys.None && pressedKey != Keys.Escape)
                {
                    Keys lastBind = keys;
                    foreach (string key in keybind.keyBindings.Keys)
                    {
                        Console.WriteLine(key);
                        foreach (Keys key1 in keybind.GetKeys(key))
                        {
                            if (key1 == pressedKey)
                            {
                                pressedKey = Keys.None;
                                return;
                            }
                        }

                    }

                    Console.WriteLine(pressedKey);
                    keys = pressedKey;
                    texture = content.Load<Texture2D>("Sprites/KeyBindSprite/" + keys.ToString() + "KeyBouton");
                    UpdateKeyBindFile(ref keybind, lastBind);
                    isClicked = false;
                    pressedKey = Keys.None;

                }
            }
        }






        public void UpdateKeyBindFile(ref KeyBind keyBind, Keys lastBind)
        {
            Keys[] newKeys = keyBind.GetKeys(name);
            for (int i = 0; i < newKeys.Length; i++)
            {
                if (newKeys[i] == lastBind)
                {
                    newKeys[i] = keys;
                }
            }
            keyBind.SetKeys(name, newKeys);
            keyBind.SaveData();

        }




    }
}