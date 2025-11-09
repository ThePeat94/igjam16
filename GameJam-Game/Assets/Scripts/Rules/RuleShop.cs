using System;
using System.Collections.Generic;
using System.Linq;
using Nidavellir.Scriptables.Rules;
using Nidavellir.UI.Rules;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

namespace Nidavellir.Rules
{
	public class RuleShop : MonoBehaviour
	{
		[SerializeField]
		private RuleShopUI m_ruleShopUI;

		[SerializeField]
		private Purse m_purse;

		private List<RuleData> m_availableRules;

		private void Awake()
		{
			this.m_ruleShopUI ??= FindFirstObjectByType<RuleShopUI>(FindObjectsInactive.Include);
			this.m_purse ??= FindFirstObjectByType<Purse>(FindObjectsInactive.Include);
			this.m_availableRules = Resources.LoadAll<RuleData>("Data/Rules").ToList();

			this.m_ruleShopUI.OnRulePurchased += this.HandleRulePurchase;
		}

		public void ShowShop(Action onExitButtonClicked = null)
		{
			if (!m_purse)
			{
				this.m_purse = FindFirstObjectByType<Purse>(FindObjectsInactive.Include);
			}

			var ignoreList = new List<RuleData>();
			ignoreList.AddRange(PlayerInventory.Instance.PurchasedRules);
			m_ruleShopUI.Display(this.m_availableRules.Except(ignoreList).ToList(), PlayerInventory.Instance.PurchasedRules, this.m_purse.CoinCount);
			m_ruleShopUI.gameObject.SetActive(true);
			m_ruleShopUI.OnExitButtonClicked += () =>
			{
				m_ruleShopUI.gameObject.SetActive(false);
				onExitButtonClicked?.Invoke();
			};
		}

		private void HandleRulePurchase(RuleData rule)
		{
			if (this.m_purse.CoinCount < rule.UnlockCost)
			{
				return;
			}

			this.m_purse.SpendCoins(rule.UnlockCost);
			PlayerInventory.Instance.AddRule(rule);
			this.m_availableRules.Remove(rule);
			this.m_ruleShopUI.DisplayRuleState(rule, true);
			this.m_ruleShopUI.TogglePurchasability(this.m_purse.CoinCount);
		}
	}
}