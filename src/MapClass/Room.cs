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


public struct TileType(int firstValue, int secondValue, int thirdValue)
{
    public int FirstValue = firstValue;
    public int SecondValue = secondValue;
    public int ThirdValue = thirdValue;
}
public static class TileTypes
{
    public static readonly TileType Floor = new(0, 0, 0);
    public static readonly TileType Wall = new(1, 0, 0);
    public static readonly TileType Door = new(2, 0, 0);

    public static readonly TileType Holder = new(4, 0, 0);

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
            T.NextRoomRewardDisplay => new TileType(5, 0, 0),
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
    Holder,
    NextRoomRewardDisplay
}

public class Room(RewardType rewardType)
{
    private readonly int x = 0, y = 0, width = 52, height = 30;
    private Tiles[,] tiles = new Tiles[26, 14];

    private RewardType rewardType = rewardType;
    private bool finished = false;
    private string fileContent;
    private List<Enemy> enemies;
    private Drop[] dropsList = [];
    private Texture2D[] texture2Ds;
    public Tiles[,] Tiles { get { return tiles; } set { tiles = value; } }
    public List<Enemy> Enemies { get { return enemies; } set { enemies = value; } }
    public Drop[] DropsList { get { return dropsList; } set { dropsList = value; } }
    public bool Finished { get { return finished; } set { finished = value; } }

    public void LoadContent(ContentManager content, int room)
    {
        using (var stream = TitleContainer.OpenStream("Content/RoomPatern/Room" + room + ".txt"))
        using (var reader = new StreamReader(stream))
        {
            fileContent = reader.ReadToEnd();
        }
        texture2Ds = new Texture2D[2];
        texture2Ds[0] = content.Load<Texture2D>("Sprites/Sol");
        texture2Ds[1] = content.Load<Texture2D>("Sprites/Mur");

        Generate(content);



    }
    public void Update(Vector2 playerPosition, GameTime gameTime, ContentManager content)
    {

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].Update(playerPosition, this, gameTime);

            if (enemies[i].Hp <= 0)
            {
                Drop[] newDropsList = new Drop[dropsList.Length + enemies[i].Loot.Length];
                for (int j = 0; j < dropsList.Length; j++)
                {
                    newDropsList[j] = dropsList[j];
                }
                for (int j = 0; j < enemies[i].Loot.Length; j++)
                {
                    Rectangle hitbox = new();
                    if (enemies[i].Loot[j] is XpDrop)
                    {
                        hitbox = new((int)enemies[i].Position.X, (int)enemies[i].Position.Y, enemies[i].Loot[j].Height, enemies[i].Loot[j].Width);
                    }
                    else if (enemies[i].Loot[j] is HeartDrop)
                    {
                        hitbox = new((int)enemies[i].Position.X + 25, (int)enemies[i].Position.Y, enemies[i].Loot[j].Height, enemies[i].Loot[j].Width);
                    }
                    enemies[i].Loot[j].Hitbox = hitbox;

                    newDropsList[j + dropsList.Length] = enemies[i].Loot[j];
                }
                dropsList = newDropsList;
                enemies = enemies.Where((e, index) => index != i).ToList();
                i--;
            }
            if (enemies.Count == 0)
            {
                finished = true;
                if (tiles[9, 0].NextRoomRewardDisplay == null)
                {

                    if (tiles[9, 0] != null && tiles[10, 0] != null && tiles[10, 0].Door != null)
                    {
                        tiles[9, 0].NextRoomRewardDisplay = new NextRoomRewardDisplay(
                            tiles[9, 0].Hitbox.X,
                            tiles[9, 0].Hitbox.Y,
                            tiles[9, 0].Hitbox.Width,
                            tiles[9, 0].Hitbox.Height,
                            tiles[10, 0].Door.RewardType,
                            "NextRoomDisplay" + tiles[10, 0].Door.RewardType.ToString()

                        );
                    }
                    tiles[9, 0].NextRoomRewardDisplay.LoadContent(content);
                }
                if (tiles[15, 0].NextRoomRewardDisplay == null)
                {

                    if (tiles[15, 0] != null && tiles[14, 0] != null && tiles[14, 0].Door != null)
                    {
                        tiles[15, 0].NextRoomRewardDisplay = new NextRoomRewardDisplay(
                            tiles[15, 0].Hitbox.X,
                            tiles[15, 0].Hitbox.Y,
                            tiles[15, 0].Hitbox.Width,
                            tiles[15, 0].Hitbox.Height,
                            tiles[14, 0].Door.RewardType,
                            "NextRoomDisplay" + tiles[14, 0].Door.RewardType.ToString()
                        );
                    }
                    tiles[15, 0].NextRoomRewardDisplay.LoadContent(content);
                }
            }





        }
        for (int k = 0; k < 26; k++)
        {
            for (int j = 0; j < 14; j++)
            {
                tiles[k, j].Update(gameTime);
            }
        }
        for (int j = 0; j < dropsList.Length; j++)
        {
            dropsList[j].Update(gameTime);
        }
    }


    public void Generate(ContentManager content)
    {
        string[] lines = fileContent.Split('\n');

        enemies = [];
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
                    Enemy enemy = new()
                    {
                        Position = new Vector2(j * 70 + 40, i * 70 + 40)
                    };
                    enemy.LoadContent(content);

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
                    enemies.Add(enemy);

                }

                Tuple<int, int> tile = new(firstValue, secondValue);
                tiles[j, i] = new Tiles(tile, j * 70 + 40, i * 70 + 40, 70, 70, rewardType, content);

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
                tiles[i, j].Draw(spriteBatch, finished, texture2Ds);
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