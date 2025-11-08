using Nidavellir;
using UnityEngine;
using UnityEngine.UI;

namespace Nidavellir.UI
{
	public class AirBarUI : MonoBehaviour
	{
		[SerializeField]
		private Slider m_airSlider;

		[SerializeField]
		private AirController m_airController;

		private void Start()
		{
			// Find AirController if not assigned
			if (m_airController == null)
			{
				m_airController = FindFirstObjectByType<AirController>();
			}

			if (m_airController == null)
			{
				Debug.LogWarning("[AirBarUI] AirController not found. Air bar will not update.");
				return;
			}

			if (m_airSlider == null)
			{
				Debug.LogWarning("[AirBarUI] Air slider not assigned. Please assign a slider component.");
				return;
			}

			// Subscribe to air changes
			m_airController.OnAirPercentChanged += UpdateSlider;

			// Initialize slider value
			UpdateSlider(m_airController.AirPercent);
		}

		private void OnDestroy()
		{
			if (m_airController != null)
			{
				m_airController.OnAirPercentChanged -= UpdateSlider;
			}
		}

		private void UpdateSlider(float airPercent)
		{
			if (m_airSlider != null)
			{
				m_airSlider.value = Mathf.Clamp01(airPercent);
			}
		}
	}
}

