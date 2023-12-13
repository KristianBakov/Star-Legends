using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Camera PlayerCamera;
    public float mouseSensitivity { get; private set; } = 100f;
    public float xRotation { get; private set; } = 0f;
    public float movementSpeed { get; private set; } = 6f;
    private CharacterController characterController;
    //set up player actions
    private IA_Player playerActions;
    private Rigidbody rb;

    private void Awake()
    {
        playerActions = new IA_Player();
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {

        Vector3 movementInputDir = playerActions.Player.Movement.ReadValue<Vector2>();
        Vector3 movement = new Vector3(movementInputDir.x, 0, movementInputDir.y);
        characterController.Move(movement * movementSpeed * Time.deltaTime);
    }

    private void HandleMouseLook()
    {

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
