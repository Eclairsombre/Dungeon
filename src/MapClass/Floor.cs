using Dungeon.src.AnimationClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon.src.MapClass
{
    public class Floor
    {
        private Rectangle hitbox;
        private Animation _animation;
        private readonly CallBack callBack;

        public Floor(int x, int y, int width, int height, string file)
        {
            hitbox = new Rectangle(x, y, width, height);
            callBack = new CallBack();
            _animation = new Animation(file, callBack.StaticMyCallback, 0, 0);
            _animation.ParseData();
        }

        public void LoadContent(ContentManager content)
        {
            _animation.LoadContent(content);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_animation.texture, new Vector2(hitbox.X, hitbox.Y), callBack.SourceRectangle, Color.White);
        }
    }
}