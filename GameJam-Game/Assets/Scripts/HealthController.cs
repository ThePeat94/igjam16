using System;
using Nidavellir.EventArgs;
using Nidavellir.Scriptables;
using UnityEngine;

namespace Nidavellir
{
    public class HealthController : MonoBehaviour
    {
        [SerializeField] private ResourceData m_initialResourceData;
        
        private ResourceController m_resourceController;

        private Action m_onDeath;
        
        public event Action OnDeath
        {
            add => this.m_onDeath += value;
            remove => this.m_onDeath -= value;
        }

        public DamageMode DamageMode
        {
            get;
            set;
        }

        private void Awake()
        {
            this.m_resourceController = new ResourceController(this.m_initialResourceData);
            this.m_resourceController.ResourceValueChanged += this.HandleHealthChange;
        }

        private void HandleHealthChange(object sender, ResourceValueChangedEventArgs e)
        {
            if (e.NewValue <= 0f)
            {
                this.m_onDeath?.Invoke();
            }
        }

        public void ProcessDamage(float damageAmount)
        {
            switch (this.DamageMode)
            {
                case DamageMode.Damage:
                    this.m_resourceController.UseResource(damageAmount);
                    break;
                case DamageMode.Heal:
                    this.m_resourceController.Add(damageAmount);
                    break;
            }
        }
    }
}