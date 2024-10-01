using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
using Microsoft.Xna.Framework.Content;

namespace Dungeon.src.MapClass;


public class Map
{
    private readonly Random random = new();
    private Room room;
    public Room ActualRoom { get { return room; } set { room = value; } }
    public Map()
    {
    }

    public void GenerateDungeon(ContentManager content)
    {
        room = new Room(RewardType.None);
        room.LoadContent(content, 1);
    }

    public void Update(Vector2 playerPosition, GameTime gameTime, ContentManager content)
    {
        room.Update(playerPosition, gameTime, content);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        room.Draw(spriteBatch);
    }


}