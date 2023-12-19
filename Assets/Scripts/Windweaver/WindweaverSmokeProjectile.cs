using UnityEngine;

public class WindweaverSmokeProjectile : MonoBehaviour
{
    [SerializeField] GameObject smokeBallPrefab;

    public bool isControlled = false;

    private Vector3 startingPosition;
    private Camera playerCamera;
    private float distanceTraveled = 0f;
    private float downwardForce = -2f;

    private const float projectileSpeed = 20f;
    private const float maxDistance = 30f;
    private const float downwardForceIncrement = -3.8f;

    private void Start()
    {
        startingPosition = transform.position;
    }
    private void Update()
    {
        if(isControlled)
        {
            transform.rotation = playerCamera.transform.rotation;
        }

        Vector3 movementVector = transform.forward * projectileSpeed * Time.deltaTime;
        if(!isControlled)
        {
            //drop down
            downwardForce += downwardForceIncrement * Time.deltaTime;
            movementVector += (transform.up * downwardForce * Time.deltaTime);
        }

        Vector3 newPosition = transform.position + movementVector;
        distanceTraveled = Vector3.Distance(startingPosition, newPosition);

        if(distanceTraveled > maxDistance)
        {
            OnCreateSmokeBall(transform.position);
        }
        else
        {
            transform.position += movementVector;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnCreateSmokeBall(collision.contacts[0].point);
    }
    public void InitializeValues(bool _isControlled, Camera _playerCamera)
    {
        isControlled = _isControlled;
        playerCamera = _playerCamera;
    }
    public void SetIsControlled(bool _isControlled)
    {
        isControlled = _isControlled;
    }

    private void OnCreateSmokeBall(Vector3 _position)
    {
        Instantiate(smokeBallPrefab, _position, transform.rotation);
        Destroy(this.gameObject);
    }

}
