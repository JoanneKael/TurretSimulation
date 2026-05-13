using System.Collections;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public Transform orbitPivot;
    public float orbitSpeed = 30f;
    public int hp = 3;

    public AudioSource audioSource;
    public AudioClip clip;

    private void Update()
    {
        if (orbitPivot == null) return;

        orbitPivot.Rotate(Vector3.up, orbitSpeed * Time.deltaTime, Space.Self);
    }

    public void GetDamage(int amount)
    {
        hp -= amount;

        if (hp <= 0) StartCoroutine(IEDamage());
    }

    private IEnumerator IEDamage()
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }

        float timer = 0f;
        float duration = 0.3f;

        while (timer <= duration)
        {
            timer += Time.deltaTime;

            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timer / duration);

            yield return null;
        }

        Destroy(gameObject);
    }
}