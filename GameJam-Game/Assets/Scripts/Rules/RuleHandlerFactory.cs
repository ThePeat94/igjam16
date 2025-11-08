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
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Serialization;

namespace Nidavellir.Rules
{
	public class RuleHandlerFactory : MonoBehaviour
	{
		[FormerlySerializedAs("m_movementOldInput")] [SerializeField]
		private MovementController movementController;

		[SerializeField]
		private HealthController m_playerHealthController;

		[SerializeField]
		private GameObject m_nightVisualsGameObject;

		private List<EnemyShooter> m_enemyShooters;

		private void Awake()
		{
			m_enemyShooters = FindObjectsByType<EnemyShooter>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
			if (!m_nightVisualsGameObject)
			{
				m_nightVisualsGameObject = FindFirstObjectByType<PostProcessVolume>(FindObjectsInactive.Include)?.gameObject;
			}
		}

		public IRuleHandler CreateRuleHandler(RuleData ruleData)
		{
			if (ruleData is DayNightRuleData dayNightRuleData)
			{
				dayNightRuleData.NightVisualsGameObjectRefference = m_nightVisualsGameObject;
			}

			return ruleData switch
			{
				GravityRuleData gravityRuleData => new GravityRuleHandler(movementController, gravityRuleData),
				InvertDamageRuleData => new InvertDamageRuleHandler(m_playerHealthController),
				EnemyShootingFrequencyRuleData => new EnemyShootingFrequencyRuleHandler(m_enemyShooters),
				NoJumpRuleData => new NoJumpRuleHandler(movementController),
				NoDashRuleData => new NoDashRuleHandler(movementController),
				NoWallRunRuleData => new NoWallRunRuleHandler(movementController),
				InvertControlsRuleData => new InvertControlsRuleHandler(movementController),
				FixedSpeedRuleData => new FixedSpeedRuleHandler(movementController),
				DayNightRuleData => new NightRuleHandler(ruleData),
				_ => new NoopRuleHandler(ruleData),
			};
		}
	}
}