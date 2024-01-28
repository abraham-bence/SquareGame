using System.Collections;
using System.Collections.Generic;
//using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement main;
    [SerializeField] PlayerInput controls;
    public Rigidbody2D rb;

    //Jump
    public Transform groundCheck;
    public LayerMask groundLayer;
    private float lastGroundedTime;
    private float jumpCoyoteTime = 0.2f;
    //Wall Slide
    public Transform wallCheck;
    [SerializeField] public LayerMask wallLayer;
    private bool isWallSliding;
    [SerializeField] float wallSlidingSpeed = 2f;

    //Wall jump
    private bool isWallJumping;
    private float wallJumoingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJUmpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(12f, 24f);

    //Jump
    public float speed;
    public float JumpingPower;

    public float gravityScaleMultiplier = 2f;
    public float gravityScale = 5f;


    //dash
    public float DashPower;
    public float DashCooldown;
    private bool isDashing;
    private bool canDash = true;
    private float dashTime = 0.1f;
    public TrailRenderer dashTrail;

    private float horizontal;
    private bool isFacingRight = true;
    private float direction;

    public float acceleration = 7f;
    public float decceleration = 7f;
    public float velPower = 1f;

    public float rotationSpeed;

    public float cameraYPos = 0;

    private void Awake()
    {
        main = this;
        controls.ActivateInput();
    }

    void Update()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = gravityScale * gravityScaleMultiplier;
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
    }

    private void WallJump()
    {
        if(isWallSliding)
        {
            Debug.Log("sliding");
            isWallJumping = false;
            wallJumoingDirection = -transform.localScale.x;
            wallJUmpingCounter = wallJumpingTime;
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJUmpingCounter -= Time.deltaTime;
        }
    }
    private void StopWallJumping()
    {
        isWallJumping = false;
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(wallCheck.position, 0.2f);
    //}

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    private bool WallCheck()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if(WallCheck() && !IsGrounded())
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            direction = localScale.x * -1f;
            transform.localScale = localScale;
        }
    }
    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }
}
