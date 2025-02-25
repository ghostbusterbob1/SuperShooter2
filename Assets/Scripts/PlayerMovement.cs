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
    public Camera camera;
    private float targetFOV;
    private float fovSmoothSpeed = 10f; // Adjust speed as needed

    public LayerMask groundMask;
    public bool isgrounded;
    Transform t;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        t = transform;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 10f;
            targetFOV = 80f;
        }
        else
        {
            moveSpeed = 7f;
            targetFOV = 60f;

        }
        MInput();

        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFOV, Time.deltaTime * fovSmoothSpeed);
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
