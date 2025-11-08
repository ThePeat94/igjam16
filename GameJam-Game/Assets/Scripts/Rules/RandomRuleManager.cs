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
        [SerializeField] private LevelData m_levelData;
        [SerializeField] private int m_rotateAfterFrames;
        [SerializeField] private int m_firstRuleAfterFrames;
        [SerializeField] private RandomRuleUI m_randomRuleUi;
        
        private IReadOnlyList<RuleData> m_availableRules;
        private readonly List<RuleData> m_activeRules = new();

        private int m_currentFrameCooldown;

        private void Awake()
        {
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
                this.m_ruleHandlerFactory.CreateRuleHandler(ruleToRemove).Revert();
                this.m_randomRuleUi.RemoveRule(ruleToRemove);
            }

            this.m_activeRules.Add(rndRule);
            this.m_ruleHandlerFactory.CreateRuleHandler(rndRule).Apply();
            this.m_randomRuleUi.DisplayRule(rndRule);
        }
    }
}