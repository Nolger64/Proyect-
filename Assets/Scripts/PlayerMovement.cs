using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float rotationSpeed = 8f;

    [Header("Jump & Gravity")]
    public float gravity = -9.81f;
    public float jumpForce = 3f;

    CharacterController controller;
    Animator animator;
    Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        MovePlayer();
        ApplyGravity();
    }

    void MovePlayer()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 input = new Vector3(x, 0f, z);

        // No movimiento
        if (input.magnitude < 0.1f)
        {
            animator.SetFloat("Speed", 0f);
            return;
        }

        // Determinar si corre
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        float targetSpeed = isRunning ? runSpeed : walkSpeed;

        // Movimiento relativo a la rotación del personaje
        Vector3 direction = transform.forward * z + transform.right * x;
        direction.Normalize();

        controller.Move(direction * targetSpeed * Time.deltaTime);

        animator.SetFloat("Speed", isRunning ? 3f : 1f);

        // Rotación suave hacia la dirección de movimiento
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Saltar
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }
}