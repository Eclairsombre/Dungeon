using System;
using System.Linq;
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
        private CallBack callBack;
        private Animation _animation, voidAnimation;
        public bool isClicked;


        private readonly string name;

        private Keys pressedKey = Keys.None;





        public KeyBouton(int x, int y, int width, int height, Keys keys, string file, string name)
        {
            hitbox = new Rectangle(x, y, width, height);

            this.keys = keys;
            this.name = name;

            callBack = new CallBack();
            _animation = new Animation("KeyBindSprites\\" + keys.ToString() + "KeyBouton", callBack.StaticMyCallback, 0, 0);
            _animation.ParseData();

            voidAnimation = new Animation("KeyBindSprites\\NoneKeyBouton", callBack.StaticMyCallback, 0, 0);
            voidAnimation.ParseData();


            isClicked = false;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_animation.texture, new Vector2(hitbox.X, hitbox.Y), callBack.SourceRectangle, Color.White);
            //spriteBatch.Draw(texture, new Vector2(hitbox.X, hitbox.Y), Color.White);
        }

        public void LoadContent(ContentManager content)
        {
            _animation.LoadContent(content);
            voidAnimation.LoadContent(content);
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

            if (!isClicked)
            {
                if (hitbox.Contains(mouseState.Position) && _animation.GetTimeline() == 0)
                {
                    _animation.SetTimeLine(1);
                }
                else if (_animation.GetTimeline() == 1 && !hitbox.Contains(mouseState.Position))
                {
                    _animation.SetTimeLine(0);
                }
            }

            if (!isClicked && mouseState.LeftButton == ButtonState.Pressed && hitbox.Contains(mouseState.Position))
            {
                isClicked = true;
                (_animation, voidAnimation) = (voidAnimation, _animation);
            }

            if (isClicked)
            {

                Keys[] pressedKeys = Keyboard.GetState().GetPressedKeys();
                pressedKey = pressedKeys.Length > 0 ? pressedKeys[0] : Keys.None;


                if (pressedKey == Keys.Escape)
                {
                    ResetKeyBinding();
                    (_animation, voidAnimation) = (voidAnimation, _animation);
                    return;
                }
                if (pressedKey == Keys.Back)
                {
                    ResetKeyBinding();
                    UpdateKeyBinding(ref keybind, content);

                    return;
                }

                if (pressedKey != Keys.None && !IsKeyAlreadyBound(keybind, pressedKey))
                {
                    UpdateKeyBinding(ref keybind, content);
                }
            }
        }

        private void ResetKeyBinding()
        {
            pressedKey = Keys.None;
            isClicked = false;
        }

        private static bool IsKeyAlreadyBound(KeyBind keybind, Keys key)
        {
            return keybind.keyBindings.Values.Any(keys => keys.Contains(key));
        }

        private void UpdateKeyBinding(ref KeyBind keybind, ContentManager content)
        {
            voidAnimation = _animation;
            _animation = new Animation("KeyBindSprites\\" + pressedKey.ToString() + "KeyBouton", callBack.StaticMyCallback, 0, 0);
            _animation.ParseData();
            _animation.LoadContent(content);
            Keys lastBind = keys;
            keys = pressedKey;


            UpdateKeyBindFile(ref keybind, lastBind);
            ResetKeyBinding();
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