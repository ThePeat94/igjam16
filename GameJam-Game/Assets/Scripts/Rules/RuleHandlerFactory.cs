using Nidavellir.Scriptables;
using Nidavellir.Scriptables.Rules;
using UnityEngine;

namespace Nidavellir.Rules
{
    public class RuleHandlerFactory : MonoBehaviour
    {
        [SerializeField] private PlayerController m_playerController;
        
        public IRuleHandler CreateRuleHandler(RuleData ruleData)
        {
            return ruleData switch
            {
                GravityRuleData => new GravityRuleHandler(this.m_playerController),
                _ => new NoopRuleHandler()
            };
        }
    }
}