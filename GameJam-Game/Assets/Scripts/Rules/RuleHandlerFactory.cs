using Nidavellir.Scriptables;
using Nidavellir.Scriptables.Rules;
using UnityEngine;

namespace Nidavellir.Rules
{
    public class RuleHandlerFactory : MonoBehaviour
    {
        public IRuleHandler CreateRuleHandler(RuleData ruleData)
        {
            return new NoopRuleHandler();
        }
    }
}