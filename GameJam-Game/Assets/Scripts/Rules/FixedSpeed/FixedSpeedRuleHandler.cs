using Unity.VisualScripting;
using UnityEngine;

namespace Nidavellir.Rules.FixedSpeed
{
    public class FixedSpeedRuleHandler : IRuleHandler
    {
        private readonly MovementOldInput m_movementOldInput;
        
        public FixedSpeedRuleHandler(MovementOldInput movementOldInput)
        {
            m_movementOldInput = movementOldInput; 
        }
        
        public void Apply()
        {
            m_movementOldInput.AddComponent<FixedSpeedRule>();
        }

        public void Revert()
        {
            var fixedSpeedRule = m_movementOldInput.GetComponent<FixedSpeedRule>();
            if (fixedSpeedRule != null)
            {
                Object.Destroy(fixedSpeedRule);
            }
        }
    }
}