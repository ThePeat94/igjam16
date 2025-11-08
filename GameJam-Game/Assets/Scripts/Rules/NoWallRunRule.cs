using UnityEngine;

namespace Nidavellir.Rules
{
	public class NoWallRunRule : MonoBehaviour
	{
		private MovementOldInput movementOldInput;
		private Collision coll;
		private float originalSlideSpeed;

		private void Awake()
		{
			movementOldInput = GetComponent<MovementOldInput>();
			coll = GetComponent<Collision>();

			if (movementOldInput != null)
			{
				originalSlideSpeed = movementOldInput.slideSpeed;
			}
		}

		private void OnEnable()
		{
			if (movementOldInput != null)
			{
				// Increase slide speed significantly to make walls very slippery
				movementOldInput.slideSpeed = 30f; // Much faster than default 5
			}

			Debug.Log("No Wall Run Rule Applied - Walls are non-sticky and slide very fast");
		}

		private void OnDisable()
		{
			if (movementOldInput != null)
			{
				movementOldInput.slideSpeed = originalSlideSpeed;
				// Force wallGrab to false when rule is disabled
				movementOldInput.wallGrab = false;
			}

			Debug.Log("No Wall Run Rule Reverted - Walls work normally again");
		}

		private void Update()
		{
			if (movementOldInput == null || coll == null)
				return;

			// Block wall grab input (Fire3 button)
			if (UnityEngine.Input.GetButton("Fire3"))
			{
				// Prevent wall grab from being set
				movementOldInput.wallGrab = false;
			}

			// Force wall slide when on wall and not on ground
			if (coll.onWall && !coll.onGround)
			{
				// Ensure wallGrab is always false
				if (movementOldInput.wallGrab)
				{
					movementOldInput.wallGrab = false;
				}

				// Force wall slide to be active
				if (!movementOldInput.wallSlide)
				{
					movementOldInput.wallSlide = true;
				}

				// Force slide down even if not moving horizontally
				// This makes walls completely non-sticky
				if (movementOldInput.rb != null)
				{
					bool pushingWall = false;
					if ((movementOldInput.rb.linearVelocity.x > 0 && coll.onRightWall) || 
					    (movementOldInput.rb.linearVelocity.x < 0 && coll.onLeftWall))
					{
						pushingWall = true;
					}

					float push = pushingWall ? 0 : movementOldInput.rb.linearVelocity.x;
					movementOldInput.rb.linearVelocity = new Vector2(push, -movementOldInput.slideSpeed);
				}
			}
		}
	}
}

