using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

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

    public float gunRange;
    public float gunDamage;
    public float roundsPerSecond;
    public LayerMask targetMask;
    private float timeToNextShot = 0f;

    void Update()
    {
        Shoot();
        MouseMovement();
        Movement();
    }

    private void Shoot()
    {
        if (timeToNextShot > 0f)
        {
            timeToNextShot -= Time.deltaTime;
            return;
        }

        if(Input.GetMouseButton(0))
        {
            timeToNextShot = 1f / roundsPerSecond;
            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, gunRange, targetMask);

            foreach(var hit in hits)
            {
                if (hit.transform.CompareTag("Enemy"))
                {
                    hit.transform.GetComponent<Enemy>().GetDamage(gunDamage);
                }
            }
        }
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
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
