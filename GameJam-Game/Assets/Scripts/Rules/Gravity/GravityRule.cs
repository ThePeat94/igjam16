using UnityEngine;

namespace Nidavellir.Rules.Gravity
{
	public class GravityRule : MonoBehaviour
	{
		private MovementController m_movementController;
		private float m_originalGravityMultiplier;

		public float _gravityMultiplier = 1;

		public float GravityMultiplier
		{
			get => _gravityMultiplier;
			set
			{
				_gravityMultiplier = value;
				ApplyRule();
			}
		}

		private void Awake()
		{
			m_movementController = GetComponent<MovementController>();
			if (m_movementController != null)
			{
				m_originalGravityMultiplier = m_movementController.gravityMultiplier;
			}
		}

		private void OnEnable()
		{
			ApplyRule();
		}

		private void ApplyRule()
		{
			if (m_movementController != null)
			{
				m_movementController.gravityMultiplier = GravityMultiplier;
			}
		}

		private void OnDisable()
		{
			if (m_movementController != null)
			{
				m_movementController.gravityMultiplier = m_originalGravityMultiplier;
			}
		}
	}
}