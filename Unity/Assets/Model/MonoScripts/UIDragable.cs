using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ETModel
{
    public  class UIDragable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        private Action<PointerEventData> onBeginDrag;
        private Action<PointerEventData> onDrag;
        private Action<PointerEventData> onEndDrag;

        private Action<PointerEventData> onPointDown;
        private Action<PointerEventData> onPointUp;


        private Vector3 offect;

        public void RegistOnBeginDrag(Action<PointerEventData> p)
        {
            this.onBeginDrag += p;
        }

        public void RegistOnDrag(Action<PointerEventData> p)
        {
            this.onDrag += p;
        }

        public void RegistOnEndDrag(Action<PointerEventData> p)
        {
            this.onEndDrag += p;
        }

        public void RegisterPointDown(Action<PointerEventData> p)
        {
            this.onPointDown += p;
        }

        public void RegisterPointUp(Action<PointerEventData> p)
        {
            this.onPointUp += p;
        }

        #region 拖动

        public void OnBeginDrag(PointerEventData eventData)
        {
            SetDraggedPosition(eventData);
            
            if(this.onBeginDrag!= null)
                this.onBeginDrag.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            SetDraggedPosition(eventData);

            if (this.onDrag != null)
                this.onDrag.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            SetDraggedPosition(eventData);

            if (this.onEndDrag != null)
                this.onEndDrag.Invoke(eventData);
        }

        #endregion
        

        public void OnPointerDown(PointerEventData eventData)
        {
            
            Log.Info("down");
            
            this.SetOffect(eventData);
            
            if (this.onPointDown != null)
                this.onPointDown.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (this.onPointUp != null)
                this.onPointUp.Invoke(eventData);
        }

        
        private void SetOffect(PointerEventData eventData)
        {
            var rt = gameObject.GetComponent<RectTransform>();

            // transform the screen point to world point int rectangle
            Vector3 globalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out globalMousePos))
            {
                this.offect =  rt.position - globalMousePos;
            }
        }


        private void SetDraggedPosition(PointerEventData eventData)
        {
            var rt = gameObject.GetComponent<RectTransform>();

            // transform the screen point to world point int rectangle
            Vector3 globalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out globalMousePos))
            {
                rt.position = globalMousePos + this.offect;
            }
        }


    }
}
