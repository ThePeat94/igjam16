using System;
using Nidavellir.EventArgs;
using Nidavellir.Scriptables;
using UnityEngine;

namespace Nidavellir
{
	public class HealthController : MonoBehaviour
	{
		[SerializeField]
		private ResourceData m_initialResourceData;

		private ResourceController m_resourceController;

		private Action m_onDeath;

		// UI events
		public event Action<float, float> OnHealthChanged;
		public event Action<float> OnHealthPercentChanged;

		public event Action OnDeath
		{
			add => m_onDeath += value;
			remove => m_onDeath -= value;
		}

		public DamageMode DamageMode { get; set; }

		public float CurrentHealth => m_resourceController != null ? m_resourceController.CurrentValue : 0f;
		public float MaxHealth => m_resourceController != null ? m_resourceController.MaxValue : 0f;
		public float HealthPercent => MaxHealth > 0f ? CurrentHealth / MaxHealth : 0f;

		private void Awake()
		{
			if (m_initialResourceData == null)
			{
				Debug.LogError($"[{name}] HealthController: m_initialResourceData is not assigned. Please assign a ResourceData asset.", this);
				return;
			}

			m_resourceController = new ResourceController(m_initialResourceData);
			m_resourceController.ResourceValueChanged += HandleHealthChange;

			OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
			OnHealthPercentChanged?.Invoke(HealthPercent);
		}

		private void OnDestroy()
		{
			if (m_resourceController != null)
				m_resourceController.ResourceValueChanged -= HandleHealthChange;
		}

		private void HandleHealthChange(object sender, ResourceValueChangedEventArgs e)
		{
			  OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
			OnHealthPercentChanged?.Invoke(HealthPercent);

			if (e.NewValue <= 0f)
			{
				m_onDeath?.Invoke();
			}
		}

		public void ProcessDamage(float amount)
		{
			if (m_resourceController == null) return;

			switch (DamageMode)
			{
				case DamageMode.Damage:
					m_resourceController.UseResource(amount);
					break;
				case DamageMode.Heal:
					m_resourceController.Add(amount);
					break;
			}
		}
	}
}