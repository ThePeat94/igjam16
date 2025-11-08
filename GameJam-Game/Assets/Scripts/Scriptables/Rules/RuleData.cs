using UnityEngine;

namespace Nidavellir.Scriptables.Rules
{
    public abstract class RuleData : ScriptableObject
    {
        [SerializeField] private string m_name;
        [SerializeField] private string m_description;
        [SerializeField] private Sprite m_icon;
        [SerializeField] private int m_unlockCost;
        
        public string Name => this.m_name;
        public string Description => this.m_description;
        public Sprite Icon => this.m_icon;
        public int UnlockCost => this.m_unlockCost;
    }
}