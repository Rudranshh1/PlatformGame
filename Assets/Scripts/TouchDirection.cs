using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDirection : MonoBehaviour
{

    public ContactFilter2D contactFilter;   
    CapsuleCollider2D capsuleCollider;
    RaycastHit2D[] raycastHit = new RaycastHit2D[5];
    RaycastHit2D[] wallHit = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHit = new RaycastHit2D[5];
    private float groundDistance = 0.05f;
    private float wallDistance = 0.2f;
    private float ceilingDistance = 0.05f;
    Animator animator;
    [SerializeField]
    private bool _isGrounded = true;
    [SerializeField]
    private bool _isOnWall = true;
    [SerializeField]
    private bool _isOnCeiling = true;
    private Vector2 wallcheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    public bool IsGrounded
    {
        get 
        { 
            return _isGrounded; 
        }
        private set
        { 
            _isGrounded = value;
            animator.SetBool(AnimationString.isGrounded, value);
        }
    }
    public bool IsOnWall
    {
        get
        {
            return _isOnWall;
        }
        private set
        {
            _isOnWall = value;
            animator.SetBool(AnimationString.isOnWall, value);
        }
    }
    public bool IsOnCeiling
    {
        get
        {
            return _isOnCeiling;
        }
        private set
        {
            _isOnCeiling = value;
            animator.SetBool(AnimationString.isOnCeiling, value);
        }
    }
    // Start is called before the first frame update
    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        IsGrounded = capsuleCollider.Cast(Vector2.down, contactFilter, raycastHit, groundDistance)>0;
        IsOnWall = capsuleCollider.Cast(wallcheckDirection, contactFilter, wallHit, wallDistance) > 0;
        IsOnCeiling = capsuleCollider.Cast(Vector2.up, contactFilter, ceilingHit, ceilingDistance) > 0;
    }
}
