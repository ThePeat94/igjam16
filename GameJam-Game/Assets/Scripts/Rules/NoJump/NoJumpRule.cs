using UnityEngine;

namespace Nidavellir.Rules.NoJump
{
	public class NoJumpRule : MonoBehaviour
	{
		private BetterJumping betterJumping;
		private MovementController m_movementController;
		private Rigidbody2D rb;
		private bool wasBetterJumpingEnabled;
		private float originalJumpForce;
		private Collision coll;

		private void Awake()
		{
			betterJumping = GetComponent<BetterJumping>();
			m_movementController = GetComponent<MovementController>();
			rb = GetComponent<Rigidbody2D>();
			coll = GetComponent<Collision>();

			if (betterJumping != null)
			{
				wasBetterJumpingEnabled = betterJumping.enabled;
			}

			if (m_movementController != null)
			{
				originalJumpForce = m_movementController.jumpForce;
			}
		}

		private void OnEnable()
		{
			if (betterJumping != null && betterJumping.enabled)
			{
				wasBetterJumpingEnabled = betterJumping.enabled;
				betterJumping.enabled = false;
			}

			if (m_movementController != null)
			{
				m_movementController.jumpForce = 0;
			}

			Debug.Log("No Jump Rule Applied - Player cannot jump");
		}

		private void OnDisable()
		{
			if (betterJumping != null && wasBetterJumpingEnabled)
			{
				betterJumping.enabled = true;
			}

			if (m_movementController != null)
			{
				m_movementController.jumpForce = originalJumpForce;
			}

			Debug.Log("No Jump Rule Reverted - Player can jump again");
		}

		private void Update()
		{
			if (UnityEngine.Input.GetButtonDown("Jump") && rb != null && coll != null)
			{
				if (coll.onGround && rb.linearVelocity.y > 0)
				{
					rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
				}
			}
		}
	}
}