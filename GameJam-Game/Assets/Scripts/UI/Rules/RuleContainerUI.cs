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
        [SerializeField] private Image m_lockIcon;
        [SerializeField] private TextMeshProUGUI m_unlockCostText;
        [SerializeField] private Button m_purchaseButton;
        
        private Action m_onClicked;
        private Action m_onPurchased;
        
        private RuleData m_ruleData;
        private bool m_isActive;
        
        public event Action OnClicked
        {
            add => this.m_onClicked += value;
            remove => this.m_onClicked -= value;
        }
        
        public event Action OnPurchased
        {
            add => this.m_onPurchased += value;
            remove => this.m_onPurchased -= value;
        }

        private void Awake()
        {
            this.m_actualCard.OnPointerClickEvent += this.HandleCardClicked;
            this.m_actualCard.OnPointerEnterEvent += this.HandleCardHovered;
            this.m_actualCard.OnPointerExitEvent += this.HandleCardUnhovered;
            
            this.m_purchaseButton.onClick.AddListener(this.HandlePurchaseClick);
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
            this.m_descriptionText.text = this.m_ruleData.Description;
            this.m_icon.sprite = this.m_ruleData.Icon;
        }

        public void DisplayLockedState()
        {
            this.m_lockIcon.gameObject.SetActive(true);
            this.m_unlockCostText.gameObject.SetActive(true);
            this.m_unlockCostText.text = this.m_ruleData.UnlockCost.ToString();
            this.m_purchaseButton.gameObject.SetActive(true);
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

        public void DisplayPurchasability(bool canPurchase)
        {
            this.m_purchaseButton.interactable = canPurchase;
        }

        private void HandlePurchaseClick()
        {
            this.m_onPurchased?.Invoke();
        }
    }
}