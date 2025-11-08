using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol")] public Transform leftPoint;
    public Transform rightPoint;
    public float moveSpeed = 2f;
    public float waitSeconds = 2f;
    public bool startGoingRight = true;

    [Header("Player detection")] public string playerTag = "Player";
    public float sightRange = 5f; // horizontal range to spot the player
    public float verticalSightTolerance = 2f; // vertical tolerance

    [Header("Attack (single Attack+Stun clip)")]
    public float attackCooldown = 1.0f; // time after action before we can attack again

    public float lungeSpeed = 6f; // horizontal push during striking frames
    public Vector2 hitboxOffset = new Vector2(0.6f, 0f);
    public Vector2 hitboxSize = new Vector2(0.9f, 0.8f);
    public int damage = 1;

    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer sr;

    Transform target;
    Transform player;

    bool waiting;
    
    bool inAction = false;
    bool hitActive = false;
    bool didHit = false;
    float lastAttackTime = -999f;
    float attackDir = 1f;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        
        if (!leftPoint) leftPoint = transform.Find("LeftPoint");
        if (!rightPoint) rightPoint = transform.Find("RightPoint");
        
        if (leftPoint && leftPoint.IsChildOf(transform)) leftPoint.SetParent(null, true);
        if (rightPoint && rightPoint.IsChildOf(transform)) rightPoint.SetParent(null, true);
        
        var pGo = GameObject.FindGameObjectWithTag(playerTag);
        if (pGo) player = pGo.transform;
    }

    void Start()
    {
        rb.freezeRotation = true;
        target = startGoingRight ? rightPoint : leftPoint;
    }

    void Update()
    {
        if (inAction)
        {
            if (hitActive) DoAttackHit(); 
            anim.SetBool("IsMoving", false);
            return;
        }
        
        if (CanSeePlayer() && Time.time >= lastAttackTime + attackCooldown)
        {
            StartAttack();
            return;
        }
        
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
        bool reachedLeft = dir < 0 && transform.position.x <= target.position.x + 0.02f;
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

    bool CanSeePlayer()
    {
        if (!player) return false;
        float dx = Mathf.Abs(player.position.x - transform.position.x);
        float dy = Mathf.Abs(player.position.y - transform.position.y);
        return dx <= sightRange && dy <= verticalSightTolerance;
    }

    void StartAttack()
    {
        inAction = true;
        waiting = false;
        didHit = false;
        
        if (player)
        {
            attackDir = Mathf.Sign(player.position.x - transform.position.x);
            if (sr) sr.flipX = attackDir > 0f;
        }
        else
        {
            attackDir = transform.localScale.x >= 0 ? 1f : -1f;
        }

        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        anim.SetBool("IsMoving", false);
        
        anim.ResetTrigger("Attack");
        anim.SetTrigger("Attack");
    }
    
    public void AE_AttackStart()
    {
        hitActive = true;
        didHit = false;
        rb.linearVelocity = new Vector2(attackDir * lungeSpeed, rb.linearVelocity.y);
    }

    public void AE_AttackEnd()
    {
        hitActive = false;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }
    
    public void AE_ActionDone()
    {
        inAction = false;
        hitActive = false;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        lastAttackTime = Time.time; 
    }

    void DoAttackHit()
    {
        Vector2 center = (Vector2)transform.position +
                         new Vector2(Mathf.Sign(attackDir) * Mathf.Abs(hitboxOffset.x), hitboxOffset.y);

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, hitboxSize, 0f);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] != null && hits[i].CompareTag(playerTag))
            {
                if (!didHit)
                {
                    hits[i].SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
                    didHit = true;
                }

                break;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        
        Gizmos.color = Color.red;
        Vector2 rightCenter = (Vector2)transform.position + new Vector2(Mathf.Abs(hitboxOffset.x), hitboxOffset.y);
        Vector2 leftCenter = (Vector2)transform.position + new Vector2(-Mathf.Abs(hitboxOffset.x), hitboxOffset.y);
        Gizmos.DrawWireCube(rightCenter, hitboxSize);
        Gizmos.DrawWireCube(leftCenter, hitboxSize);
    }
}