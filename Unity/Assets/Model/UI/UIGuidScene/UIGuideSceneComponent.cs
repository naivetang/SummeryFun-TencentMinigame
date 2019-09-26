using System;
using System.Collections;
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

        private GameObject ZhuziAnimation;

        private GameObject stayHine;


        private Vector3 zhuziInitPos;

        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UIBase>().GameObject.GetComponent<ReferenceCollector>();

            this.hine1 = rc.Get<GameObject>("hine1");
            
            this.hine1.GetComponent<UIMultImage>().enabled = false;
            
            this.hine2 = rc.Get<GameObject>("hine2");

            this.hine2.GetComponent<UIMultImage>().enabled = false;
            
            this.zhuzi = rc.Get<GameObject>("DragZhuzi");

            this.ZhuziAnimation = rc.Get<GameObject>("ZhuziAnimation");
            
            this.ZhuziAnimation.SetActive(false);

            this.zhuziInitPos = this.zhuzi.transform.position;
            
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

            this.zhuzi.transform.DOScale(Vector3.one * 1.1f, 0.15f);
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
                this.SolveFaild();
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

        void SolveFaild()
        {
            Log.Info("解题失败");

            // 设置掉落竹子的位置
            
            Vector3 vector3 = this.ZhuziAnimation.transform.position ;
            
            vector3.x = this.zhuzi.transform.position.x;

            this.ZhuziAnimation.transform.position = vector3;

            //this.zhuzi.GetComponent<Rigidbody2D>().gravityScale
            
            //return;

            this.ZhuziAnimation.SetActive(true);
            
            // 隐藏可拖拽的竹子
            this.zhuzi.SetActive(false);

            // 隐藏两个hine竹子

            this.hine1.GetComponent<UIMultImage>().enabled = false;

            this.hine2.GetComponent<UIMultImage>().enabled = false;

            this.stayHine = null;

            // 设置动画从第几帧开始键播

            Animator animator = this.ZhuziAnimation.GetComponent<Animator>();

            
            animator.Play("ZhuziAnimation",0,this.GetNormalizedTime());
            
            //AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            //sta

            //stateInfo.normalizedTime = 1;

            return;
            
            AnimatorClipInfo stateinfo = animator.GetCurrentAnimatorClipInfo(0)[0];

            stateinfo.clip.frameRate = 10;
        }

        /// <summary>
        ///  从第几帧开始播
        /// </summary>
        /// <returns></returns>
        float GetNormalizedTime()
        {

            float y = this.zhuzi.transform.localPosition.y;
            
            Log.Info("当前竹子的高度y : " + y);
            
            if (y > 600)
                return 0f / 17;
            else if (y > 377)
                return 1f / 17;
            else if (y > 76)
                return 2f / 17;
            else if (y > -284)
                return 3f / 17;
            else
                return 4f / 17;
            
            return 0;
        }
        
        
        
        
        
        
        

        Animator animator;
        const float kDuration = 1.7f;
        bool init = false;
        // Use this for initialization
        IEnumerator Start()
        {
            init = true;
            Debug.Log("start");
            //animator = GetComponent<Animator>();
            const float frameRate = 30f;
            const int frameCount = (int)((kDuration * frameRate) + 2);
            animator.Rebind();
            animator.StopPlayback();
            animator.recorderStartTime = 0;

            // 开始记录指定的帧数
            animator.StartRecording(frameCount);

            for (var i = 0; i < frameCount - 1; i++)
            {
                // 记录每一帧
                animator.Update(1.0f / frameRate);
            }
            animator.speed = 0;
            yield return new WaitForEndOfFrame();
            animator.speed = 1;
            // 完成记录
            animator.StopRecording();
            Debug.LogFormat("{0},{1}", animator.recorderStartTime, animator.recorderStopTime);

            // 开启回放模式
            animator.StartPlayback();
        }
        float m_CurTime = 0f;
        void Update()
        {
            if (!init)
            {
                Start();
            }
            int i = 0;
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                i = -1;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                i = 1;
            }
            //Debug.Log(m_CurTime);
            animator.playbackTime = m_CurTime;
            animator.Update(0);
            m_CurTime += (1 / 70f) * i;
            if (m_CurTime > 1.7f)
            {
                m_CurTime = 0;
            }
            if (m_CurTime < 0)
            {
                m_CurTime = 1.7f;
            }
        }

    }
}
