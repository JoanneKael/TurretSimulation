using UnityEngine;
using UnityEngine.Pool;

public class EnemyLinearMover : MonoBehaviour
{
    private float moveSpeedUnitsPerSecond = 5f;
    private float lifeTimeSeconds = 10f;
    private float remainingLifeSeconds;

    private IObjectPool<GameObject> pool;

    public void Initialize(IObjectPool<GameObject> targetPool, float speedUnitsPerSecond, float lifeTime)
    {
        pool = targetPool;
        moveSpeedUnitsPerSecond = speedUnitsPerSecond;
        lifeTimeSeconds = lifeTime;
        remainingLifeSeconds = lifeTimeSeconds;
    }

    private void OnEnable()
    {
        remainingLifeSeconds = lifeTimeSeconds;
    }

    private void Update()
    {
        // Projectile과 동일하게 forward 기준 등속 이동으로 단순한 테스트 타겟 행동을 보장합니다.
        transform.position += transform.forward * (moveSpeedUnitsPerSecond * Time.deltaTime);

        remainingLifeSeconds -= Time.deltaTime;
        if (remainingLifeSeconds <= 0f)
        {
            if (pool != null)
            {
                pool.Release(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
