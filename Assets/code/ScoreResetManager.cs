using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ScoreResetManager : MonoBehaviour
{
    [SerializeField]
    private string titleScene = "select";

    [SerializeField]
    private float holdTime = 10f;

    private float timer = 0f;

    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.deleteKey.isPressed)
        {
            timer += Time.unscaledDeltaTime;

            if (timer >= holdTime)
            {
                ResetHighScore();
            }
        }
        else
        {
            timer = 0f;
        }
    }

    /*
     * ハイスコア初期化
     */
    private void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore_Easy");
        PlayerPrefs.DeleteKey("HighScore_Normal");
        PlayerPrefs.DeleteKey("HighScore_Hard");
        PlayerPrefs.DeleteKey("HighScore_Hell");
        PlayerPrefs.DeleteKey("HellSeen");
        PlayerPrefs.Save();

        HighScoreData.Load();

        Time.timeScale = 1f;

        SceneManager.LoadScene(titleScene);
    }
}