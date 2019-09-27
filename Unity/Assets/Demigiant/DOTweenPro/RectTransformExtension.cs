using DG.Tweening;
using System;
using UnityEngine;

public static class RectTransformExtension
{
    public static Tweener DoUILocalMove(this RectTransform target, Vector3 endValue, float duration, bool snapping = false)
    {
        return TweenSettingsExtensions.SetTarget<Tweener>(
            TweenSettingsExtensions.SetOptions(
                DOTween.To(delegate { return target.anchoredPosition3D; }, delegate(Vector3 x) { target.anchoredPosition3D = x; }, endValue,
                    duration), snapping), target);
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

    public static void GetRectBoardSize(Vector3[] rectCorners, Vector3[] containerConrners, out float leftSize, out float rightSize,
    out float topSize, out float bottomSize)
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

    public static void SetAnchor(this RectTransform source, AnchorPresets allign, int offsetX = 0, int offsetY = 0)
    {
        source.anchoredPosition = new Vector3(offsetX, offsetY, 0);

        switch (allign)
        {
            case (AnchorPresets.TopLeft):
            {
                source.anchorMin = new Vector2(0, 1);
                source.anchorMax = new Vector2(0, 1);
                break;
            }

            case (AnchorPresets.TopCenter):
            {
                source.anchorMin = new Vector2(0.5f, 1);
                source.anchorMax = new Vector2(0.5f, 1);
                break;
            }

            case (AnchorPresets.TopRight):
            {
                source.anchorMin = new Vector2(1, 1);
                source.anchorMax = new Vector2(1, 1);
                break;
            }

            case (AnchorPresets.MiddleLeft):
            {
                source.anchorMin = new Vector2(0, 0.5f);
                source.anchorMax = new Vector2(0, 0.5f);
                break;
            }

            case (AnchorPresets.MiddleCenter):
            {
                source.anchorMin = new Vector2(0.5f, 0.5f);
                source.anchorMax = new Vector2(0.5f, 0.5f);
                break;
            }

            case (AnchorPresets.MiddleRight):
            {
                source.anchorMin = new Vector2(1, 0.5f);
                source.anchorMax = new Vector2(1, 0.5f);
                break;
            }

            case (AnchorPresets.BottomLeft):
            {
                source.anchorMin = new Vector2(0, 0);
                source.anchorMax = new Vector2(0, 0);
                break;
            }

            case (AnchorPresets.BottonCenter):
            {
                source.anchorMin = new Vector2(0.5f, 0);
                source.anchorMax = new Vector2(0.5f, 0);
                break;
            }

            case (AnchorPresets.BottomRight):
            {
                source.anchorMin = new Vector2(1, 0);
                source.anchorMax = new Vector2(1, 0);
                break;
            }

            case (AnchorPresets.HorStretchTop):
            {
                source.anchorMin = new Vector2(0, 1);
                source.anchorMax = new Vector2(1, 1);
                break;
            }

            case (AnchorPresets.HorStretchMiddle):
            {
                source.anchorMin = new Vector2(0, 0.5f);
                source.anchorMax = new Vector2(1, 0.5f);
                break;
            }

            case (AnchorPresets.HorStretchBottom):
            {
                source.anchorMin = new Vector2(0, 0);
                source.anchorMax = new Vector2(1, 0);
                break;
            }

            case (AnchorPresets.VertStretchLeft):
            {
                source.anchorMin = new Vector2(0, 0);
                source.anchorMax = new Vector2(0, 1);
                break;
            }

            case (AnchorPresets.VertStretchCenter):
            {
                source.anchorMin = new Vector2(0.5f, 0);
                source.anchorMax = new Vector2(0.5f, 1);
                break;
            }

            case (AnchorPresets.VertStretchRight):
            {
                source.anchorMin = new Vector2(1, 0);
                source.anchorMax = new Vector2(1, 1);
                break;
            }

            case (AnchorPresets.StretchAll):
            {
                source.anchorMin = new Vector2(0, 0);
                source.anchorMax = new Vector2(1, 1);
                break;
            }
        }
    }


    public static void SetPivot(this RectTransform source, PivotPresets preset)
    {

        switch (preset)
        {
            case (PivotPresets.TopLeft):
            {
                source.pivot = new Vector2(0, 1);
                break;
            }

            case (PivotPresets.TopCenter):
            {
                source.pivot = new Vector2(0.5f, 1);
                break;
            }

            case (PivotPresets.TopRight):
            {
                source.pivot = new Vector2(1, 1);
                break;
            }

            case (PivotPresets.MiddleLeft):
            {
                source.pivot = new Vector2(0, 0.5f);
                break;
            }

            case (PivotPresets.MiddleCenter):
            {
                source.pivot = new Vector2(0.5f, 0.5f);
                break;
            }

            case (PivotPresets.MiddleRight):
            {
                source.pivot = new Vector2(1, 0.5f);
                break;
            }

            case (PivotPresets.BottomLeft):
            {
                source.pivot = new Vector2(0, 0);
                break;
            }

            case (PivotPresets.BottomCenter):
            {
                source.pivot = new Vector2(0.5f, 0);
                break;
            }

            case (PivotPresets.BottomRight):
            {
                source.pivot = new Vector2(1, 0);
                break;
            }
        }

    }
}

public enum AnchorPresets
{
    TopLeft,
    TopCenter,
    TopRight,
 
    MiddleLeft,
    MiddleCenter,
    MiddleRight,
 
    BottomLeft,
    BottonCenter,
    BottomRight,
    BottomStretch,
 
    VertStretchLeft,
    VertStretchRight,
    VertStretchCenter,
 
    HorStretchTop,
    HorStretchMiddle,
    HorStretchBottom,
 
    StretchAll
}
 
public enum PivotPresets
{
    TopLeft,
    TopCenter,
    TopRight,
 
    MiddleLeft,
    MiddleCenter,
    MiddleRight,
 
    BottomLeft,
    BottomCenter,
    BottomRight,
}