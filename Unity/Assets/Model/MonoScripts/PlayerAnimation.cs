using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spine.Unity;
using UnityEngine;
using AnimationState = Spine.AnimationState;

namespace ETModel
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField]
        private SkeletonDataAsset _leftSkeletonData;

        [SerializeField]
        private SkeletonDataAsset _rightSkeletonData;

        [SerializeField]
        private SkeletonDataAsset _upSkeletonData;

        [SerializeField]
        private SkeletonDataAsset _downSkeletonData;


        private MoveDir _moveDir = MoveDir.Stop;

        //private SkeletonAnimation _animation;


        private SkeletonGraphic _animation;


        private int _trackIndex = 0;

        private readonly string _idleAnimation = "idle";
        private readonly string _walkAnimation = "walk";

        private AudioSource audio;

        void Start()
        {
            //this._animation = this.GetComponent<SkeletonAnimation>();

            this._animation = this.GetComponent<SkeletonGraphic>();

            if (this._animation == null)
            {
                Log.Error("不存在组件SkeletonAnimation");
                return;
            }

            this.audio = this.GetComponent<AudioSource>();
            
            this._animation.skeletonDataAsset = this._upSkeletonData;

            this._animation.Initialize(true);

            this._animation.AnimationState.SetAnimation(this._trackIndex, this._idleAnimation, true);

            this.AddListener();
        }

        private void AddListener()
        {
            Game.EventSystem.RegisterEvent(EventIdType.MoveDirChange, new EventProxy(MoveDirChange));
        }

        private void RemoveListener()
        {
            Game.EventSystem.UnRegisterEvent(EventIdType.MoveDirChange, new EventProxy(MoveDirChange));
        }

        private void MoveDirChange(List<object> obj)
        {
            MoveDir dir = (MoveDir)obj[0];
            
            //Log.Warning("朝向 " + dir.ToString());

            if (this._moveDir != dir)
            {
                ChangAnimation(dir);    
            }

            this._moveDir = dir;
            
            
        }

        private void ChangAnimation(MoveDir dir)
        {

            AnimationState state = this._animation?.AnimationState;

            if (state == null)
            {
                Log.Error("不存在动画数据");
            }
            
            this.audio.enabled = true;
            
            switch (dir)
            {
                        
                case MoveDir.Stop:
                    
                    state.SetAnimation(_trackIndex, this._idleAnimation, true);

                    this.audio.enabled = false;
                    
                    break;

                case MoveDir.Up:
                    
                    this.ChangeDataAsset(this._upSkeletonData);
                    
                    break;
                case MoveDir.Down:
                    
                    this.ChangeDataAsset(this._downSkeletonData);
                    
                    break;
                case MoveDir.Left:

                    this.ChangeDataAsset(this._leftSkeletonData);

                    break;
                case MoveDir.Right:

                    this.ChangeDataAsset(this._rightSkeletonData);

                    break;
                case MoveDir.LeftUp:

                    this.ChangeDataAsset(this._upSkeletonData);

                    break;
                case MoveDir.RightUp:

                    this.ChangeDataAsset(this._upSkeletonData);
                    
                    break;
                case MoveDir.LeftDown:

                    this.ChangeDataAsset(this._downSkeletonData);

                    break;
                case MoveDir.RightDown:

                    this.ChangeDataAsset(this._downSkeletonData);

                    break;
                
            }
        }

        private void ChangeDataAsset(SkeletonDataAsset data )
        {
            if (this._animation.skeletonDataAsset == data)
            {
                if(!this._animation.AnimationState.GetCurrent(this._trackIndex).Animation.Name.Equals(this._walkAnimation))
                    this._animation.AnimationState.SetAnimation(_trackIndex, this._walkAnimation, true);

                return;

            }

            this._animation.skeletonDataAsset = data;
            
            this._animation.Initialize(true);

            this._animation.AnimationState.SetAnimation(_trackIndex, this._walkAnimation, true);
        }

        private void OnDisable()
        {
            this.RemoveListener();
        }
    }
}
