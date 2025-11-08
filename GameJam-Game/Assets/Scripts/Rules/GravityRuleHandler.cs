using Nidavellir.Scriptables.Rules;
using Unity.VisualScripting;
using UnityEngine;

namespace Nidavellir.Rules
{
    public class GravityRuleHandler : IRuleHandler
    {
        private readonly MovementOldInput m_movementOldInput;
        private readonly GravityRuleData m_gravityRuleData;

        public GravityRuleHandler(MovementOldInput movementOldInput, GravityRuleData gravityRuleData)
        {
            this.m_movementOldInput = movementOldInput;
            this.m_gravityRuleData = gravityRuleData;
        }

        public void Apply()
        {
            var gravityRule = m_movementOldInput.AddComponent<GravityRule>();

            float gravityMultiplier = m_gravityRuleData.GravityStrength switch
            {
                GravityRuleData.GravityMode.None => m_gravityRuleData.NoneGravityScale,
                GravityRuleData.GravityMode.Low => m_gravityRuleData.LowGravityScale,
                GravityRuleData.GravityMode.Normal => m_gravityRuleData.NormalGravityScale,
                GravityRuleData.GravityMode.High => m_gravityRuleData.HighGravityScale,
                _ => m_gravityRuleData.NormalGravityScale
            };

            gravityRule.GravityMultiplier = gravityMultiplier;
        }

        public void Revert()
        {
            var gravityRule = this.m_movementOldInput.GetComponent<GravityRule>();
            if (gravityRule != null)
            {
                Object.Destroy(gravityRule);
            }
        }
    }
}