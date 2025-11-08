using System.Collections.Generic;
using Mono.Cecil;
using Nidavellir.Scriptables.Rules;
using UnityEngine;
using UnityEngine.UI;

namespace Nidavellir.UI.Rules
{
    public class RandomRuleUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_parent;
        [SerializeField] private Image m_imagePrefab;

        private readonly Dictionary<RuleData, Image> m_imageDisplays = new();
        
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        public void DisplayRule(RuleData rule)
        {
            var newImage = Instantiate(this.m_imagePrefab, this.m_parent.transform);
            newImage.sprite = rule.Icon;
            newImage.gameObject.SetActive(true);
            this.m_imageDisplays.Add(rule, newImage);
        }

        public void RemoveRule(RuleData rule)
        {
            if (this.m_imageDisplays.TryGetValue(rule, out var image))
            {
                Destroy(image.gameObject);
                this.m_imageDisplays.Remove(rule);
            }
        }
    }
}