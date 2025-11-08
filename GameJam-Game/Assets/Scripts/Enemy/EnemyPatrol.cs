using UnityEngine;

namespace Nidavellir.Enemy
{
    [RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
    public class EnemyPatrol : MonoBehaviour
    {
        public Transform leftPoint;
        public Transform rightPoint;
        public float moveSpeed = 2f;
        public float waitSeconds = 2f;
        public bool startGoingRight = true;

        Animator anim;
        Rigidbody2D rb;
        SpriteRenderer sr;
        Transform target;
        bool waiting;

        void Awake()
        {
            this.anim = this.GetComponent<Animator>();
            this.rb   = this.GetComponent<Rigidbody2D>();
            this.sr   = this.GetComponent<SpriteRenderer>();
        }

        void Start()
        {
            this.rb.freezeRotation = true;
            this.target = this.startGoingRight ? this.rightPoint : this.leftPoint;
        }

        void Update()
        {
            if (this.waiting)
            {
                this.rb.linearVelocity = new Vector2(0, this.rb.linearVelocity.y);
                this.anim.SetBool("IsMoving", false);
                return;
            }

            float dir = Mathf.Sign(this.target.position.x - this.transform.position.x);
            this.rb.linearVelocity = new Vector2(dir * this.moveSpeed, this.rb.linearVelocity.y);
        
            this.anim.SetBool("IsMoving", Mathf.Abs(this.rb.linearVelocity.x) > 0.01f);
        
            if (this.sr) this.sr.flipX = dir > 0f;
        
            bool reachedRight = dir > 0 && this.transform.position.x >= this.target.position.x - 0.02f;
            bool reachedLeft  = dir < 0 && this.transform.position.x <= this.target.position.x + 0.02f;
            if (reachedRight || reachedLeft)
                this.StartCoroutine(this.WaitAndSwap());
        }

        System.Collections.IEnumerator WaitAndSwap()
        {
            this.waiting = true;
            this.rb.linearVelocity = new Vector2(0, this.rb.linearVelocity.y);
            this.anim.SetBool("IsMoving", false);
            yield return new WaitForSeconds(this.waitSeconds);
            this.waiting = false;
            this.target = (this.target == this.rightPoint) ? this.leftPoint : this.rightPoint;
        }
    }
}