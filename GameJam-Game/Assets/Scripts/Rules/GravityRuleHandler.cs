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
            var gravityRule = this.m_movementOldInput.AddComponent<GravityRule>();

            float gravityScale = this.m_gravityRuleData.GravityStrength switch
            {
                GravityRuleData.GravityMode.None => 0f,
                GravityRuleData.GravityMode.Low => 0.5f,
                GravityRuleData.GravityMode.Normal => 1f,
                GravityRuleData.GravityMode.High => 2f,
                _ => 1f
            };

            gravityRule.GravityScale = gravityScale;
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