namespace Gameplay.Combat.Enemies
{
    public class EnemyUtil
    {
        public static int CompareEnemiesByHealth(EnemyController a, EnemyController b)
        {
            if (a.health > b.health)
            {
                return -1;
            }
            else if (a.health < b.health)
            {
                return 1;
            }
            return 0;
        }
    }
}
