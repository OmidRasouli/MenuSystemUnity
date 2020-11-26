using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeableMenu : Menu, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField, Tooltip("The left page is the first index")] private RectTransform[] pages;
    [SerializeField, Tooltip("The left page is the first index")] private RectTransform[] tabs;
    [SerializeField] private RectTransform pageIndicator;
    [SerializeField, Tooltip("Which one is in the center?")] private int centerIndex = 0;
    [SerializeField] private bool deadEnd;
    [SerializeField, Range(10, 80)] private int speed = 20;
    private Vector2 drag;
    private float dragDelta;

    private IEnumerator Start()
    {
        yield return null;
        dragDelta = Screen.width * 0.2f;
        pageIndicator.sizeDelta = tabs[0].sizeDelta;
        pageIndicator.localPosition = tabs[centerIndex].localPosition;

        for (int i = 0; i < tabs.Length; i++)
        {
            var index = i;
            tabs[i].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => OnClick(index));
        }

        for (int i = 0; i < pages.Length; i++)
        {
            if (i != centerIndex)
                pages[i].gameObject.SetActive(false);
            pages[i].localPosition = Vector2.zero;
        }
        pages[centerIndex].localPosition = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        drag = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        drag = eventData.position - drag;
        var last = centerIndex;
        if (drag.magnitude > dragDelta)
        {
            var direction = drag.normalized.x > 0 ? -1 : 1;
            centerIndex += direction;
            if (deadEnd)
            {
                centerIndex = Mathf.Clamp(centerIndex, 0, pages.Length - 1);
            }
            else
            {
                centerIndex %= pages.Length;
                if (centerIndex < 0)
                    centerIndex = pages.Length - 1;
            }
            if (centerIndex != last)
                StartCoroutine(Move(direction, last));
        }
    }

    private void OnClick(int index)
    {
        if (centerIndex == index)
            return;

        var direction = index < centerIndex ? -1 : 1;
        var last = centerIndex;
        centerIndex = index;
        StartCoroutine(Move(direction, last));
    }

    private IEnumerator Move(int direction, int last)
    {
        var fps = (float)speed;
        var fromTab = pageIndicator.localPosition;
        pages[centerIndex].localPosition = new Vector2(pages[centerIndex].rect.width * direction, 0);
        var pageTo = new Vector2(pages[centerIndex].rect.width * direction * -1f, 0);
        var pageFrom = (Vector2)pages[centerIndex].localPosition;
        pages[centerIndex].gameObject.SetActive(true);
        for (int i = 0; i < speed; i++)
        {
            pageIndicator.localPosition = Timing.Lerp(i / fps, Timing.EasingType.EaseOutQuint, fromTab, tabs[centerIndex].localPosition);
            pages[last].localPosition = Timing.Lerp(i / fps, Timing.EasingType.EaseOutQuint, Vector2.zero, pageTo);
            pages[centerIndex].localPosition = Timing.Lerp(i / fps, Timing.EasingType.EaseOutQuint, pageFrom, Vector2.zero);
            yield return new WaitForEndOfFrame();
        }
        pages[last].gameObject.SetActive(false);
    }
}
