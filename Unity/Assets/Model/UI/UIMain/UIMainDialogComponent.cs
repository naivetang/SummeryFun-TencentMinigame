using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    [ObjectSystem]
    public class UIMainDialogComponentAwakeSystem: AwakeSystem<UIMainDialogComponent, GameObject>
    {
        public override void Awake(UIMainDialogComponent self, GameObject go)
        {
            self.Awake(go);
        }
    }

    [HideInHierarchy]
    public class UIMainDialogComponent: Component
    {

        /// <summary>
        /// 对话组开始的id
        /// </summary>
        private int beginDialogId;
        /// <summary>
        /// 当前对话组id
        /// </summary>
        private int currentDialogId;

        private Button btn;

        public void Awake(GameObject go)
        {

            this.GameObject = go;

            btn = this.GameObject.GetComponent<UINextButton>();

            this.btn.gameObject.SetActive(false);

            this.AddLsitener();
        }

        void AddLsitener()
        {
            this.btn.onClick.AddListener(this.BtnOnClick);

            this.dialogAeraHandler = new EventProxy(this.DialogAeraHandler);

            Game.EventSystem.RegisterEvent(EventIdType.DialogAera, this.dialogAeraHandler);
        }

        void BtnOnClick()
        {
            //this.currentDialogId;

            DialogConfig dialogConfig = Game.Scene.GetComponent<ConfigComponent>().Get(typeof (DialogConfig), this.currentDialogId) as DialogConfig;

            if (dialogConfig == null)
            {

                Log.Error("不存在dialog配置,dialog id = " + this.currentDialogId);

                return;
            }

            Unit unit = Game.Scene.GetComponent<UnitComponent>().Get(dialogConfig.RoleId);

            UnityEngine.GameObject dialogGo = unit.GameObject.GetComponent<ReferenceCollector>().Get<GameObject>("UIDialog");

            DialogShowText(dialogGo, dialogConfig.Text, dialogConfig.ShowTime);

            this.currentDialogId = dialogConfig.NextId;

            this.CheckDialogBtn();
        }

        void CheckDialogBtn()
        {
            if (this.currentDialogId == -1)
            {
                Log.Info("结束对话");
                
                Game.EventSystem.Run(EventIdType.CompleteDialog, this.beginDialogId);
                
                this.GameObject.SetActive(false);
            }
        }

        void DialogShowText(GameObject dialogGo, string text, float closeTime)
        {
            Log.Info("对话内容：" + text);

            ///GameObject textGo = dialogGo.GetComponent<ReferenceCollector>().Get<GameObject>("TextContext");

            dialogGo.SetActive(true);

            dialogGo.GetComponent<DialogTextCtl>().SetText(text, closeTime);
            
            //UIFactory.Create<UIGuideSceneComponent>()
        }


        private EventProxy dialogAeraHandler;

        private void DialogAeraHandler(List<object> obj)
        {
            string state = obj[0] as string;

            int dialogId = (int) obj[1];

            // 进入对话区域
            if (state.Equals("Enter"))
            {
                Log.Info("进入对话区域，id ： " + dialogId);

                beginDialogId = dialogId;
                
                currentDialogId = dialogId;

                this.GameObject.SetActive(true);
            }

            // 离开对话区域
            else if (state.Equals("Exit"))
            {
                this.currentDialogId = 0;

                this.GameObject.SetActive(false);
            }

        }

        void RemoveListener()
        {
            Game.EventSystem.UnRegisterEvent(EventIdType.DialogAera, this.dialogAeraHandler);
        }

        public override void Dispose()
        {
            base.Dispose();

            this.RemoveListener();
        }
    }
}