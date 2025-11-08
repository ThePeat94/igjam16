using Unity.VisualScripting;
using UnityEngine;

namespace Nidavellir.Rules
{
    public class GravityRuleHandler : IRuleHandler
    {
        private readonly PlayerController m_playerController;
        
        public GravityRuleHandler(PlayerController playerController)
        {
            this.m_playerController = playerController;
        }

        public void Apply()
        {
            var gravityRule = this.m_playerController.AddComponent<GravityRule>();
            gravityRule.GravityScale = 2f;
        }

        public void Revert()
        {
            var gravityRule = this.m_playerController.GetComponent<GravityRule>();
            if (gravityRule != null)
            {
                Object.Destroy(gravityRule);
            }
        }
    }
}