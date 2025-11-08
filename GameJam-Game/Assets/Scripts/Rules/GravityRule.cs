using System;
using UnityEngine;

namespace Nidavellir.Rules
{
    public class GravityRule : MonoBehaviour
    {
        private CharacterController m_characterController;
        
        public float GravityScale
        {
            get;
            set;
        }

        private void Awake()
        {
            this.m_characterController = this.GetComponent<CharacterController>();
        }

        private void FixedUpdate()
        {
            Vector3 gravity = Vector3.up * (this.GravityScale);
            this.m_characterController.Move(gravity);
        }
    }
}