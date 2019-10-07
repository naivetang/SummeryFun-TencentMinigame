
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

namespace ETModel
{
    /// <summary>
    /// 小猪进猪圈
    /// </summary>
    public class PigIntoSty : MonoBehaviour
    {

        private SkeletonGraphic _animation;

        [SerializeField]
        private Transform _endPos;


        [SerializeField]
        private SkeletonDataAsset _upSkeletonData;

        [SerializeField]
        private SkeletonDataAsset _leftSkeletonData;



        private int _trackIndex = 0;

        private readonly string _idleAnimation = "idle";
        
        private readonly string _walkAnimation = "walk";

        //private float speed = -1f;

        [SerializeField]
        private float moveTime = 8f;
        
        void Awake()
        {
            this._animation = this.GetComponent<SkeletonGraphic>();

            this._animation.skeletonDataAsset = this._leftSkeletonData;

            this._animation.Initialize(true);

            this._animation.AnimationState.SetAnimation(this._trackIndex, this._idleAnimation, true);
        }

        private void OnEnable()
        {
            this._animation.skeletonDataAsset = this._upSkeletonData;
            
            this._animation.Initialize(true);

            this._animation.AnimationState.SetAnimation(this._trackIndex, this._walkAnimation, true);

            this.transform.DOMove(this._endPos.position, this.moveTime).SetEase(Ease.Linear).OnComplete(this.MoveFinish);

            //this.speed = 10f;
        }

        void MoveFinish()
        {
            this._animation.skeletonDataAsset = this._leftSkeletonData;

            this._animation.Initialize(true);

            this._animation.AnimationState.SetAnimation(this._trackIndex, this._idleAnimation, true);
            
            Game.EventSystem.Run(EventIdType.PigIntoStyFinish);
        }

        void Update()
        {
            // if (this.speed > 0)
            // {
            //     this.transform.Translate(new Vector3(0, this.speed * Time.deltaTime, 0), Space.World);
            //
            //     if (this.transform.position.x < this._endPos.DOLocalMoveX())
            //     {
            //         
            //     }
            //
            //     // 到了目的地
            //     if (this.transform.position.y > this._endPos.position.y)
            //     {
            //         this.speed = -1;
            //
            //         this._animation.skeletonDataAsset = this._leftSkeletonData;
            //
            //         this._animation.Initialize(true);
            //
            //         this._animation.AnimationState.SetAnimation(this._trackIndex, this._idleAnimation, true);
            //     }
            // }
        }



    }
}
