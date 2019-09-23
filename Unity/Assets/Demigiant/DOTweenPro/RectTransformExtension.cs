using DG.Tweening;
using System;
using UnityEngine;

public static class RectTransformExtension
{
    public static Tweener DoUILocalMove(this RectTransform target, Vector3 endValue, float duration, bool snapping = false)
    {
        return TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(DOTween.To(delegate { return target.anchoredPosition3D; }, delegate (Vector3 x) { target.anchoredPosition3D = x; }, endValue, duration), snapping), target);
    }

    public static Transform FindChildRecursive(this Transform trm, string name)
    {

        Transform child = null;
        //从外层向内存找能提高一下速度
        foreach (Transform t in trm)
        {
            if (t.name == name)
            {
                child = t;
                return child;
            }
            else if (t.childCount > 0)
            {
                child = t.FindChildRecursive(name);
                if (child)
                {
                    return child;
                }
            }
        }

        return child;
    }

    public static void GetRectBoardSize(Vector3[] rectCorners, Vector3[] containerConrners, out float leftSize, out float rightSize, out float topSize, out float bottomSize)
    {
        if (rectCorners != null && containerConrners != null && rectCorners.Length == 4 && containerConrners.Length == 4)
        {
            leftSize = rectCorners[0].x - containerConrners[0].x;
            rightSize = containerConrners[2].x - rectCorners[2].x;

            topSize = rectCorners[0].y - containerConrners[0].y;
            bottomSize = containerConrners[2].y - rectCorners[2].y;
        }
        else
        {
            leftSize = rightSize = topSize = bottomSize = 0;
        }

    }
}