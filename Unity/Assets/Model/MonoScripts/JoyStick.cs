using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ETModel
{
    /// <summary>
    /// 移动方向
    /// </summary>
    public enum MoveDir
    {
        Stop,
        Up,
        Down,
        Left,
        Right,
        LeftUp,
        LeftDown,
        RightUp,
        RightDown,
    }

    public class JoyStick : MonoBehaviour,IDragHandler,IPointerDownHandler,IPointerUpHandler
    {
        /// <summary>
        /// 操作杆状态
        /// </summary>
        public enum JoystickState
        {
            /// <summary>
            /// 闲置
            /// </summary>
            ldle,
            /// <summary>
            /// 抬起
            /// </summary>
            TouchUp,
            /// <summary>
            /// 按下
            /// </summary>
            TouchDown,
            /// <summary>
            /// 准备
            /// </summary>
            Ready,
            /// <summary>
            /// 拖动
            /// </summary>
            Drag,
        }



        /// <summary>
        /// 杆的底图
        /// </summary>
        [SerializeField]
        private Image _stickBG;

        /// <summary>
        /// 操作杆：包括杆、指向
        /// </summary>
        [SerializeField]
        private GameObject _dragStick;

        /// <summary>
        /// 摇杆
        /// </summary>
        [SerializeField]
        private Image _stick;

        /// <summary>
        /// 指向箭头
        /// </summary>
        [SerializeField]
        private Image _arrow;


        private PointerEventData _pointerEventData;
        
        private Vector3 _touchPosition;


        private Vector3 _joystickDirection;
        public Vector3 JoystickDirection
        {
            get
            {
                return this._joystickDirection;
            }
        }

        private double _dragAngle;

        /// <summary>
        /// 摇杆初始位置
        /// </summary>
        [SerializeField]
        private Vector3 _joystickInitPosition = new Vector3(0, 0, 0);

        /// <summary>
        /// 可触发摇杆动作的左下角
        /// </summary>
        [SerializeField]
        private Vector2 _canClickLeftDown = new Vector2(-100, -100);

        /// <summary>
        /// 可触发摇杆动作的右上角
        /// </summary>
        [SerializeField]
        private Vector2 _canClickRightUp = new Vector2(106, 106);


        /// <summary>
        /// 切换到拖动状态最小距离差
        /// </summary>
        [SerializeField]
        private float _switchDragStateMinDst = 15f;

        /// <summary>
        /// 杆拖动最大距离
        /// </summary>
        [SerializeField]
        private float _stickMaxDst = 73f;

        /// <summary>
        /// 显示指向 鼠标距离中心最小距离
        /// </summary>
        [SerializeField]
        private float _showDirection = 15f;

        /// <summary>
        /// 手柄状态
        /// </summary>
        [SerializeField]
        private JoystickState _joystickState = JoystickState.ldle;
        
        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private MoveDir _moveDir = MoveDir.Stop;



        private void Start()
        {
            this._camera = Game.Scene.GetComponent<UIComponent>().Camera;
            
            this.IdleState();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _pointerEventData = eventData;
            this.SwitchState(JoystickState.TouchDown);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _pointerEventData = eventData;
            this.SwitchState(JoystickState.TouchUp);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _pointerEventData = eventData;
            this.UpdateStickState();
        }
        
        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="state"></param>
        public void SwitchState(JoystickState state)
        {
            this._joystickState = state;

            this.UpdateStickState();
        }

        
        void UpdateStickState()
        {
            if (this._joystickState == JoystickState.ldle)
            {
                return;
            }

            switch (this._joystickState)
            {
                case JoystickState.TouchUp:

                   this.MouseUpAction();

                    this.SwitchState(JoystickState.ldle);

                    break;
                case JoystickState.TouchDown:

                    this.MouseDownAction();

                    this.SwitchState(JoystickState.Ready);

                    break;
                case JoystickState.Ready:

                    ReadyState();

                    break;
                case JoystickState.Drag:

                    DragState();

                    break;
            }
            
            this.UpdateMoveDir();
        }

        private void UpdateMoveDir()
        {
            MoveDir dir = MoveDir.Stop;

            if (this._joystickState == JoystickState.Drag)
            {
                if (this._dragAngle > 67.5 && this._dragAngle < 112.5)
                {
                    dir = MoveDir.Right;
                }
                else if(this._dragAngle > 22.5 && this._dragAngle < 67.5)
                {
                    dir = MoveDir.RightUp;
                }
                else if (this._dragAngle > -22.5 && this._dragAngle < 22.5)
                {
                    dir = MoveDir.Up;
                }
                else if (this._dragAngle > -67.5 && this._dragAngle < -22.5)
                {
                    dir = MoveDir.LeftUp;
                }
                else if (this._dragAngle > -112.5 && this._dragAngle < -67.5)
                {
                    dir = MoveDir.Left;
                }
                else if (this._dragAngle > -157.5 && this._dragAngle < -112.5)
                {
                    dir = MoveDir.LeftDown;
                }
                else if (this._dragAngle > 157.5 || this._dragAngle < -157.5)
                {
                    dir = MoveDir.Down;
                }
                else if (this._dragAngle > 112.5 && this._dragAngle < 157.5)
                {
                    dir = MoveDir.RightDown;
                }
            }

            if (dir != this._moveDir)
            {
                this._moveDir = dir;

                Game.EventSystem.Run(EventIdType.MoveDirChange, dir);
            }
            
        }

        /// <summary>
        /// 初始状态
        /// </summary>
        private void IdleState()
        {
            //this._dragStick.rectTransform.anchoredPosition = this._joystickInitPosition;


            this._dragStick.transform.localPosition = this._joystickInitPosition;

            this._stick.transform.localPosition = Vector3.zero;

            this._arrow.gameObject.SetActive(false);
        }

        /// <summary>
        /// 鼠标按下状态
        /// </summary>
        void MouseDownAction()
        {
            this._touchPosition = GetMouseLocalPosition(this.transform);

            Vector3 position = this._touchPosition;

            position.x = Mathf.Clamp(position.x, this._canClickLeftDown.x, this._canClickRightUp.x);
            
            position.y = Mathf.Clamp(position.y, this._canClickLeftDown.y, this._canClickRightUp.y);

            this._dragStick.transform.localPosition = position;

        }

        /// <summary>
        /// 鼠标抬起行为
        /// </summary>
        void MouseUpAction()
        {
            this.IdleState();
        }

        /// <summary>
        /// 准备状态
        /// </summary>
        void ReadyState()
        {
            Vector3 position = GetMouseLocalPosition(transform);

            float distance = Vector3.Distance(position, this._touchPosition);

            //点击屏幕拖动大于切换拖动状态最小距离 则切换到拖动状态
            if (distance > this._switchDragStateMinDst)
            {
                this.SwitchState(JoystickState.Drag);
            }
        }


        /// <summary>
        /// 拖动状态
        /// </summary>
        void DragState()
        {
            // 相对于摇杆中心位置
            Vector3 mouseLocalPosition = GetMouseLocalPosition(this._dragStick.transform);

            // Log.Debug($"鼠标位置( {mouseLocalPosition.x},{mouseLocalPosition.y} )," +
            //     $"stickBG位置( {this._stickBG.transform.localPosition.x} , {this._stickBG.transform.localPosition.y} )" +
            //     $"角度 : {this._dragAngle}");

            //鼠标与摇杆的距离
            float distance = Vector3.Distance(mouseLocalPosition, Vector3.zero);


            //设置杆的位置
            Vector3 stickLocalPosition = mouseLocalPosition;


            //鼠标位置大于杆拖动的最大值
            if (distance > this._stickMaxDst)
            {
                float proportion = this._stickMaxDst / distance;

                stickLocalPosition = (mouseLocalPosition - this._stickBG.transform.localPosition) * proportion;
            }


            this._stick.transform.localPosition = stickLocalPosition;


            //设置指向位置

            //摇杆与鼠标的距离 大于 指向显示最小距离  则显示指向 
            if (distance > this._showDirection)
            {
                this._arrow.gameObject.SetActive(true);
                
                

                //获取鼠标位置与摇杆的角度
                this._dragAngle = Math.Atan2((mouseLocalPosition.x), (mouseLocalPosition.y)) * 180 / Math.PI;

                // Log.Debug($"鼠标位置( {mouseLocalPosition.x},{mouseLocalPosition.y} )," +
                //     $"stickBG位置( {this._stickBG.transform.localPosition.x} , {this._stickBG.transform.localPosition.y} )" +
                //     $"角度 : {this._dragAngle}");

                //Log.Debug("角度 ： " + this._dragAngle);
                //Log.Debug("角度 ： " + Math.Atan2(3,4) * 180/Math.PI);
                
                this._arrow.transform.eulerAngles = new Vector3(0, 0, (float)this._dragAngle);

                //设置摇杆指向
                this._joystickDirection = mouseLocalPosition - this._stickBG.transform.localPosition;
                this._joystickDirection.z = 0;
            }
            else
            {
                this._arrow.gameObject.SetActive(false);
            }
            
        }

        Vector3 GetMouseLocalPosition(Transform transform)
        {
            Vector3 mousePosition = this._pointerEventData.position;

            //Vector3 mousePosition = Input.mousePosition;

            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform) transform, mousePosition, this._camera, out Vector2 localPos);
            
            return localPos;
        }

    }
}
