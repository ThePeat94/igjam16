using Unity.VisualScripting;
using UnityEngine;

namespace Nidavellir.Rules.NoJump
{
	public class NoJumpRuleHandler : IRuleHandler
	{
		private readonly MovementController m_movementController;

		public NoJumpRuleHandler(MovementController movementController)
		{
			this.m_movementController = movementController;
		}

		public void Apply()
		{
			var noJumpRule = this.m_movementController.AddComponent<NoJumpRule>();
		}

		public void Revert()
		{
			var noJumpRule = this.m_movementController.GetComponent<NoJumpRule>();
			if (noJumpRule != null)
			{
				Object.Destroy(noJumpRule);
			}
		}
	}
}