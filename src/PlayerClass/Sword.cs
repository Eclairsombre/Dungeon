using Microsoft.Xna.Framework;

namespace Dungeon.src.PlayerClass
{
    public class Sword : Weapon
    {
        public Sword(Vector2 position) : base(position)
        {
            SetRange(50);
            SetDamage(2);
        }
    }
}