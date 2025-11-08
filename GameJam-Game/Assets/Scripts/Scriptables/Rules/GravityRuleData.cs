using UnityEngine;

namespace Nidavellir.Scriptables.Rules
{
    [CreateAssetMenu(menuName = "Data/Rule/Gravity Data", fileName = "Gravity Rule", order = 0)]
    public class GravityRuleData : RuleData
    {
        [SerializeField] private GravityMode m_gravityMode = GravityMode.Normal;
        
        [Header("Gravity Scale Factors")]
        [SerializeField] private float m_noneGravityScale = 0f;
        [SerializeField] private float m_lowGravityScale = 0.5f;
        [SerializeField] private float m_normalGravityScale = 1f;
        [SerializeField] private float m_highGravityScale = 2f;
        
        public GravityMode GravityStrength => this.m_gravityMode;
        
        public float NoneGravityScale => this.m_noneGravityScale;
        public float LowGravityScale => this.m_lowGravityScale;
        public float NormalGravityScale => this.m_normalGravityScale;
        public float HighGravityScale => this.m_highGravityScale;

        public enum GravityMode
        {
            None,
            Low,
            Normal,
            High,
        }
    }
}