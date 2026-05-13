using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class Muzzle : MonoBehaviour
{
    public Transform pitchPivot;
    public Transform targetDrone;
    public GameObject projectilePrefab;

    [Header("Rotation")]
    public float minPitch = -45f;
    public float maxPitch = 20f;
    public float rotationSpeed = 30f;

    [Header("Ready")]
    public Transform muzzlePoint;
    public float fireAngleThreshold = 5f;
    public bool isReady = false;

    [Header("Fire")]
    public float fireInterval = 0.5f;
    public float projectileSpeed = 12f;
    public float projectileLifeTime = 3f;
    public bool isFire = false;

    private Coroutine fireCoroutine;
    private Coroutine resetCoroutine;

    [Header("SFX")]
    public AudioSource audioSource;
    public AudioClip fireSfx;
    float recoilTime = 0.05f;
    float returnTime = 0.1f;

    private void Update()
    {
        if (targetDrone == null) return;

        RotateTurretHead();
        CheckTurretAim();

        if (isReady && !isFire && targetDrone.GetComponent<Drone>().hp > 0)
        {
            fireCoroutine = StartCoroutine(IEFire());
        }
        else if (!isReady && isFire || targetDrone.GetComponent<Drone>().hp <= 0)
        {
            StopFiring();
        }
    }

    private void RotateTurretHead()
    {
        Vector3 direction = targetDrone.position - pitchPivot.position;

        float distanceXZ = new Vector3(direction.x, 0f, direction.z).magnitude;
        float targetPitch = -Mathf.Atan2(direction.y, distanceXZ) * Mathf.Rad2Deg;

        targetPitch = Mathf.Clamp(targetPitch, minPitch, maxPitch);

        Quaternion targetRotation = Quaternion.Euler(targetPitch, 0f, 0f);

        pitchPivot.localRotation = Quaternion.RotateTowards(pitchPivot.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void CheckTurretAim()
    {
        Vector3 targetDirection = targetDrone.position - muzzlePoint.position;

        float currentAngle = Vector3.Angle(muzzlePoint.forward, targetDirection);

        if (currentAngle <= fireAngleThreshold)
        {
            isReady = true;
        }
        else
        {
            isReady = false;
        }
    }

    private IEnumerator IEFire()
    {
        isFire = true;

        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
            resetCoroutine = null;
        }

        GameObject projectile = Instantiate(projectilePrefab);
        projectile.transform.localScale = Vector3.one * 0.4f;
        projectile.transform.position = muzzlePoint.position;
        projectile.transform.rotation = muzzlePoint.rotation;

        Projectile script = projectile.AddComponent<Projectile>();
        script.Initialize(fireInterval, projectileLifeTime, projectileSpeed);

        if (audioSource != null && fireSfx != null)
        {
            audioSource.PlayOneShot(fireSfx);
        }

        Vector3 originalPos = new Vector3(0f, 0f, 0.8f);
        Vector3 recoilPos = new Vector3(originalPos.x, originalPos.y, 0.3f);

        float elapsed = 0f;
        while (elapsed < recoilTime)
        {
            elapsed += Time.deltaTime;
            muzzlePoint.localPosition = Vector3.Lerp(originalPos, recoilPos, elapsed / recoilTime);
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < returnTime)
        {
            elapsed += Time.deltaTime;
            muzzlePoint.localPosition = Vector3.Lerp(recoilPos, originalPos, elapsed / returnTime);
            yield return null;
        }

        muzzlePoint.localPosition = originalPos;

        float remainingCooldown = fireInterval - (recoilTime + returnTime);
        if (remainingCooldown > 0)
        {
            yield return new WaitForSeconds(remainingCooldown);
        }

        isFire = false;
        fireCoroutine = null;
    }

    private void StopFiring()
    {
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
            fireCoroutine = null;
        }

        isFire = false;

        if (gameObject.activeInHierarchy && muzzlePoint != null)
        {
            if (resetCoroutine != null)
            {
                StopCoroutine(resetCoroutine);
            }
            resetCoroutine = StartCoroutine(IEResetMuzzle());
        }
    }

    private IEnumerator IEResetMuzzle()
    {
        float resetTime = 0.1f;
        float elapsed = 0f;

        Vector3 currentPos = muzzlePoint.localPosition;

        Vector3 targetPos = new Vector3(currentPos.x, currentPos.y, 0.8f);

        while (elapsed < resetTime)
        {
            elapsed += Time.deltaTime;
            muzzlePoint.localPosition = Vector3.Lerp(currentPos, targetPos, elapsed / resetTime);
            yield return null;
        }

        muzzlePoint.localPosition = targetPos;
        resetCoroutine = null;
    }
}