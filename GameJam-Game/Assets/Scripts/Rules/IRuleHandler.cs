namespace Nidavellir.Rules
{
    /**
     * Used to define a rule handler. The implementation should contain the actual controllers and other related
     * systems to apply and revert a rule.
     */
    public interface IRuleHandler
    {
        public void Apply();
        public void Revert();
    }
}