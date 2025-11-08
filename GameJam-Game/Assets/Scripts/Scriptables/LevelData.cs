using System.Collections.Generic;
using Nidavellir.Scriptables.Rules;
using UnityEngine;

namespace Nidavellir.Scriptables
{
    [CreateAssetMenu(fileName = "Level Data", menuName = "Data/Level", order = 0)]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private int m_minimumRules;
        [SerializeField] private int m_maximumRules;
        [SerializeField] private List<RuleData> m_availableFreeRules;
        [SerializeField] private List<RuleData> m_availableLockedRules;
        [SerializeField] private float m_levelDurationInSeconds;
        
        
        public int MinimumRules => this.m_minimumRules;
        public int MaximumRules => this.m_maximumRules;
        public IReadOnlyList<RuleData> AvailableFreeRules => this.m_availableFreeRules;
        public IReadOnlyList<RuleData> AvailableLockedRules => this.m_availableLockedRules;
        public float LevelDurationInSeconds => m_levelDurationInSeconds;
    }
}