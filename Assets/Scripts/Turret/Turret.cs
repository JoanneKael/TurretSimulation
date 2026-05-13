using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Turret : MonoBehaviour
{
    public Transform yawPivot;
    public Transform targetDrone;
    public float rotationSpeed = 20f;

    private int maxHP = 10;
    public int currHP;

    public float invincibleTimer;
    public bool isInvincible;

    private void Start()
    {
        currHP = maxHP;
    }

    private void Update()
    {
        if(targetDrone == null) return;

        Vector3 direction = targetDrone.position - yawPivot.position;

        direction.y = 0f;

        if(direction.sqrMagnitude > 0.001)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            yawPivot.rotation = Quaternion.RotateTowards(yawPivot.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void GetDamage(int amount)
    {
        StartCoroutine(IEGetDamage(amount));
    }

    private IEnumerator IEGetDamage(int amount)
    {
        currHP -= amount;

        if (currHP <= 0)
        {
            StartCoroutine(IEDie());
            yield break;
        }

        float elapsed = 0f;

        while(elapsed < invincibleTimer)
        {

        }
    }

    private IEnumerator IEDie()
    {
        //float elapsed = 0f;

        

        yield return null;
    }

}