using UnityEngine;

public class WindweaverController : MonoBehaviour
{
    public bool isDashing = false;
    public bool isThrowingSmoke = false;

    public int availableDashAttempts = 50;
    public float dashSpeed = 30f;
    public float dashDuration = 0.4f;

    [SerializeField] Camera playerCamera;
    [SerializeField] ParticleSystem forwardDashParticleSystem;
    [SerializeField] ParticleSystem backwardDashParticleSystem;
    [SerializeField] ParticleSystem leftDashParticleSystem;
    [SerializeField] ParticleSystem rightDashParticleSystem;
    [SerializeField] GameObject smokeProjectile;
    [SerializeField] Transform smokeFiringTransform;

    WindweaverSmokeProjectile currentSmokeProjectile;
    private float lastTimeSmokeEnded = 0f;
    private float smokeDelaySeconds = 0.3f;

    private int dashAttempts = 0;
    private float dashStartTime;

    private PlayerController playerController;
    private CharacterController characterController;
    private PlayerWeapon playerWeapon;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
        playerWeapon = GetComponent<PlayerWeapon>();
    }

    private void Update()
    {
        HandleDash();
        HandleSmokeFunction();
    }

    #region Dashing
    private void HandleDash()
    {
        bool isTryingToDash = playerController.playerActions.Player.AbilityE.ReadValue<float>() == 0 ? false : true;

        if(isTryingToDash && !isDashing)
        {
            if(dashAttempts < availableDashAttempts)
            {
                OnStartDash();
            }
        }

        if(isDashing)
        {
            if(Time.time - dashStartTime <= dashDuration)
            {
                if(playerController.movementVector.Equals(Vector3.zero))
                {
                    //no input, just dash forward
                    characterController.Move(transform.forward * dashSpeed * Time.deltaTime);
                }
                else
                {
                    characterController.Move(playerController.movementVector.normalized * dashSpeed * Time.deltaTime);
                }
            }
            else
            {
                OnEndDash();
            }
        }
    }

    private void OnStartDash()
    {
        isDashing = true;
        dashStartTime = Time.time;
        dashAttempts++;

        PlayDashParticles();
    }

    private void OnEndDash()
    {
        isDashing = false;
        dashStartTime = 0f;
    }

    private void PlayDashParticles()
    {
        Vector2 inputVector = playerController.inputVector;

        if (inputVector.y > 0 && Mathf.Abs(inputVector.x) <= inputVector.y)
        {
            //forward & forward diagonals
            forwardDashParticleSystem.Play();
            return;
        }

        if (inputVector.y < 0 && Mathf.Abs(inputVector.x) <= Mathf.Abs(inputVector.y))
        {
            //backward & backward diagonals
            backwardDashParticleSystem.Play();
            return;
        }

        if(inputVector.x > 0)
        {
            rightDashParticleSystem.Play();
            return;
        }

        if(inputVector.x < 0)
        {
            leftDashParticleSystem.Play();
            return;
        }

        //defaulting to forward
        forwardDashParticleSystem.Play();
    }
    #endregion

    #region Smoke
    private bool isTryingToThrowSmoke = false;
    private bool isAbilityCPressed = false;

    private void HandleSmokeFunction()
    {
        // Check if the ability C button is being pressed down
        bool isTryingToThrowSmokeThisFrame = playerController.playerActions.Player.AbilityC.triggered;

        if (isTryingToThrowSmokeThisFrame && Time.time - lastTimeSmokeEnded >= smokeDelaySeconds)
        {
            Debug.Log("pressed once");
            isTryingToThrowSmoke = true;
            ThrowSmoke();
        }

        if (isTryingToThrowSmoke)
        {
            bool isControlled = playerController.playerActions.Player.AbilityC.ReadValue<float>() > 0.0f;
            Debug.Log("Pressing continuously " + isControlled + " test " + isTryingToThrowSmoke);
            currentSmokeProjectile.SetIsControlled(isControlled);

            // Check if the ability C button is pressed
            bool isAbilityCPressedThisFrame = playerController.playerActions.Player.AbilityC.ReadValue<float>() > 0.0f;

            if (isAbilityCPressed && !isAbilityCPressedThisFrame)
            {
                Debug.Log("is stopping control");
                OnThrowingSmokeEnd();
                isTryingToThrowSmoke = false; // Reset the flag when the button is released
            }

            isAbilityCPressed = isAbilityCPressedThisFrame;
        }
    }




    private void ThrowSmoke()
    {
        isThrowingSmoke = true;
        //playerWeapon.gameObject.SetActive(false);

        GameObject _smokeProjectile = Instantiate(smokeProjectile, smokeFiringTransform.position, playerCamera.transform.rotation);
        currentSmokeProjectile = _smokeProjectile.GetComponent<WindweaverSmokeProjectile>();
        currentSmokeProjectile.InitializeValues(false, playerCamera);
    }

    private void OnThrowingSmokeEnd()
    {
         Debug.Log("Smoke end was called");
        lastTimeSmokeEnded = Time.time;
        isThrowingSmoke = false;
        currentSmokeProjectile.SetIsControlled(false);

        currentSmokeProjectile = null;
    }
    #endregion
}
