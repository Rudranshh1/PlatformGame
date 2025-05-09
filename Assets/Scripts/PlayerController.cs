using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    Vector2 moveInput;
    Rigidbody2D rb;
    [SerializeField]
    private bool _isMoving = false;
    [SerializeField]
    private bool _isRunning = false;
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

    public bool isFacingRight = true;

    Animator animator;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x*walkSpeed,rb.velocity.y);

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

    public void onRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isRunnig = true;
            walkSpeed = 7;
        }
        else if(context.canceled)
        {
            isRunnig = false;
            walkSpeed = 5;
        }
    }
}
