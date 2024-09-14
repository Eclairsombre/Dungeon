using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.IO;
using Microsoft.Xna.Framework.Content;


namespace Dungeon.src.MapClass;

public class Room
{
    public int x, y, width, height, nbDoors;

    public Tiles[,] tiles = new Tiles[26, 14];

    public bool finished = false;

    Random random = new Random();

    string fileContent;

    public Room()
    {
        this.x = 0;
        this.y = 0;
        this.width = 52;
        this.height = 30;



        string[] lines = System.IO.File.ReadAllLines("Room1.txt");

        for (int i = 0; i < 26; i++)
        {
            string[] tileValues = lines[i].Split(' ');
            for (int j = 0; j < 14; j++)
            {
                int tileType = int.Parse(tileValues[j]);
                tiles[i, j] = new Tiles(tileType, i * 70 + 40, j * 70 + 40, 70, 70);
            }
        }
    }

    public void LoadContent(ContentManager content)
    {
        this.fileContent = content.Load<string>("Room1");

        //TODO Fix load error
    }

    public void Generate()
    {
        string[] lines = fileContent.Split('\n');

        for (int i = 0; i < 26; i++)
        {
            string[] tileValues = lines[i].Split('\t');
            for (int j = 0; j < 14; j++)
            {
                int tileType = int.Parse(tileValues[j]);
                tiles[i, j] = new Tiles(tileType, i * 70 + 40, j * 70 + 40, 70, 70);
            }
        }
    }


    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawRectangle(new Rectangle(x * 32 + 40, y * 32 + 40, width * 35, height * 32 + 20), Color.Black);

        for (int i = 0; i < 26; i++)
        {
            for (int j = 0; j < 14; j++)
            {
                tiles[i, j].Draw(spriteBatch, finished);
            }
        }
    }
}