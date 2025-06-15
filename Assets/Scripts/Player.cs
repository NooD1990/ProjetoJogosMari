using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;
    private Animator anim;
    public bool hasSupply = false;
    public float speed = 5f;
    public float jumpForce = 8f;
    public float gravity = -9.81f;
    private float verticalVelocity;

    public Transform cameraTransform;         // C�mera externa (terceira pessoa)
    public float mouseSensitivity = 2f;
    private float xRotation = 0f;

    public Vector3 cameraOffset = new Vector3(0f, 2f, -4f); // Posi��o da c�mera em terceira pessoa
    public float cameraSmoothSpeed = 10f;

    private bool isAttacking = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Inicializa a posi��o da c�mera
        UpdateCameraPosition(true);
    }

    void Update()
    {
        HandleMouseLook();
        Move();
        HandleJump();
        HandleAttack();
        ApplyGravity();
        UpdateCameraPosition(false);
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        if (move != Vector3.zero && controller.isGrounded)
        {
            controller.Move(move * speed * Time.deltaTime);
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    void HandleJump()
    {
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            verticalVelocity = jumpForce;
            anim.SetTrigger("jump");
        }
    }

    void ApplyGravity()
    {
        verticalVelocity += gravity * Time.deltaTime;
        Vector3 gravityMove = Vector3.up * verticalVelocity;
        controller.Move(gravityMove * Time.deltaTime);
    }

    void HandleAttack()
    {
        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            isAttacking = true;
            anim.SetTrigger("attack");
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -40f, 60f);
    }

    void UpdateCameraPosition(bool instant)
    {
        // Calcula a rota��o da c�mera com base na rota��o do player
        Quaternion camRot = Quaternion.Euler(xRotation, transform.eulerAngles.y, 0f);

        // Posi��o desejada da c�mera com offset
        Vector3 desiredPosition = transform.position + camRot * cameraOffset;

        if (instant)
        {
            cameraTransform.position = desiredPosition;
        }
        else
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, Time.deltaTime * cameraSmoothSpeed);
        }

        // A c�mera sempre olha para o player
        cameraTransform.LookAt(transform.position + Vector3.up * 1.5f);
    }
}
