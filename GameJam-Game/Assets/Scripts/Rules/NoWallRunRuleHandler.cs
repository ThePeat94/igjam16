using Unity.VisualScripting;
using UnityEngine;

namespace Nidavellir.Rules
{
	public class NoWallRunRuleHandler : IRuleHandler
	{
		private readonly MovementOldInput m_movementOldInput;

		public NoWallRunRuleHandler(MovementOldInput movementOldInput)
		{
			this.m_movementOldInput = movementOldInput;
		}

		public void Apply()
		{
			var noWallRunRule = this.m_movementOldInput.AddComponent<NoWallRunRule>();
		}

		public void Revert()
		{
			var noWallRunRule = this.m_movementOldInput.GetComponent<NoWallRunRule>();
			if (noWallRunRule != null)
			{
				Object.Destroy(noWallRunRule);
			}
		}
	}
}

