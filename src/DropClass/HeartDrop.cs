using Dungeon.src.AnimationClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;


namespace Dungeon.src.DropClass
{
    public class HeartDrop : Drop
    {
        private readonly CallBack callBack = new();
        private readonly Animation _animation;
 public HeartDrop(int x, int y, int height, int width) : base(x, y, height, width)
        {
            callBack = new CallBack();
            _animation = new Animation("Coeur-Sheet", callBack.StaticMyCallback, 1, 0);
            _animation.ParseData();
        }

        public override void Update(GameTime gameTime)
        {
            _animation.Update(gameTime);
        }

        public override void LoadContent(ContentManager content)
        {
            _animation.LoadContent(content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_animation.texture, new Vector2(Hitbox.X, Hitbox.Y), callBack.SourceRectangle, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
        }
    }
}