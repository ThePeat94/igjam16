using Unity.VisualScripting;
using UnityEngine;

namespace Nidavellir.Rules.NoJump
{
	public class NoJumpRuleHandler : IRuleHandler
	{
		private readonly MovementOldInput m_movementOldInput;

		public NoJumpRuleHandler(MovementOldInput movementOldInput)
		{
			this.m_movementOldInput = movementOldInput;
		}

		public void Apply()
		{
			var noJumpRule = this.m_movementOldInput.AddComponent<NoJumpRule>();
		}

		public void Revert()
		{
			var noJumpRule = this.m_movementOldInput.GetComponent<NoJumpRule>();
			if (noJumpRule != null)
			{
				Object.Destroy(noJumpRule);
			}
		}
	}
}