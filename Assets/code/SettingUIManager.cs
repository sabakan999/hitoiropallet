using UnityEngine;
using UnityEngine.UI;

public class SettingUIManager : MonoBehaviour
{
    [Header("設定パネル")]
    [SerializeField]
    private GameObject settingPanel;

    [Header("音量表示")]
    [SerializeField]
    private Text bgmValueText;

    [SerializeField]
    private Text seValueText;

    [Header("カーソル感度")]
    [SerializeField]
    private Text cursorValueText;

    [SerializeField]
    private SelectDescriptionManager descriptionManager;

    private const float step = 0.1f;

    private void Start()
    {
        settingPanel.SetActive(false);

        RefreshText();
    }

    /*
     * 保存されている設定をUIへ反映
     */
    private void RefreshText()
    {
        if (SettingManager.Instance == null)
        {
            return;
        }

        SettingManager.Instance.LoadSetting();

        bgmValueText.text =
            Mathf.RoundToInt(
                SettingManager.Instance.BGMVolume * 100f
            ) + "%";

        seValueText.text =
            Mathf.RoundToInt(
                SettingManager.Instance.SEVolume * 100f
            ) + "%";

        cursorValueText.text =
            SettingManager.Instance.CursorSpeed.ToString("0.0");
    }

    /*
     * パネルを開く
     */
    public void OpenSetting()
    {
        RefreshText();

        settingPanel.SetActive(true);

        UIState.IsWindowOpen = true;
        UIState.CurrentWindow = settingPanel.transform;

        descriptionManager.Clear();
    }

    /*
     * パネルを閉じる
     */
    public void CloseSetting()
    {
        settingPanel.SetActive(false);

        SEManager.Instance.PlayBack();

        UIState.IsWindowOpen = false;
        UIState.CurrentWindow = null;
    }

    /*
     * BGM ＋
     */
    public void BGMPlus()
    {
        float value =
            Mathf.Clamp01(
                SettingManager.Instance.BGMVolume + step
            );

        SettingManager.Instance.SetBGM(value);

        SEManager.Instance.PlayDecide();

        RefreshText();
    }

    /*
     * BGM －
     */
    public void BGMMinus()
    {
        float value =
            Mathf.Clamp01(
                SettingManager.Instance.BGMVolume - step
            );

        SettingManager.Instance.SetBGM(value);

        SEManager.Instance.PlayDecide();

        RefreshText();
    }

    /*
     * SE ＋
     */
    public void SEPlus()
    {
        float value =
            Mathf.Clamp01(
                SettingManager.Instance.SEVolume + step
            );

        SettingManager.Instance.SetSE(value);

        SEManager.Instance.PlayDecide();

        RefreshText();
    }

    /*
     * SE －
     */
    public void SEMinus()
    {
        float value =
            Mathf.Clamp01(
                SettingManager.Instance.SEVolume - step
            );

        SettingManager.Instance.SetSE(value);

        SEManager.Instance.PlayDecide();

        RefreshText();
    }

    /*
     * カーソル感度 ＋
     */
    public void CursorPlus()
    {
        float value =
            SettingManager.Instance.CursorSpeed + step;

        SettingManager.Instance.SetCursorSpeed(value);

        SEManager.Instance.PlayDecide();

        RefreshText();
    }

    /*
     * カーソル感度 －
     */
    public void CursorMinus()
    {
        float value =
            SettingManager.Instance.CursorSpeed - step;

        SettingManager.Instance.SetCursorSpeed(value);

        SEManager.Instance.PlayDecide();

        RefreshText();
    }

    /*
     * フルスクリーン
     */
    public void FullScreen()
    {
        if (SettingManager.Instance != null)
        {
            SettingManager.Instance.SetFullScreen();

            SEManager.Instance.PlayDecide();
        }
    }

    /*
     * ウィンドウ
     */
    public void Window()
    {
        if (SettingManager.Instance != null)
        {
            SettingManager.Instance.SetWindow();

            SEManager.Instance.PlayDecide();
        }
    }
}