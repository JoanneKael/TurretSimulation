using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform enemyRoot;
    public Transform[] spawnPoints;
    public Transform lookAtCenter;

    public float spawnTimer = 0f;
    public float spawnIntervalSeconds = 1f;
    public float enemyMoveSpeedUnitsPerSecond = 5f;
    public float enemyLifeTimeSeconds = 10f;

    private IObjectPool<GameObject> enemyPool;

    private void Awake()
    {
        enemyPool = new ObjectPool<GameObject>(
            createFunc: CreateEnemy,
            actionOnGet: OnGetEnemy,
            actionOnRelease: OnReleaseEnemy,
            actionOnDestroy: OnDestroyEnemy,
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 100);
    }

    private GameObject CreateEnemy() => Instantiate(enemyPrefab, enemyRoot);
    private void OnGetEnemy(GameObject enemy) => enemy.SetActive(true);
    private void OnReleaseEnemy(GameObject enemy) => enemy.SetActive(false);
    private void OnDestroyEnemy(GameObject enemy) => Destroy(enemy);

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if(spawnTimer>= spawnIntervalSeconds)
        {
            TrySpawnOneEnemy();
            spawnTimer = 0f;
        }
    }

    private bool TrySpawnOneEnemy()
    {
        if (enemyPrefab == null || spawnPoints == null || spawnPoints.Length == 0)
        {
            return false;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        if (spawnPoint == null)
        {
            return false;
        }

        Quaternion rotation = spawnPoint.rotation;
        if (lookAtCenter != null)
        {
            Vector3 toCenter = lookAtCenter.position - spawnPoint.position;

            toCenter.y = 0f;

            if (toCenter.sqrMagnitude > 1e-8f)
            {
                rotation = Quaternion.LookRotation(toCenter.normalized, Vector3.up);
            }
        }

        GameObject spawned = enemyPool.Get();

        spawned.transform.position = spawnPoint.position;
        spawned.transform.rotation = rotation;

        EnemyLinearMover mover = spawned.GetComponent<EnemyLinearMover>();
        if (mover != null)
        {
            mover.Initialize(enemyPool, enemyMoveSpeedUnitsPerSecond, enemyLifeTimeSeconds);
        }

        return true;
    }

}