using System;
using System.Collections.Generic;
using Nidavellir.Scriptables;
using Nidavellir.Scriptables.Rules;
using UnityEngine;
using UnityEngine.UI;

namespace Nidavellir.UI.Rules
{
    public class AvailableRulesUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_parent;
        [SerializeField] private RuleContainerUI m_prefab;
        [SerializeField] private Button m_startLevelButton;

        private readonly Dictionary<RuleData, RuleContainerUI> m_availableRuleCards = new();
        
        private Action<RuleData> m_onRuleClicked;
        private Action m_onStartLevelClicked;
        
        public event Action<RuleData> OnRuleClicked
        {
            add => this.m_onRuleClicked += value;
            remove => this.m_onRuleClicked -= value;
        }
        
        public event Action OnStartLevelClicked
        {
            add => this.m_onStartLevelClicked += value;
            remove => this.m_onStartLevelClicked -= value;
        }

        private void Awake()
        {
            this.m_startLevelButton.onClick.AddListener(this.HandleStartLevel);
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

        public void DisplayStartLevelState(bool active)
        {
            this.m_startLevelButton.interactable = active;
        }

        private void HandleStartLevel()
        {
            this.m_onStartLevelClicked?.Invoke();
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