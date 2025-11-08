using System;
using System.Collections.Generic;
using Nidavellir.Scriptables.Rules;
using UnityEngine;

namespace Nidavellir.UI.Rules
{
    public class RuleShopUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_parent;
        [SerializeField] private RuleContainerUI m_ruleContainerUiPrefab;
        
        private Action<RuleData> m_onRulePurchased;
        
        private readonly Dictionary<RuleData, RuleContainerUI> m_availableRuleCards = new();
        
        public event Action<RuleData> OnRulePurchased
        {
            add => this.m_onRulePurchased += value;
            remove => this.m_onRulePurchased -= value;
        }
        
        public void Display(IReadOnlyList<RuleData> availableRules)
        {
            this.ClearAvailableRules();
            foreach (var ruleData in availableRules)
            {
                var card = Instantiate(this.m_ruleContainerUiPrefab, this.m_parent.transform);
                card.DisplayBase(ruleData);
                card.DisplayLockedState();
                card.OnPurchased += () => this.HandleCardPurchase(ruleData);
                this.m_availableRuleCards.Add(ruleData, card);
            }
        }

        public void TogglePurchasability(int money)
        {
            foreach (var (ruleData, card) in this.m_availableRuleCards)
            {
                card.DisplayPurchasability(money >= ruleData.UnlockCost);
            }
        }
        
        private void ClearAvailableRules()
        {
            foreach (Transform child in this.m_parent.transform)
            {
                Destroy(child.gameObject);
            }
            this.m_availableRuleCards.Clear();
        }

        private void HandleCardPurchase(RuleData ruleData)
        {
            this.m_onRulePurchased?.Invoke(ruleData);
        }
    }
}