using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BetterJumping : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private bool jumpHeld;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    void Awake()
    {
        playerInput = new PlayerInput();
    }

    void OnEnable()
    {
        playerInput.Enable();
        playerInput.Actions.Jump.performed += OnJumpPressed;
        playerInput.Actions.Jump.canceled += OnJumpReleased;
    }

    void OnDisable()
    {
        playerInput.Actions.Jump.performed -= OnJumpPressed;
        playerInput.Actions.Jump.canceled -= OnJumpReleased;
        playerInput.Disable();
    }

    void OnJumpPressed(InputAction.CallbackContext context)
    {
        jumpHeld = true;
    }

    void OnJumpReleased(InputAction.CallbackContext context)
    {
        jumpHeld = false;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }else if(rb.linearVelocity.y > 0 && !jumpHeld)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
