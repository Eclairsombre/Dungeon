using System.Collections.Generic;
using Dungeon.src.EnemyClass;
using Microsoft.Xna.Framework;

namespace Dungeon.src.PlayerClass.WeaponClass
{
    public class Sword : Weapon
    {
        public Sword(Vector2 position) : base(position)
        {
            SetRange(50);
            SetDamage(2);
        }

        public void Attack(Vector2 direction, List<Enemy> enemies)
        {
            if (timeSinceLastAttack >= attackCooldown)
            {
                timeSinceLastAttack = 0f;
                foreach (var enemy in enemies)
                {
                    if (enemy.Hitbox.Intersects(new Rectangle((int)position.X, (int)position.Y, width, height)))
                    {
                        enemy.Hp -= Damage;
                    }
                }
            }
        }
    }
}