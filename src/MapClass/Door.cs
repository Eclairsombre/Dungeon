using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
namespace Dungeon.src.MapClass;

public class Door(int x, int y, int width, int height, RewardType rewardType)
{
    private Rectangle hitbox = new(x, y, width, height);

    private readonly RewardType rewardType = rewardType;
    public Rectangle Hitbox { get { return hitbox; } }
    public RewardType RewardType { get { return rewardType; } }
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.FillRectangle(hitbox, Color.Red);
    }
}