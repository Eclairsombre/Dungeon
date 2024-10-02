using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon.src.TexteClass
{
    public class Texte(ContentManager content, string text, Vector2 position, Color color, int size = 12)
    {
        private readonly SpriteFont _font = content.Load<SpriteFont>("VCR_OSD_MONO_1");
        private string _text = text;
        private Vector2 _position = position;
        private Color _color = color;
        private int _size = size;

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 textSize = _font.MeasureString(_text);
            Vector2 adjustedPosition = new(_position.X - textSize.X / 2, _position.Y - textSize.Y / 2);
            spriteBatch.DrawString(_font, _text, adjustedPosition, _color, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }

        public void Update(GraphicsDevice GraphicsDevice)
        {
            // Update logic here if needed
        }

        public string Content
        {
            get { return _text; }
            set { _text = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }
    }
}