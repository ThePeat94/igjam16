using Unity.VisualScripting;
using UnityEngine;

namespace Nidavellir.Rules.NoDashRule
{
	public class NoDashRuleHandler : IRuleHandler
	{
		private readonly MovementOldInput m_movementOldInput;

		public NoDashRuleHandler(MovementOldInput movementOldInput)
		{
			this.m_movementOldInput = movementOldInput;
		}

		public void Apply()
		{
			var noDashRule = this.m_movementOldInput.AddComponent<Rules.NoDashRule.NoDashRule>();
		}

		public void Revert()
		{
			var noDashRule = this.m_movementOldInput.GetComponent<Rules.NoDashRule.NoDashRule>();
			if (noDashRule != null)
			{
				Object.Destroy(noDashRule);
			}
		}
	}
}
