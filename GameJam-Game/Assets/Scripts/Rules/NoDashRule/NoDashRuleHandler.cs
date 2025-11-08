using Unity.VisualScripting;
using UnityEngine;

namespace Nidavellir.Rules.NoDashRule
{
	public class NoDashRuleHandler : IRuleHandler
	{
		private readonly MovementController m_movementController;

		public NoDashRuleHandler(MovementController movementController)
		{
			this.m_movementController = movementController;
		}

		public void Apply()
		{
			var noDashRule = this.m_movementController.AddComponent<Rules.NoDashRule.NoDashRule>();
		}

		public void Revert()
		{
			var noDashRule = this.m_movementController.GetComponent<Rules.NoDashRule.NoDashRule>();
			if (noDashRule != null)
			{
				Object.Destroy(noDashRule);
			}
		}
	}
}
