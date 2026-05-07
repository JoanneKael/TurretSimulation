using UnityEngine;

public class Muzzle : MonoBehaviour
{
    public Transform pitchPivot;
    public Transform targetDrone;

    [Header("Rotation")]
    public float minPitch = -45f;
    public float maxPitch = 20f;
    public float rotationSpeed = 30f;

    [Header("Ready")]
    public Transform muzzlePoint;
    public float fireAngleThreshold = 5f;
    public bool isReady = false;

    [Header("Fire")]
    public float fireTimer = 0f;
    public float fireInterval = 0.5f;    
    public float projectileSpeed = 12f;  
    public float projectileLifeTime = 3f;

    private void Update()
    {
        if (targetDrone == null) return;

        fireTimer += Time.deltaTime;

        RotateTurretHead();
        CheckTurretAim();

        if(isReady && fireTimer>= fireInterval)
        {
            FireProjectile();
            fireTimer = 0f;
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

    private void FireProjectile()
    {
        GameObject projectile = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        projectile.transform.localScale = Vector3.one * 0.4f ;
        projectile.transform.position = muzzlePoint.position;
        projectile.transform.rotation = muzzlePoint.rotation;

        Projectile script = projectile.AddComponent<Projectile>();
        script.Initialize(fireInterval, projectileLifeTime, projectileSpeed);
    }
}