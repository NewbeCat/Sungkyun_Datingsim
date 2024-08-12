using System;
using System.Collections;
using UnityEngine;

public class OpenCloseWindow : MonoBehaviour
{
    [Header("Window Setup")]
    [SerializeField] private GameObject window;

    [SerializeField] private RectTransform windowRectTransform;
    [SerializeField] private CanvasGroup windowCanvasGroup;

    public enum AnimateToDirection
    {
        Top,
        Bottom,
        Left,
        Right
    }

    [Header("Animation Setup")]
    [SerializeField] private AnimateToDirection openDirection = AnimateToDirection.Top;
    [SerializeField] private AnimateToDirection closeDirection = AnimateToDirection.Bottom;
    [SerializeField] private Vector2 distanceToAnimate = new Vector2(100, 100);
    [SerializeField] private AnimationCurve easingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [Range(0, 1f)][SerializeField] private float animationDuration = 0.5f;

    private bool _isOpen;
    private Vector2 _initialPosition;
    private Coroutine _animateWindowCoroutine;

    public static event Action OnOpenWindow;
    public static event Action OnCloseWindow;

    private void OnValidate()
    {
        if (window != null)
        {
            windowRectTransform = window.GetComponent<RectTransform>();
            windowCanvasGroup = window.GetComponent<CanvasGroup>();
        }

        distanceToAnimate = new Vector2(Mathf.Max(0, distanceToAnimate.x), Mathf.Max(0, distanceToAnimate.y));
    }

    private void Awake()
    {
        if (windowRectTransform == null) windowRectTransform = window.GetComponent<RectTransform>();
        if (windowCanvasGroup == null) windowCanvasGroup = window.GetComponent<CanvasGroup>();

        _initialPosition = windowRectTransform.anchoredPosition;
    }

    public void ToggleOpenClose()
    {
        if (_isOpen)
            CloseWindow();
        else
            OpenWindow();
    }

    public void OpenWindow()
    {
        if (_isOpen) return;

        _isOpen = true;
        OnOpenWindow?.Invoke();

        StartAnimation(true);
    }

    public void CloseWindow()
    {
        if (!_isOpen) return;

        _isOpen = false;
        OnCloseWindow?.Invoke();

        StartAnimation(false);
    }

    private void StartAnimation(bool opening)
    {
        if (_animateWindowCoroutine != null)
            StopCoroutine(_animateWindowCoroutine);

        _animateWindowCoroutine = StartCoroutine(AnimateWindow(opening));
    }

    private Vector2 GetOffset(AnimateToDirection direction)
    {
        switch (direction)
        {
            case AnimateToDirection.Top:
                return new Vector2(0, distanceToAnimate.y);
            case AnimateToDirection.Bottom:
                return new Vector2(0, -distanceToAnimate.y);
            case AnimateToDirection.Left:
                return new Vector2(-distanceToAnimate.x, 0);
            case AnimateToDirection.Right:
                return new Vector2(distanceToAnimate.x, 0);
            default:
                return Vector2.zero;
        }
    }

    private IEnumerator AnimateWindow(bool open)
    {
        if (open) window.SetActive(true);

        float elapsedTime = 0;
        Vector2 startPosition = _initialPosition - (open ? GetOffset(openDirection) : Vector2.zero);
        Vector2 targetPosition = _initialPosition + (open ? Vector2.zero : GetOffset(closeDirection));

        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;
            float curveValue = easingCurve.Evaluate(t);

            windowRectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, curveValue);
            windowCanvasGroup.alpha = Mathf.Lerp(open ? 0f : 1f, open ? 1f : 0f, curveValue);
            windowCanvasGroup.interactable = open;
            windowCanvasGroup.blocksRaycasts = open;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        windowRectTransform.anchoredPosition = _initialPosition;
        windowCanvasGroup.alpha = open ? 1 : 0;
        windowCanvasGroup.interactable = open;
        windowCanvasGroup.blocksRaycasts = open;

        if (!open)
        {
            window.SetActive(false);
            windowRectTransform.anchoredPosition = _initialPosition;
        }
    }
}


