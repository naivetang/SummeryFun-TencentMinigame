using DG.Tweening;
using DG.Tweening.Core.Easing;
using UnityEngine;

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

        private Vector2 endPos = new Vector2(66,23);

        //private DOTweenAnimation animation;

        //private AnimationCurve easeCurve;
        public void Awake()
        {
          //  easeCurve = new AnimationCurve(this.easeCurve)
            
            ReferenceCollector rc = this.GetParent<UIBase>().GameObject.GetComponent<ReferenceCollector>();

            this.context = rc.Get<GameObject>("Context");

            this.context.SetActive(false);

            //this.animation = this.context.GetComponent<DOTweenAnimation>();
        }

        public void AddImageGo(GameObject image)
        {
            if (image == null)
            {
                Log.Error("image is null");
                return;
            }
            
            image.transform.SetParent(this.context.transform);
            
            this.context.SetActive(true);

            image.transform.DOScale(Vector3.one * 0.6f, 1f).SetEase(Ease.OutBack);

            image.transform.DOLocalMove(this.endPos, 1f).SetEase(Ease.Linear);

            //image.AddComponent<DOTweenAnimation>(this.animation);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}