using System;
using System.Collections;
using UnityEngine;

public class CoroutineBasicSimple : MonoBehaviour
{
    public float cooldown = 2f;
    private bool canUseSkill = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryUseSkill();
        }
    }

    private void TryUseSkill()
    {
        if (!canUseSkill) return;

        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        canUseSkill = false;

        yield return new WaitForSeconds(cooldown);

        canUseSkill = true;
    }
}
