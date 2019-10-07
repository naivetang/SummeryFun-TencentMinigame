using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spine.Unity;
using UnityEngine;

namespace ETModel
{
    public class PigAnimation : MonoBehaviour,IDisposable
    {


        [SerializeField]
        private SkeletonDataAsset _leftSkeletonData;

        [SerializeField]
        private SkeletonDataAsset _rightSkeletonData;

        [SerializeField]
        private SkeletonDataAsset _upSkeletonData;

        [SerializeField]
        private SkeletonDataAsset _downSkeletonData;

        private SkeletonGraphic _animation;

        private int _trackIndex = 0;

        private readonly string _idleAnimation = "idle";
        
        private readonly string _walkAnimation = "walk";

        public string curAnimation;

        public Vector3 lastPos;

        public Vector2 offect;

        
        public MoveDir dir;

        /// <summary>
        /// 判断移动需要最小的位移值
        /// </summary>
        private float minOffctToMove = 0.01f;
        
        void Start()
        {
            //this._animation = this.GetComponent<SkeletonAnimation>();

            this._animation = this.GetComponent<SkeletonGraphic>();

            if (this._animation == null)
            {
                Log.Error("不存在组件SkeletonAnimation");
                return;
            }


            this._animation.skeletonDataAsset = this._leftSkeletonData;

            this._animation.Initialize(true);
            
            this.dir = MoveDir.Stop;

            this._animation.AnimationState.SetAnimation(this._trackIndex, this._idleAnimation, true);

            this.curAnimation = this._idleAnimation;

            this.lastPos = this.transform.position;

            this.AddListener();
        }


        void Update()
        {
            this.offect = this.transform.position - this.lastPos;

            this.lastPos = this.transform.position;

           this.UpdateAinmation();
        }

        void UpdateAinmation()
        {
            // 判定未移动，idle
            if (Mathf.Abs(this.offect.x)  < this.minOffctToMove && Mathf.Abs(this.offect.y) < this.minOffctToMove)
            {
                
                //Log.Info("未移动");
                if (!this.curAnimation.Equals(this._idleAnimation))
                {
                    this._animation.AnimationState.SetAnimation(this._trackIndex, this._idleAnimation, true);

                    this.curAnimation = this._idleAnimation;

                    this.dir = MoveDir.Stop;
                }
            }
            else
            {
                

                // 左右移动
                if (Mathf.Abs(this.offect.x) > Mathf.Abs(this.offect.y))
                {
                    
                    //Log.Info("左右移动");
                    // 右移动
                    if (this.offect.x > 0)
                    {
                        if (this.dir != MoveDir.Right)
                        {

                            this._animation.skeletonDataAsset = this._rightSkeletonData;

                            isRight(true);

                            this._animation.Initialize(true);

                            this.dir = MoveDir.Right;
                        }
                    }
                    // 左移动
                    else
                    {
                        if (this.dir != MoveDir.Left)
                        {

                            this._animation.skeletonDataAsset = this._leftSkeletonData;
                            
                            isRight(false);

                            this._animation.Initialize(true);

                            this.dir = MoveDir.Left;
                        }
                    }


                }
                // 上下移动
                else
                {
                    //Log.Info("上下移动");
                    // 上移动
                    if (this.offect.y > 0)
                    {
                        if (this.dir != MoveDir.Up)
                        {

                            this._animation.skeletonDataAsset = this._upSkeletonData;
                            
                            isRight(false);

                            this._animation.Initialize(true);

                            this.dir = MoveDir.Up;
                        }
                    }
                    // 下移动
                    else
                    {
                        if (this.dir != MoveDir.Down)
                        {

                            this._animation.skeletonDataAsset = this._downSkeletonData;
                            
                            isRight(false);

                            this._animation.Initialize(true);

                            this.dir = MoveDir.Down;
                        }
                    }
                }

                if (!this.curAnimation.Equals(this._walkAnimation))
                {
                    this._animation.AnimationState.SetAnimation(this._trackIndex, this._walkAnimation, true);

                    this.curAnimation = this._walkAnimation;
                }
            }
        }

        void isRight(bool b)
        {
            if (b&& this.transform.localScale.x > 0)
            {
                this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y,
                    this.transform.localScale.z);
            }
            else if (!b && transform.localScale.x < 0)
            {
                this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y,
                    this.transform.localScale.z);
            }
        }

        private void AddListener()
        {
        }

        private void RemoveListener()
        {
        }

        public void Dispose()
        {
            
        }
    }
}
