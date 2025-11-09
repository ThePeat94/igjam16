using UnityEngine;

namespace Nidavellir
{
	public class Projectile : MonoBehaviour
	{
		[SerializeField]
		private float m_damage;

		[SerializeField]
		private Rigidbody2D m_rigidbody;

		public Vector2 Velocity { get; set; }

		private void Awake()
		{
			m_rigidbody ??= GetComponent<Rigidbody2D>();
		}

		private void Start()
		{
			Destroy(gameObject, 5f);
		}

		private void FixedUpdate()
		{
			m_rigidbody.linearVelocity = Velocity;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!other.GetComponent<MovementController>())
				return;
			var healthController = other.GetComponent<HealthController>();
			healthController?.ProcessDamage(m_damage);
			Destroy(gameObject);
		}
	}
}