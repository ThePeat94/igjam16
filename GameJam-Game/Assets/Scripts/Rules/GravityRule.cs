using System;
using UnityEngine;

namespace Nidavellir.Rules
{
    public class GravityRule : MonoBehaviour
    {
        private Rigidbody2D m_rigidbody2D;
        private float m_originalGravityScale;

        public float GravityScale
        {
            get;
            set;
        }

        private void Awake()
        {
            this.m_rigidbody2D = this.GetComponent<Rigidbody2D>();
            if (this.m_rigidbody2D != null)
            {
                this.m_originalGravityScale = this.m_rigidbody2D.gravityScale;
            }
        }

        private void OnEnable()
        {
            if (this.m_rigidbody2D != null)
            {
                this.m_rigidbody2D.gravityScale = this.GravityScale;
            }
        }

        private void OnDisable()
        {
            if (this.m_rigidbody2D != null)
            {
                this.m_rigidbody2D.gravityScale = this.m_originalGravityScale;
            }
        }
    }
}