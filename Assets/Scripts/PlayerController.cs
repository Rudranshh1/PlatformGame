using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchDirection))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float airSpeed = 3f;
    Vector2 moveInput;
    TouchDirection touchdirection;
    Rigidbody2D rb;
    public float jumpImpulse = 10;
    [SerializeField]
    private bool _isMoving = false;
    [SerializeField]
    private bool _isRunning = false;
    [SerializeField]
    private bool _isJumping = false;
    public bool isMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value; animator.SetBool(AnimationString.onMoving, value);
        }
    }
    public bool isRunnig
    {
        get
        {
            return _isRunning;
        }
        set
        {
            _isRunning = value;
            animator.SetBool(AnimationString.isRunning, value);
        }
    }
    public bool canMove
    {
        get
        {
          return animator.GetBool(AnimationString.canMove);
        }
    }
    public float CurrentMovingSpeed
    {
        get
        {
            if(canMove)
            {
                if (isMoving && !touchdirection.IsOnWall)
                {
                    if (touchdirection.IsGrounded)
                    {
                        if (isRunnig)
                            return runSpeed;
                        else
                            return walkSpeed;
                    }
                    else
                    {
                        // air speed
                        return airSpeed;
                    }


                }
                else
                    return 0;

            }
            else
            {
                return 0;
            }

            
        }
    }
            

    public bool isFacingRight = true;

    Animator animator;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchdirection = GetComponent<TouchDirection>();
    }
    

    // Update is called once per frame
    void Update()
    {
        float fps = 1f / Time.deltaTime;
        //Debug.Log("FPS: " + fps);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x*CurrentMovingSpeed,rb.velocity.y);
        animator.SetFloat(AnimationString.yVelocity, rb.velocity.y);      
    }
    public void onMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        isMoving = moveInput != Vector2.zero;
        setFaceDirection(moveInput);
    }

    private void setFaceDirection(Vector2 moveInput)
    {
        if(moveInput.x>0 && !isFacingRight)
        {
            // face to right
            isFacingRight = true;
            transform.localScale *= new Vector2(-1, 1);
        }
        else if(moveInput.x<0 && isFacingRight)
        {
            //face to left
            isFacingRight = false;
            transform.localScale *= new Vector2(-1, 1);
        }
    }

    public void onJump(InputAction.CallbackContext context)
    {
        // todo check as alive as well
        if(context.started && touchdirection.IsGrounded && canMove)
        {
            animator.SetTrigger(AnimationString.jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }
    public void onAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationString.attack1);
        }
    }
    public void onRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isRunnig = true;
        }
        else if(context.canceled)
        {
            isRunnig = false;
        }
    }
}
