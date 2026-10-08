using UnityEngine;
using UnityEngine.UI;

public class SelectDescriptionManager : MonoBehaviour
{
    [SerializeField]
    private Text descriptionText;

    private void Start()
    {
        HighScoreData.Load();
    }

    public void Show(SelectButtonInfo info)
    {
        switch (info.infoType)
        {
            case SelectButtonInfo.InfoType.Easy:
                descriptionText.text =
                    $"{info.description}\n\nハイスコア : " +
                    HighScoreData.Easy;
                break;

            case SelectButtonInfo.InfoType.Normal:
                descriptionText.text =
                    $"{info.description}\n\nハイスコア : " +
                    HighScoreData.Normal;
                break;

            case SelectButtonInfo.InfoType.Hard:
                descriptionText.text =
                    $"{info.description}\n\nハイスコア : " +
                    HighScoreData.Hard;
                break;
            
            case SelectButtonInfo.InfoType.Hell:
                descriptionText.text =
                    $"{info.description}\n\nハイスコア : " +
                    HighScoreData.Hell;
                break;

            default:
                descriptionText.text =
                    info.description;
                break;
        }
    }

    public void Clear()
    {
        descriptionText.text = "";
    }
}