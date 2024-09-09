using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;


namespace Dungeon;

public class Room
{
    public int x, y, width, height;

    public int id;

    public Door[] doors = new Door[4];
    public Room(int x, int y, int width, int height, int id)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;

        this.id = id;



        doors[0] = new Door(0, "up", width, height);
        doors[1] = new Door(0, "down", width, height);
        doors[2] = new Door(0, "left", width, height);
        doors[3] = new Door(0, "right", width, height);


    }
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawRectangle(new Rectangle(x * 32 + 40, y * 32 + 40, width * 35, height * 32), Color.Black);

        if (doors != null)
        {
            foreach (Door door in doors)
            {
                door.Draw(spriteBatch);
            }
        }

    }
}