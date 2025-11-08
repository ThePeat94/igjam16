using Unity.VisualScripting;
using UnityEngine;

namespace Nidavellir.Rules.NoWallRun
{
	public class NoWallRunRuleHandler : IRuleHandler
	{
		private readonly MovementController m_movementController;

		public NoWallRunRuleHandler(MovementController movementController)
		{
			this.m_movementController = movementController;
		}

		public void Apply()
		{
			var noWallRunRule = this.m_movementController.AddComponent<NoWallRunRule>();
		}

		public void Revert()
		{
			var noWallRunRule = this.m_movementController.GetComponent<NoWallRunRule>();
			if (noWallRunRule != null)
			{
				Object.Destroy(noWallRunRule);
			}
		}
	}
}

