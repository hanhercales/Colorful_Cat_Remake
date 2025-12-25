using UnityEngine;

public class Movement : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected bool isFacingRight = true;
    protected bool isGrounded;
    protected bool canMove = true;
    
    [SerializeField] protected float moveSpeed = 3f;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckRadius = 0.2f;
    
    protected virtual void CheckIfGrounded(){}
    protected virtual void Flip(){}

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void SetMoveable(bool moveable)
    {
        canMove = moveable;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
    
    protected void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
