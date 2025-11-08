using Nidavellir.Scriptables.Rules;
using Unity.VisualScripting;
using UnityEngine;

namespace Nidavellir.Rules.Gravity
{
    public class GravityRuleHandler : IRuleHandler
    {
        private readonly MovementController m_movementController;
        private readonly GravityRuleData m_gravityRuleData;

        public GravityRuleHandler(MovementController movementController, GravityRuleData gravityRuleData)
        {
            this.m_movementController = movementController;
            this.m_gravityRuleData = gravityRuleData;
        }

        public void Apply()
        {
            var gravityRule = m_movementController.AddComponent<GravityRule>();

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
            var gravityRule = this.m_movementController.GetComponent<GravityRule>();
            if (gravityRule != null)
            {
                Object.Destroy(gravityRule);
            }
        }
    }
}