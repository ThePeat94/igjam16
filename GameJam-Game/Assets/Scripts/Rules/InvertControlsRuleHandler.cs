using Unity.VisualScripting;

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
            throw new System.NotImplementedException();
        }
    }
}