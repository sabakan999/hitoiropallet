using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [Header("遷移先シーン名")]
    [SerializeField]
    private string nextScene = "Select";

    [Header("タイトル用ぬりえ")]
    [SerializeField]
    private Texture2D[] titleIllustrations;

    [SerializeField]
    private RawImage illustrationImage;

    private bool isLoading = false;

    private void Start()
    {
        ShowRandomIllustration();
    }

    private void Update()
{
    if (isLoading)
    {
        return;
    }

    bool key =
        Keyboard.current != null &&
        Keyboard.current.anyKey.wasPressedThisFrame;

    bool mouse =
        Mouse.current != null &&
        Mouse.current.leftButton.wasPressedThisFrame;

    bool gamepad =
        Gamepad.current != null &&
        Gamepad.current.allControls
            .OfType<ButtonControl>()
            .Any(button => button.wasPressedThisFrame);

    bool joystick =
        Joystick.current != null &&
        Joystick.current.allControls
            .OfType<ButtonControl>()
            .Any(button => button.wasPressedThisFrame);

    if (key || mouse || gamepad || joystick)
    {
        isLoading = true;

        StartCoroutine(
            SEManager.Instance.PlayTitleClickThen(() =>
            {
                SceneManager.LoadScene(nextScene);
            })
        );
    }
}

    /*
     * タイトル用ぬりえをランダム表示
     */
    private void ShowRandomIllustration()
    {
        if (titleIllustrations == null ||
            titleIllustrations.Length == 0)
        {
            Debug.LogWarning("タイトル用ぬりえが登録されていません。");
            return;
        }

        int randomIndex =
            Random.Range(0, titleIllustrations.Length);

        illustrationImage.texture =
            titleIllustrations[randomIndex];
    }
}