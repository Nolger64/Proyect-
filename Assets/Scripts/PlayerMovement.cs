using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Parámetros de movimiento")]
    public float speed = 5f;               // Velocidad de movimiento
    public float rotationSpeed = 10f;      // Velocidad de rotación
    public Transform cameraTransform;      // Asigna la cámara del jugador (drag & drop desde el Inspector)

    private Rigidbody rb;
    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Evita que el Rigidbody se caiga o rote por física
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Leer entrada del jugador
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Dirección base (según cámara)
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Asegura que el movimiento esté solo en el plano XZ
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Dirección final de movimiento (relativa a la cámara)
        moveDirection = (forward * vertical + right * horizontal).normalized;
    }

    void FixedUpdate()
    {
        // Mover personaje
        Vector3 moveVelocity = moveDirection * speed;
        rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);

        // Si hay movimiento, rotar hacia la dirección
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
