using UnityEngine;

namespace Nidavellir
{
	/// <summary>
	/// Component that makes the player a child of this platform when they touch it.
	/// This allows the player to move with moving platforms without sliding.
	/// The player is automatically unparented when jumping or leaving the platform.
	/// </summary>
	[RequireComponent(typeof(Collider2D))]
	public class PlatformParent : MonoBehaviour
	{
		[Header("Settings")]
		[Tooltip("Tag of the player GameObject")]
		public string playerTag = "Player";

		[Tooltip("Minimum upward velocity to consider as jumping (unparents player)")]
		public float jumpVelocityThreshold = 5f;

		[Tooltip("Check if player is on top of platform (only parent if player is above)")]
		public bool onlyParentFromTop = true;

		[Tooltip("Vertical offset to check if player is on top (in world units)")]
		public float topCheckOffset = 0.5f;

		private Transform playerTransform;
		private MovementOldInput playerMovement;
		private Collider2D platformCollider;
		private bool isPlayerOnPlatform = false;
		private Transform originalParent;

		private void Awake()
		{
			platformCollider = GetComponent<Collider2D>();

			// Ensure collider is not a trigger for proper collision detection
			if (platformCollider.isTrigger)
			{
				Debug.LogWarning(
					$"PlatformParent on {gameObject.name}: Collider2D is set as trigger. Consider using OnTriggerEnter2D/Exit2D instead, or disable trigger for collision-based detection.");
			}
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (collision.gameObject.CompareTag(playerTag))
			{
				HandlePlayerEnter(collision.gameObject, collision);
			}
		}

		private void OnCollisionExit2D(Collision2D collision)
		{
			if (collision.gameObject.CompareTag(playerTag))
			{
				HandlePlayerExit(collision.gameObject);
			}
		}

		// Alternative trigger-based detection (if using trigger colliders)
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.CompareTag(playerTag))
			{
				// For triggers, we need to check position manually
				if (ShouldParentPlayer(other.gameObject))
				{
					HandlePlayerEnter(other.gameObject, null);
				}
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.gameObject.CompareTag(playerTag))
			{
				HandlePlayerExit(other.gameObject);
			}
		}

		private void FixedUpdate()
		{
			// Check if player is jumping and unparent if so
			if (isPlayerOnPlatform && playerMovement != null && playerTransform != null)
			{
				Rigidbody2D playerRb = playerMovement.rb;
				if (playerRb != null && playerRb.linearVelocity.y > jumpVelocityThreshold)
				{
					UnparentPlayer();
				}
			}
		}

		private void HandlePlayerEnter(GameObject player, Collision2D collision)
		{
			if (isPlayerOnPlatform)
				return;

			// Check if player should be parented (e.g., only from top)
			if (onlyParentFromTop && !IsPlayerOnTop(player, collision))
			{
				return;
			}

			playerTransform = player.transform;
			playerMovement = player.GetComponent<MovementOldInput>();

			if (playerTransform != null)
			{
				// Store original parent
				originalParent = playerTransform.parent;

				// Parent player to platform
				playerTransform.SetParent(transform);
				isPlayerOnPlatform = true;
			}
		}

		private void HandlePlayerExit(GameObject player)
		{
			if (playerTransform != null && player.transform == playerTransform)
			{
				UnparentPlayer();
			}
		}

		private void UnparentPlayer()
		{
			if (playerTransform != null)
			{
				// Restore original parent (or set to null if it was null)
				playerTransform.SetParent(originalParent);
				isPlayerOnPlatform = false;
				playerTransform = null;
				playerMovement = null;
				originalParent = null;
			}
		}

		private bool ShouldParentPlayer(GameObject player)
		{
			if (!onlyParentFromTop)
				return true;

			// Check if player is above the platform
			float platformTop = platformCollider.bounds.max.y;
			float playerBottom = player.GetComponent<Collider2D>()?.bounds.min.y ?? player.transform.position.y;

			return playerBottom >= platformTop - topCheckOffset;
		}

		private bool IsPlayerOnTop(GameObject player, Collision2D collision)
		{
			if (collision != null)
			{
				// Check collision contacts to see if player is on top
				foreach (ContactPoint2D contact in collision.contacts)
				{
					// If contact normal points up, player is on top
					if (contact.normal.y < -0.7f) // ~45 degrees tolerance
					{
						return true;
					}
				}
			}
			else
			{
				// Fallback: check position
				return ShouldParentPlayer(player);
			}

			return false;
		}

		private void OnDisable()
		{
			// Unparent player if component is disabled
			if (isPlayerOnPlatform)
			{
				UnparentPlayer();
			}
		}

		private void OnDestroy()
		{
			// Unparent player if platform is destroyed
			if (isPlayerOnPlatform)
			{
				UnparentPlayer();
			}
		}
	}
}