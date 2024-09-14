using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
using System.Linq;
using System.Collections.Generic;


namespace Dungeon.src.MapClass;

public class Room
{
    public int x, y, width, height, nbDoors;

    public Tiles[,] tiles = new Tiles[26, 14];

    public bool finished = false;
    public Room()
    {
        this.x = 0;
        this.y = 0;
        this.width = 52;
        this.height = 30;


        for (int i = 0; i < 26; i++)
        {
            for (int j = 0; j < 14; j++)
            {
                if (j == 0 && (i == 11 || i == 13))
                {
                    tiles[i, j] = new Tiles(2, i * 70 + 40, j * 70 + 40, 70, 70);
                }
                else if ((j == 0 || j == 13) || (i == 0 || i == 25))
                {
                    tiles[i, j] = new Tiles(1, i * 70 + 40, j * 70 + 40, 70, 70);
                }
                else
                {
                    tiles[i, j] = new Tiles(0, i * 70 + 40, j * 70 + 40, 70, 70);
                }
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