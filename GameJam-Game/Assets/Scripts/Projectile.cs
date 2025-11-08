using System;
using UnityEngine;

namespace Nidavellir
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float m_damage;
        [SerializeField] private Rigidbody2D m_rigidbody;
        
        public Vector2 Velocity { get; set; }

        private void Awake()
        {
            this.m_rigidbody ??= this.GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            Destroy(this.gameObject, 5f);
        }

        private void FixedUpdate()
        {
            this.m_rigidbody.linearVelocity = this.Velocity;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.GetComponent<PlayerController>()) 
                return;
            var healthController = other.GetComponent<HealthController>();
            healthController?.ProcessDamage(this.m_damage);
            Destroy(this.gameObject);
        }
    }
}