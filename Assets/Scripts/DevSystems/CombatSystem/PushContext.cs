using Enemies;

namespace DevSystems.CombatSystem
{
    public class PushContext
    {
        public EnemyType? enemyType = null;

        public PushContext(EnemyType? enemyType = null)
        {
            this.enemyType = enemyType;
        }
    }
}