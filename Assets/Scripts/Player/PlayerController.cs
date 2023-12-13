using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Camera PlayerCamera;
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] LayerMask groundLayerMask;
    public Vector2 mouseSensitivity = new Vector2(1, 1);
    public float xRotation { get; private set; } = 0f;
    public bool isGrounded { get; private set; } = false;
    public bool isWalking { get; private set; } = false;
    public bool isJumping { get; private set; } = false;
    [System.NonSerialized] public Vector3 jumpVelocity = Vector3.zero;
    private CharacterController characterController;
    //set up player actions
    private IA_Player playerActions;
    private PlayerStats playerStats;

    private void Awake()
    {
        playerActions = new IA_Player();
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerStats = GetComponent<PlayerStats>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheckTransform.position, 0.4f, groundLayerMask);

        HandleJumpInput();
        HandleMovement();
    }

    private void LateUpdate()
    {
        HandleMouseLook();
    }

    private void HandleMovement()
    {

        Vector3 movementInput = playerActions.Player.Movement.ReadValue<Vector2>();

        isWalking = playerActions.Player.Run.ReadValue<float>() == 0 ? false : true;

        Vector3 movement = transform.right * movementInput.x + transform.forward * movementInput.y;

        if (isWalking)
        {
            characterController.Move(movement * playerStats.walkingMovementSpeed * Time.deltaTime);
        }
        else 
        { 
            characterController.Move(movement * playerStats.runningMovementSpeed * Time.deltaTime); 
        }
    }

    private void HandleMouseLook()
    {
        Vector2 mouseInput = playerActions.Player.Look.ReadValue<Vector2>();
        xRotation -= mouseInput.y * mouseSensitivity.y;
        xRotation = Mathf.Clamp(xRotation, -45f, 45f);
        PlayerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseInput.x * mouseSensitivity.x);

    }

    private void HandleJumpInput()
    {
        bool isTryingToJump = playerActions.Player.Jump.ReadValue<float>() == 0 ? false : true;
        isJumping = isGrounded && isTryingToJump;

        if(isGrounded && jumpVelocity.y < 0)
        {
            jumpVelocity.y = -2f;
        }

        if (isJumping)
        {
            jumpVelocity.y = Mathf.Sqrt(playerStats.jumpHeight * -2f * playerStats.gravity);
        }
        jumpVelocity.y += playerStats.gravity * Time.deltaTime;
        characterController.Move(jumpVelocity * Time.deltaTime);
    }

    private void OnEnable()
    {
        playerActions.Player.Enable();
    }

    private void OnDisable()
    {
        playerActions.Player.Disable();
    }
}
