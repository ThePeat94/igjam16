using UnityEngine;

namespace Nidavellir.Rules.NoJump
{
	public class NoJumpRule : MonoBehaviour
	{
		private BetterJumping betterJumping;
		private MovementOldInput movementOldInput;
		private Rigidbody2D rb;
		private bool wasBetterJumpingEnabled;
		private float originalJumpForce;
		private Collision coll;

		private void Awake()
		{
			betterJumping = GetComponent<BetterJumping>();
			movementOldInput = GetComponent<MovementOldInput>();
			rb = GetComponent<Rigidbody2D>();
			coll = GetComponent<Collision>();

			if (betterJumping != null)
			{
				wasBetterJumpingEnabled = betterJumping.enabled;
			}

			if (movementOldInput != null)
			{
				originalJumpForce = movementOldInput.jumpForce;
			}
		}

		private void OnEnable()
		{
			if (betterJumping != null && betterJumping.enabled)
			{
				wasBetterJumpingEnabled = betterJumping.enabled;
				betterJumping.enabled = false;
			}

			if (movementOldInput != null)
			{
				movementOldInput.jumpForce = 0;
			}

			Debug.Log("No Jump Rule Applied - Player cannot jump");
		}

		private void OnDisable()
		{
			if (betterJumping != null && wasBetterJumpingEnabled)
			{
				betterJumping.enabled = true;
			}

			if (movementOldInput != null)
			{
				movementOldInput.jumpForce = originalJumpForce;
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