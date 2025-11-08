using System;
using UnityEngine;

namespace Nidavellir
{
    public class EnemyShooter : MonoBehaviour
    {
        [SerializeField] private int m_frameShootCooldown = 60;
        [SerializeField] private Projectile m_projectilePrefab;
        
        private int m_currentFrameCooldown;
        
        public int ShootingFrequency
        {
            get => this.m_frameShootCooldown;
            set => this.m_frameShootCooldown = value;
        }

        private void Awake()
        {
            this.m_currentFrameCooldown = this.m_frameShootCooldown;
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
            projectile.transform.position = this.transform.position;
            projectile.Velocity = Vector2.left * 5f;
            this.m_currentFrameCooldown = this.m_frameShootCooldown;
        }
    }
}