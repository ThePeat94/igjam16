using Nidavellir.Input;
using Nidavellir.Scriptables;
using UnityEngine;

namespace Nidavellir
{
    public class PlayerController : MonoBehaviour
    {
        private static PlayerController s_instance;
        private static readonly int s_isWalkingHash = Animator.StringToHash("IsWalking");

        [SerializeField] private PlayerData m_playerData;

        private Vector2 m_moveDirection;
        private InputProcessor m_inputProcessor;
        private Rigidbody2D m_rigidbody;
        private Animator m_animator;


        public static PlayerController Instance => s_instance;


        private void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this;
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }

            this.m_inputProcessor = this.GetComponent<InputProcessor>();
            this.m_animator = this.GetComponent<Animator>();
            this.m_rigidbody = this.GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            this.m_moveDirection = this.m_inputProcessor.Movement;
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void LateUpdate()
        {
            this.UpdateAnimator();
        }

        protected void Move()
        {
            m_rigidbody.linearVelocity = new Vector2(m_moveDirection.x * m_playerData.MovementSpeed, m_rigidbody.linearVelocity.y);
        }

        private void UpdateAnimator()
        {
            if (m_animator == null || !m_animator.enabled)
            {
                return;
            }

            //this.m_animator.SetBool(s_isWalkingHash, this.m_moveDirection.magnitude > 0.01f);
        }


    }
}
