using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectCursorManager : MonoBehaviour
{
    [SerializeField]
    private float controllerSpeed = 900f;

    [SerializeField]
    private RectTransform canvasRect;

    [Header("UIクリック")]
    [SerializeField]
    private GraphicRaycaster raycaster;

    [SerializeField]
    private EventSystem eventSystem;

    [Header("説明表示")]
    [SerializeField]
    private SelectDescriptionManager descriptionManager;

    [Header("インク演出")]
    [SerializeField]
    private GameObject inkPrefab;

    [SerializeField]
    private RectTransform effectParent;

    private RectTransform cursorRect;
    private SelectButtonInfo currentInfo;

    /* 現在のスティック入力 */
    private Vector2 moveInput = Vector2.zero;

    private void Awake()
    {
        cursorRect =
            GetComponent<RectTransform>();
    }

    /*
     * 毎フレーム移動
     */
    private void Update()
    {
        if (moveInput.sqrMagnitude < 0.001f)
        {
            moveInput = Vector2.zero;
            return;
        }

        float speed =
            controllerSpeed *
            SettingManager.Instance.CursorSpeed;

        cursorRect.localPosition +=
            new Vector3(
                moveInput.x,
                moveInput.y,
                0f
            ) *
            speed *
            Time.unscaledDeltaTime;

        ClampPosition();

        UpdateDescription();
    }

    /*
     * マウス移動
     */
    public void OnPoint(
        InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        Vector2 screenPosition =
            context.ReadValue<Vector2>();

        RectTransformUtility
            .ScreenPointToLocalPointInRectangle(
                canvasRect,
                screenPosition,
                null,
                out Vector2 localPosition
            );

        cursorRect.localPosition =
            new Vector3(
                localPosition.x,
                localPosition.y,
                0f
            );

        ClampPosition();

        UpdateDescription();
    }

    /*
     * スティック入力取得
     */
    public void OnMove(
        InputAction.CallbackContext context)
    {
        moveInput =
            context.ReadValue<Vector2>();

        if (context.canceled)
        {
            moveInput = Vector2.zero;
        }
    }

    /*
     * 決定（A / 左クリック）
     */
    public void OnSubmit(
        InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        SpawnInk();

        ClickUI();
    }

    /*
     * インク生成
     */
    private void SpawnInk()
    {
        if (inkPrefab == null ||
            effectParent == null)
        {
            return;
        }

        GameObject ink =
            Instantiate(
                inkPrefab,
                effectParent
            );

        RectTransform inkRect =
            ink.GetComponent<RectTransform>();

        inkRect.localPosition =
            cursorRect.localPosition;
    }

    /*
     * 画面外に出ない
     */
    private void ClampPosition()
    {
        Rect rect =
            canvasRect.rect;

        Vector3 pos =
            cursorRect.localPosition;

        float halfW =
            cursorRect.rect.width * 0.5f;

        float halfH =
            cursorRect.rect.height * 0.5f;

        pos.x =
            Mathf.Clamp(
                pos.x,
                rect.xMin + halfW,
                rect.xMax - halfW
            );

        pos.y =
            Mathf.Clamp(
                pos.y,
                rect.yMin + halfH,
                rect.yMax - halfH
            );

        cursorRect.localPosition =
            pos;
    }

    /*
     * UIボタンを押す
     */
    private void ClickUI()
    {
        PointerEventData pointer =
            new PointerEventData(eventSystem);

        pointer.position =
            RectTransformUtility.WorldToScreenPoint(
                null,
                cursorRect.position
            );

        List<RaycastResult> results =
            new List<RaycastResult>();

        raycaster.Raycast(pointer, results);

        foreach (RaycastResult result in results)
        {
            Button button =
                result.gameObject.GetComponent<Button>();

            if (button == null)
            {
                continue;
            }

            if (UIState.IsWindowOpen)
            {
                if (!result.gameObject.transform.IsChildOf(
                        UIState.CurrentWindow))
                {
                    continue;
                }
            }

            button.onClick.Invoke();
            return;
        }

        if (SEManager.Instance != null)
        {
            SEManager.Instance.PlayEmptyClick();
        }
    }

    /*
     * 説明文更新
     */
    private void UpdateDescription()
    {
        PointerEventData pointer =
            new PointerEventData(eventSystem);

        pointer.position =
            RectTransformUtility.WorldToScreenPoint(
                null,
                cursorRect.position
            );

        List<RaycastResult> results =
            new List<RaycastResult>();

        raycaster.Raycast(pointer, results);

        foreach (RaycastResult result in results)
        {
            SelectButtonInfo info =
                result.gameObject.GetComponent<SelectButtonInfo>();

            if (info == null)
            {
                continue;
            }

            if (UIState.IsWindowOpen)
            {
                if (!result.gameObject.transform.IsChildOf(
                        UIState.CurrentWindow))
                {
                    continue;
                }
            }

            if (currentInfo != info)
            {
                currentInfo = info;
                descriptionManager.Show(info);
            }

            return;
        }

        currentInfo = null;
        descriptionManager.Clear();
    }
}