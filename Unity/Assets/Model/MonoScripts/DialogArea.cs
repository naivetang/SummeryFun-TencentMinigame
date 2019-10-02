using NPOI.OpenXmlFormats.Dml.Chart;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class DialogArea : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("起始对话组ID")]
        private int dialogId;

        [SerializeField]
        [CustomLabel("对话按钮")]
        private Button dialogBtn;

        void Start()
        {
            if (this.dialogBtn != null)
            {
                this.dialogBtn.gameObject.SetActive(false);
                
                this.dialogBtn.onClick.AddListener(this.BtnClick);
            }
        }

        public static void Dialog(int dialogId)
        {
            DialogConfig dialogConfig = Game.Scene.GetComponent<ConfigComponent>().Get(typeof(DialogConfig), dialogId) as DialogConfig;

            if (dialogConfig == null)
            {

                Log.Error("不存在dialog配置,dialog id = " + dialogId);

                return;
            }

            Unit unit = Game.Scene.GetComponent<UnitComponent>().Get(dialogConfig.RoleId);

            UnityEngine.GameObject dialogGo = unit.GameObject.GetComponent<ReferenceCollector>().Get<GameObject>("UIDialog");

            Log.Info("对话内容：" + dialogConfig.Text);

            dialogGo.SetActive(true);

            

            int nextDialogId = dialogConfig.NextId;

            dialogGo.GetComponent<DialogTextCtl>().SetText(dialogConfig.Text, dialogConfig.ShowTime,nextDialogId != -1);

            if (nextDialogId == -1)
            {
                Log.Info("结束对话");

                Game.EventSystem.Run(EventIdType.CompleteDialog, dialogConfig.Id);

                return;
            }
            
            
            Game.EventSystem.Run(EventIdType.NextDialog, nextDialogId);
        }

        void BtnClick()
        {
            Dialog(this.dialogId);
            
            this.dialogBtn.gameObject.SetActive(false);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.transform.tag.Equals("Player"))
                return;

            Game.EventSystem.Run(EventIdType.DialogAera, "Enter", this.dialogId);
            
            this.dialogBtn.gameObject.SetActive(true);
        }
        void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.transform.tag.Equals("Player"))
                return;

            Game.EventSystem.Run(EventIdType.DialogAera, "Exit", this.dialogId);
            
            this.dialogBtn.gameObject.SetActive(false);
        }
    }
}