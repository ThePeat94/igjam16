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

    // UI events
    public event Action<float, float> OnHealthChanged;      // current, max
    public event Action<float> OnHealthPercentChanged;      // 0..1

    public event Action OnDeath
    {
        add => this.m_onDeath += value;
        remove => this.m_onDeath -= value;
    }

    public DamageMode DamageMode { get; set; }

    // Expose values (adjust property names if your ResourceController differs)
    public float CurrentHealth => m_resourceController != null ? m_resourceController.CurrentValue : 0f;
    public float MaxHealth => m_resourceController != null ? m_resourceController.MaxValue : 0f;
    public float HealthPercent => MaxHealth > 0f ? CurrentHealth / MaxHealth : 0f;

    private void Awake()
    {
        if (m_initialResourceData == null)
        {
            Debug.LogError($"[{name}] HealthController: m_initialResourceData is not assigned. Please assign a ResourceData asset.", this);
            return; // prevent NRE in ResourceController
        }

        this.m_resourceController = new ResourceController(this.m_initialResourceData);
        this.m_resourceController.ResourceValueChanged += this.HandleHealthChange;

        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        OnHealthPercentChanged?.Invoke(HealthPercent);
    }

    private void OnDestroy()
    {
        if (m_resourceController != null)
            m_resourceController.ResourceValueChanged -= this.HandleHealthChange;
    }

    private void HandleHealthChange(object sender, ResourceValueChangedEventArgs e)
    {
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        OnHealthPercentChanged?.Invoke(HealthPercent);

        if (e.NewValue <= 0f)
        {
            this.m_onDeath?.Invoke();
        }
    }

    public void ProcessDamage(float amount)
    {
        if (m_resourceController == null) return;

        switch (this.DamageMode)
        {
            case DamageMode.Damage:
                this.m_resourceController.UseResource(amount);
                break;
            case DamageMode.Heal:
                this.m_resourceController.Add(amount);
                break;
        }
    }
}
}