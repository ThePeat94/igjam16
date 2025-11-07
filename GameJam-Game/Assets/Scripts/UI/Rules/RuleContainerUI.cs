using System;
using Nidavellir.Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nidavellir.UI.Rules
{
    public class RuleContainerUI : MonoBehaviour
    {
        [SerializeField] private RuleData m_ruleData;
        [SerializeField] private TextMeshProUGUI m_nameText;
        [SerializeField] private TextMeshProUGUI m_descriptionText;
        [SerializeField] private Image m_icon;
        [SerializeField] private Image m_panelBackground;
        [SerializeField] private PointerRecipient m_actualCard;

        private void Awake()
        {
            this.m_actualCard.OnPointerClickEvent += this.HandleCardClicked;
            this.m_actualCard.OnPointerEnterEvent += this.HandleCardHovered;
            this.m_actualCard.OnPointerExitEvent += this.HandleCardUnhovered;
        }

        private void OnDestroy()
        {
            this.m_actualCard.OnPointerClickEvent -= this.HandleCardClicked;
            this.m_actualCard.OnPointerEnterEvent -= this.HandleCardHovered;
            this.m_actualCard.OnPointerExitEvent -= this.HandleCardUnhovered;
        }

        public void Display(RuleData ruleData)
        {
            this.m_ruleData = ruleData;
            
            this.m_nameText.text = this.m_ruleData.name;
            this.m_descriptionText.text = this.m_ruleData.Description;
            this.m_icon.sprite = this.m_ruleData.Icon;
        }

        private void HandleCardClicked()
        {
            Debug.Log($"{this.m_ruleData.name} clicked");
        }
        
        private void HandleCardHovered()
        {
            this.m_panelBackground.color = Color.yellow;
        }
        
        private void HandleCardUnhovered()
        {
            this.m_panelBackground.color = Color.white;
        }
    }
}