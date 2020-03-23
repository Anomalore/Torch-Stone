using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    private float moveInput;
    private Rigidbody rigidBody;
    private bool isFacingRight;
    [SerializeField]private bool isGrounded;
    [SerializeField] Transform groundCheck;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask whatIsGround;
    Animator animator;
    private float jumpTimeCounter;
    [SerializeField] private float jumpTime;
    [SerializeField] private bool isJumping;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        Physics.gravity = new Vector3(0, -27, 0);
    }

    void FixedUpdate()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        rigidBody.velocity = new Vector3(moveInput * speed, rigidBody.velocity.y, rigidBody.velocity.z);
        animator.SetFloat("Speed", Mathf.Abs(rigidBody.velocity.normalized.x));

        isGrounded = doGroundCheck();


        if (isGrounded == true && Input.GetKeyDown(KeyCode.Space))
        {
            
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, jumpForce, rigidBody.velocity.z);
        }

        if (Input.GetKey(KeyCode.Space) && isJumping == true)
        {
            if (jumpTimeCounter >= 0)
            {
                rigidBody.velocity = new Vector3(rigidBody.velocity.x, jumpForce, rigidBody.velocity.z);
                jumpTimeCounter -= Time.fixedDeltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpTimeCounter = 0;
            isJumping = false;
        }

        if (isFacingRight == false && moveInput < 0)
        {
            flip();
        }
        else if (isFacingRight == true && moveInput > 0)
        {
            flip();
        }
    }

    private bool doGroundCheck()
    {
        return Physics.CheckSphere(groundCheck.position, checkRadius, whatIsGround);
    }

    void flip()
    {
        isFacingRight = !isFacingRight;
        Quaternion Rotation = transform.rotation;
        Rotation.y *= -1;
        transform.rotation = Rotation;
    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }
}
