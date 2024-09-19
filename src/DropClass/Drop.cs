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
    public class Drop
    {
        public int x, y, height, width;

        public Rectangle hitbox;

        public Drop(int x, int y, int height, int width)
        {
            this.x = x;
            this.y = y;
            this.height = height;
            this.width = width;

            this.hitbox = new Rectangle(x, y, width, height);
        }

        public bool isColliding(Rectangle player)
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