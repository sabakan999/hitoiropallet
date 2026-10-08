using System.Collections;
using UnityEngine;

public class HellUnlockUI : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    public void Play()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        // フェードイン
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = t;
            yield return null;
        }

        canvasGroup.alpha = 1f;

        // 5秒表示
        yield return new WaitForSeconds(5f);

        // フェードアウト
        t = 1f;

        while (t > 0f)
        {
            t -= Time.deltaTime;
            canvasGroup.alpha = t;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
}