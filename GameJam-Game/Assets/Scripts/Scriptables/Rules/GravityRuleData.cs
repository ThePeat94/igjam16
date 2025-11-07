using UnityEngine;

namespace Nidavellir.Scriptables.Rules
{
    [CreateAssetMenu(menuName = "Data/Rule/Gravity Data", fileName = "Gravity Rule", order = 0)]
    public class GravityRuleData : RuleData
    {
        [SerializeField] private GravityMode m_gravityMode = GravityMode.Normal;
        
        public GravityMode GravityStrength => this.m_gravityMode;

        public enum GravityMode
        {
            None,
            Low,
            Normal,
            High,
        }
    }
}