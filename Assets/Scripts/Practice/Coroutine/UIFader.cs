using System.Collections;
using UnityEngine;

public class UIFader : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;

    public void FadeIn()
    {
        StartCoroutine(IEFade(0f, 1f));
    }

    public void FadeOut()
    {
        StartCoroutine(IEFade(1f, 0f));
    }

    private IEnumerator IEFade(float from, float to)
    {
        float time = 0f;
        canvasGroup.alpha = from;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, time/ fadeDuration);

            yield return null;
        }

        canvasGroup.alpha = to;
    }
}