using Unity.VisualScripting;
using UnityEngine;

namespace Nidavellir.Rules.InvertControls
{
    public class InvertControlsRuleHandler : IRuleHandler
    {
        private MovementController movement;

        public InvertControlsRuleHandler(MovementController movement)
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