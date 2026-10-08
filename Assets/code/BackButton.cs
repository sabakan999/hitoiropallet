using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    [Header("戻るシーン名")]
    [SerializeField]
    private string backScene = "Title";

    /*
     * 戻るボタン
     */
    public void Back()
    {
        StartCoroutine(
    SEManager.Instance.PlayBackThen(() =>
    {
        SceneManager.LoadScene(backScene);
    })
);
    }
}