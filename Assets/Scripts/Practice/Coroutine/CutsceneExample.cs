using System.Collections;
using UnityEngine;

public class CutsceneExample : MonoBehaviour
{
    [SerializeField] private Transform cameraRig;
    [SerializeField] private Transform bossSpawnPoint;
    [SerializeField] private GameObject bossPrefab;

    public void PlayBossIntro()
    {
        StartCoroutine(IEBossIntro());
    }

    private IEnumerator IEBossIntro()
    {
        Debug.Log("플레이어 입력 잠금");

        yield return IEMoveCameraToBossPoint();

        Debug.Log("보스 등장 연출");

        Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);

        yield return new WaitForSeconds(2f);

        Debug.Log("보스 체력바 표시");

        yield return new WaitForSeconds(1f);

        Debug.Log("플레이어 입력 잠금해제");
    }

    private IEnumerator IEMoveCameraToBossPoint()
    {
        Vector3 start = cameraRig.position;
        Vector3 end = bossSpawnPoint.position + new Vector3(0f, 3f, -6f);

        float duration = 1.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cameraRig.position = Vector3.Lerp(start, end, elapsed/duration);
            yield return null;
        }

        cameraRig.position = end;
    }
}
