using Unity.VisualScripting;
using UnityEngine;

namespace Nidavellir.Rules.FixedSpeed
{
    public class FixedSpeedRuleHandler : IRuleHandler
    {
        private readonly MovementController m_movementController;
        
        public FixedSpeedRuleHandler(MovementController movementController)
        {
            m_movementController = movementController; 
        }
        
        public void Apply()
        {
            m_movementController.AddComponent<FixedSpeedRule>();
        }

        public void Revert()
        {
            var fixedSpeedRule = m_movementController.GetComponent<FixedSpeedRule>();
            if (fixedSpeedRule != null)
            {
                Object.Destroy(fixedSpeedRule);
            }
        }
    }
}