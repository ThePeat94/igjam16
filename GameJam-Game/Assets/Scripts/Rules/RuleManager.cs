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
            m_levelData = FindFirstObjectByType<GameManager>()
                .LevelData;
            m_ruleShop ??= FindFirstObjectByType<RuleShop>(FindObjectsInactive.Include);
            m_availableRulesUI ??= FindFirstObjectByType<AvailableRulesUI>(FindObjectsInactive.Include);
            m_ruleHandlerFactory ??= FindFirstObjectByType<RuleHandlerFactory>(FindObjectsInactive.Include);
            m_availableRulesUI.OnRuleClicked += HandleRuleToggle;
            // Combine free rules from level data with purchased rules from inventory
            m_availableRules = m_levelData.AvailableFreeRules
                .Concat(PlayerInventory.Instance.PurchasedRules)
                .ToList();
            
            PlayerInventory.Instance.OnRuleAdded += () =>
            {
                // Update available rules when a new rule is purchased
                m_availableRules = m_levelData.AvailableFreeRules
                    .Concat(PlayerInventory.Instance.PurchasedRules)
                    .ToList();
                m_availableRulesUI.DisplayAvailableRules(m_availableRules);
                m_availableRulesUI.DisplayStartLevelState(m_levelData.MinimumRules <= 0);
            };
        }

        private void Start()
        {
            m_availableRulesUI.DisplayAvailableRules(m_availableRules);
            m_availableRulesUI.DisplayStartLevelState(m_levelData.MinimumRules <= 0);
        }

        private void HandleRuleToggle(RuleData ruleData)
        {
            var ruleHandler = m_ruleHandlerFactory.CreateRuleHandler(ruleData);
            bool isInverted = ruleData.InvertedRule;
            
            if (m_activeRules.Contains(ruleData))
            {
                m_activeRules.Remove(ruleData);
                // If inverted, apply when deactivating; otherwise revert
                if (isInverted)
                {
                    ruleHandler.Apply();
                }
                else
                {
                    ruleHandler.Revert();
                }
            }
            else if (m_activeRules.Count < m_levelData.MaximumRules)
            {
                m_activeRules.Add(ruleData);
                // If inverted, revert when activating; otherwise apply
                if (isInverted)
                {
                    ruleHandler.Revert();
                }
                else
                {
                    ruleHandler.Apply();
                }
            }

            m_availableRulesUI.DisplayRuleState(ruleData, m_activeRules.Contains(ruleData));
            m_availableRulesUI.DisplayStartLevelState(m_activeRules.Count >= m_levelData.MinimumRules);
        }
        
        /// <summary>
        /// Checks if a specific rule is currently active.
        /// </summary>
        public bool IsRuleActive(RuleData ruleData)
        {
            return m_activeRules.Contains(ruleData);
        }
        
        /// <summary>
        /// Checks if any active rule matches the given predicate.
        /// </summary>
        public bool HasActiveRule(Func<RuleData, bool> predicate)
        {
            return m_activeRules.Any(predicate);
        }
    }
}