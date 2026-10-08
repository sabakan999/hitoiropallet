using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance;

    private const string BGM_KEY = "BGM";
    private const string SE_KEY = "SE";
    private const string FULLSCREEN_KEY = "FULLSCREEN";
    private const string CURSOR_KEY = "CURSOR_SPEED";

    public float BGMVolume { get; private set; }
    public float SEVolume { get; private set; }
    public bool IsFullScreen { get; private set; }

    // 0.1～2.0
    public float CursorSpeed { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadSetting();
        ApplyScreen();
    }

    public void LoadSetting()
    {
        BGMVolume =
            PlayerPrefs.GetFloat(BGM_KEY, 1f);

        SEVolume =
            PlayerPrefs.GetFloat(SE_KEY, 1f);

        IsFullScreen =
            PlayerPrefs.GetInt(FULLSCREEN_KEY, 1) == 1;

        CursorSpeed =
            PlayerPrefs.GetFloat(CURSOR_KEY, 1f);
    }

    public void SetBGM(float value)
    {
        BGMVolume = value;
        PlayerPrefs.SetFloat(BGM_KEY, value);
        PlayerPrefs.Save();
    }

    public void SetSE(float value)
    {
        SEVolume = value;
        PlayerPrefs.SetFloat(SE_KEY, value);
        PlayerPrefs.Save();
    }

    public void SetCursorSpeed(float value)
    {
        CursorSpeed =
            Mathf.Clamp(value, 0.1f, 2f);

        PlayerPrefs.SetFloat(
            CURSOR_KEY,
            CursorSpeed
        );

        PlayerPrefs.Save();
    }

    public void SetFullScreen()
    {
        IsFullScreen = true;
        PlayerPrefs.SetInt(FULLSCREEN_KEY, 1);
        PlayerPrefs.Save();
        ApplyScreen();
    }

    public void SetWindow()
    {
        IsFullScreen = false;
        PlayerPrefs.SetInt(FULLSCREEN_KEY, 0);
        PlayerPrefs.Save();
        ApplyScreen();
    }

    private void ApplyScreen()
    {
        Screen.fullScreen = IsFullScreen;
    }
}