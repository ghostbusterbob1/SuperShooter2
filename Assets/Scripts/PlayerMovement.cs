using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDir;
    private Rigidbody rb;

    public LayerMask groundMask;
    public bool isgrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 6f;
        }
        else
        {
            moveSpeed = 4f;
        }
        MInput();


        MovePlayer();
        IsGrounded();   

        if (Input.GetKeyDown(KeyCode.Space) && isgrounded == true)
        {
            Jump();
        }
    }

    private void MInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.velocity = new Vector3(moveDir.x * moveSpeed, rb.velocity.y, moveDir.z * moveSpeed);
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }

    private void IsGrounded()
    {
        if( Physics.Raycast(transform.position, Vector3.down, 2f, groundMask))
        {
            isgrounded = true;
        }
        else
        {
            isgrounded = false;
        }
    }
}
