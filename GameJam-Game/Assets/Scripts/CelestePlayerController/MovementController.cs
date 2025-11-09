using System.Collections;
using DG.Tweening;
using Nidavellir;
using Nidavellir.Rules;
using UnityEngine;

public class MovementController : MonoBehaviour
{
	private Collision coll;

	[HideInInspector]
	public Rigidbody2D rb;

	private AnimationScript anim;

	[Space]
	[Header("Stats")]
	public float speed = 10;

	public float jumpForce = 50;
	public float slideSpeed = 5;
	public float wallJumpLerp = 10;
	public float dashSpeed = 40;
	public float dashDistance = 5f;
	public float gravityScale = 3;
	public float gravityMultiplier = 1f;

	[Space]
	[Header("Booleans")]
	public bool canMove;

	public bool wallGrab;
	public bool wallJumped;
	public bool wallSlide;
	public bool isDashing;

	[Space]
	private bool groundTouch;

	private bool hasDashed;

	public int side = 1;

	[Space]
	[Header("Polish")]
	public ParticleSystem dashParticle;

	public ParticleSystem jumpParticle;
	public ParticleSystem wallJumpParticle;
	public ParticleSystem slideParticle;

	private bool allowInput = true;
	private bool inverted = false;
	private bool toggled = false;
	private float originalSpeed;
	private Vector2 lastDirection = Vector2.zero;

	// Start is called before the first frame update
	void Start()
	{
		coll = GetComponent<Collision>();
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponentInChildren<AnimationScript>();

		var manager = FindFirstObjectByType<GameManager>();
		if (manager != null)
		{
			manager.OnGameOver += OnGameOver;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (!allowInput)
		{
			return;
		}

		Vector2 direction = GetInputDirection();
		Vector2 directionRaw = GetRawInputDirection();
		if (toggled)
		{
			direction = directionRaw;
		}
		lastDirection = directionRaw;

		Walk(direction);
		anim.SetHorizontalMovement(direction.x, direction.y, rb.linearVelocity.y);

		if (coll.onWall && Input.GetButton("Fire3") && canMove)
		{
			if (side != coll.wallSide)
				anim.Flip(side * -1);
			wallGrab = true;
			wallSlide = false;
		}

		if (Input.GetButtonUp("Fire3") || !coll.onWall || !canMove)
		{
			wallGrab = false;
			wallSlide = false;
		}

		if (coll.onGround && !isDashing)
		{
			wallJumped = false;
			GetComponent<BetterJumping>().enabled = true;
		}

		if (wallGrab && !isDashing)
		{
			rb.gravityScale = 0;
			if (direction.x > .2f || direction.x < -.2f)
				rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);

			float speedModifier = direction.y > 0 ? .5f : 1;

			rb.linearVelocity = new Vector2(rb.linearVelocity.x, direction.y * (speed * speedModifier));
		}
		else
		{
			rb.gravityScale = gravityScale * gravityMultiplier;
		}

		if (coll.onWall && !coll.onGround)
		{
			if (direction.x != 0 && !wallGrab)
			{
				wallSlide = true;
				WallSlide();
			}
		}

		if (!coll.onWall || coll.onGround)
			wallSlide = false;

		if (Input.GetButtonDown("Jump"))
		{
			anim.SetTrigger("jump");

			if (coll.onGround)
				Jump(Vector2.up, false);
			if (coll.onWall && !coll.onGround)
				WallJump();
		}

		if (Input.GetButtonDown("Fire1") && !hasDashed)
		{
			if (directionRaw.x != 0)
				Dash(directionRaw.x, 0);
		}

		if (coll.onGround && !groundTouch)
		{
			GroundTouch();
			groundTouch = true;
		}

		if (!coll.onGround && groundTouch)
		{
			groundTouch = false;
		}

		WallParticle(directionRaw.y);

		if (wallGrab || wallSlide || !canMove)
			return;

		if (directionRaw.x > 0)
		{
			side = 1;
			anim.Flip(side);
		}

		if (directionRaw.x < 0)
		{
			side = -1;
			anim.Flip(side);
		}
	}

	public void InvertControls(bool value)
	{
		inverted = value;
	}
	
	public void ToggleMode(bool value)
	{
		toggled = value;
		if (value)
		{
			originalSpeed = speed;
			speed *= 1.25f;
		}
		else
		{
			speed = originalSpeed;
		}
	}
	
	private Vector2 GetInputDirection()
	{
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");

		if (inverted)
		{
			return new Vector2(x * -1, y * -1);
		}

		return new Vector2(x, y);
	}

	private Vector2 GetRawInputDirection()
	{
		float x = Input.GetAxisRaw("Horizontal");
		float y = Input.GetAxisRaw("Vertical");

		if (inverted)
		{
			return new Vector2(x * -1, y * -1);
		}
		
		if (toggled && x == 0)
		{
			return lastDirection;
		}

		return new Vector2(x, y);
	}

	private void OnGameOver(bool win)
	{
		allowInput = false;
	}

	void GroundTouch()
	{
		hasDashed = false;
		isDashing = false;

		side = anim.sr.flipX ? -1 : 1;

		jumpParticle.Play();
	}

	private void Dash(float x, float y)
	{
		Camera.main.transform.DOComplete();
		Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
		FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));

		hasDashed = true;

		anim.SetTrigger("dash");

		rb.linearVelocity = Vector2.zero;
		Vector2 dir = new Vector2(x, y);

		// Start distance-based dash movement
		StartCoroutine(DashMovement(dir.normalized));
		StartCoroutine(DashWait());
	}

	IEnumerator DashWait()
	{
		StartCoroutine(GroundDash());
		DOVirtual.Float(14, 0, .8f, RigidbodyDrag);

		dashParticle.Play();
		rb.gravityScale = 0;
		GetComponent<BetterJumping>().enabled = false;
		wallJumped = true;
		isDashing = true;

		yield return new WaitForSeconds(.3f);

		dashParticle.Stop();
		rb.gravityScale = gravityScale * gravityMultiplier;
		GetComponent<BetterJumping>().enabled = true;
		wallJumped = false;
		isDashing = false;
	}

	IEnumerator DashMovement(Vector2 direction)
	{
		Vector3 startPosition = transform.position;
		Vector3 targetPosition = startPosition + (Vector3)(direction * dashDistance);
		float dashDuration = 0.3f;
		float elapsedTime = 0f;

		while (elapsedTime < dashDuration)
		{
			elapsedTime += Time.deltaTime;
			float progress = elapsedTime / dashDuration;
			
			float easedProgress = 1f - Mathf.Pow(1f - progress, 3f);
			
			Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, easedProgress);
			
			Vector3 deltaMovement = newPosition - transform.position;
			rb.MovePosition(transform.position + deltaMovement);
			
			yield return null;
		}
	}

	IEnumerator GroundDash()
	{
		yield return new WaitForSeconds(.15f);
		if (coll.onGround)
			hasDashed = false;
	}

	private void WallJump()
	{
		if ((side == 1 && coll.onRightWall) || side == -1 && !coll.onRightWall)
		{
			side *= -1;
			anim.Flip(side);
		}

		StopCoroutine(DisableMovement(0));
		StartCoroutine(DisableMovement(.1f));

		Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;

		Jump((Vector2.up / 1.5f + wallDir / 1.5f), true);

		wallJumped = true;
	}

	private void WallSlide()
	{
		if (coll.wallSide != side)
			anim.Flip(side * -1);

		if (!canMove)
			return;

		bool pushingWall = false;
		if ((rb.linearVelocity.x > 0 && coll.onRightWall) || (rb.linearVelocity.x < 0 && coll.onLeftWall))
		{
			pushingWall = true;
		}

		float push = pushingWall ? 0 : rb.linearVelocity.x;

		rb.linearVelocity = new Vector2(push, -slideSpeed);
	}

	private void Walk(Vector2 dir)
	{
		if (!canMove)
			return;

		if (wallGrab)
			return;

		if (!wallJumped)
		{
			rb.linearVelocity = new Vector2(dir.x * speed, rb.linearVelocity.y);
		}
		else
		{
			rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, (new Vector2(dir.x * speed, rb.linearVelocity.y)), wallJumpLerp * Time.deltaTime);
		}
	}

	private void Jump(Vector2 dir, bool wall)
	{
		slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
		ParticleSystem particle = wall ? wallJumpParticle : jumpParticle;

		rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
		rb.linearVelocity += dir * jumpForce;

		particle.Play();
	}

	IEnumerator DisableMovement(float time)
	{
		canMove = false;
		yield return new WaitForSeconds(time);
		canMove = true;
	}

	void RigidbodyDrag(float x)
	{
		rb.linearDamping = x;
	}

	void WallParticle(float vertical)
	{
		var main = slideParticle.main;

		if (wallSlide || (wallGrab && vertical < 0))
		{
			slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
			main.startColor = Color.white;
		}
		else
		{
			main.startColor = Color.clear;
		}
	}

	int ParticleSide()
	{
		int particleSide = coll.onRightWall ? 1 : -1;
		return particleSide;
	}
}