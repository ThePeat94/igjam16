using System;
using Nidavellir.EventArgs;
using UnityEngine;

namespace Nidavellir
{
	public class AirController : MonoBehaviour
	{
		private ResourceController m_resourceController;

		private Action m_onAirDepleted;
		private bool processChanges = true;

		// UI events
		public event Action<float, float> OnAirChanged;
		public event Action<float> OnAirPercentChanged;

		public event Action OnAirDepleted
		{
			add => m_onAirDepleted += value;
			remove => m_onAirDepleted -= value;
		}

		public float CurrentAir => m_resourceController != null ? m_resourceController.CurrentValue : 0f;
		public float MaxAir => m_resourceController != null ? m_resourceController.MaxValue : 0f;
		public float AirPercent => MaxAir > 0f ? CurrentAir / MaxAir : 0f;

		private void Awake()
		{
			// Initialize with 100 air as starting and max value
			m_resourceController = new ResourceController(100f, 100f);
			m_resourceController.ResourceValueChanged += HandleAirChange;

			OnAirChanged?.Invoke(CurrentAir, MaxAir);
			OnAirPercentChanged?.Invoke(AirPercent);
			FindFirstObjectByType<GameManager>().OnGameOver += OnGameOver;
		}

		private void OnGameOver(bool win)
		{
			processChanges = false;
		}

		private void OnDestroy()
		{
			if (m_resourceController != null)
				m_resourceController.ResourceValueChanged -= HandleAirChange;
		}

		private void HandleAirChange(object sender, ResourceValueChangedEventArgs e)
		{
			OnAirChanged?.Invoke(CurrentAir, MaxAir);
			OnAirPercentChanged?.Invoke(AirPercent);

			if (e.NewValue <= 0f)
			{
				m_onAirDepleted?.Invoke();
			}
		}

		public void ReduceAir(float amount)
		{
			if (m_resourceController == null || !processChanges) return;

			// Clamp to prevent going below 0
			float actualAmount = Mathf.Min(amount, CurrentAir);
			if (actualAmount > 0f)
			{
				m_resourceController.UseResource(actualAmount);
			}
		}

		public void AddAir(float amount)
		{
			if (m_resourceController == null || !processChanges) return;

			// Clamp to prevent going above max
			float actualAmount = Mathf.Min(amount, MaxAir - CurrentAir);
			if (actualAmount > 0f)
			{
				m_resourceController.Add(actualAmount);
			}
		}
	}
}

