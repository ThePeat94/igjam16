using System.Collections.Generic;
using System.Linq;
using Nidavellir.Scriptables;
using Nidavellir.Scriptables.Rules;
using UnityEngine;

namespace Nidavellir.Rules
{
    public class RuleHandlerFactory : MonoBehaviour
    {
        [SerializeField] private MovementOldInput m_movementOldInput;
        [SerializeField] private HealthController m_playerHealthController;
        
        private List<EnemyShooter> m_enemyShooters;
        
        private void Awake()
        {
            this.m_enemyShooters = FindObjectsByType<EnemyShooter>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        }
        
        public IRuleHandler CreateRuleHandler(RuleData ruleData)
        {
            return ruleData switch
            {
                GravityRuleData gravityRuleData => new GravityRuleHandler(this.m_movementOldInput, gravityRuleData),
                InvertDamageRuleData => new InvertDamageRuleHandler(this.m_playerHealthController),
                EnemyShootingFrequencyRuleData => new EnemyShootingFrequencyRuleHandler(this.m_enemyShooters),
                NoJumpRuleData => new NoJumpRuleHandler(this.m_movementOldInput),
                InvertControlsRuleData => new InvertControlsRuleHandler(m_movementOldInput),
                _ => new NoopRuleHandler(ruleData),
            };
        }
    }
}