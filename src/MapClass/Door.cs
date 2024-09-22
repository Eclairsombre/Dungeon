using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
namespace Dungeon.src.MapClass;

public class Door(int x, int y, int width, int height)
{
    private Rectangle hitbox = new(x, y, width, height);

    public Rectangle Hitbox { get { return hitbox; } }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.FillRectangle(hitbox, Color.Red);
    }
}