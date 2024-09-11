using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
using System.Linq;
using System.Collections.Generic;


namespace Dungeon;

public class Room
{
    public int x, y, width, height, nbDoors;

    public int[,] tiles = new int[26, 14];

    public int id;

    public Door[] doors = new Door[4];
    public Room(int id, int nbDoors)
    {
        this.x = 0;
        this.y = 0;
        this.width = 52;
        this.height = 30;

        this.nbDoors = nbDoors;

        this.id = id;

        for (int i = 0; i < 26; i++)
        {
            for (int j = 0; j < 14; j++)
            {
                tiles[i, j] = 0;
            }
        }


        AddDoors(nbDoors);





    }

    private void AddDoors(int nbDoors)
    {

        string[] directions = { "up", "down", "left", "right" };
        Random random = new Random();
        for (int i = 0; i < nbDoors; i++)
        {
            int randomIndex = random.Next(directions.Length);
            string randomDirection = directions[randomIndex];

            doors[i] = new Door(1, randomDirection, width, height);

            directions = directions.Where(val => val != randomDirection).ToArray();
        }
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawRectangle(new Rectangle(x * 32 + 40, y * 32 + 40, width * 35, height * 32 + 20), Color.Black);

        for (int i = 0; i < 26; i++)
        {
            for (int j = 0; j < 14; j++)
            {
                if (tiles[i, j] == 0)
                {
                    spriteBatch.DrawRectangle(new Rectangle(x * 70 + 40 + i * 70, y * 70 + 40 + j * 70, 70, 70), Color.White);
                }
            }
        }

        if (doors != null)
        {
            foreach (Door door in doors)
            {
                door.Draw(spriteBatch);
            }
        }

    }
}