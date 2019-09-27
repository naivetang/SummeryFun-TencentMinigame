using UnityEngine;

namespace ETModel
{
    public class TriggerArea : MonoBehaviour
    {
        [SerializeField]
        private int _triggerId;

        [SerializeField]
        private GameObject _TriggerBtn;

        void Start()
        {
            TriggerAreaConfig  config= Game.Scene.GetComponent<ConfigComponent>().Get(typeof (TriggerAreaConfig), this._triggerId) as TriggerAreaConfig;

            if (config == null || this._TriggerBtn == null)
            {
                Log.Error("conf or btn is nuill");

                return;
            }
            
            
            TriggerAreaBtn btn = ComponentFactory.Create<TriggerAreaBtn,GameObject, TriggerAreaConfig>(this._TriggerBtn, config);

            btn.TriggerArea = this.gameObject;
            
            TriggerAreaBtnComponent.Instance.Add(btn);
            
            btn.GameObject.SetActive(false);
        }
        
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
