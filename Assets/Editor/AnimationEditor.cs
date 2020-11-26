using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AnimationMenu))]
public class AnimationEditor : Editor
{
    public override void OnInspectorGUI()
    {

        var animation = (AnimationMenu)target;

        animation.animationType = (AnimationMenu.AnimationType)EditorGUILayout.EnumPopup("AnimationType", animation.animationType);

        if (animation.animationType == AnimationMenu.AnimationType.Move)
        {
            EditorGUI.BeginDisabledGroup(true);
            animation.trs = (RectTransform)EditorGUILayout.ObjectField(animation.transform, typeof(RectTransform), true);
            EditorGUI.EndDisabledGroup();

            animation.direction = (AnimationMenu.MoveDirection)EditorGUILayout.EnumPopup("From ", animation.direction);

            EditorGUILayout.BeginHorizontal();
            animation.changeDirection = EditorGUILayout.Toggle("Opposite exit", animation.changeDirection);

            var warning = "";

            if (animation.changeDirection)
            {
                warning = $"Comes in from {animation.direction} and goes out to {((AnimationMenu.MoveDirection)(((int)animation.direction + 2) % 4))}";
            }
            else
            {
                warning = $"Comes in from {animation.direction} and goes out to {animation.direction}";
            }

            EditorGUILayout.HelpBox(warning, MessageType.Warning);
            EditorGUILayout.EndHorizontal();

            if (animation.gameObject.GetComponent<CanvasGroup>() != null)
            {
                DestroyImmediate(animation.gameObject.GetComponent<CanvasGroup>());
            }
        }
        else
        {
            var cg = animation.gameObject.GetComponent<CanvasGroup>();
            if (cg == null)
            {
                animation.gameObject.AddComponent<CanvasGroup>();
            }

            animation.canvasGroup = cg;
            EditorGUI.BeginDisabledGroup(true);
            animation.canvasGroup = (CanvasGroup)EditorGUILayout.ObjectField(animation.canvasGroup, typeof(CanvasGroup), true);
            EditorGUI.EndDisabledGroup();
        }

        animation.time = Mathf.Round(EditorGUILayout.Slider("Animation Time", animation.time, 0.1f, 2) * 10f) / 10f;

        animation.curveIn = new AnimationCurve();
        animation.curveOut = new AnimationCurve();
        for (int i = 0; i < 11; i++)
        {
            var point = i * 0.1f;
            var t = Timing.Ease(point, animation.easingIn);
            animation.curveIn.AddKey(point, t);
            var t2 = Timing.Ease(point, animation.easingOut);
            animation.curveOut.AddKey(point, t2);
        }
        animation.easingIn = (Timing.EasingType)EditorGUILayout.EnumPopup("Easing in", animation.easingIn);
        animation.curveIn = EditorGUILayout.CurveField("Curve in", animation.curveIn, Color.red, new Rect(0, 0, 1f, 1f), GUILayout.Height(200f));
        animation.easingOut = (Timing.EasingType)EditorGUILayout.EnumPopup("Easing out", animation.easingOut);
        animation.curveOut = EditorGUILayout.CurveField("Curve out", animation.curveOut, Color.red, new Rect(0, 0, 1f, 1f), GUILayout.Height(200f));
    }
}
