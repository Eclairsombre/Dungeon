using System.Drawing;
using Dungeon.src.AnimationClass;

namespace Dungeon.src.MapClass
{
    public enum RewardType
    {
        Weapon,
        Health,
        Gold,
        Xp
    }
    public class NextRoomRewardDisplay(int x, int y, int width, int height)
    {
        private Animation animation;
        private CallBack callBack = new();

        private Rectangle hitbox = new Rectangle(x, y, width, height);

        public Rectangle Hitbox { get { return hitbox; } set { hitbox = value; } }
    }
}