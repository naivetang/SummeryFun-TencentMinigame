using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ETModel
{
    
    /// <summary>
    /// 柚子被打
    /// </summary>
    public class ShaddockTrigger : UIBehaviour,IDisposable
    {
        //[SerializeField]
        private Animator animator;

        [SerializeField]
        [CustomLabel("柚子ID")]
        private int Id;
        
        [SerializeField]
        [CustomLabel("掉落需要击中次数")]
        private int shootTimes;

        private string animationName;

        /// <summary>
        /// 当前被连续击中次数
        /// </summary>
        private int curShtTimes;
        
        void Start()
        {
            //this.animationName = this.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            this.AddListener();
        }


        private EventProxy eventProxy;
        
        void AddListener()
        {
            eventProxy = new EventProxy(this.ShootThing);
            
            Game.EventSystem.RegisterEvent(EventIdType.ShaddockShootThing, this.eventProxy);
        }
        void RemoveListener()
        {
            if(this.eventProxy != null)
                Game.EventSystem.UnRegisterEvent(EventIdType.ShaddockShootThing, this.eventProxy);
        }
        
        void ShootThing(List<object> obj)
        {
            int shootId = (int) obj[0];

            if (shootId == this.Id)
                ++this.curShtTimes;
            else
                this.curShtTimes = 0;
            
            this.CheckState();
        }

        
        /// <summary>
        /// 每一次出杆之后，需要检查每个柚子状态，落下去？晃动？静止
        /// </summary>
        void CheckState()
        {
            // 静止
            if (this.curShtTimes == 0)
            {
                //Log.Info(Id + "静止");
            }
            // 落下去
            else if (this.curShtTimes == this.shootTimes)
            {
                Log.Info(Id + "掉落");
            }
            // 晃动
            else 
            {
                Log.Info(Id + "晃动");
            }
        }
        
        private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
        {
            //Log.Info("Enter");

            if (collision.gameObject.transform.tag.Equals("ShootStick"))
            {
                //Game.EventSystem.Run(EventIdType.ShaddockShootThing, Id);
            }
        }

        public int GetShaddockId()
        {
            return this.Id;
        }

        public void Dispose()
        {
            this.RemoveListener();
        }
    }
}