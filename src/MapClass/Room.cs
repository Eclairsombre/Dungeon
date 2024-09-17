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
using Dungeon.src.EnemyClass;



namespace Dungeon.src.MapClass;

public class Room
{
    public int x, y, width, height, nbDoors;

    public Tiles[,] tiles = new Tiles[26, 14];

    public bool finished = false;

    Random random = new Random();

    string fileContent;

    public Enemy[] enemies;

    public Room()
    {
        this.x = 0;
        this.y = 0;
        this.width = 52;
        this.height = 30;

        enemies = new Enemy[2];
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i] = new Enemy();
        }
        enemies[1].Position = new Vector2(600, 200);
    }

    public void LoadContent(ContentManager content, int room)
    {
        // Load the file content using the ContentManager
        using (var stream = TitleContainer.OpenStream("Content/RoomPatern/Room" + room + ".txt"))
        using (var reader = new StreamReader(stream))
        {
            fileContent = reader.ReadToEnd();
        }
    }

    public void Update(Vector2 playerPosition, GameTime gameTime)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].Update(playerPosition, this, gameTime);

            if (enemies[i].hp <= 0)
            {
                enemies = enemies.Where((e, index) => index != i).ToArray();
                i--;
            }
        }

        if (enemies.Length == 0)
        {
            finished = true;
        }

    }


    public void Generate()
    {
        // Lire les données des tiles à partir du fichier texte
        string[] lines = fileContent.Split('\n');

        for (int i = 0; i < 14; i++)
        {
            string[] tileValues = lines[i].Split(',');
            for (int j = 0; j < 26; j++)
            {
                int tileType = int.Parse(tileValues[j]);
                tiles[j, i] = new Tiles(tileType, j * 70 + 40, i * 70 + 40, 70, 70);
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

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].Draw(spriteBatch);
        }
    }
}