using Unity.VisualScripting;
using UnityEngine;

namespace Nidavellir.Rules.NoWallGrab
{
    public class NoWallGrabRuleHandler : IRuleHandler
    {
        private readonly MovementController m_movementController;

        public NoWallGrabRuleHandler(MovementController movementController)
        {
            this.m_movementController = movementController;
        }

        public void Apply()
        {
            var noWallGrabRule = this.m_movementController.AddComponent<NoWallGrabRule>();
        }

        public void Revert()
        {
            var noWallGrabRule = this.m_movementController.GetComponent<NoWallGrabRule>();
            if (noWallGrabRule != null)
            {
                Object.Destroy(noWallGrabRule);
            }
        }
    }
}
