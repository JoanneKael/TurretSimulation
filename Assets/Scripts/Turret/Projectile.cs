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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            Debug.Log(other.gameObject.layer);
            Drone d = other.gameObject.GetComponent<Drone>();
            d.GetDamage(1);
        }
    }
}