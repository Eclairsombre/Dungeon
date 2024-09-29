using Dungeon.src.MenuClass;
using Microsoft.Xna.Framework;


namespace Dungeon.src.PlayerClass.StatsClass
{
    public class Stats
    {
        private int _health;
        private int _maxHealth;
        private float _attack;
        private float _defense;
        private float _speed;
        private int _level;
        private int _xp;
        private int _xpToLevelUp;

        public int Health { get { return _health; } set { _health = value; } }
        public int MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }
        public float Attack { get { return _attack; } set { _attack = value; } }
        public float Defense { get { return _defense; } set { _defense = value; } }
        public float Speed { get { return _speed; } set { _speed = value; } }
        public int Level { get { return _level; } set { _level = value; } }
        public int Xp { get { return _xp; } set { _xp = value; } }
        public int XpToLevelUp { get { return _xpToLevelUp; } set { _xpToLevelUp = value; } }

        public Stats()
        {
            _health = 3;
            _maxHealth = 3;
            _attack = 1;
            _defense = 1;
            _speed = 1;
            _level = 1;
            _xp = 0;
            _xpToLevelUp = 100;
        }

        public void Update(GameTime gameTime, ref GameState gameState)
        {
            if (_xp >= _xpToLevelUp)
            {
                _level++;
                _xp -= _xpToLevelUp;
                _xpToLevelUp = (int)(_xpToLevelUp * 1.2f);

                gameState = GameState.LevelUp;
            }
        }
    }
}