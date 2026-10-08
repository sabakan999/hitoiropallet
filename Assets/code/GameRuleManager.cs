using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRuleManager : MonoBehaviour
{
    [Header("ゲーム設定")]
    [SerializeField]
    private float gameTime = 60f;

    [SerializeField]
    private int scorePerCorrect = 100;

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private UIManager uiManager;

    public float RemainingTime { get; private set; }
    public int Score { get; private set; }
    public int Combo { get; private set; }
    public int MaxCombo { get; private set; }
    public int CorrectCount { get; private set; }
    public bool IsPlaying { get; private set; }

    private void Start()
    {
        StartCoroutine(StartGameSequence());
    }

    private void Update()
    {
        if (!IsPlaying)
        {
            return;
        }

        RemainingTime -= Time.deltaTime;

        if (RemainingTime < 0f)
        {
            RemainingTime = 0f;
        }

        uiManager.SetTime(RemainingTime);

        if (RemainingTime <= 0f)
        {
            StartCoroutine(EndGameSequence());
        }
    }

    /*
     * READY → GO → ゲーム開始
     */
    private IEnumerator StartGameSequence()
    {
        Score = 0;
        Combo = 0;
        MaxCombo = 0;
        CorrectCount = 0;

        RemainingTime = gameTime;
        IsPlaying = false;

        uiManager.ResetUI();
        uiManager.SetTime(RemainingTime);

        uiManager.ShowReadyGo("レディー…");
        yield return new WaitForSeconds(0.8f);

        uiManager.ShowReadyGo("ゴー！！！");
        SEManager.Instance.PlayStart();
        yield return new WaitForSeconds(0.5f);

        gameManager.PaintCurrentIllustration();

        uiManager.HideReadyGo();
        IsPlaying = true;

        Debug.Log("ゲームスタート！");
    }

    /*
     * 正解
     */
    public void Correct()
    {
        if (!IsPlaying)
        {
            return;
        }

        Combo++;
        CorrectCount++;

        if (Combo > MaxCombo)
        {
            MaxCombo = Combo;
        }

        Score += scorePerCorrect * Combo;

        uiManager.SetScore(Score);
        uiManager.SetCombo(Combo);

        SEManager.Instance.PlayCorrect();

        Debug.Log(
            $"正解！ Score:{Score} Combo:{Combo}"
        );

        gameManager.CorrectAnswer();
    }

    /*
     * ミス
     */
    public void Miss()
    {
        if (!IsPlaying)
        {
            return;
        }

        Combo = 0;

        uiManager.SetCombo(Combo);

        SEManager.Instance.PlayMiss();

        Debug.Log("ミス！");
    }

    /*
     * TIME UP → 正解演出 → リザルト
     */
    private IEnumerator EndGameSequence()
    {
        IsPlaying = false;

        SEManager.Instance.PlayFinish();

        yield return StartCoroutine(
            gameManager.ShowAnswerRoutine()
        );

        /*
         * ハイスコア更新
         */
        HighScoreData.Load();

        switch (GameSettings.SelectedDifficulty)
        {
            case Difficulty.Easy:
                if (Score > HighScoreData.Easy)
                {
                    HighScoreData.Easy = Score;
                }
                break;

            case Difficulty.Normal:
                if (Score > HighScoreData.Normal)
                {
                    HighScoreData.Normal = Score;
                }
                break;

            case Difficulty.Hard:
                if (Score > HighScoreData.Hard)
                {
                    HighScoreData.Hard = Score;
                }
                break;
            case Difficulty.Hell:
                if (Score > HighScoreData.Hell)
                {
                    HighScoreData.Hell = Score;
                }
                break;
        }

        HighScoreData.Save();

        uiManager.ShowResult(
            Score,
            MaxCombo,
            CorrectCount
        );

        Debug.Log(
            $"ゲーム終了！ 最終スコア:{Score}"
        );
    }

    /*
     * リトライ
     */
    public void Retry()
    {
        StartCoroutine(
            SEManager.Instance.PlayDecideThen(() =>
            {
                SceneManager.LoadScene(
                    SceneManager.GetActiveScene().buildIndex
                );
            })
        );
    }

    /*
     * タイトルへ戻る
     */
    public void BackToTitle()
    {
        StartCoroutine(
            SEManager.Instance.PlayBackThen(() =>
            {
                SceneManager.LoadScene("select");
            })
        );
    }
}