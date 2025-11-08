using Unity.VisualScripting;
using UnityEngine;

namespace Nidavellir.Rules
{
    public class InvertControlsRuleHandler : IRuleHandler
    {
        private MovementOldInput movement;

        public InvertControlsRuleHandler(MovementOldInput movement)
        {
            this.movement = movement;
        }

        public void Apply()
        {
            movement.AddComponent<InvertControlsRule>();
        }

        public void Revert()
        {
            var invertControlsRule = movement.GetComponent<InvertControlsRule>();
            if (invertControlsRule != null)
            {
                Object.Destroy(invertControlsRule);
            }           
        }
    }
}