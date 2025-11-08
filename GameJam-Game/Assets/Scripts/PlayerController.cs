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
                Destroy(gameObject);
                return;
            }

            m_inputProcessor = GetComponent<InputProcessor>();
            m_animator = GetComponent<Animator>();
            m_rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            m_moveDirection = m_inputProcessor.Movement;
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void LateUpdate()
        {
            UpdateAnimator();
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
