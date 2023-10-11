using System.Collections;
using System.Collections.Generic;
//using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;

    //Jump
    public Transform groundCheck;
    public LayerMask groundLayer;
    private float lastGroundedTime;
    private float jumpCoyoteTime = 0.2f;
    //Wall Jump

    public Transform wallCheck;
    [SerializeField] public LayerMask wallLayer;

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

    
    void Start()
    {
    }
    void Update()
    {
        transform.rotation = Quaternion.identity;

        //Move

        float targetSpeed = horizontal * speed;
        float speedDif = targetSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(speedDif) > 0.01f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

        if (WallCheck() && !IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.8f);
            if (direction == horizontal)
            {
                rb.AddForce(movement * Vector2.right);

            }
            return;
        }
        if (isDashing)
        {
            return;
        }
        if (IsGrounded())
        {
            lastGroundedTime = jumpCoyoteTime;
        }
        else
        {
            lastGroundedTime -= Time.deltaTime;
        }

        rb.AddForce(movement * Vector2.right,ForceMode2D.Force);

        JumpGravity();
        Flip();
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log(Input.mousePosition);
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * DashPower, 0);
        dashTrail.emitting = true;
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        dashTrail.emitting = false;
        yield return new WaitForSeconds(DashCooldown);
        canDash = true;

    }
    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
            StartCoroutine(Dash());
        }
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (lastGroundedTime > 0 && context.performed)
        {
            rb.AddForce(Vector2.up * JumpingPower, ForceMode2D.Impulse);
        }
        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2 (rb.velocity.x, rb.velocity.y * 0.5f);
            lastGroundedTime = 0f;
        }
    }
    private void JumpGravity()
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
