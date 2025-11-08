using System.Collections.Generic;

namespace Nidavellir.Rules
{
    public class EnemyShootingFrequencyRuleHandler : IRuleHandler
    {

        private readonly IReadOnlyList<EnemyShooter> m_enemyShooters;

        public EnemyShootingFrequencyRuleHandler(IReadOnlyList<EnemyShooter> enemyShooters)
        {
            this.m_enemyShooters = enemyShooters;
        }

        public void Apply()
        {
            foreach (var enemyShooter in this.m_enemyShooters)
            {
                enemyShooter.ShootingFrequency /= 2;
            }
        }

        public void Revert()
        {
            foreach (var enemyShooter in this.m_enemyShooters)
            {
                enemyShooter.ShootingFrequency *= 2;
            }
        }
    }
}