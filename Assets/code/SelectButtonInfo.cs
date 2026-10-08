using UnityEngine;

public class SelectButtonInfo : MonoBehaviour
{
public enum InfoType
{
    Easy,
    Normal,
    Hard,
    Hell,
    Tutorial,
    Setting
}

    public InfoType infoType;

    [TextArea]
    public string description;
}