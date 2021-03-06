using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using ILRuntime.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
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

        private GameObject image;

        private AnimationCurve easeCurve;

        private GameObject pictures;

        private Text indexText;

        private Button PreBtn;
        
        private Button NextBtn;
        
        private Button cancel;
        
        private GameObject onePagePrompt;
        private GameObject noFinishPrompt;

        private UIBookText bookText;

        private int currentPage = 0;

        //public static List<bool> hadOpenPage = new List<bool>(20){true};
        public static List<bool> hadOpenPage = new List<bool>(new bool[20]);


        private string[] indexArr = new[] { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖", "拾", "", "", "", "", "" };
        
        public void Awake()
        {
            hadOpenPage[0] = true;
            
            ReferenceCollector rc = this.GetParent<UIBase>().GameObject.GetComponent<ReferenceCollector>();

            this.context = rc.Get<GameObject>("Context");

            this.context.SetActive(false);

            this.closeBtn = rc.Get<GameObject>("CloseBtn");
            
            this.pictures = rc.Get<GameObject>("Pictures");
            
            this.indexText = rc.Get<GameObject>("index").GetComponent<Text>();
            
            this.PreBtn = rc.Get<GameObject>("Pre").GetComponent<Button>();
            
            this.NextBtn = rc.Get<GameObject>("Next").GetComponent<Button>();
            
            this.cancel = rc.Get<GameObject>("Cancel").GetComponent<Button>();
            
            this.bookText = rc.Get<GameObject>("PointDown").GetComponent<UIBookText>();

            this.bookText.gameObject.SetActive(true);
            
            this.bookText.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
            
            this.onePagePrompt = rc.Get<GameObject>("Prompt");

            this.onePagePrompt.SetActive(false);

            this.noFinishPrompt = rc.Get<GameObject>("notFinishPrompt");
            
            this.noFinishPrompt.SetActive(false);
            
            DOTweenAnimation animation = context.GetComponent<DOTweenAnimation>();
            
            easeCurve = animation.easeCurve;
            
            this.closeBtn.GetComponent<Button>().onClick.AddListener(this.CloseBtnOnClick);

            this.closeBtn.SetActive(false);
            
            this.pictures.SetActive(false);
            //this.animation = this.context.GetComponent<DOTweenAnimation>();
            
            this.Opened();
            
            this.Addlistener();
        }


        void Addlistener()
        {
            this.pictures.GetComponent<UIPointHandler>().RegisterPointDown(this.PicturesrPointDown);
            this.pictures.GetComponent<UIPointHandler>().RegisterPointUp(this.PicturesrPointUp);
            
            this.context.GetComponent<UIPointHandler>().RegisterPointDown(this.ContextPointDown);
            this.context.GetComponent<UIPointHandler>().RegisterPointUp(this.ContextPointUp);
        }

        private CancellationTokenSource cancellation = new CancellationTokenSource();

        void PicturesrPointDown(PointerEventData p)
        {
            if (this.currentPage == 0)
                return;

            this.pictures.GetComponent<CanvasGroup>().DOFade(0.1f, 0.1f).SetEase(Ease.OutExpo);
            
            this.bookText.Show(this.currentPage);
            
            this.bookText.gameObject.GetComponent<CanvasGroup>().alpha = 0f;

            this.bookText.gameObject.GetComponent<CanvasGroup>().DOFade(1, 0.1f).SetEase(Ease.InExpo);
        }

        void PicturesrPointUp(PointerEventData p)
        {
            if (this.currentPage == 0)
                return;

            this.pictures.GetComponent<CanvasGroup>().DOFade(1f, 0.3f).SetEase(Ease.InExpo);

            this.bookText.gameObject.GetComponent<CanvasGroup>().DOFade(0, 0.3f).SetEase(Ease.OutExpo);
        }

        void ContextPointDown(PointerEventData p)
        {
            
            //Log.Error("ContextPointDown");
            
            if (this.currentPage == 0)
                return;

            this.context.GetComponent<CanvasGroup>().DOFade(0.1f, 0.1f).SetEase(Ease.OutExpo);

            this.bookText.Show(this.currentPage);

            this.bookText.gameObject.GetComponent<CanvasGroup>().alpha = 0f;

            this.bookText.gameObject.GetComponent<CanvasGroup>().DOFade(1, 0.1f).SetEase(Ease.InExpo);
        }

        void ContextPointUp(PointerEventData p)
        {

            //Log.Error("ContextPointUp");

            if (this.currentPage == 0)
                return;

            this.context.GetComponent<CanvasGroup>().DOFade(1f, 0.3f).SetEase(Ease.InExpo);

            this.bookText.gameObject.GetComponent<CanvasGroup>().DOFade(0, 0.3f).SetEase(Ease.OutExpo);
        }

        void Opened()
        {
            Game.EventSystem.Run(EventIdType.BookState, true);
            
            this.cancel.onClick.AddListener(() =>
            {
                Game.Scene.GetComponent<UIComponent>().RemoveUI(UIType.UIBook);
            });
            
        }

        public bool hasOpenedPage(int index)
        {
            if (index < 0 || index > hadOpenPage.Count)
                return false;
            return hadOpenPage[index];
        }

        public void ShowPicture(int index)
        {
            this.currentPage = index;
            
            this.pictures.SetActive(true);

            
            this.CheckPagePromptShow();
            
            UIMultImage uiMultImage = this.pictures.GetComponent<UIMultImage>();
            
            uiMultImage.SetSprite(index);

            this.indexText.text = this.indexArr[index];

            CanvasGroup canvasGroup = this.GetParent<UIBase>().GameObject.GetComponent<CanvasGroup>();

            canvasGroup.alpha = 0;

            canvasGroup.DOFade(1, 0.5f).OnComplete(() =>
            {
                this.closeBtn.SetActive(true);


                
                {
                    this.PreBtn.onClick.AddListener(this.PreBtnOnClick);
                    this.NextBtn.onClick.AddListener(this.NextBtnOnClick);
                }
                
                
            });

            this.UpdateBtnState();
            
            
        }

        void CheckPagePromptShow()
        {
            this.onePagePrompt.SetActive(currentPage == 0);
        }

        public void ShowNotFinishTip()
        {
            this.noFinishPrompt.SetActive(true);
        }

        void UpdateBtnState()
        {
            this.PreBtn.gameObject.SetActive(false);
            this.NextBtn.gameObject.SetActive(false);

            this.PreBtn.gameObject.SetActive(this.CanLeftCut());
            
            this.NextBtn.gameObject.SetActive(this.CanRightCut());
            
        }

        
        /// <summary>
        /// 能否向左翻页
        /// </summary>
        /// <returns></returns>
        bool CanLeftCut()
        {
            for (int i = this.currentPage - 1; i >= 0; i--)
            {
                if (hadOpenPage[i])
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 能否向右翻页
        /// </summary>
        /// <returns></returns>
        bool CanRightCut()
        {
            for (int i = this.currentPage + 1; i < hadOpenPage.Count; i++)
            {
                if (hadOpenPage[i])
                {
                    return true;
                }
            }

            return false;
        }

        int OpendPages()
        {
            int count = 0;
            foreach (bool b in hadOpenPage)
            {
                if (b)
                    ++count;
            }

            return count;
        }

        void PreBtnOnClick()
        {
            for (int i = this.currentPage - 1; i >= 0; i--)
            {
                if (hadOpenPage[i])
                {
                    this.currentPage = i;
                    
                    this.CutPage().Coroutine();
                    
                    this.UpdateBtnState();
                    
                    break;
                }
            }
        }
        
        void NextBtnOnClick()
        {
            for (int i = this.currentPage + 1; i < hadOpenPage.Count; i++)
            {
                if (hadOpenPage[i])
                {
                    this.currentPage = i;
                    
                    this.CutPage().Coroutine();
                    
                    this.UpdateBtnState();
                    
                    break;
                }
            }
        }

        /// <summary>
        /// 切页
        /// </summary>
        async ETVoid CutPage()
        {
            UIMultImage uiMultImage = this.pictures.GetComponent<UIMultImage>();
            
            uiMultImage.SetSprite(this.currentPage);
            
            this.indexText.text = this.indexArr[this.currentPage];

            this.CheckPagePromptShow();
            
            this.UpdateBtnState();

            await ETTask.CompletedTask;

            // CanvasGroup canvasGroup = uiMultImage.GetComponent<CanvasGroup>();
            //
            // // 0.6f内消失
            // canvasGroup.DOFade(0, 0.6f).OnComplete(() =>
            // {
            //     uiMultImage.SetSprite(this.currentPage);
            // });
        }

        public void CutPage(int pageIndex)
        {
            this.currentPage = pageIndex;
            
            this.CutPage().Coroutine();
        }
       

        public void AddImageGo(GameObject image, int index)
        {
            
            this.image = image;
            if (image == null)
            {
                Log.Error("image is null");
                return;
            }

            this.currentPage = index;
            
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

            uiBoook.GetComponent<CanvasGroup>().DOFade(0f, 0.8f).OnComplete(() =>
            {
                Game.Scene.GetComponent<UIComponent>().RemoveUI(UIType.UIBook);
            });
            
            return;
            uiBoook =  this.GetParent<UIBase>().GameObject;

            uiBoook.transform.DOMove(vector2, 1f);

            uiBoook.transform.DOScale(Vector3.one * 0.4f, 1).OnComplete(() =>
            {
                Game.Scene.GetComponent<UIComponent>().RemoveUI(UIType.UIBook);
            });
        }

        void Closed()
        {
            Game.EventSystem.Run(EventIdType.BookState, false);
        }

        public override void Dispose()
        {
            base.Dispose();
            
            this.Closed();
        }
    }
}