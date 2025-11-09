using UnityEngine;

namespace Nidavellir.Rules.NoWallRun
{
	public class NoWallRunRule : MonoBehaviour
	{
		private MovementController m_movementController;
		private Collision coll;
		private float originalSlideSpeed;

		private void Awake()
		{
			m_movementController = GetComponent<MovementController>();
			coll = GetComponent<Collision>();

			if (m_movementController != null)
			{
				originalSlideSpeed = m_movementController.slideSpeed;
			}
		}

		private void OnEnable()
		{
			if (m_movementController != null)
			{
				// Increase slide speed significantly to make walls very slippery
				m_movementController.slideSpeed = 30f; // Much faster than default 5
				m_movementController.canWallGrab = false; // Disable wall grab entirely
			}

			Debug.Log("No Wall Run Rule Applied - Walls are non-sticky and slide very fast");
		}

		private void OnDisable()
		{
			if (m_movementController != null)
			{
				m_movementController.slideSpeed = originalSlideSpeed;
				// Force wallGrab to false when rule is disabled
				m_movementController.wallGrab = false;
				m_movementController.canWallGrab = true;
			}

			Debug.Log("No Wall Run Rule Reverted - Walls work normally again");
		}

		private void Update()
		{
			if (m_movementController == null || coll == null)
				return;

			// Block wall grab input (Fire3 button)
			if (UnityEngine.Input.GetButton("Fire3"))
			{
				// Prevent wall grab from being set
				m_movementController.wallGrab = false;
			}

			// Force wall slide when on wall and not on ground
			if (coll.onWall && !coll.onGround)
			{
				// Ensure wallGrab is always false
				if (m_movementController.wallGrab)
				{
					m_movementController.wallGrab = false;
				}

				// Force wall slide to be active
				if (!m_movementController.wallSlide)
				{
					m_movementController.wallSlide = true;
				}

				// Force slide down even if not moving horizontally
				// This makes walls completely non-sticky
				if (m_movementController.rb != null)
				{
					bool pushingWall = false;
					if ((m_movementController.rb.linearVelocity.x > 0 && coll.onRightWall) ||
					    (m_movementController.rb.linearVelocity.x < 0 && coll.onLeftWall))
					{
						pushingWall = true;
					}

					float push = pushingWall ? 0 : m_movementController.rb.linearVelocity.x;
					m_movementController.rb.linearVelocity = new Vector2(push, -m_movementController.slideSpeed);
				}
			}
		}
	}
}