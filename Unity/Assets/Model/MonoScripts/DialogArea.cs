using UnityEngine;

namespace ETModel
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class DialogArea : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("起始对话组ID")]
        private int dialogId;
        
        void OnTriggerEnter2D(Collider2D other)
        {
            Game.EventSystem.Run(EventIdType.DialogAera, "Enter", this.dialogId);
        }
        void OnTriggerExit2D(Collider2D other)
        {
            Game.EventSystem.Run(EventIdType.DialogAera, "Exit", this.dialogId);
        }
    }
}