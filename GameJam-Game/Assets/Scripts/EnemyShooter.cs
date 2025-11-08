using System;
using UnityEngine;

namespace Nidavellir
{
    public class EnemyShooter : MonoBehaviour
    {
        [SerializeField] private int m_frameShootCooldown = 60;
        [SerializeField] private Projectile m_projectilePrefab;
        
        private int m_currentFrameCooldown;
        private Rigidbody2D m_rigidbody;
        
        public int ShootingFrequency
        {
            get => this.m_frameShootCooldown;
            set => this.m_frameShootCooldown = value;
        }

        private void Awake()
        {
            this.m_currentFrameCooldown = this.m_frameShootCooldown;
            this.m_rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (this.m_currentFrameCooldown > 0)
            {
                this.m_currentFrameCooldown--;
                if (this.m_currentFrameCooldown == 0)
                {
                    this.Shoot();
                }
            }
        }

        private void Shoot()
        {
            var projectile = Instantiate(this.m_projectilePrefab);
            projectile.transform.position = transform.position;
            
            Vector2 shootDirection = Vector2.left;
            if (this.m_rigidbody != null && Mathf.Abs(m_rigidbody.linearVelocity.x) > 0.1f)
            {
                shootDirection = new Vector2(Mathf.Sign(m_rigidbody.linearVelocity.x), 0);
            }
            
            projectile.Velocity = shootDirection * 5f;
            this.m_currentFrameCooldown = this.m_frameShootCooldown;
        }
    }
}