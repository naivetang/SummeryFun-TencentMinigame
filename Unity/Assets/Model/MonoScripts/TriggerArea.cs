using UnityEngine;

namespace ETModel
{
    public class TriggerArea : MonoBehaviour
    {
        [SerializeField]
        private int _triggerId;
        
        void OnTriggerEnter2D(Collider2D other)
        {
            Game.EventSystem.Run(EventIdType.TriggerAera, "Enter", this._triggerId);
        }
        void OnTriggerStay2D(Collider2D other)    //每帧调用一次OnTriggerStay()函数
        {
        }
        void OnTriggerExit2D(Collider2D other)
        {
            Game.EventSystem.Run(EventIdType.TriggerAera, "Exit", this._triggerId);
        }
        
    }
}
