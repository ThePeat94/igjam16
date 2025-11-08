using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Nidavellir;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol")] public Transform leftPoint;
    public Transform rightPoint;
    public float moveSpeed = 2f;
    public float waitSeconds = 2f;
    public bool startGoingRight = true;

    [Header("Player detection")] public string playerTag = "Player";
    public float sightRange = 5f;
    public float verticalSightTolerance = 2f;

    [Header("Attack (single Attack+Stun clip)")]
    public float attackCooldown = 1.0f;

    public float lungeSpeed = 6f;
    public Vector2 hitboxOffset = new Vector2(0.6f, 0f);
    public Vector2 hitboxSize = new Vector2(0.9f, 0.8f);
    public int damage = 1;

    [Header("Debug")] public bool debugLogs = true;

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private Transform target;
    private Transform playerTr;
    private HealthController playerHC;
    private HashSet<Collider2D> playerColliders = new HashSet<Collider2D>();

    private bool waiting;
    private bool inAction = false;
    private bool hitActive = false;
    private bool didHit = false;
    private float lastAttackTime = -999f;
    private float attackDir = 1f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (!leftPoint) leftPoint = transform.Find("LeftPoint");
        if (!rightPoint) rightPoint = transform.Find("RightPoint");

        if (leftPoint && leftPoint.IsChildOf(transform)) leftPoint.SetParent(null, true);
        if (rightPoint && rightPoint.IsChildOf(transform)) rightPoint.SetParent(null, true);

        FindAndCachePlayer();
    }

    private void Start()
    {
        rb.freezeRotation = true;
        target = startGoingRight ? rightPoint : leftPoint;
    }

    private void Update()
    {
        // Safety: re-acquire player if lost (eg. on scene reload)
        if (playerHC == null || playerTr == null || playerColliders.Count == 0)
            FindAndCachePlayer();

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

    private IEnumerator WaitAndSwap()
    {
        waiting = true;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        anim.SetBool("IsMoving", false);
        yield return new WaitForSeconds(waitSeconds);
        waiting = false;
        target = (target == rightPoint) ? leftPoint : rightPoint;
    }

    private bool CanSeePlayer()
    {
        if (!playerTr) return false;
        float dx = Mathf.Abs(playerTr.position.x - transform.position.x);
        float dy = Mathf.Abs(playerTr.position.y - transform.position.y);
        return dx <= sightRange && dy <= verticalSightTolerance;
    }

    private void StartAttack()
    {
        inAction = true;
        waiting = false;
        didHit = false;

        if (playerTr)
        {
            attackDir = Mathf.Sign(playerTr.position.x - transform.position.x);
            if (sr) sr.flipX = attackDir > 0f;
        }
        else
        {
            attackDir = transform.localScale.x >= 0 ? 1f : -1f;
        }

        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        anim.SetBool("IsMoving", false);

        if (debugLogs) Debug.Log("Enemy: StartAttack", this);

        anim.ResetTrigger("Attack");
        anim.SetTrigger("Attack");

        AE_AttackStart();
        Invoke(nameof(AE_AttackEnd), 0.18f);
        Invoke(nameof(AE_ActionDone), 0.35f);
    }

    public void AE_AttackStart()
    {
        if (debugLogs) Debug.Log("Enemy: AE_AttackStart (hit active)", this);
        hitActive = true;
        didHit = false;
        rb.linearVelocity = new Vector2(attackDir * lungeSpeed, rb.linearVelocity.y);
    }

    public void AE_AttackEnd()
    {
        if (debugLogs) Debug.Log("Enemy: AE_AttackEnd (hit inactive)", this);
        hitActive = false;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    public void AE_ActionDone()
    {
        if (debugLogs) Debug.Log("Enemy: AE_ActionDone", this);
        inAction = false;
        hitActive = false;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        lastAttackTime = Time.time;
    }

    private void DoAttackHit()
    {
        if (playerHC == null || playerColliders.Count == 0)
        {
            if (debugLogs) Debug.LogWarning("Enemy: No player cached, cannot deal damage.", this);
            return;
        }

        Vector2 center = (Vector2)transform.position +
                         new Vector2(Mathf.Sign(attackDir) * Mathf.Abs(hitboxOffset.x), hitboxOffset.y);

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, hitboxSize, 0f);
        if (debugLogs)
            Debug.Log($"Enemy: Hit check -> {hits.Length} colliders in box at {center} size {hitboxSize}", this);

        Collider2D playerHit = null;
        for (int i = 0; i < hits.Length; i++)
        {
            var col = hits[i];
            if (col == null) continue;
            if (playerColliders.Contains(col))
            {
                playerHit = col;
                break;
            }
        }

        if (playerHit == null)
        {
            if (debugLogs && hits.Length > 0)
            {
                foreach (var c in hits)
                {
                    Debug.Log($"Enemy: Overlap saw [{c.name}] tag [{c.tag}]", c);
                }
            }

            return;
        }

        float dmg = Mathf.Clamp(damage, 0f, playerHC.CurrentHealth);
        if (dmg > 0f)
        {
            playerHC.DamageMode = DamageMode.Damage;
            playerHC.ProcessDamage(dmg);
            didHit = true;

            if (debugLogs)
                Debug.Log(
                    $"Enemy: Damaged player {playerHC.name} for {dmg}. New HP: {playerHC.CurrentHealth}/{playerHC.MaxHealth}",
                    this);
        }
    }

    private void FindAndCachePlayer()
    {
        playerColliders.Clear();
        playerHC = null;
        playerTr = null;

        GameObject pGo = null;

        var tagged = GameObject.FindGameObjectWithTag(playerTag);
        if (tagged) pGo = tagged;

        if (!pGo && PlayerController.Instance != null)
            pGo = PlayerController.Instance.gameObject;

        if (!pGo)
        {
            if (debugLogs) Debug.LogWarning("Enemy: No player found (tag or PlayerController.Instance).", this);
            return;
        }

        playerTr = pGo.transform;
        playerHC = pGo.GetComponentInParent<HealthController>();
        if (!playerHC)
        {
            if (debugLogs) Debug.LogWarning($"Enemy: Player [{pGo.name}] has no HealthController.", pGo);
        }

        var cols = pGo.GetComponentsInChildren<Collider2D>(true);
        foreach (var c in cols)
            playerColliders.Add(c);

        if (debugLogs)
            Debug.Log(
                $"Enemy: Cached player [{pGo.name}] — HC {(playerHC ? "OK" : "MISSING")}, colliders {playerColliders.Count}",
                pGo);
    }

    private void OnDrawGizmosSelected()
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