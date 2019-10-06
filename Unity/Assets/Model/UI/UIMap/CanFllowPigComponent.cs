using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    
    [ObjectSystem]
    public class CanFllowPigAwakeSystem : AwakeSystem<CanFllowPigComponent,GameObject>
    {
        public override void Awake(CanFllowPigComponent self,GameObject a)
        {
            self.Awake(a);
        }
    }

    [ObjectSystem]
    public class CanFllowPigLateUpdateSystem : LateUpdateSystem<CanFllowPigComponent>
    {
        public override void LateUpdate(CanFllowPigComponent self)
        {
            self.LateUpdate();
        }
    }

    [HideInHierarchy]
    public class CanFllowPigComponent :Component
    {

        private GameObject dialogPig;

        private GameObject dialogArea;

        private GameObject followBubble;

        private int triggerId = 3002;

        private Transform player;

        /// <summary>
        /// 头顶按钮
        /// </summary>
        //private Button topButton;
        
        private bool completeTask;

        private bool isFllowed;

        private NavMeshAgent2D navAgent;

        private int completeDialogId = 2122;
        
        public void Awake(GameObject gameObject)
        {
            this.isFllowed = false;
            
            this.completeTask = false;
            
            this.GameObject = gameObject;

            this.navAgent = this.GameObject.GetComponent<NavMeshAgent2D>();

            this.player = Game.Scene.GetComponent<UnitComponent>().MyUnit.GameObject.transform;

            ReferenceCollector rc = this.GameObject.GetComponent<ReferenceCollector>();

            dialogPig = rc.Get<GameObject>("DialogPig");

            dialogArea = this.GameObject.Get<GameObject>("DialogArea");

            followBubble = this.GameObject.Get<GameObject>("FollowBubble");
            
            this.followBubble.SetActive(false);

            //this.topButton = rc.Get<GameObject>("DialogPrompt").GetComponent<Button>();

            this.GameObject.SetActive(false);
            
            this.dialogPig.SetActive(true);
            
            this.Addlistener();
        }

        public void LateUpdate()
        {
            if (this.completeTask && this.isFllowed)
            {
                navAgent.destination = this.player.position;
            }
        }

        private EventProxy completeTaskProxy;
        private EventProxy completeDialogProxy;
        
        
        void Addlistener()
        {
            completeTaskProxy = new EventProxy(this.CompleteTask);

            completeDialogProxy = new EventProxy(this.CompleteDialog);


            Game.EventSystem.RegisterEvent(EventIdType.CompleteTask, this.completeTaskProxy);
            Game.EventSystem.RegisterEvent(EventIdType.CompleteDialog, this.completeDialogProxy);
            
            //this.topButton.onClick.AddListener(this.TopBtnClick);
            
            this.GameObject.GetComponent<UIColliderTrigger>().RegistOnTriggerEnter2D(this.TriggerEnter);
            this.GameObject.GetComponent<UIColliderTrigger>().RegistOnTriggerExit2D(this.TriggerExit);
        }

        void TopBtnClick()
        {
            this.isFllowed = true;

            this.followBubble.SetActive(true);

            this.dialogArea.SetActive(false);
                                  
            //this.topButton.gameObject.SetActive(false);
        }

        void TriggerEnter(Collider2D other)
        {
            if (!other.gameObject.transform.tag.Equals("Player") || this.isFllowed)
                return;
            
            //this.topButton.gameObject.SetActive(true);
        }

        void TriggerExit(Collider2D other)
        {
            if (!other.gameObject.transform.tag.Equals("Player") || this.isFllowed)
                return;
            
            //this.topButton.gameObject.SetActive(false);
        }

        /// <summary>
        /// 完成柚子关
        /// </summary>
        /// <param name="obj"></param>
        void CompleteTask(List<object> obj)
        {
            int triggerid = (int)obj[0];

            if (triggerid == this.triggerId)
            {
                this.dialogPig.SetActive(false);
                
                this.GameObject.SetActive(true);

                this.completeTask = true;
            }

        }


        /// <summary>
        /// 完成对话
        /// </summary>
        /// <param name="obj"></param>
        void CompleteDialog(List<object> obj)
        {
            int dialogId = (int)obj[0];

            if (dialogId == this.completeDialogId)
            {
                this.dialogArea.GetComponent<BoxCollider2D>().enabled = false;
                

                this.followBubble.SetActive(true);

                this.isFllowed = true;
            }

        }

        void RemoveListener()
        {
            Game.EventSystem.UnRegisterEvent(EventIdType.CompleteTask, this.completeTaskProxy);
            Game.EventSystem.UnRegisterEvent(EventIdType.CompleteDialog, this.completeDialogProxy);

        }

        public override void Dispose()
        {
            base.Dispose();
            
            this.RemoveListener();
        }


    }
}
