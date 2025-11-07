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
        
        private IReadOnlyList<RuleData> m_availableRules;
        private List<RuleData> m_activeRules = new();

        private void Awake()
        {
            this.m_availableRulesUI ??= FindFirstObjectByType<AvailableRulesUI>(FindObjectsInactive.Include);
            this.m_ruleHandlerFactory ??= FindFirstObjectByType<RuleHandlerFactory>(FindObjectsInactive.Include);
            this.m_availableRulesUI.OnRuleClicked += this.HandleRuleToggle;
        }

        private void Start()
        {
            this.m_availableRules = Resources.LoadAll<RuleData>("Data/Rules").ToList();
            this.m_availableRulesUI.DisplayAvailableRules(this.m_availableRules);
        }

        private void HandleRuleToggle(RuleData ruleData)
        {
            if (this.m_activeRules.Contains(ruleData))
            {
                this.m_activeRules.Remove(ruleData);
                this.m_ruleHandlerFactory.CreateRuleHandler(ruleData).Revert();
            }
            else
            {
                this.m_activeRules.Add(ruleData);
                this.m_ruleHandlerFactory.CreateRuleHandler(ruleData).Apply();
            }
            
            this.m_availableRulesUI.DisplayRuleState(ruleData, this.m_activeRules.Contains(ruleData));
        }
    }
}