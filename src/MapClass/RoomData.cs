using System.Collections.Generic;

namespace Dungeon.src.MapClass
{
    public class RoomData
    {
        public Dictionary<string, Wall> Walls { get; set; }
        public string FloorSprite { get; set; }
    }
}