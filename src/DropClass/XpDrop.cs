using Dungeon.src.AnimationClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;


namespace Dungeon.src.DropClass
{
    public class XpDrop : Drop
    {
        private int xp;
        public int Xp { get { return xp; } set { xp = value; } }

        private Animation animation;
        private CallBack callBack = new();

        private float scale;
        public XpDrop(int x, int y, int height, int width, int xp, float scale) : base(x, y, height, width)
        {
            this.xp = xp;
            color = Color.Green;
            this.scale = scale;
            animation = new("XpDrop", callBack.StaticMyCallback, 0, 0);
            animation.ParseData();
        }

        public override void LoadContent(ContentManager content)
        {
            animation.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            animation.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(animation.texture, new Vector2(Hitbox.X, Hitbox.Y), callBack.SourceRectangle, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            //spriteBatch.FillRectangle(Hitbox, color);
        }
    }
}
