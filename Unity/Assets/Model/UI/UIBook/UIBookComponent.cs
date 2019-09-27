using DG.Tweening;
using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    [ObjectSystem]
    public class UIBookComponentAwakeSystem : AwakeSystem<UIBookComponent>
    {
        public override void Awake(UIBookComponent self)
        {
            self.Awake();
        }
    }

    public class UIBookComponent : Component
    {
        private GameObject context;
        
        private GameObject closeBtn;

        private Vector2 endPos = new Vector2(66,23);


        private AnimationCurve easeCurve;

        private GameObject image;

        
        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UIBase>().GameObject.GetComponent<ReferenceCollector>();

            this.context = rc.Get<GameObject>("Context");

            this.context.SetActive(false);

            this.closeBtn = rc.Get<GameObject>("CloseBtn");
            

            DOTweenAnimation animation = context.GetComponent<DOTweenAnimation>();
            
            easeCurve = animation.easeCurve;
            
            this.closeBtn.GetComponent<Button>().onClick.AddListener(this.CloseBtnOnClick);

            this.closeBtn.SetActive(false);
            //this.animation = this.context.GetComponent<DOTweenAnimation>();
        }

        public void AddImageGo(GameObject image)
        {
            this.image = image;
            if (image == null)
            {
                Log.Error("image is null");
                return;
            }
            
            image.transform.SetParent(this.context.transform);
            
            
            
            this.context.SetActive(true);

            image.transform.DOScale(Vector3.one * 0.6f, 1f).SetEase(this.easeCurve);

            image.transform.DOLocalMove(this.endPos, 1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                this.closeBtn.SetActive(true);

                this.SetImageAnchor();    
                
                
            });

            //image.AddComponent<DOTweenAnimation>(this.animation);
        }

        void SetImageAnchor()
        {
            // RectTransform rectTransform = image.gameObject.transform as RectTransform;
            //
            // //Vector2 rect = rectTransform.rect.size;
            //
            //
            //
            // rectTransform.anchorMin = Vector2.zero;
            //     
            // rectTransform.anchorMax = Vector2.one;
            //
            //
            //
            // rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
            // //.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, 0);
            // rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
            // ///rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 0);
            // ///
            //
            // rectTransform.anchorMin = Vector2.zero;
            //     
            // rectTransform.anchorMax = Vector2.one;
            
            //rectTransform.sizeDelta = rect;
            
            
            
            RectTransform rectTransform = this.image.gameObject.transform as RectTransform;
            

            Vector2 anchoredPos = rectTransform.anchoredPosition;

            float rectWidth = rectTransform.rect.width;

            float rectHeight = rectTransform.rect.height;
            
            // Log.Error("w" +rectWidth);
            // Log.Error("y:"+rectHeight);
            
            rectTransform.SetAnchor(AnchorPresets.StretchAll);
            
            rectTransform.anchoredPosition = anchoredPos;
            
            // 宽高设置回去
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectWidth);
            
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectHeight);
        }

        void CloseBtnOnClick()
        {
            UIMainComponent mainComponent = Game.Scene.GetComponent<UIComponent>().GetUI(UIType.UIMain).GetComponent<UIMainComponent>();

            if (mainComponent == null)
            {
                Log.Error("UIMainComponent is null");

                return;
            }

            Vector2 vector2 = mainComponent.GetBookPositon();
            
            this.Close(vector2).Coroutine();
        }

        async ETVoid Close(Vector2 vector2)
        {
            UnityEngine.GameObject uiBoook =  this.GetParent<UIBase>().GameObject;

            uiBoook.transform.DOMove(vector2, 1f);

            uiBoook.transform.DOScale(Vector3.one * 0.4f, 1).OnComplete(() =>
            {
                Game.Scene.GetComponent<UIComponent>().RemoveUI(UIType.UIBook);
            });
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}