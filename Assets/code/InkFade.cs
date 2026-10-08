using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InkFade : MonoBehaviour
{
    [SerializeField]
    private float lifeTime = 1f;

    private Image image;

    private void Awake()
    {
        image =
            GetComponent<Image>();
    }

    private void Start()
    {
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        Color color =
            image.color;

        float time = 0f;

        while (time < lifeTime)
        {
            time += Time.deltaTime;

            color.a =
                Mathf.Lerp(
                    1f,
                    0f,
                    time / lifeTime
                );

            image.color = color;

            yield return null;
        }

        Destroy(gameObject);
    }
}