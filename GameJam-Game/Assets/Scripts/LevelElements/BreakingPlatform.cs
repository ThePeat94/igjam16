using System.Collections;
using Nidavellir.Rules;
using Nidavellir.Scriptables.Rules;
using UnityEngine;

namespace Nidavellir
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class BreakingPlatform : MonoBehaviour
	{
		[SerializeField]
		private int durability = 2;

		[SerializeField]
		private bool listenToGravityRule = false;

		[SerializeField]
		private GravityRuleData requiredGravityRule;

		[SerializeField]
		private bool listenToSlipperyWallsRule = false;

		[SerializeField]
		private NoWallRunRuleData requiredSlipperyWallsRule;

		[SerializeField]
		private RuleManager m_ruleManager;

		private void Awake()
		{
			if (m_ruleManager == null)
			{
				m_ruleManager = FindFirstObjectByType<RuleManager>();
			}
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			if (!other.CompareTag("Player"))
			{
				return;
			}

			bool canTriggerBreak = false;
			// Check if we need to listen to gravity rule and if it's active
			canTriggerBreak |= listenToGravityRule && IsRequiredGravityRuleActive();

			canTriggerBreak |= listenToSlipperyWallsRule && IsSlipperyWallsRuleActive() && IsPlayerSlidingDown(other);
			
			// Normal breaking logic (when player is above the platform)
			canTriggerBreak |= !listenToGravityRule && !listenToSlipperyWallsRule && other.transform.position.y > transform.position.y;
			

			if (canTriggerBreak)
			{
				StartCoroutine(DestroyAfterDelay());
			}
		}

		private bool IsRequiredGravityRuleActive()
		{
			if (m_ruleManager == null || requiredGravityRule == null)
			{
				return false;
			}

			return m_ruleManager.IsRuleActive(requiredGravityRule);
		}

		private bool IsSlipperyWallsRuleActive()
		{
			if (m_ruleManager == null)
			{
				return false;
			}

			// Check if any NoWallRunRuleData is active (slippery walls rule)
			if (requiredSlipperyWallsRule != null)
			{
				return m_ruleManager.IsRuleActive(requiredSlipperyWallsRule);
			}

			// Fallback: check if any NoWallRunRuleData is active
			return m_ruleManager.HasActiveRule(ruleData => ruleData is NoWallRunRuleData);
		}

		private bool IsPlayerSlidingDown(Collider2D playerCollider)
		{
			// Try to get MovementController component first
			var movementController = playerCollider.GetComponent<MovementController>();
			if (movementController != null)
			{
				// Check if player is wall sliding and moving downward
				if (movementController.wallSlide && movementController.rb != null)
				{
					return true;
				}
			}

			// Fallback: Try to get MovementOldInput component using reflection
			// MovementOldInput might be in a different namespace or assembly
			var allComponents = playerCollider.GetComponents<MonoBehaviour>();
			foreach (var component in allComponents)
			{
				if (component.GetType().Name == "MovementOldInput")
				{
					// Use reflection to check for wallSlide property
					var wallSlideField = component.GetType().GetField("wallSlide");
					var rbField = component.GetType().GetField("rb");

					if (wallSlideField != null && rbField != null)
					{
						bool isWallSliding = (bool)wallSlideField.GetValue(component);
						var rb = rbField.GetValue(component) as Rigidbody2D;

						if (isWallSliding && rb != null)
						{
							return rb.linearVelocity.y < 0;
						}
					}

					break;
				}
			}

			return false;
		}

		IEnumerator DestroyAfterDelay()
		{
			StartCoroutine(ShakeEffect(durability - 0.5f));
			yield return new WaitForSeconds(durability);
			Destroy(gameObject);
		}

		IEnumerator ShakeEffect(float duration = 1f, float magnitude = 0.05f)
		{
			Vector3 originalPos = transform.localPosition;
			float elapsed = 0f;

			while (elapsed < duration)
			{
				float x = Random.Range(-1f, 1f) * magnitude;
				float y = Random.Range(-1f, 1f) * magnitude;

				transform.localPosition = originalPos + new Vector3(x, y, 0);
				elapsed += Time.deltaTime;
				yield return null;
			}

			transform.localPosition = originalPos;
		}
	}
}