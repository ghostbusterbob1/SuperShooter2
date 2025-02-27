using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public float slowDescentRate = 1.0f; // Controls how fast you descend while wall-running
    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDir;
    private Rigidbody rb;
    public Camera camera;
    private float targetFOV;
    private float fovSmoothSpeed = 10f; // Adjust speed as needed
    bool wallruning = false;

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
        // Adjust move speed and FOV based on input
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

        if (Input.GetKeyDown(KeyCode.Space) && isgrounded)
        {
            Jump();
        }

        if (wallruning)
        {
            rb.useGravity = false;
            rb.velocity = new Vector3(rb.velocity.x,
                                      Mathf.Lerp(rb.velocity.y, -slowDescentRate, Time.deltaTime * 5f),
                                      rb.velocity.z);
        }
        else
        {
            rb.useGravity = true;
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

        if (isgrounded)
        {
            
            rb.velocity = new Vector3(moveDir.x * moveSpeed, rb.velocity.y, moveDir.z * moveSpeed);
        }


        else
        {
            float airControl = 0.3f; 

            rb.AddForce(moveDir.normalized * moveSpeed * airControl, ForceMode.Acceleration);
            rb.drag = 1.2f; 
        }

        if (!isgrounded)
        {
            float maxAirSpeed = moveSpeed * 1f; // Adjust as needed
            Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            if (horizontalVelocity.magnitude > maxAirSpeed)
            {
                rb.velocity = new Vector3(horizontalVelocity.normalized.x * maxAirSpeed, rb.velocity.y, horizontalVelocity.normalized.z * maxAirSpeed);
            }
        }
    }


    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }

    private void IsGrounded()
    {
        isgrounded = Physics.Raycast(transform.position, Vector3.down, 1.2f, groundMask);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "WallRun")
        {
            wallruning = true;
        }
        else
        {
            wallruning = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "WallRun")
        {
            wallruning = false;
        }
    }
}
