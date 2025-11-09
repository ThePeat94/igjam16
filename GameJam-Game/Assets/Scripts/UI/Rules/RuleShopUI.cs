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
            add => m_onRulePurchased += value;
            remove => m_onRulePurchased -= value;
        }
        
        public void Display(IReadOnlyList<RuleData> availableRules, IReadOnlyList<RuleData> purchasedRules, int money)
        {
            UpdateCurrencyDisplay(money);
            ClearAvailableRules();
            var allRules = availableRules.Concat(purchasedRules).ToList();
            foreach (var ruleData in allRules)
            {
                var card = Instantiate(m_ruleContainerUiPrefab, m_parent.transform);
                card.DisplayBase(ruleData);
                if (!purchasedRules.Contains(ruleData))
                {
                    card.DisplayLockedState();
                    card.DisplayPurchasability(money >= ruleData.UnlockCost);
                    card.OnClicked += () => HandleCardPurchase(ruleData);
                }
                else
                {
                    card.HideLockedState();
                }

                m_availableRuleCards.Add(ruleData, card);
            }
        }

        public void TogglePurchasability(int money)
        {
            UpdateCurrencyDisplay(money);
            foreach (var (ruleData, card) in m_availableRuleCards)
            {
                card.DisplayPurchasability(money >= ruleData.UnlockCost);
            }
        }
        
        private void ClearAvailableRules()
        {
            foreach (Transform child in m_parent.transform)
            {
                Destroy(child.gameObject);
            }
            m_availableRuleCards.Clear();
        }

        private void HandleCardPurchase(RuleData ruleData)
        {
            m_onRulePurchased?.Invoke(ruleData);
        }

        public void DisplayRuleState(RuleData rule, bool purchased)
        {
            if (m_availableRuleCards.TryGetValue(rule, out var card))
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
            if (m_currencyText != null)
            {
                m_currencyText.text = money.ToString();
            }
        }
    }
}