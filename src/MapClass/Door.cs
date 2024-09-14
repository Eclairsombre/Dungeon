using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
namespace Dungeon.src.MapClass;

public class Door
{


    public Rectangle hitbox;
    public Door(int x, int y, int width, int height)
    {


        hitbox = new Rectangle(x, y, width, height);

    }
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.FillRectangle(hitbox, Color.Red);
    }
}