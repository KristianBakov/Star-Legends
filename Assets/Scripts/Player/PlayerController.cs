using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] Camera PlayerCamera;
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] Transform handsTransform;
    public Vector2 mouseSensitivity = new Vector2(1, 1);
    public float xRotation { get; private set; } = 0f;
    public bool isGrounded { get; private set; } = false;
    public bool isWalking { get; private set; } = false;
    public bool isJumping { get; private set; } = false;
    public bool isCrouching { get; private set; } = false;
    public bool useGravity = true;
    public bool isCrouchingUnderObstacle { get; private set; } = false;
    public Vector3 movementVector = Vector3.zero;
    public Vector3 inputVector = Vector3.zero;
    public Vector3 gunRotation { get; private set; } = Vector3.zero;
    [System.NonSerialized] public Vector3 jumpVelocity = Vector3.zero;
    private CharacterController characterController;
    //set up player actions
    public IA_Player playerActions;
    private PlayerStats playerStats;
    private const float shootRotationoffsetDampening = 1.2f;

    private void Awake()
    {
        playerActions = new IA_Player();
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerStats = GetComponent<PlayerStats>();
        //Cursor.lockState = CursorLockMode.Locked;
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

        inputVector = playerActions.Player.Movement.ReadValue<Vector2>();

        isWalking = playerActions.Player.Run.ReadValue<float>() == 0 ? false : true;
        isCrouching = playerActions.Player.Crouch.ReadValue<float>() == 0 ? false : true;

        if(isCrouching)
        {
            HandleCrouching();
        }
        else
        {
            HandleStand();
        }

        movementVector = transform.right * inputVector.x + transform.forward * inputVector.y;

        if (isCrouching)
        {
            characterController.Move(movementVector * playerStats.crouchingMovementSpeed * Time.deltaTime);
        }
        else if (isWalking)
        {
            characterController.Move(movementVector * playerStats.walkingMovementSpeed * Time.deltaTime);
        }
        else 
        { 
            characterController.Move(movementVector * playerStats.runningMovementSpeed * Time.deltaTime); 
        }
    }

    private void HandleStand()
    {
        if(characterController.height < playerStats.standingHeightY)
        {
            float lastHeight = characterController.height;

            RaycastHit hit;
            if(Physics.Raycast(transform.position, Vector3.up, out hit, playerStats.standingHeightY))
            {
                UpdateCharacterHeight(hit.distance);
                isCrouchingUnderObstacle = true;
                return;
            }

            UpdateCharacterHeight(playerStats.standingHeightY);
            isCrouchingUnderObstacle = false;

            if (characterController.height + 0.05f >= playerStats.standingHeightY)
            {
                characterController.height = playerStats.standingHeightY;
            }

            transform.position += new Vector3(0, (characterController.height - lastHeight) / 2, 0);
        }
    }

    private void HandleCrouching()
    {
       if (isCrouchingUnderObstacle) return;
       if(characterController.height > playerStats.crouchingHeightY)
        {
            UpdateCharacterHeight(playerStats.crouchingHeightY);

            if(characterController.height - 0.05f <= playerStats.crouchingHeightY)
            {
                characterController.height = playerStats.crouchingHeightY;
            }
        }
    }

    private void UpdateCharacterHeight(float newHeight)
    {
        characterController.height = Mathf.Lerp(characterController.height, newHeight, Time.deltaTime * playerStats.crouchTransitionSpeed);
    }

    private void HandleMouseLook()
    {
        Vector2 mouseInput = playerActions.Player.Look.ReadValue<Vector2>();
        xRotation -= mouseInput.y * mouseSensitivity.y;
        xRotation = Mathf.Clamp(xRotation, -45f, 45f);

        if (gunRotation != Vector3.zero)
        {
            PlayerCamera.transform.localRotation = Quaternion.Euler(xRotation + gunRotation.x / shootRotationoffsetDampening,
                gunRotation.y / shootRotationoffsetDampening, gunRotation.z / shootRotationoffsetDampening);
        }
        else
        {
            PlayerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        transform.Rotate(Vector3.up * mouseInput.x * mouseSensitivity.x);

    }

    private void HandleJumpInput()
    {
        if(isCrouchingUnderObstacle) return;
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
        if (useGravity)
        {
            jumpVelocity.y += playerStats.gravity * Time.deltaTime;
        }
        characterController.Move(jumpVelocity * Time.deltaTime);
    }
    public void SetGunRotation(Vector3 _gunRotation)
    {
        gunRotation = _gunRotation;
        handsTransform.localRotation = Quaternion.Euler(gunRotation);
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
