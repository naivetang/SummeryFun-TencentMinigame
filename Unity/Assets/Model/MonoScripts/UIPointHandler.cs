using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ETModel
{
    public class UIPointHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private Action<PointerEventData> onPointDown;
        private Action<PointerEventData> onPointUp;
        
        public void RegisterPointDown(Action<PointerEventData> p)
        {
            this.onPointDown += p;
        }

        public void RegisterPointUp(Action<PointerEventData> p)
        {
            this.onPointUp += p;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            Log.Info("down");
            
            if (this.onPointDown != null)
                this.onPointDown.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (this.onPointUp != null)
                this.onPointUp.Invoke(eventData);
        }
    }
}