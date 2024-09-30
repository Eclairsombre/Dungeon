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
    public class NextRoomRewardDisplay
    {
        private Animation animation;
        private readonly CallBack callBack = new();
        private Rectangle hitbox;
        public Rectangle Hitbox { get { return hitbox; } set { hitbox = value; } }

        private RewardType rewardType;


        public NextRoomRewardDisplay(int x, int y, int width, int height, RewardType rewardType, string file)
        {
            animation = new(file, callBack.StaticMyCallback, 0, 0);
            hitbox = new(x, y, width, height);

            this.rewardType = rewardType;
        }

    }
}