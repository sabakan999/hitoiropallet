using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("ゲームUI")]
    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Text comboText;

    [SerializeField]
    private Text timeText;

    [Header("READY・GO")]
    [SerializeField]
    private Text readyGoText;

    [Header("リザルトUI")]
    [SerializeField]
    private GameObject resultPanel;

    [SerializeField]
    private Text resultScoreText;

    [SerializeField]
    private Text resultMaxComboText;

    [SerializeField]
    private Text resultCorrectText;

    private void Awake()
    {
        resultPanel.SetActive(false);

        readyGoText.gameObject.SetActive(false);
    }

    /*
     * スコア表示
     */
    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
    }

    /*
     * コンボ表示
     */
    public void SetCombo(int combo)
    {
        if (combo <= 1)
        {
            comboText.text = "";
        }
        else
        {
            comboText.text = combo + " COMBO";
        }
    }

    /*
     * タイム表示
     */
    public void SetTime(float time)
    {
        int second = Mathf.CeilToInt(time);

        timeText.text = second.ToString();
    }

    /*
     * READY / GO 表示
     */
    public void ShowReadyGo(string message)
    {
        readyGoText.gameObject.SetActive(true);

        readyGoText.text = message;
    }

    /*
     * READY / GO 非表示
     */
    public void HideReadyGo()
    {
        readyGoText.gameObject.SetActive(false);
    }

    /*
     * リザルト表示
     */
    public void ShowResult(
        int score,
        int maxCombo,
        int correct)
    {
        resultPanel.SetActive(true);

        resultScoreText.text =
            score.ToString();

        resultMaxComboText.text =
            maxCombo.ToString();

        resultCorrectText.text =
            correct.ToString();
    }

    /*
     * ゲーム開始時に呼ぶ
     */
    public void ResetUI()
    {
        resultPanel.SetActive(false);

        HideReadyGo();

        SetScore(0);
        SetCombo(0);
        SetTime(0);
    }
}