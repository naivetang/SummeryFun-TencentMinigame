using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;

namespace ETModel
{
    public class AnchorTest : UIBehaviour
    {
        void OnEnable()
        {
            RectTransform rectTransform = this.gameObject.transform as RectTransform;
            
            // rectTransform.anchorMin = Vector2.zero;
            //
            // rectTransform.anchorMax = Vector2.one;

            Vector2 v2 = rectTransform.anchoredPosition;
            //
            // rectTransform.SetAnchor(AnchorPresets.StretchAll);
            //
            rectTransform.anchoredPosition = v2;

            float w = rectTransform.rect.width;

            float y = rectTransform.rect.height;
            
            Log.Error("w" +w);
            Log.Error("y:"+y);
            
            rectTransform.SetAnchor(AnchorPresets.StretchAll);
            
            rectTransform.anchoredPosition = v2;
            
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, y);
        }
    }
}