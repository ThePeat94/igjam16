using UnityEngine;

namespace Nidavellir.Scriptables.Rules
{
	[CreateAssetMenu(menuName = "Data/Rule/Day Night", fileName = "Day Night Rule", order = 0)]
	public class DayNightRuleData : RuleData
	{
		[SerializeField]
		public GameObject NightVisualsGameObjectRefference;
	}
}