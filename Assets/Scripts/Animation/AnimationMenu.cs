using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimationMenu : MonoBehaviour
{
    public enum AnimationType { Fade, Move }
    public enum MoveDirection { Up, Right, Down, Left }
    public MoveDirection direction;
    public Timing.EasingType easingIn;
    public AnimationCurve curveIn;
    public Timing.EasingType easingOut;
    public AnimationCurve curveOut;
    public AnimationType animationType;
    public bool changeDirection;
    public float time;
    public RectTransform trs;
    public CanvasGroup canvasGroup;
    private bool isIn;
    private Vector2 animatePos;
    private bool isActive;

    public void Init(float scaler)
    {
        if (animationType == AnimationType.Move)
        {
            var x = 0f;
            var y = 0f;
            switch (direction)
            {
                case MoveDirection.Up:
                    x = 0;
                    y = trs.rect.height;
                    break;
                case MoveDirection.Down:
                    x = 0;
                    y = -trs.rect.height;
                    break;
                case MoveDirection.Right:
                    x = trs.sizeDelta.x;
                    y = 0;
                    break;
                case MoveDirection.Left:
                    x = -trs.sizeDelta.x;
                    y = 0;
                    break;
                default:
                    break;
            }
            animatePos = new Vector2(x, y);
            trs.localPosition = new Vector2(x, y);
        }
        else
        {
            canvasGroup.alpha = 0;
        }
    }

    public void Play()
    {
        if (isActive)
            return;

        isActive = true;

        if (animationType == AnimationType.Move)
        {
            StartCoroutine(AnimateMove());
        }
        else
        {
            StartCoroutine(AnimateFade());
        }
    }

    private IEnumerator AnimateMove()
    {
        var timer = 0f;
        Vector2 from;
        Vector2 to;
        Timing.EasingType easing;
        if (isIn)
        {
            var dir = changeDirection ? -1f : 1f;
            from = Vector2.zero;
            to = animatePos * dir;
            easing = easingIn;
        }
        else
        {
            from = animatePos;
            to = Vector2.zero;
            easing = easingOut;
        }
        isIn = !isIn;

        while (timer < this.time)
        {
            trs.localPosition = Timing.Lerp(timer / time, easing, from, to);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        trs.localPosition = to;
        isActive = false;
    }

    private IEnumerator AnimateFade()
    {
        var timer = 0f;
        var from = isIn ? 1f : 0f;
        var to = 1f - from;
        var easing = isIn ? easingIn : easingOut;
        isIn = !isIn;

        while (timer < this.time)
        {
            canvasGroup.alpha = Timing.Lerp(timer / time, easing, from, to);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        canvasGroup.alpha = to;
        isActive = false;
    }
}