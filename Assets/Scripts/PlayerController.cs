using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    public float sprintSpeed;
    public float sprintDuration;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    private float sprintRemaining;
    private float currentSpeed;
    private bool isSprinting = false;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public float mouseSensitivityX = 1;
    public float mouseSensitivityY = 1;
    public float snappiness = 1;
    public float upDownRange = 90;

    private bool isGrounded;
    private Vector3 velocity;

    private float xVelocity;
    private float yVelocity;
    private float rotY;

    private PlayerInventory weapons;
    public PlayerInventory Weapons
    {
        get
        {
            return weapons;
        }
    }

    private void Awake()
    {
        weapons = GetComponent<PlayerInventory>();
    }

    private void Start()
    {
        currentSpeed = speed;
        sprintRemaining = sprintDuration;
    }

    void Update()
    {
        weapons.Shoot();
        MouseMovement();
        Movement();
    }

    private void MouseMovement()
    {
        var rotX = Input.GetAxis("Mouse X") * mouseSensitivityX;
        rotY -= Input.GetAxis("Mouse Y") * mouseSensitivityY;
        rotY = Mathf.Clamp(rotY, -upDownRange, upDownRange);

        xVelocity = Mathf.Lerp(xVelocity, rotX, snappiness * Time.deltaTime);
        yVelocity = Mathf.Lerp(yVelocity, rotY, snappiness * Time.deltaTime);

        Camera.main.transform.localRotation = Quaternion.Euler(yVelocity, 0, 0);
        transform.Rotate(0, xVelocity, 0);
    }

    private void Movement()
    {
        Sprint();

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * currentSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && sprintRemaining >= sprintDuration)
        {
            isSprinting = true;
            currentSpeed = sprintSpeed;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
            currentSpeed = speed;
        }

        if (isSprinting)
        {
            if (sprintRemaining > 0f) sprintRemaining -= Time.deltaTime;
            else
            {
                currentSpeed = speed;
                isSprinting = false;
            }
        }
        else
        {
            if (sprintRemaining < sprintDuration) sprintRemaining += Time.deltaTime;
        }
    }
}
