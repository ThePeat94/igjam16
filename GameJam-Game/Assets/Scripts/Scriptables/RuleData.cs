using UnityEngine;

namespace Nidavellir.Scriptables
{
    [CreateAssetMenu(fileName = "Rule", menuName = "Data/Rule", order = 0)]
    public class RuleData : ScriptableObject
    {
        [SerializeField] private string m_name;
        [SerializeField] private string m_description;
        [SerializeField] private Sprite m_icon;
        
        public string Name => m_name;
        public string Description => m_description;
        public Sprite Icon => m_icon;
    }
}