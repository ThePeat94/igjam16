using UnityEngine;

namespace Nidavellir.Scriptables.Rules
{
    public abstract class RuleData : ScriptableObject
    {
        [SerializeField] private string m_name;
        [SerializeField] private string m_description;
        [SerializeField] private Sprite m_icon;
        [SerializeField] private int m_unlockCost;
        [SerializeField] private bool m_invertedRule;
        
        public string Name => this.m_name;
        public string Description => this.m_description;
        public Sprite Icon => this.m_icon;
        public int UnlockCost => this.m_unlockCost;
        public bool InvertedRule => this.m_invertedRule;
    }
}