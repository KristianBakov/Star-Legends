using UnityEngine;

public class WindweaverController : MonoBehaviour
{
    public bool isDashing;
    public int availableDashAttempts = 50;
    public float dashSpeed = 30f;
    public float dashDuration = 0.4f;

    [SerializeField] ParticleSystem forwardDashParticleSystem;
    [SerializeField] ParticleSystem backwardDashParticleSystem;
    [SerializeField] ParticleSystem leftDashParticleSystem;
    [SerializeField] ParticleSystem rightDashParticleSystem;


    public int dashAttempts = 0;
    private float dashStartTime;
    private PlayerController playerController;
    private CharacterController characterController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleDash();
    }

    private void HandleDash()
    {
        bool isTryingToDash = playerController.playerActions.Player.AbilityE.ReadValue<float>() == 0 ? false : true;

        if(isTryingToDash && !isDashing)
        {
            if(dashAttempts <= availableDashAttempts)
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
}
