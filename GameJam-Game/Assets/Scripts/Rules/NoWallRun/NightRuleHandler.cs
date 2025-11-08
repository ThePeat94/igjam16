using Nidavellir.Scriptables.Rules;
using UnityEngine;

namespace Nidavellir.Rules
{
	public class NightRuleHandler : IRuleHandler
	{
		private readonly DayNightRuleData m_ruleData;

		public NightRuleHandler(RuleData ruleData)
		{
			m_ruleData = (DayNightRuleData)ruleData;
		
		}

		public void Apply()
		{
			m_ruleData.NightVisualsGameObjectRefference.SetActive(true);
		}

		public void Revert()
		{
			m_ruleData.NightVisualsGameObjectRefference.SetActive(false);
		}
	}
}