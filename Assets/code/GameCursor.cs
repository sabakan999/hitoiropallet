using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameCursor : MonoBehaviour
{
    [SerializeField]
    private float controllerSpeed = 900f;

    [SerializeField]
    private RectTransform canvasRect;

    [SerializeField]
    private RectTransform imageRect;

    [SerializeField]
    private GameManager gameManager;

    [Header("UIクリック")]
    [SerializeField]
    private GraphicRaycaster raycaster;

    [SerializeField]
    private EventSystem eventSystem;

    [Header("カーソル色表示")]
    [SerializeField]
    private Image colorPreview;

    [Header("〇×演出")]
    [SerializeField]
    private GameObject circlePrefab;

    [SerializeField]
    private GameObject crossPrefab;

    [SerializeField]
    private RectTransform effectParent;

    [Header("連打防止")]
    [SerializeField]
    private float clickInterval = 0.15f;

    private bool canClick = true;

    private Texture2D currentTexture;
    private List<Region> currentRegions;

    private RectTransform cursorRect;

    /* 現在のスティック入力 */
    private Vector2 moveInput = Vector2.zero;

    private void Awake()
    {
        cursorRect =
            GetComponent<RectTransform>();

        colorPreview.enabled = false;
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

        CheckHover();
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

        MoveToScreenPosition(screenPosition);

        CheckHover();
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
     * 決定
     */
    public void OnSubmit(
        InputAction.CallbackContext context)
    {
        if (!context.performed ||
            !canClick)
        {
            return;
        }

        StartCoroutine(
            ClickCooldown()
        );

        if (PauseManager.IsPaused)
        {
            if (!ClickUI())
            {
                SEManager.Instance.PlayEmptyClick();
            }

            return;
        }

        if (ClickUI())
        {
            return;
        }

        if (!CheckRegion())
        {
            SEManager.Instance.PlayEmptyClick();
        }
    }

    /*
     * 連打防止
     */
    private IEnumerator ClickCooldown()
    {
        canClick = false;

        yield return new WaitForSecondsRealtime(
            clickInterval
        );

        canClick = true;
    }

    /*
     * 現在の問題を受け取る
     */
    public void SetCurrentImage(
        Texture2D texture,
        List<Region> regions)
    {
        currentTexture = texture;
        currentRegions = regions;

        CheckHover();
    }

    /*
     * マウス座標→Canvas座標
     */
    private void MoveToScreenPosition(
        Vector2 screenPosition)
    {
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
    }

    /*
     * 画面外に出ない
     */
    private void ClampPosition()
    {
        Rect rect =
            canvasRect.rect;

        Vector3 position =
            cursorRect.localPosition;

        float halfWidth =
            cursorRect.rect.width * 0.5f;

        float halfHeight =
            cursorRect.rect.height * 0.5f;

        position.x =
            Mathf.Clamp(
                position.x,
                rect.xMin + halfWidth,
                rect.xMax - halfWidth
            );

        position.y =
            Mathf.Clamp(
                position.y,
                rect.yMin + halfHeight,
                rect.yMax - halfHeight
            );

        cursorRect.localPosition =
            position;
    }

    /*
     * UIクリック
     */
    private bool ClickUI()
    {
        PointerEventData pointerData =
            new PointerEventData(eventSystem);

        pointerData.position =
            RectTransformUtility.WorldToScreenPoint(
                null,
                cursorRect.position
            );

        List<RaycastResult> results =
            new List<RaycastResult>();

        raycaster.Raycast(
            pointerData,
            results
        );

        foreach (RaycastResult result in results)
        {
            Button button =
                result.gameObject.GetComponent<Button>();

            if (button != null)
            {
                button.onClick.Invoke();
                return true;
            }
        }

        return false;
    }

    /*
     * カーソル下の色表示
     */
    private void CheckHover()
    {
        Region region =
            GetCurrentRegion();

        if (region == null)
        {
            colorPreview.enabled = false;
            return;
        }

        colorPreview.enabled = true;

        colorPreview.color =
            gameManager.GetRegionColor(region);
    }

    /*
     * 決定判定
     */
    private bool CheckRegion()
    {
        Region region =
            GetCurrentRegion();

        if (region == null)
        {
            return false;
        }

        if (region.isIsshiki)
        {
            SpawnJudge(circlePrefab);
        }
        else
        {
            SpawnJudge(crossPrefab);
        }

        gameManager.SelectRegion(region);

        return true;
    }

    /*
     * 〇×生成
     */
    private void SpawnJudge(
        GameObject prefab)
    {
        if (prefab == null ||
            effectParent == null)
        {
            return;
        }

        GameObject obj =
            Instantiate(
                prefab,
                effectParent
            );

        RectTransform rect =
            obj.GetComponent<RectTransform>();

        rect.localPosition =
            cursorRect.localPosition;
    }

    /*
     * 現在カーソル下のRegion取得
     */
    private Region GetCurrentRegion()
    {
        if (currentTexture == null ||
            currentRegions == null)
        {
            return null;
        }

        Vector2 screenPosition =
            RectTransformUtility.WorldToScreenPoint(
                null,
                cursorRect.position
            );

        bool inside =
            RectTransformUtility
                .ScreenPointToLocalPointInRectangle(
                    imageRect,
                    screenPosition,
                    null,
                    out Vector2 localPosition
                );

        if (!inside)
        {
            return null;
        }

        Rect imageArea =
            imageRect.rect;

        if (!imageArea.Contains(localPosition))
        {
            return null;
        }

        int pixelX =
            Mathf.FloorToInt(
                Mathf.InverseLerp(
                    imageArea.xMin,
                    imageArea.xMax,
                    localPosition.x
                ) * currentTexture.width
            );

        int pixelY =
            Mathf.FloorToInt(
                Mathf.InverseLerp(
                    imageArea.yMin,
                    imageArea.yMax,
                    localPosition.y
                ) * currentTexture.height
            );

        if (pixelX < 0 ||
            pixelX >= currentTexture.width ||
            pixelY < 0 ||
            pixelY >= currentTexture.height)
        {
            return null;
        }

        foreach (Region region in currentRegions)
        {
            foreach (Vector2Int pixel in region.pixels)
            {
                if (pixel.x == pixelX &&
                    pixel.y == pixelY)
                {
                    return region;
                }
            }
        }

        return null;
    }
}