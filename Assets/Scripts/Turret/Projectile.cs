using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;

    public void Initialize(float fireInterval, float projectileLifeTime, float projectileSpeed)
    {
        speed = projectileSpeed;

        Destroy(gameObject, projectileLifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}