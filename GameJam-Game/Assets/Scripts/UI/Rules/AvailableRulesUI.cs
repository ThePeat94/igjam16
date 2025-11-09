using System;
using System.Collections.Generic;
using System.Linq;
using Nidavellir.Scriptables.Rules;
using UnityEngine;
using UnityEngine.UI;

namespace Nidavellir.UI.Rules
{
	public class AvailableRulesUI : MonoBehaviour
	{
		[SerializeField]
		private GameObject m_parent;

		[SerializeField]
		private RuleContainerUI m_prefab;

		[SerializeField]
		private Button m_startLevelButton;

		[SerializeField]
		private bool m_debugUnlockAllRules;

		private readonly Dictionary<RuleData, RuleContainerUI> m_availableRuleCards = new();

		private Action<RuleData> m_onRuleClicked;
		private Action m_onStartLevelClicked;

		public event Action<RuleData> OnRuleClicked
		{
			add => this.m_onRuleClicked += value;
			remove => this.m_onRuleClicked -= value;
		}

		private void Awake()
		{
			this.m_startLevelButton.onClick.AddListener(HandleStartLevel);
		}

		public void Hide()
		{
			this.gameObject.SetActive(false);
		}

		public void DisplayAvailableRules(IReadOnlyList<RuleData> availableRules)
		{
			this.ClearAvailableRules();

			// If debug mode is enabled, load all rules from Resources instead
			IReadOnlyList<RuleData> rulesToDisplay = availableRules;
#if UNITY_EDITOR

			if (this.m_debugUnlockAllRules)
			{
				rulesToDisplay = Resources.LoadAll<RuleData>("Data/Rules").ToList();
			}
#endif

			foreach (var ruleData in rulesToDisplay)
			{
				var card = Instantiate(this.m_prefab, this.m_parent.transform);
				card.DisplayBase(ruleData);
				card.OnClicked += () => this.HandleRuleClicked(ruleData);
				m_availableRuleCards.Add(ruleData, card);
			}
		}

		public void DisplayRuleState(RuleData ruleData, bool active)
		{
			if (this.m_availableRuleCards.TryGetValue(ruleData, out var card))
			{
				card.DisplayState(active);
			}
		}

		public void DisplayStartLevelState(bool active)
		{
			this.m_startLevelButton.interactable = active;
		}

		private void HandleStartLevel()
		{
			FindFirstObjectByType<GameManager>().StartLevel();
			gameObject.SetActive(false);
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