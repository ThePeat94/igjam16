using System.Collections.Generic;
using System.Linq;
using Nidavellir.Rules.FixedSpeed;
using Nidavellir.Rules.Gravity;
using Nidavellir.Rules.InvertControls;
using Nidavellir.Rules.NoDashRule;
using Nidavellir.Rules.NoJump;
using Nidavellir.Rules.NoWallRun;
using Nidavellir.Scriptables.Rules;
using UnityEngine;

namespace Nidavellir.Rules
{
	public class RuleHandlerFactory : MonoBehaviour
	{
		[SerializeField]
		private MovementOldInput m_movementOldInput;

		[SerializeField]
		private HealthController m_playerHealthController;

		[SerializeField]
		private GameObject m_nightVisualsGameObject;

		private List<EnemyShooter> m_enemyShooters;

		private void Awake()
		{
			m_enemyShooters = FindObjectsByType<EnemyShooter>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
		}

		public IRuleHandler CreateRuleHandler(RuleData ruleData)
		{
			if (ruleData is DayNightRuleData dayNightRuleData)
			{
				dayNightRuleData.NightVisualsGameObjectRefference = m_nightVisualsGameObject;
			}

			return ruleData switch
			{
				GravityRuleData gravityRuleData => new GravityRuleHandler(m_movementOldInput, gravityRuleData),
				InvertDamageRuleData => new InvertDamageRuleHandler(m_playerHealthController),
				EnemyShootingFrequencyRuleData => new EnemyShootingFrequencyRuleHandler(m_enemyShooters),
				NoJumpRuleData => new NoJumpRuleHandler(m_movementOldInput),
				NoDashRuleData => new NoDashRuleHandler(m_movementOldInput),
				NoWallRunRuleData => new NoWallRunRuleHandler(m_movementOldInput),
				InvertControlsRuleData => new InvertControlsRuleHandler(m_movementOldInput),
				FixedSpeedRuleData => new FixedSpeedRuleHandler(m_movementOldInput),
				DayNightRuleData => new NightRuleHandler(ruleData),
				_ => new NoopRuleHandler(ruleData),
			};
		}
	}
}