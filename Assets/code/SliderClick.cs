using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Slider slider;

    public void OnPointerClick(PointerEventData eventData)
    {
        RectTransform area =
            slider.fillRect.parent as RectTransform;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            area,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPos
        );

        float t = Mathf.InverseLerp(
            area.rect.xMin,
            area.rect.xMax,
            localPos.x
        );

        slider.value = Mathf.Lerp(
            slider.minValue,
            slider.maxValue,
            t
        );
    }
}