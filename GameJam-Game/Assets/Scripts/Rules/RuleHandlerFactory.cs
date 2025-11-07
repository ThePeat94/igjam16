using UnityEngine;

namespace Nidavellir.Rules
{
    public class RuleHandlerFactory : MonoBehaviour
    {
        public IRuleHandler CreateRuleHandler(object ruleData)
        {
            return new NoopRuleHandler();
        }
    }
}