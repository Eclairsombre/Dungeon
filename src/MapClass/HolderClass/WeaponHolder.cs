using Dungeon.src.PlayerClass.WeaponClass;

namespace Dungeon.src.MapClass.HolderClass
{
    public class WeaponHolder(int x, int y, Weapon weapon) : Holder(x, y)
    {
        private Weapon weapon = weapon;

        public Weapon WeaponHold { get { return weapon; } set { weapon = value; } }
    }

}