using System;
using System.Collections.Generic;
using Nidavellir.Scriptables;
using Nidavellir.Scriptables.Rules;
using UnityEngine;

namespace Nidavellir.UI.Rules
{
    public class AvailableRulesUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_parent;
        [SerializeField] private RuleContainerUI m_prefab;

        private Dictionary<RuleData, RuleContainerUI> m_availableRuleCards = new();
        
        private Action<RuleData> m_onRuleClicked;
        
        public event Action<RuleData> OnRuleClicked
        {
            add => this.m_onRuleClicked += value;
            remove => this.m_onRuleClicked -= value;
        }
        
        public void DisplayAvailableRules(IReadOnlyList<RuleData> availableRules)
        {
            this.ClearAvailableRules();
            foreach (var ruleData in availableRules)
            {
                var card = Instantiate(this.m_prefab, this.m_parent.transform);
                card.DisplayBase(ruleData);
                card.OnClicked += () => this.HandleRuleClicked(ruleData);
                this.m_availableRuleCards.Add(ruleData, card);
            }
        }
        
        public void DisplayRuleState(RuleData ruleData, bool active)
        {
            if (this.m_availableRuleCards.TryGetValue(ruleData, out var card))
            {
                card.DisplayState(active);
            }
        }
        
        private void HandleRuleClicked(RuleData ruleData)
        {
            this.m_onRuleClicked?.Invoke(ruleData);
        }

        private void ClearAvailableRules()
        {
            foreach (var (_, card) in this.m_availableRuleCards)
            {
                Destroy(card.gameObject);
            }
            
            this.m_availableRuleCards.Clear();
        }
    }
}