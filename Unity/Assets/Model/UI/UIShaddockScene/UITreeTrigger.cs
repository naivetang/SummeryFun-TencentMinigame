using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ETModel
{
    public class UITreeTrigger : UIBehaviour
    {

        [SerializeField]
        private Animator animator;

        private GameObject image;

        private string animationName;

        void Start()
        {
            this.image = this.gameObject.transform.Find("Image").gameObject;
            
            
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