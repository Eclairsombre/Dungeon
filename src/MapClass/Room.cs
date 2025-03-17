using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Dungeon.src.EnemyClass;
using Dungeon.src.DropClass;
using Newtonsoft.Json;

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
    private readonly RewardType rewardType = rewardType;
    private bool finished = false;
    private Floor floor;
    private List<Wall> walls = [];
    private List<Enemy> enemies;
    public List<Enemy> Enemies { get { return enemies; } set { enemies = value; } }
    public bool Finished { get { return finished; } set { finished = value; } }

    public void LoadContent(ContentManager content, int room)
    {
        string filePath = $"Content/RoomPatern/Room{room}.json";
        RoomData roomData;

        using (var stream = TitleContainer.OpenStream(filePath))
        using (var reader = new StreamReader(stream))
        {
            string fileContent = reader.ReadToEnd();
            roomData = JsonConvert.DeserializeObject<RoomData>(fileContent);
        }

        Console.WriteLine(JsonConvert.SerializeObject(roomData, Formatting.Indented));
        Generate(content, roomData);

    }
    public void Update(Vector2 playerPosition, GameTime gameTime, ContentManager content)
    {
    }




    public void Generate(ContentManager content, RoomData roomData)
    {
        floor = new Floor(160, 120, 1600, 800, roomData.FloorSprite);
        floor.LoadContent(content);
        foreach (var wall in roomData.Walls.Values)
        {
            walls.Add(new Wall(wall.X, wall.Y, wall.Width, wall.Height));
        }
    }



    public void Draw(SpriteBatch spriteBatch)
    {
        floor.Draw(spriteBatch);
        foreach (var wall in walls)
        {
            wall.Draw(spriteBatch);
        }
    }
}