using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("ポーズUI")]
    [SerializeField]
    private GameObject pausePanel;

    [Header("シーン名")]
    [SerializeField]
    private string selectScene = "select";

    /* GameCursorから参照する */
    public static bool IsPaused { get; private set; }

    private void Start()
    {
        Time.timeScale = 1f;
        IsPaused = false;

        pausePanel.SetActive(false);
    }

    /*
     * ポーズ入力
     * 右クリック / Startボタン
     */
    public void OnPause(
        InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        if (IsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    /*
     * ポーズ開始
     */
    public void Pause()
    {
        IsPaused = true;

        Time.timeScale = 0f;

        pausePanel.SetActive(true);
    }

    /*
     * 再開
     */
    public void Resume()
    {
        IsPaused = false;

        Time.timeScale = 1f;
        SEManager.Instance.PlayDecide();

        pausePanel.SetActive(false);
    }

    /*
     * リトライ
     */
    public void Retry()
    {
        IsPaused = false;
        StartCoroutine(
    SEManager.Instance.PlayDecideThen(() =>
    {
        Time.timeScale = 1f;
    })
);
       

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    /*
     * セレクトへ戻る
     */
    public void BackToTitle()
    {
        IsPaused = false;
        Time.timeScale = 1f;

       StartCoroutine(
    SEManager.Instance.PlayDecideThen(() =>
    {
        SceneManager.LoadScene(selectScene);
    })
);
    }
}