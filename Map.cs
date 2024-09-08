using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
namespace Dungeon;


public class Map
{

    public int[,] map = new int[100, 100];
    private Random random = new Random();

    public Room[] rooms;

    public Map()
    {
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                map[i, j] = 0;
            }
        }
    }

    public void GenerateDungeon()
    {
        int roomCount = 1;
        rooms = new Room[roomCount];
        for (int i = 0; i < roomCount; i++)
        {
            rooms[i] = new Room(0, 0, 52, 30);
        }

    }





    public void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].Draw(spriteBatch);
        }
    }

    public void DisplayMapInConsole()
    {
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                if (map[i, j] == 1)
                {
                    Console.Write("R"); // R pour Room (pièce)
                }
                else if (map[i, j] == 2)
                {
                    Console.Write("D"); // D pour Door (porte)
                }
                else
                {
                    Console.Write("."); // . pour un espace vide
                }
            }
            Console.WriteLine();
        }
    }
}