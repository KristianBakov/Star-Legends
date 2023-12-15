using System;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] Animator handAnimator;
    [SerializeField] Transform firePoint;
    [SerializeField] Camera playerCamera;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject wallHitDecalPrefab;
    [SerializeField] GameObject projectilePrefab;

    public Gun equippedWeapon;
    private PlayerController playerController;

    private float lastTimeShot = 0f;
    private int currentRecoilIndex = 0;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        equippedWeapon = new Vandal();
    }

    private void Update()
    {
        if (playerController.playerActions == null) return;
        bool isTryingToShoot = playerController.playerActions.Player.Fire.ReadValue<float>() == 0 ? false : true;

        if(isTryingToShoot)
        {
            HandleShooting();
        }
        else
        {
            //start recoil reset
            playerController.SetGunRotation(Vector3.Lerp(playerController.gunRotation,Vector3.zero, equippedWeapon.fireRatePerSecond * Time.deltaTime));
        }

    }

    private void HandleShooting()
    {
        if(Time.time - lastTimeShot >= 1 / equippedWeapon.fireRatePerSecond)
        {
            //we can shoot again
            PlayFireAnimation();

            HandleGunRecoil();

            RaycastHit hit;
            if(Physics.Raycast(firePoint.transform.position,
                firePoint.transform.TransformDirection(Vector3.forward),
                out hit, equippedWeapon.falloffDistance))
            {
                Debug.Log("Hit " + hit.collider.gameObject.name);
                Instantiate(wallHitDecalPrefab, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
            }

            lastTimeShot = Time.time;
        }
    }

    private void HandleGunRecoil()
    {
        if(Time.time - lastTimeShot >= equippedWeapon.recoilResetTimeSeconds)
        {
            playerController.SetGunRotation(playerController.gunRotation + equippedWeapon.recoilPattern[0]);
            currentRecoilIndex = 1;
        }
        else
        {
            playerController.SetGunRotation(playerController.gunRotation + equippedWeapon.recoilPattern[currentRecoilIndex]);

            if(currentRecoilIndex + 1 <= equippedWeapon.recoilPattern.Length - 1)
            {
                currentRecoilIndex++;
            }
            else
            {
                currentRecoilIndex = 0;
            }
        }
    }

    private void PlayFireAnimation()
    {
        handAnimator.Play("Fire", 0, 0f);
    }

    //private void OnDrawGizmos()
    //{
    //    equippedWeapon = new Vandal();
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawRay(firePoint.transform.position, firePoint.transform.TransformDirection(Vector3.forward) * equippedWeapon.falloffDistance);
    //}
}
