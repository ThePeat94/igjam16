using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Nidavellir.UI
{
    public class PointerRecipient : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private Action m_onPointerEnter;
        private Action m_onPointerExit;
        private Action m_onPointerClick;
        
        public event Action OnPointerEnterEvent
        {
            add => this.m_onPointerEnter += value;
            remove => this.m_onPointerEnter -= value;
        }
        
        public event Action OnPointerExitEvent
        {
            add => this.m_onPointerExit += value;
            remove => this.m_onPointerExit -= value;
        }
        
        public event Action OnPointerClickEvent
        {
            add => this.m_onPointerClick += value;
            remove => this.m_onPointerClick -= value;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            this.m_onPointerEnter?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.m_onPointerExit?.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            this.m_onPointerClick?.Invoke();
        }
    }
}