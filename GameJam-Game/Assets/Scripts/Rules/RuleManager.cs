using System;
using System.Collections.Generic;
using System.Linq;
using Nidavellir.Scriptables;
using Nidavellir.Scriptables.Rules;
using Nidavellir.UI.Rules;
using UnityEngine;

namespace Nidavellir.Rules
{
    public class RuleManager : MonoBehaviour
    {
        [SerializeField] private AvailableRulesUI m_availableRulesUI;
        [SerializeField] private RuleHandlerFactory m_ruleHandlerFactory;
        [SerializeField] private Purse m_purse;
        [SerializeField] private RuleShop m_ruleShop;
        
        private IReadOnlyList<RuleData> m_availableRules;
        private readonly List<RuleData> m_activeRules = new();
        private LevelData m_levelData;

        private void Awake()
        {
            this.m_levelData = FindFirstObjectByType<GameManager>()
                .LevelData;
            this.m_ruleShop ??= FindFirstObjectByType<RuleShop>(FindObjectsInactive.Include);
            this.m_availableRulesUI ??= FindFirstObjectByType<AvailableRulesUI>(FindObjectsInactive.Include);
            this.m_ruleHandlerFactory ??= FindFirstObjectByType<RuleHandlerFactory>(FindObjectsInactive.Include);
            this.m_availableRulesUI.OnRuleClicked += this.HandleRuleToggle;
            this.m_availableRules = PlayerInventory.Instance.PurchasedRules;
        }

        private void Start()
        {
            this.m_availableRulesUI.DisplayAvailableRules(this.m_availableRules);
            this.m_availableRulesUI.DisplayStartLevelState(this.m_levelData.MinimumRules <= 0);
        }

        private void HandleRuleToggle(RuleData ruleData)
        {
            if (this.m_activeRules.Contains(ruleData))
            {
                this.m_activeRules.Remove(ruleData);
                this.m_ruleHandlerFactory.CreateRuleHandler(ruleData).Revert();
            }
            else if (this.m_activeRules.Count < this.m_levelData.MaximumRules)
            {
                this.m_activeRules.Add(ruleData);
                this.m_ruleHandlerFactory.CreateRuleHandler(ruleData).Apply();
            }

            this.m_availableRulesUI.DisplayRuleState(ruleData, this.m_activeRules.Contains(ruleData));
            this.m_availableRulesUI.DisplayStartLevelState(this.m_activeRules.Count >= this.m_levelData.MinimumRules);
        }
        
        /// <summary>
        /// Checks if a specific rule is currently active.
        /// </summary>
        public bool IsRuleActive(RuleData ruleData)
        {
            return this.m_activeRules.Contains(ruleData);
        }
        
        /// <summary>
        /// Checks if any active rule matches the given predicate.
        /// </summary>
        public bool HasActiveRule(System.Func<RuleData, bool> predicate)
        {
            return this.m_activeRules.Any(predicate);
        }
    }
}