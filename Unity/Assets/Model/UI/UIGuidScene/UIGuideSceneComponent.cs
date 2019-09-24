using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ETModel
{ 
    [ObjectSystem]
    public class UIGuideAwakeSystem: AwakeSystem<UIGuideSceneComponent>
    {
        public override void Awake(UIGuideSceneComponent self)
        {
            self.Awake();
        }
    }

    public class UIGuideSceneComponent : Component
    {
        private GameObject hine1;
        private GameObject hine2;
        private GameObject zhuzi;

        private GameObject stayHine;
        
        

        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UIBase>().GameObject.GetComponent<ReferenceCollector>();

            this.hine1 = rc.Get<GameObject>("hine1");
            
            this.hine2 = rc.Get<GameObject>("hine2");
            
            this.zhuzi = rc.Get<GameObject>("DragZhuzi");
            
            this.Init();
        }

        void Init()
        {
            this.RegistColliderTrriger();
            
            this.RegistDrageEvent();
        }

        /// <summary>
        /// 注册竹子进入退出事件
        /// </summary>
        void RegistColliderTrriger()
        {
            {
                this.hine1.GetComponent<UIColliderTrigger>().RegistOnTriggerEnter2D((p) =>
                {
                    HineTriggerEnter2D(this.hine1, p);
                });

                this.hine1.GetComponent<UIColliderTrigger>().RegistOnTriggerExit2D((p) =>
                {
                    HineTriggerExit2D(this.hine1, p);
                });

                this.hine1.GetComponent<UIColliderTrigger>().RegistOnTriggerStay2D((p) =>
                {
                    HineTriggerStaty2D(this.hine1, p);

                });

            }

            {

                this.hine2.GetComponent<UIColliderTrigger>().RegistOnTriggerEnter2D((p) =>
                {
                    HineTriggerEnter2D(this.hine2, p);
                });

                this.hine2.GetComponent<UIColliderTrigger>().RegistOnTriggerExit2D((p) =>
                {
                    HineTriggerExit2D(this.hine2, p);
                });

                this.hine2.GetComponent<UIColliderTrigger>().RegistOnTriggerStay2D((p) =>
                {
                    HineTriggerStaty2D(this.hine2, p);

                });
            }
        }

        void HineTriggerEnter2D(GameObject hine, Collider2D collider2D)
        {
            //Log.Info( collider2D.name + collider2D.ToString());

            UIMultImage multImage = hine.GetComponent<UIMultImage>();

            multImage.enabled = true;
            
            multImage.SetSprite(0);
            
            multImage.CrossFadeAlpha(0.5f, 0f, true);

            this.stayHine = hine;

            //this.zhuzi.transform.DOScale(Vector3.one * 1.2f, 0.15f);
        }

        void HineTriggerExit2D(GameObject hine, Collider2D collider2D)
        {
            //Log.Info(collider2D.name + collider2D.ToString());
            
            UIMultImage multImage = hine.GetComponent<UIMultImage>();

            multImage.enabled = false;

            this.stayHine = null;

            //this.zhuzi.transform.DOScale(Vector3.one, 0.15f);
        }


        void HineTriggerStaty2D(GameObject hine, Collider2D collider2D)
        {
            //Log.Info("staty");
        }

        /// <summary>
        /// 注册拖动竹子事件
        /// </summary>
        void RegistDrageEvent()
        {
            var darageable = this.zhuzi.GetComponent<UIDragable>(); 
            
            
            darageable.RegistOnBeginDrag(this.BeginDrageZhuzi);
            darageable.RegistOnDrag(this.DrageingZhuzi);
            darageable.RegistOnEndDrag(this.EndDrageZhuzi);
            
            darageable.RegisterPointDown(this.DragPointDown);
            darageable.RegisterPointUp(this.DragPointUp);
        }

        void DragPointDown(PointerEventData p)
        {
            //Log.Info("down");

            this.zhuzi.transform.DOScale(Vector3.one * 1.2f, 0.15f);
        }

        void DragPointUp(PointerEventData p)
        {
            //Log.Info("up");

            this.zhuzi.transform.DOScale(Vector3.one, 0.15f);
        }

        void BeginDrageZhuzi(PointerEventData p)
        {
            //Log.Info("begin drage");
        }

        void DrageingZhuzi(PointerEventData p)
        {
            //Log.Info("draging");
        }

        void EndDrageZhuzi(PointerEventData p)
        {
            //Log.Info("end draging");

            // 解题成功
            if (this.stayHine != null && this.stayHine == this.hine2)
            {
                this.SolveSucces();    
            }
            else // 解题失败，竹子掉下来
            {
                
            }
        }

        void SolveSucces()
        {
            Log.Info("解题成功");

            {
                var multImage = this.hine2.GetComponent<UIMultImage>();

                multImage.SetSprite(1);

                multImage.CrossFadeAlpha(1, 0.1f, true);
            }

            {
                this.zhuzi.GetComponent<Image>().CrossFadeAlpha(0f, 0.1f, true);

                this.zhuzi.GetComponent<Image>().raycastTarget = false;
            }
        }

    }
}
