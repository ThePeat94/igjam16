using UnityEngine;

namespace Nidavellir.Rules.NoDashRule
{
	public class NoDashRule : MonoBehaviour
	{
		private MovementOldInput movementOldInput;
		private float originalDashSpeed;
		private bool dashInputBlocked;

		private void Awake()
		{
			movementOldInput = GetComponent<MovementOldInput>();

			if (movementOldInput != null)
			{
				originalDashSpeed = movementOldInput.dashSpeed;
			}
		}

		private void OnEnable()
		{
			if (movementOldInput != null)
			{
				// Set dash speed to 0 to effectively disable dash movement
				movementOldInput.dashSpeed = 0;
			}

			Debug.Log("No Dash Rule Applied - Player cannot dash");
		}

		private void OnDisable()
		{
			if (movementOldInput != null)
			{
				movementOldInput.dashSpeed = originalDashSpeed;
			}

			Debug.Log("No Dash Rule Reverted - Player can dash again");
		}

		private void Update()
		{
			// Block dash input by consuming Fire1 input before MovementOldInput processes it
			// We check for Fire1 input and prevent it from being processed
			// Since we can't directly intercept MovementOldInput's Update, we set dashSpeed to 0
			// which makes the dash have no effect even if it triggers
			// The dash will still consume hasDashed, but won't move the player
			if (movementOldInput != null && UnityEngine.Input.GetButtonDown("Fire1"))
			{
				// Ensure dashSpeed remains 0 to prevent any dash movement
				if (movementOldInput.dashSpeed > 0)
				{
					movementOldInput.dashSpeed = 0;
				}
			}
		}
	}
}
