using System;
using System.Collections.Generic;
using System.Linq;
using Nidavellir.Scriptables;
using Nidavellir.Scriptables.Rules;
using Nidavellir.UI.Rules;
using Nidavellir.Utils;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Nidavellir.Rules
{
    public class RandomRuleManager : MonoBehaviour
    {
        [SerializeField] private RuleHandlerFactory m_ruleHandlerFactory;
        [SerializeField] private int m_rotateAfterFrames;
        [SerializeField] private int m_firstRuleAfterFrames;
        [SerializeField] private RandomRuleUI m_randomRuleUi;
        
        private LevelData m_levelData;
        private IReadOnlyList<RuleData> m_availableRules;
        private readonly List<RuleData> m_activeRules = new();

        private int m_currentFrameCooldown;

        private void Awake()
        {
            m_levelData = FindFirstObjectByType<GameManager>().LevelData;
            this.m_randomRuleUi ??= FindFirstObjectByType<RandomRuleUI>(FindObjectsInactive.Include);
            this.m_ruleHandlerFactory ??= FindFirstObjectByType<RuleHandlerFactory>(FindObjectsInactive.Include);
            this.m_availableRules =
                this.m_levelData.AvailableFreeRules
                    .Concat(this.m_levelData.AvailableLockedRules)
                    .ToList();

            this.m_currentFrameCooldown = this.m_firstRuleAfterFrames;
        }

        private void FixedUpdate()
        {
            this.m_currentFrameCooldown--;
            if (this.m_currentFrameCooldown > 0) 
                return;
            this.RotateRules();
            this.m_currentFrameCooldown = this.m_rotateAfterFrames;
        }

        private void RotateRules()
        {
            var rndRule = this.m_availableRules.Except(this.m_activeRules).ToList().GetRandomElement();
            if (this.m_activeRules.Count >= this.m_levelData.MaximumRules)
            {
                var ruleToRemove = this.m_activeRules.GetRandomElement();
                this.m_activeRules.Remove(ruleToRemove);
                var ruleHandlerToRemove = this.m_ruleHandlerFactory.CreateRuleHandler(ruleToRemove);
                bool isInverted = ruleToRemove.InvertedRule;
                // If inverted, apply when deactivating; otherwise revert
                if (isInverted)
                {
                    ruleHandlerToRemove.Apply();
                }
                else
                {
                    ruleHandlerToRemove.Revert();
                }
                this.m_randomRuleUi.RemoveRule(ruleToRemove);
            }

            this.m_activeRules.Add(rndRule);
            var ruleHandler = this.m_ruleHandlerFactory.CreateRuleHandler(rndRule);
            bool isRndRuleInverted = rndRule.InvertedRule;
            // If inverted, revert when activating; otherwise apply
            if (isRndRuleInverted)
            {
                ruleHandler.Revert();
            }
            else
            {
                ruleHandler.Apply();
            }
            this.m_randomRuleUi.DisplayRule(rndRule);
        }
    }
}