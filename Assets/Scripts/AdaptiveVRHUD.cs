
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class AdaptiveVRHUD : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;

    [Header("HUD Position")]
    [SerializeField] private float distance = 1.5f;
    [SerializeField] private float marginX = 0.04f;
    [SerializeField] private float marginY = 0.04f;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (targetCamera == null)
            return;

        float verticalFov =
            targetCamera.fieldOfView * Mathf.Deg2Rad;

        float viewHeight =
            2f * distance * Mathf.Tan(verticalFov / 2f);

        float viewWidth =
            viewHeight * targetCamera.aspect;

        float hudWidth =
            rectTransform.rect.width *
            rectTransform.localScale.x;

        float hudHeight =
            rectTransform.rect.height *
            rectTransform.localScale.y;

        float x =
            -viewWidth / 2f +
            hudWidth / 2f +
            viewWidth * marginX;

        float y =
            viewHeight / 2f -
            hudHeight / 2f -
            viewHeight * marginY;

        Vector3 localPosition =
            new Vector3(x, y, distance);

        rectTransform.localPosition = localPosition;
        rectTransform.localRotation = Quaternion.identity;
    }
}
