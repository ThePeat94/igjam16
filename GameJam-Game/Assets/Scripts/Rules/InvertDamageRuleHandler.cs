namespace Nidavellir.Rules
{
    public class InvertDamageRuleHandler : IRuleHandler
    {
        private readonly HealthController m_playerHealthController;
        
        public InvertDamageRuleHandler(HealthController playerHealthController)
        {
            this.m_playerHealthController = playerHealthController;
        }
        
        public void Apply()
        {
            this.m_playerHealthController.DamageMode = DamageMode.Heal;
        }

        public void Revert()
        {
            this.m_playerHealthController.DamageMode = DamageMode.Damage;
        }
    }
}