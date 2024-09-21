using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using MonoGame.Extended;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Dungeon.src.EnemyClass;
using Dungeon.src.DropClass;



namespace Dungeon.src.MapClass;


public struct TileType
{
    public int FirstValue;
    public int SecondValue;
    public int ThirdValue;

    public TileType(int firstValue, int secondValue, int thirdValue)
    {
        FirstValue = firstValue;
        SecondValue = secondValue;
        ThirdValue = thirdValue;
    }
}
public static class TileTypes
{
    public static readonly TileType Floor = new TileType(0, 0, 0);
    public static readonly TileType Wall = new TileType(1, 0, 0);
    public static readonly TileType Door = new TileType(2, 0, 0);

    public static readonly TileType Holder = new TileType(4, 0, 0);

    public static TileType GetTileType(T tilesType)
    {
        return tilesType switch
        {
            T.Floor => Floor,
            T.Wall => Wall,
            T.Door => Door,
            T.EnemyGoDown => new TileType(3, 2, 3),
            T.EnemyGoRight => new TileType(3, 2, 1),
            T.EnemyGoLeft => new TileType(3, 2, 2),
            T.EnemyGoUp => new TileType(3, 2, 4),
            T.Holder => Holder,
            _ => throw new ArgumentOutOfRangeException(nameof(tilesType), tilesType, null)
        };
    }
}

public enum T
{
    Floor,
    Wall,
    Door,

    EnemyGoDown,
    EnemyGoRight,
    EnemyGoLeft,
    EnemyGoUp,
    Holder
}

public class Room
{
    public int x, y, width, height, nbDoors;

    public Tiles[,] tiles = new Tiles[26, 14];

    public bool finished = false;

    Random random = new Random();

    string fileContent;

    public List<Enemy> enemies;

    public Drop[] dropsList;

    Texture2D sol;

    public Room()
    {
        this.x = 0;
        this.y = 0;
        this.width = 52;
        this.height = 30;

        this.dropsList = new Drop[0];

    }

    public void LoadContent(ContentManager content, int room)
    {
        // Load the file content using the ContentManager
        using (var stream = TitleContainer.OpenStream("Content/RoomPatern/Room" + room + ".txt"))
        using (var reader = new StreamReader(stream))
        {
            fileContent = reader.ReadToEnd();
        }

        sol = content.Load<Texture2D>("Sol");


    }

    public void Update(Vector2 playerPosition, GameTime gameTime)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].Update(playerPosition, this, gameTime);

            if (enemies[i].hp <= 0)
            {
                Drop[] newDropsList = new Drop[dropsList.Length + enemies[i].loot.Length];
                for (int j = 0; j < dropsList.Length; j++)
                {
                    newDropsList[j] = dropsList[j];
                }
                for (int j = 0; j < enemies[i].loot.Length; j++)
                {
                    enemies[i].loot[j].x = (int)enemies[i].Position.X;
                    enemies[i].loot[j].y = (int)enemies[i].Position.Y;
                    newDropsList[j + dropsList.Length] = enemies[i].loot[j];
                }
                dropsList = newDropsList;
                enemies = enemies.Where((e, index) => index != i).ToList();
                i--;
            }
        }

        if (enemies.Count == 0)
        {
            finished = true;
        }

    }


    public void Generate()
    {
        string[] lines = fileContent.Split('\n');

        this.enemies = new List<Enemy>();
        for (int i = 0; i < 14; i++)
        {
            string[] tileValues = lines[i].Split(' ');
            for (int j = 0; j < 26; j++)
            {
                T tileType = (T)int.Parse(tileValues[j]);
                TileType tileTypeValues = TileTypes.GetTileType(tileType);
                int firstValue = tileTypeValues.FirstValue;
                int secondValue = tileTypeValues.SecondValue;
                int thirdValue = tileTypeValues.ThirdValue;

                if (secondValue == 2)
                {
                    Enemy enemy = new Enemy();
                    enemy.Position = new Vector2(j * 70 + 40, i * 70 + 40);
                    if (thirdValue == 1)
                    {
                        enemy.Direction = new Vector2(1, 0);
                    }
                    else if (thirdValue == 4)
                    {
                        enemy.Direction = new Vector2(0, -1);
                    }
                    else if (thirdValue == 2)
                    {
                        enemy.Direction = new Vector2(-1, 0);
                    }
                    else if (thirdValue == 3)
                    {
                        enemy.Direction = new Vector2(0, 1);
                    }
                    this.enemies.Add(enemy);

                }
                Tuple<int, int> tile = new Tuple<int, int>(firstValue, secondValue);
                tiles[j, i] = new Tiles(tile, j * 70 + 40, i * 70 + 40, 70, 70);
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
                tiles[i, j].Draw(spriteBatch, finished, sol);
            }
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].Draw(spriteBatch);
        }

        for (int i = 0; i < dropsList.Length; i++)
        {
            dropsList[i].Draw(spriteBatch);
        }
    }
}