using UnityEngine;

namespace Nidavellir.Rules
{
	public class GravityRule : MonoBehaviour
	{
		private MovementOldInput m_movementOldInput;
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
			m_movementOldInput = GetComponent<MovementOldInput>();
			if (m_movementOldInput != null)
			{
				m_originalGravityMultiplier = m_movementOldInput.gravityMultiplier;
			}
		}

		private void OnEnable()
		{
			ApplyRule();
		}

		private void ApplyRule()
		{
			if (m_movementOldInput != null)
			{
				m_movementOldInput.gravityMultiplier = GravityMultiplier;
			}
		}

		private void OnDisable()
		{
			if (m_movementOldInput != null)
			{
				m_movementOldInput.gravityMultiplier = m_originalGravityMultiplier;
			}
		}
	}
}