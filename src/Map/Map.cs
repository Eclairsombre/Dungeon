using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
using System.Collections.Generic; // Add this line to import the List<> class

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
        List<Color> colors = new List<Color>
        {
            Color.Red,
            Color.Blue,
            Color.Green,
            Color.Yellow,
            Color.Purple,
            Color.Orange,
            Color.Pink,
            Color.Brown,
            Color.White,
            Color.Black
        };
        for (int i = 0; i < roomCount; i++)
        {
            rooms[i] = new Room(0, 0, 52, 30, i, colors[random.Next(0, colors.Count)]);
        }

    }





    public void Draw(SpriteBatch spriteBatch)
    {

        Console.WriteLine("Drawing room " + currentRoom);

        rooms[currentRoom].Draw(spriteBatch);

    }


}