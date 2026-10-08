using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private GameObject tutorialPanel;
    [SerializeField]
    private SelectDescriptionManager descriptionManager;

    private void Start()
    {
        tutorialPanel.SetActive(false);
    }

    /*
     * 説明を開く
     */
   public void OpenTutorial()
{
    tutorialPanel.SetActive(true);

    UIState.IsWindowOpen = true;
    UIState.CurrentWindow = tutorialPanel.transform;

    descriptionManager.Clear();
}

    /*
     * 閉じる
     */
    public void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
        SEManager.Instance.PlayBack();
        UIState.IsWindowOpen = false;
        UIState.CurrentWindow = null;
    }

    
}