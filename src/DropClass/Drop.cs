using System;
using System.Collections.Generic;
using System.Linq;
using Dungeon.src.MapClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Dungeon.src.PlayerClass;
using System.IO;
namespace Dungeon.src.DropClass
{
    public class Drop(int x, int y, int height, int width)
    {
        public int x = x, y = y, height = height, width = width;

        public Rectangle hitbox = new Rectangle(x, y, width, height);

        public bool IsColliding(Rectangle player)
        {
            if (player.Intersects(new Rectangle(x, y, width, height)))
            {
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture = null)
        {
            if (texture == null)
            {
                spriteBatch.FillRectangle(new Rectangle(x, y, width, height), Color.Yellow);
                return;
            }
            else
            {
                spriteBatch.Draw(texture, new Rectangle(x, y, width, height), Color.White);
            }
        }



    }
}