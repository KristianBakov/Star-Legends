using UnityEngine;

public class WindweaverController : MonoBehaviour, IAgentController
{
    private const float JUMP_HEIGHT_DIFF = 2.5f;

    public bool isDashing { get; private set; } = false;
    public bool isThrowingSmoke { get; private set; } = false;
    public bool isUpdrafting { get; private set; } = false;
    public bool isFalling { get; private set; } = false;

    public int availableDashAttempts = 2;
    public float dashSpeed = 30f;
    public float dashDuration = 0.4f;

    private Camera playerCamera;
    private GameObject windweaverParticlesPrefab;
    private WindweaverParticles windweaverParticles;

    [SerializeField] GameObject smokeProjectile;
    [SerializeField] Transform smokeFiringTransform;

    WindweaverSmokeProjectile currentSmokeProjectile;
    private float lastTimeSmokeEnded = 0f;
    private float smokeDelaySeconds = 0.3f;

    private int dashAttempts = 0;
    private float dashStartTime;

    private float lastTimeUpdrafted = 0;
    private float updraftHeight = 4f;
    private float updraftDelaySeconds = 0.2f;
    private int updraftAttempts = 0;
    private int maxUpdraftAttempts = 5;

    private float lastJumpVelocityY = 0f;
    private float floatingGravity = -5f;

    private PlayerController playerController;
    private CharacterController characterController;
    private PlayerWeapon playerWeapon;
    private PlayerStats playerStats;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
        playerWeapon = GetComponent<PlayerWeapon>();
        playerStats = GetComponent<PlayerStats>();

        if(playerController != null)
        {
            playerCamera = playerController.GetPlayerCamera();
        }

        //get windweaver particles reference from resourtces
        windweaverParticlesPrefab = Resources.Load<GameObject>("/Windweaver/WindweaverParticleRoot");
        Debug.Log(windweaverParticlesPrefab);
        windweaverParticles = windweaverParticlesPrefab.GetComponent<WindweaverParticles>();
        Instantiate(windweaverParticlesPrefab, playerController.transform);
        windweaverParticles.floatParticleSystem.Stop();
    }

    private void Update()
    {

        HandleAbilityE();
        HandleIsFalling();
        HandleSlowfall();

        if (!isDashing)
        {
            HandleAbilityQ();
            HandleAbilityC();
        }
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
            windweaverParticles.forwardDashParticleSystem.Play();
            return;
        }

        if (inputVector.y < 0 && Mathf.Abs(inputVector.x) <= Mathf.Abs(inputVector.y))
        {
            //backward & backward diagonals
            windweaverParticles.backwardDashParticleSystem.Play();
            return;
        }

        if(inputVector.x > 0)
        {
            windweaverParticles.rightDashParticleSystem.Play();
            return;
        }

        if(inputVector.x < 0)
        {
            windweaverParticles.leftDashParticleSystem.Play();
            return;
        }

        //defaulting to forward
        windweaverParticles.forwardDashParticleSystem.Play();
    }
    #endregion

    #region Smoke
    private void HandleSmokeFunction()
    {
        bool isTryingToThrowSmoke = playerController.playerActions.Player.AbilityC.triggered;
        if (isTryingToThrowSmoke && !isThrowingSmoke && Time.time - lastTimeSmokeEnded >= smokeDelaySeconds)
        {
            ThrowSmoke();
        }

        if (isThrowingSmoke && isTryingToThrowSmoke)
        {
            bool isControlled = playerController.playerActions.Player.AbilityC.WasPressedThisFrame();
            currentSmokeProjectile.SetIsControlled(isControlled);

            bool isStoppingControl = playerController.playerActions.Player.AbilityC.WasReleasedThisFrame();
            if (isStoppingControl)
            {
                OnThrowingSmokeEnd();
            }

        }
    }

    private void ThrowSmoke()
    {
        isThrowingSmoke = true;
        //add animation for throwing smoke and disable player weapon
        //playerWeapon.gameObject.SetActive(false);

        GameObject _smokeProjectile = Instantiate(smokeProjectile, smokeFiringTransform.position, playerCamera.transform.rotation);
        currentSmokeProjectile = _smokeProjectile.GetComponent<WindweaverSmokeProjectile>();
        currentSmokeProjectile.InitializeValues(false, playerCamera);
    }

    private void OnThrowingSmokeEnd()
    {
        lastTimeSmokeEnded = Time.time;
        isThrowingSmoke = false;
        currentSmokeProjectile.SetIsControlled(false);

        currentSmokeProjectile = null;
    }
    #endregion

    #region Updraft

    private void HandleUpdraft()
    {
        bool isTryingToUpdraft = playerController.playerActions.Player.AbilityQ.triggered;
        if(Time.time - lastTimeUpdrafted < updraftDelaySeconds)
        {
            if(isUpdrafting)
            {
                OnUpdraftEnd();
            }
        }

        if (isTryingToUpdraft && updraftAttempts < maxUpdraftAttempts)
        {
            OnUpdraftStart();
            Updraft();
        }
    }

    private void Updraft()
    {
        if(!playerController.isGrounded)
        {
            playerController.jumpVelocity.y = Mathf.Sqrt(updraftHeight / JUMP_HEIGHT_DIFF * -2f * playerStats.gravity);
        }
        else
        {
            playerController.jumpVelocity.y = Mathf.Sqrt(updraftHeight * -2f * playerStats.gravity);
        }
    }

    private void OnUpdraftStart()
    {
        isUpdrafting = true;
        lastTimeUpdrafted = Time.time;
        //hide gun and play updraft animations
        updraftAttempts++;
    }

    private void OnUpdraftEnd()
    {
        isUpdrafting = false;
        //show gun
    }

    #endregion

    #region Slowfall

    private void HandleIsFalling()
    {
        if (!playerController.isGrounded &&
            playerController.jumpVelocity.y <= 0 &&
            playerController.jumpVelocity.y < lastJumpVelocityY)
        {
            isFalling = true;
        }
        else
        {
            isFalling = false;
        }

        lastJumpVelocityY = playerController.jumpVelocity.y;
    }

    private void HandleSlowfall()
    {
        //Debug.Log("HandleFloat");
        bool isTryingToFloat = isFalling && playerController.playerActions.Player.Jump.IsPressed();

        if (isTryingToFloat)
        {
           // Debug.Log("IsTryingToFloat");
            if (windweaverParticles.floatParticleSystem.isStopped)
            {
                windweaverParticles.floatParticleSystem.Play();
            }
            playerStats.gravity = floatingGravity;
        }
        else
        {
            if (windweaverParticles.floatParticleSystem.isPlaying)
            {
                windweaverParticles.floatParticleSystem.Stop();
            }
            playerStats.gravity = playerStats.defaultGravity;
        }
    }

    public void HandleAbilityQ()
    {
        HandleUpdraft();
    }

    public void HandleAbilityC()
    {
        HandleSmokeFunction();
    }

    public void HandleAbilityE()
    {
        HandleDash();
    }

    public void HandleAbilityX()
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
