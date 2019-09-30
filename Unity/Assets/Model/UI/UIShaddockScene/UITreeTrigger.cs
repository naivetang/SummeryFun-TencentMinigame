using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ETModel
{
    
    /// <summary>
    /// 树被戳 震动
    /// </summary>
    public class UITreeTrigger : UIBehaviour
    {

        [SerializeField]
        private Animator animator;


        private string animationName;

        void Start()
        {
            
            this.animationName = this.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            
            //Log.Info(animationName);
            //this.animator.s
        }
        
        private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
        {
            //Log.Info("Enter");

            if (collision.gameObject.transform.tag.Equals("ShootStick"))
            {
                Log.Info($"{this.gameObject.name}的树叶震动");
                

                this.animator.enabled = true;


                this.animator.Play(this.animationName, 0, 0);


            }
        }
    }
}