using UnityEngine;
using UnityEngine.EventSystems;

namespace ETModel
{
    [RequireComponent(typeof (Canvas))]
    public class UIDepth : UIBehaviour
    {
        [SerializeField]
        [HideInInspector]
        private Canvas canvas;


        [SerializeField]
        [HideInInspector]
        private Canvas parentCanvas;

        [SerializeField]
        [HideInInspector]
        private int _relativeDepth;

        [SerializeField]
        private int relativeDepth;

        [SerializeField]
        [HideInInspector]
        private int _absoluteDepth;

        [SerializeField]
        private int absoluteDepth;

        [SerializeField]
        [HideInInspector]
        private bool init = false;

        protected override void Start()
        {
            return;
            
            this.SetDepth();
        }

        void SetDepth()
        {
            this.canvas = this.GetComponent<Canvas>();

            this.parentCanvas = this.GetParentCanvas();

            int order = this.canvas.sortingOrder;

            if (order != this.absoluteDepth
                && this._relativeDepth != this.canvas.sortingOrder - this.parentCanvas.sortingOrder)
            {
                Log.Error( this.name +" 参数有误: 绝对层级：" + this.absoluteDepth + "相对层级：" + this.relativeDepth);

                //return;
            }

            this.canvas.sortingOrder = this.absoluteDepth;
        }
        
        Canvas GetParentCanvas()
        {
            Transform p = this.transform.parent;

            Canvas c = null;

            while (p != null)
            {
                c = p.GetComponent<Canvas>();
                if (c != null)
                {
                    break;
                }

                p = p.parent;
            }

            return c;
        }

#if UNITY_EDITOR

        void UpdateDepth()
        {
            //Log.Debug("sortOrder : " + this.parentCanvas.sortingOrder);
            //Log.Debug("renderOrder : " + this.parentCanvas.renderOrder);
            
            //this.relativeDepth = this.parentCanvas.sortingOrder;

            if (this.init == false)
            {
                this.Init();

                return;
            }

            // 修改的绝对值
            if (this.absoluteDepth != this._absoluteDepth)
            {
                int d = this.absoluteDepth - this._absoluteDepth;

                this._absoluteDepth = this.absoluteDepth;

                this.relativeDepth = this._relativeDepth = this.relativeDepth + d;

                this.canvas.sortingOrder = this.absoluteDepth;

                Log.Debug("修改绝对值");
            }
            // 修改的相对值
            else if (this.relativeDepth != this._relativeDepth)
            {
                int d = this.relativeDepth - this._relativeDepth;

                this._relativeDepth = this.relativeDepth;

                this.absoluteDepth = this._absoluteDepth = this.absoluteDepth + d;

                this.canvas.sortingOrder = this.absoluteDepth;

                Log.Debug("修改相对值");
            }
            
        }

        void Init()
        {

            this.init = true;

            this.canvas = this.GetComponent<Canvas>();

            this.parentCanvas = this.GetParentCanvas();

            this.canvas.overrideSorting = true;

            this.canvas.sortingOrder = this.parentCanvas.sortingOrder;

            this.absoluteDepth = this._absoluteDepth = this.parentCanvas.sortingOrder;

            this.relativeDepth = this._relativeDepth = 0;

            return;

        }



        protected override void OnValidate()
        {
            this.UpdateDepth();
        }
#endif
    }

}