using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nidavellir
{
	public class Water : MonoBehaviour
	{
		[SerializeField]
		private float m_airReductionPerSecond = 10f;

		[SerializeField]
		private float m_airRefillPerSecond = 20f;

		private readonly HashSet<AirController> m_playersInWater = new HashSet<AirController>();
		private readonly Dictionary<AirController, Coroutine> m_refillCoroutines = new Dictionary<AirController, Coroutine>();
		private MovementController m_player;

		private void Awake()
		{
			// Cache player reference to check if game has started
			m_player = FindFirstObjectByType<MovementController>();
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag("Player"))
			{
				var airController = other.GetComponent<AirController>();
				if (airController != null)
				{
					m_playersInWater.Add(airController);

					// Stop refilling if player re-enters water
					if (m_refillCoroutines.TryGetValue(airController, out var coroutine))
					{
						StopCoroutine(coroutine);
						m_refillCoroutines.Remove(airController);
					}
				}
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (!isActiveAndEnabled)
			{
				StopAllCoroutines();
				m_refillCoroutines.Clear();
				return;
			}

			if (other.CompareTag("Player"))
			{
				var airController = other.GetComponent<AirController>();
				if (airController != null)
				{
					m_playersInWater.Remove(airController);

					// Start refilling when player exits water
					if (!m_refillCoroutines.ContainsKey(airController))
					{
						var coroutine = StartCoroutine(RefillAirCoroutine(airController));
						m_refillCoroutines[airController] = coroutine;
					}
				}
			}
		}

		private void Update()
		{
			// Don't reduce air if game hasn't started yet (still in rule select)
			if (!IsGameStarted())
			{
				return;
			}

			// Reduce air for players in water
			foreach (var airController in m_playersInWater)
			{
				if (airController != null)
				{
					float reductionAmount = m_airReductionPerSecond * Time.deltaTime;
					airController.ReduceAir(reductionAmount);
				}
			}
		}

		private bool IsGameStarted()
		{
			// Game has started when the player is enabled
			if (m_player != null)
			{
				return m_player.enabled;
			}

			// Fallback: try to find player if not cached
			m_player = FindFirstObjectByType<MovementController>();
			return m_player != null && m_player.enabled;
		}

		private IEnumerator RefillAirCoroutine(AirController airController)
		{
			while (airController != null && airController.CurrentAir < airController.MaxAir && !m_playersInWater.Contains(airController))
			{
				float refillAmount = m_airRefillPerSecond * Time.deltaTime;
				airController.AddAir(refillAmount);
				yield return null;
			}

			m_refillCoroutines.Remove(airController);
		}

		private void OnDestroy()
		{
			// Clean up all coroutines
			StopAllCoroutines();
			m_refillCoroutines.Clear();
		}
	}
}