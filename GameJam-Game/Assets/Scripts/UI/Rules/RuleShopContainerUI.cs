using System;
using Nidavellir.Scriptables;
using Nidavellir.Scriptables.Rules;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nidavellir.UI.Rules
{
    public class RuleShopContainerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_nameText;
        [SerializeField] private Image m_icon;
        [SerializeField] private Image m_panelBackground;
        [SerializeField] private PointerRecipient m_actualCard;
        [SerializeField] private Image m_lockIcon;
        [SerializeField] private TextMeshProUGUI m_unlockCostText;
        
        private Action m_onClicked;
        
        private RuleData m_ruleData;
        private bool m_isUnlocked;
        private bool m_canBePurchased;
        
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
            
            this.m_nameText.text = this.m_ruleData.Name;
            this.m_icon.sprite = this.m_ruleData.Icon;
        }

        public void DisplayLockedState()
        {
            this.m_lockIcon.gameObject.SetActive(true);
            this.m_unlockCostText.gameObject.SetActive(true);
            this.m_unlockCostText.text = this.m_ruleData.UnlockCost.ToString();
        }

        public void HideLockedState()
        {
            this.m_lockIcon.gameObject.SetActive(false);
            this.m_unlockCostText.text = String.Empty;
        }

        public void DisplayState(bool active)
        {
            this.m_isUnlocked = active;
            this.UpdateStateColor();
        }

        private void UpdateStateColor()
        {
            this.m_panelBackground.color = this.m_isUnlocked ? Color.green : Color.white;
        }

        private void HandleCardClicked()
        {
            if (!this.m_canBePurchased) return;
            
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

        public void DisplayPurchasability(bool canPurchase)
        {
            this.m_canBePurchased = canPurchase;
        }
    }
}