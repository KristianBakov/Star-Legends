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

    private void Awake()
    {
        playerActions = new IA_Player();
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {

        Vector3 movementVector = transform.right * playerActions.Player.Movement.ReadValue<Vector2>() + transform.forward * playerActions.Player.Movement.ReadValue<Vector2>();
        characterController.Move(movementVector * movementSpeed * Time.deltaTime);
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
