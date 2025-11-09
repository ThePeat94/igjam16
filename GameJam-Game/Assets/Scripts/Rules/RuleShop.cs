using System;
using System.Collections.Generic;
using System.Linq;
using Nidavellir.Scriptables.Rules;
using Nidavellir.UI.Rules;
using UnityEngine;

namespace Nidavellir.Rules
{
    public class RuleShop : MonoBehaviour
    {
        [SerializeField] private RuleShopUI m_ruleShopUI;
        [SerializeField] private Purse m_purse;
        
        private List<RuleData> m_availableRules;
        private readonly List<RuleData> m_purchasedRules = new();

        public IReadOnlyList<RuleData> PurchasedRules =>  this.m_purchasedRules;
        
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            
            this.m_ruleShopUI ??= FindFirstObjectByType<RuleShopUI>(FindObjectsInactive.Include);
            this.m_purse ??= FindFirstObjectByType<Purse>(FindObjectsInactive.Include);
            this.m_availableRules = Resources.LoadAll<RuleData>("Data/Rules").ToList();

            this.m_ruleShopUI.OnRulePurchased += this.HandleRulePurchase;
        }

        public void ShowShop()
        {
            this.m_ruleShopUI.Display(this.m_availableRules, this.m_purchasedRules, this.m_purse.CoinCount);
        }

        private void HandleRulePurchase(RuleData rule)
        {
            if (this.m_purse.CoinCount < rule.UnlockCost)
            {
                return;
            }
            
            this.m_purse.SpendCoins(rule.UnlockCost);
            this.m_purchasedRules.Add(rule);
            this.m_availableRules.Remove(rule);
            this.m_ruleShopUI.DisplayRuleState(rule, true);
            this.m_ruleShopUI.TogglePurchasability(this.m_purse.CoinCount);
        }
    }
}