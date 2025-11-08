using Unity.VisualScripting;
using UnityEngine;

namespace Nidavellir.Rules
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
			var noDashRule = this.m_movementOldInput.AddComponent<NoDashRule>();
		}

		public void Revert()
		{
			var noDashRule = this.m_movementOldInput.GetComponent<NoDashRule>();
			if (noDashRule != null)
			{
				Object.Destroy(noDashRule);
			}
		}
	}
}
