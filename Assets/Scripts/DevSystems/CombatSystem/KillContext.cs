using Enemies;

namespace DevSystems.CombatSystem
{
    public class KillContext
    {
        public int attackPower = 0;
        public EnemyType? enemyType = null;

        public KillContext(int attackPower, EnemyType? enemyType = null)
        {
            this.attackPower = attackPower;
            this.enemyType = enemyType;
        }
    }
}