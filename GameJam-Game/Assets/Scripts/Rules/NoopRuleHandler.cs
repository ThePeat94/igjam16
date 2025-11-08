using Nidavellir.Scriptables.Rules;
using Unity.VisualScripting;
using UnityEngine;

namespace Nidavellir.Rules
{
    public class NoopRuleHandler : IRuleHandler
    {
        private readonly RuleData m_ruleData;
        
        public NoopRuleHandler(RuleData ruleData)
        {
            this.m_ruleData = ruleData;
        }
        
        public void Apply()
        {
            Debug.LogError($"Noop Handler applied for rule {this.m_ruleData.GetType().Name} ({this.m_ruleData.Name})");
        }

        public void Revert()
        {
            Debug.LogError($"Noop Handler reverted for rule {this.m_ruleData.GetType().Name} ({this.m_ruleData.Name})");
        }
    }
}