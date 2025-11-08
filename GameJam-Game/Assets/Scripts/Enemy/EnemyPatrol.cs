using UnityEngine;
using System.Collections;

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
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (!leftPoint)  leftPoint  = transform.Find("LeftPoint");
        if (!rightPoint) rightPoint = transform.Find("RightPoint");

        if (leftPoint && leftPoint.IsChildOf(transform))  leftPoint.SetParent(null, true);
        if (rightPoint && rightPoint.IsChildOf(transform)) rightPoint.SetParent(null, true);
    }

    void Start()
    {
        rb.freezeRotation = true;

        // If points are inside the prefab, unparent them so they don't move with the enemy.
        if (leftPoint && leftPoint.IsChildOf(transform))  leftPoint.SetParent(null, true);   // keep world pos
        if (rightPoint && rightPoint.IsChildOf(transform)) rightPoint.SetParent(null, true); // keep world pos

        target = startGoingRight ? rightPoint : leftPoint;
    }

    void Update()
    {
        if (waiting)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            anim.SetBool("IsMoving", false);
            return;
        }

        float dir = Mathf.Sign(target.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);

        anim.SetBool("IsMoving", Mathf.Abs(rb.linearVelocity.x) > 0.01f);
        if (sr) sr.flipX = dir > 0f;

        bool reachedRight = dir > 0 && transform.position.x >= target.position.x - 0.02f;
        bool reachedLeft  = dir < 0 && transform.position.x <= target.position.x + 0.02f;
        if (reachedRight || reachedLeft)
            StartCoroutine(WaitAndSwap());
    }

    IEnumerator WaitAndSwap()
    {
        waiting = true;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        anim.SetBool("IsMoving", false);
        yield return new WaitForSeconds(waitSeconds);
        waiting = false;
        target = (target == rightPoint) ? leftPoint : rightPoint;
    }
}