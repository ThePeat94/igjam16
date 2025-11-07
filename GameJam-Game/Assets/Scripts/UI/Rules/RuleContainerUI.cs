using System;
using Nidavellir.Scriptables;
using Nidavellir.Scriptables.Rules;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nidavellir.UI.Rules
{
    public class RuleContainerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_nameText;
        [SerializeField] private TextMeshProUGUI m_descriptionText;
        [SerializeField] private Image m_icon;
        [SerializeField] private Image m_panelBackground;
        [SerializeField] private PointerRecipient m_actualCard;
        
        private RuleData m_ruleData;
        private Action m_onClicked;
        private bool m_isActive;
        
        public event Action OnClicked
        {
            add => this.m_onClicked += value;
            remove => this.m_onClicked -= value;
        }

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

        public void DisplayBase(RuleData ruleData)
        {
            this.m_ruleData = ruleData;
            
            this.m_nameText.text = this.m_ruleData.name;
            this.m_descriptionText.text = this.m_ruleData.Description;
            this.m_icon.sprite = this.m_ruleData.Icon;
        }

        public void DisplayState(bool active)
        {
            this.m_isActive = active;
            this.UpdateStateColor();
        }

        private void UpdateStateColor()
        {
            this.m_panelBackground.color = this.m_isActive ? Color.green : Color.white;
        }

        private void HandleCardClicked()
        {
            Debug.Log($"{this.m_ruleData.name} clicked");
            this.m_onClicked?.Invoke();
        }
        
        private void HandleCardHovered()
        {
            this.m_panelBackground.color = Color.yellow;
        }
        
        private void HandleCardUnhovered()
        {
            this.UpdateStateColor();
        }
    }
}