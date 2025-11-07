using UnityEngine;

namespace Nidavellir.Rules
{
    public class NoopRuleHandler : IRuleHandler
    {
        public void Apply()
        {
            Debug.LogError("Noop Handler applied");
        }

        public void Revert()
        {
            Debug.LogError("Noop Handler reverted");
        }
    }
}