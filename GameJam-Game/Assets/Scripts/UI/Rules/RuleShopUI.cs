using System;
using System.Collections.Generic;
using System.Linq;
using Nidavellir.Scriptables.Rules;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nidavellir.UI.Rules
{
    public class RuleShopUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_parent;
        [SerializeField] private RuleShopContainerUI m_ruleContainerUiPrefab;
        [SerializeField] private TextMeshProUGUI m_currencyText;
        
        
        private Action<RuleData> m_onRulePurchased;
        
        private readonly Dictionary<RuleData, RuleShopContainerUI> m_availableRuleCards = new();
        
        public event Action<RuleData> OnRulePurchased
        {
            add => this.m_onRulePurchased += value;
            remove => this.m_onRulePurchased -= value;
        }
        
        public void Display(IReadOnlyList<RuleData> availableRules, IReadOnlyList<RuleData> purchasedRules, int money)
        {
            this.UpdateCurrencyDisplay(money);
            this.ClearAvailableRules();
            var allRules = availableRules.Concat(purchasedRules).ToList();
            foreach (var ruleData in allRules)
            {
                var card = Instantiate(this.m_ruleContainerUiPrefab, this.m_parent.transform);
                card.DisplayBase(ruleData);
                if (!purchasedRules.Contains(ruleData))
                {
                    card.DisplayLockedState();
                    card.DisplayPurchasability(money >= ruleData.UnlockCost);
                    card.OnClicked += () => this.HandleCardPurchase(ruleData);
                }
                else
                {
                    card.HideLockedState();
                }

                this.m_availableRuleCards.Add(ruleData, card);
            }
        }

        public void TogglePurchasability(int money)
        {
            this.UpdateCurrencyDisplay(money);
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

        public void DisplayRuleState(RuleData rule, bool purchased)
        {
            if (this.m_availableRuleCards.TryGetValue(rule, out var card))
            {
                if (purchased)
                {
                    card.HideLockedState();
                }
                else
                {
                    card.DisplayLockedState();
                }
            }
        }

        private void UpdateCurrencyDisplay(int money)
        {
            if (this.m_currencyText != null)
            {
                this.m_currencyText.text = money.ToString();
            }
        }
    }
}