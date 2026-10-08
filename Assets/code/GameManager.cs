using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameRuleManager ruleManager;

    [Header("かんたん")]
    [SerializeField]
    private Texture2D[] easyIllustrations;

    [Header("ふつう")]
    [SerializeField]
    private Texture2D[] normalIllustrations;

    [Header("むずかしい")]
    [SerializeField]
    private Texture2D[] hardIllustrations;

    [Header("じごく")]
    [SerializeField]
    private Texture2D[] hellIllustrations;

    [Header("表示先")]
    [SerializeField]
    private RawImage rawImage;

    [SerializeField]
    private ColorPainter colorPainter;

    [SerializeField]
    private GameCursor gameCursor;

    [Header("切り替え演出")]
    [SerializeField]
    private InkTransition inkTransition;

    private Texture2D[] currentIllustrations;
    private List<Region> currentRegions;
    private Texture2D currentTexture;

    private Dictionary<int, List<Region>> regionCache =
        new Dictionary<int, List<Region>>();

    /* 出題順 */
    private List<int> illustrationOrder =
        new List<int>();

    private int currentIllustrationIndex = 0;
    private int correctCount = 0;

    private void Start()
    {
        SelectIllustrationList();

        ShuffleIllustrations();

        PreloadRegions();

        CreateNewProblem(false);
    }

    /*
     * 難易度選択
     */
    private void SelectIllustrationList()
    {
        switch (GameSettings.SelectedDifficulty)
        {
            case Difficulty.Easy:
                currentIllustrations =
                    easyIllustrations;
                break;

            case Difficulty.Normal:
                currentIllustrations =
                    normalIllustrations;
                break;

            case Difficulty.Hard:
                currentIllustrations =
                    hardIllustrations;
                break;

            case Difficulty.Hell:
                currentIllustrations =
                    hellIllustrations;
                break;
        }

        currentIllustrationIndex = 0;
    }

    /*
     * 出題順をランダム化
     */
    private void ShuffleIllustrations()
    {
        illustrationOrder.Clear();

        for (int i = 0;
             i < currentIllustrations.Length;
             i++)
        {
            illustrationOrder.Add(i);
        }

        for (int i = 0;
             i < illustrationOrder.Count;
             i++)
        {
            int random =
                Random.Range(
                    i,
                    illustrationOrder.Count
                );

            int temp =
                illustrationOrder[i];

            illustrationOrder[i] =
                illustrationOrder[random];

            illustrationOrder[random] =
                temp;
        }
    }

    /*
     * Region先読み
     */
    private void PreloadRegions()
    {
        regionCache.Clear();

        RegionDetector detector =
            new RegionDetector();

        for (int i = 0;
             i < currentIllustrations.Length;
             i++)
        {
            regionCache[i] =
                detector.Detect(
                    currentIllustrations[i]
                );
        }

        Debug.Log("先読み完了");
    }

    /*
     * 問題生成
     */
    private void CreateNewProblem(
        bool paintNow)
    {
        if (currentIllustrations.Length == 0)
        {
            Debug.LogError(
                "イラストが登録されていません。"
            );
            return;
        }

        int illustrationID =
            illustrationOrder[
                currentIllustrationIndex
            ];

        currentRegions =
            CloneRegions(
                regionCache[illustrationID]
            );

        if (paintNow)
        {
            PaintCurrentIllustration();
        }
        else
        {
            rawImage.texture = null;

            gameCursor.SetCurrentImage(
                null,
                currentRegions
            );
        }
    }

    /*
     * 描画
     */
    public void PaintCurrentIllustration()
    {
        int illustrationID =
            illustrationOrder[
                currentIllustrationIndex
            ];

        Texture2D source =
            currentIllustrations[
                illustrationID
            ];

        currentTexture =
            colorPainter.Paint(
                source,
                currentRegions
            );

        rawImage.texture =
            currentTexture;

        gameCursor.SetCurrentImage(
            currentTexture,
            currentRegions
        );

        Debug.Log(
            $"{GameSettings.SelectedDifficulty} {currentIllustrationIndex + 1}/{currentIllustrations.Length}"
        );
    }

    /*
     * クリック判定
     */
    public void SelectRegion(
        Region region)
    {
        if (region.isIsshiki)
        {
            ruleManager.Correct();
        }
        else
        {
            ruleManager.Miss();
        }
    }

    /*
     * 正解
     */
    public void CorrectAnswer()
    {
        correctCount++;

        bool illustrationChanged =
            false;

        if (correctCount >= 3)
        {
            correctCount = 0;

            currentIllustrationIndex++;

            if (currentIllustrationIndex >=
                currentIllustrations.Length)
            {
                currentIllustrationIndex = 0;

                ShuffleIllustrations();
            }

            illustrationChanged = true;
        }

        CreateNewProblem(true);

        if (illustrationChanged &&
            inkTransition != null)
        {
            inkTransition.PlayTransition();
        }
    }

    /*
     * タイムアップ演出
     */
    public IEnumerator ShowAnswerRoutine()
    {
        Region isshiki = null;

        foreach (Region region in currentRegions)
        {
            if (region.isIsshiki)
            {
                isshiki = region;
                break;
            }
        }

        if (isshiki == null)
        {
            yield break;
        }

        Color[] darkPixels =
            (Color[])currentTexture
                .GetPixels()
                .Clone();

        for (int i = 0;
             i < darkPixels.Length;
             i++)
        {
            darkPixels[i] =
                Color.Lerp(
                    darkPixels[i],
                    Color.black,
                    0.35f
                );
        }

        HashSet<Vector2Int> isshikiPixels =
            new HashSet<Vector2Int>(
                isshiki.pixels
            );

        for (int blink = 0;
             blink < 4;
             blink++)
        {
            currentTexture.SetPixels(
                darkPixels
            );

            foreach (Vector2Int pixel
                in isshikiPixels)
            {
                currentTexture.SetPixel(
                    pixel.x,
                    pixel.y,
                    Color.white
                );
            }

            currentTexture.Apply();

            yield return
                new WaitForSeconds(0.25f);

            currentTexture.SetPixels(
                darkPixels
            );

            currentTexture.Apply();

            yield return
                new WaitForSeconds(0.25f);
        }

        currentTexture.SetPixels(
            darkPixels
        );

        foreach (Vector2Int pixel
            in isshikiPixels)
        {
            currentTexture.SetPixel(
                pixel.x,
                pixel.y,
                Color.white
            );
        }

        currentTexture.Apply();
    }

    /*
     * 正解数取得
     */
    public int GetCorrectCount()
    {
        return correctCount;
    }

    /*
     * Region複製
     */
    private List<Region> CloneRegions(
        List<Region> source)
    {
        List<Region> clone =
            new List<Region>();

        foreach (Region region in source)
        {
            Region newRegion =
                new Region();

            newRegion.id =
                region.id;

            newRegion.center =
                region.center;

            newRegion.pixels =
                region.pixels;

            newRegion.colorId = -1;
            newRegion.isIsshiki = false;

            clone.Add(newRegion);
        }

        return clone;
    }

    /*
     * 色取得
     */
    public Color GetRegionColor(
        Region region)
    {
        return colorPainter.GetColor(
            region.colorId
        );
    }
}