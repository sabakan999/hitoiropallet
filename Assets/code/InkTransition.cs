using System.Collections;
using UnityEngine;

public class InkTransition : MonoBehaviour
{
    [SerializeField]
    private RectTransform effectArea;

    [SerializeField]
    private GameObject[] inkPrefabs;

    [SerializeField]
    private int inkCount = 8;

    /*
     * 外部から呼ぶ
     */
    public void PlayTransition()
    {
        StartCoroutine(PlayRoutine());
    }

    private IEnumerator PlayRoutine()
    {
        for (int i = 0; i < inkCount; i++)
        {
            SpawnInk();

            yield return new WaitForSeconds(0.08f);
        }
    }

    private void SpawnInk()
    {
        if (inkPrefabs.Length == 0)
        {
            return;
        }

        GameObject prefab =
            inkPrefabs[
                Random.Range(0, inkPrefabs.Length)
            ];

        GameObject ink =
            Instantiate(prefab, effectArea);

        RectTransform rect =
            ink.GetComponent<RectTransform>();

        Rect area = effectArea.rect;

        rect.localPosition =
            new Vector2(
                Random.Range(area.xMin, area.xMax),
                Random.Range(area.yMin, area.yMax)
            );

        float scale =
            Random.Range(0.8f, 1.3f);

        rect.localScale =
            Vector3.one * scale;

        rect.localRotation =
            Quaternion.Euler(
                0,
                0,
                Random.Range(0f, 360f)
            );
    }
}