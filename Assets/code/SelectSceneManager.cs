using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectSceneManager : MonoBehaviour
{
    [Header("ゲームシーン名")]
    [SerializeField]
    private string gameSceneName = "Game";

    [Header("隠し要素")]
    [SerializeField]
    private GameObject hellButton;

  

    [SerializeField]
    private HellUnlockUI unlockUI;
    


    private void Start()
    {
        HighScoreData.Load();

        bool unlocked =
            HighScoreData.IsHellUnlocked();

        hellButton.SetActive(unlocked);

        if (unlocked &&
            !HighScoreData.HellSeen)
        {
            HighScoreData.HellSeen = true;
            HighScoreData.Save();

            unlockUI.Play();
        }
    }

    /*
     * かんたん
     */
    public void SelectEasy()
    {
        GameSettings.SelectedDifficulty =
            Difficulty.Easy;
        StartCoroutine(
    SEManager.Instance.PlayDecideThen(() =>
    {
        SceneManager.LoadScene(gameSceneName);
    })
);

       
    }

    /*
     * ふつう
     */
    public void SelectNormal()
    {
        GameSettings.SelectedDifficulty =
            Difficulty.Normal;
            StartCoroutine(
    SEManager.Instance.PlayDecideThen(() =>
    {
        SceneManager.LoadScene(gameSceneName);
    })
);

    }

    /*
     * むずかしい
     */
    public void SelectHard()
    {
        GameSettings.SelectedDifficulty =
            Difficulty.Hard;

            StartCoroutine(
    SEManager.Instance.PlayDecideThen(() =>
    {
        SceneManager.LoadScene(gameSceneName);
    })

);
    }

/*
 * じごく
 */
    public void SelectHell()
    {
        GameSettings.SelectedDifficulty =
            Difficulty.Hell;

        StartCoroutine(
            SEManager.Instance.PlayDecideThen(() =>
            {
                SceneManager.LoadScene(gameSceneName);
            })
        );
    }



    
}