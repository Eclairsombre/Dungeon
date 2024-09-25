using System;
using Dungeon.src.PlayerClass.WeaponClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Dungeon.src.MapClass.HolderClass
{
    public class WeaponHolder(int x, int y, Weapon weapon) : Holder(x, y)
    {
        private Weapon weapon = weapon;

        public Weapon WeaponHold { get { return weapon; } set { weapon = value; } }

        public new void Draw(SpriteBatch spriteBatch)
        {
            Rectangle weaponRectangle = new((int)hitbox.X + (hitbox.Width - weapon.Width) / 2, (int)hitbox.Y - weapon.Height - 10, weapon.Width, weapon.Height);
            spriteBatch.FillRectangle(hitbox, Color.White);
            spriteBatch.FillRectangle(weaponRectangle, Color.Red);

        }

        public Weapon SwitchWeapon(Weapon weapon)
        {
            Weapon W = this.weapon;
            this.weapon = weapon;
            this.weapon.Height = 40;
            this.weapon.Width = 40;
            return W;
        }
    }

}