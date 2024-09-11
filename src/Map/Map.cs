using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
namespace Dungeon;


public class Map
{


    private Random random = new Random();

    public Room[] rooms;

    public int currentRoom = 0;

    public int screenWidth;
    public int screenHeight;

    public Map()
    {
        screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

    }

    public void GenerateDungeon()
    {
        int roomCount = 2;
        rooms = new Room[roomCount];
        for (int i = 0; i < roomCount; i++)
        {
            rooms[i] = new Room(i, 4);
        }


    }






    public void Draw(SpriteBatch spriteBatch)
    {

        Console.WriteLine("Drawing room " + currentRoom);

        rooms[currentRoom].Draw(spriteBatch);

    }


}