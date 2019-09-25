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
    public class UnitMoveComponentSystem : AwakeSystem<UnitMoveComponent>
    {
        public override void Awake(UnitMoveComponent self)
        {
            self.Awake();
        }
    }
    [ObjectSystem]
    public class UnitMoveComponentUpdateSystem : UpdateSystem<UnitMoveComponent>
    {

        public override void Update(UnitMoveComponent self)
        {
           // self.Update();
        }
    }

    [ObjectSystem]
    public class UnitMoveComponentFixedUpdateSystem : FixedUpdateSystem<UnitMoveComponent>
    {

        public override void FixedUpdate(UnitMoveComponent self)
        {
            self.FixedUpdate();
        }
    }



    public class UnitMoveComponent : Component
    {
        [SerializeField]
        private MoveDir _moveDir = MoveDir.Stop;

        private float _moveSpeed = 30f;

        private Transform _playerTransform;
        
        public void Awake()
        {
            this._playerTransform = this.Parent.GameObject.transform;
            
            AddListener();
        }

        // public void Update()
        // {
        //     switch (_moveDir)
        //     {
        //         case MoveDir.Up:
        //             _playerTransform.Translate(new Vector3(0, this._moveSpeed * Time.deltaTime, 0));
        //             break;
        //         case MoveDir.Down:
        //             _playerTransform.Translate(new Vector3(0, -this._moveSpeed * Time.deltaTime, 0));
        //             break;
        //         case MoveDir.Left:
        //             _playerTransform.Translate(new Vector3(-this._moveSpeed * Time.deltaTime, 0, 0));
        //             break;
        //         case MoveDir.Right:
        //             _playerTransform.Translate(new Vector3(this._moveSpeed * Time.deltaTime, 0, 0));
        //             break;
        //         case MoveDir.LeftUp:
        //             break;
        //         case MoveDir.RightUp:
        //             break;
        //         case MoveDir.LeftDown:
        //             break;
        //         case MoveDir.RightDown:
        //             break;
        //         case MoveDir.Stop:
        //             break;
        //     }
        // }

        public void FixedUpdate()
        {
            switch (_moveDir)
            {
                case MoveDir.Up:
                    _playerTransform.Translate(new Vector3(0, this._moveSpeed * Time.deltaTime, 0));
                    //Log.Info($"player position : {this._playerTransform.position},local :{this._playerTransform.localPosition}");
                    break;
                case MoveDir.Down:
                    _playerTransform.Translate(new Vector3(0, -this._moveSpeed * Time.deltaTime, 0));
                    break;
                case MoveDir.Left:
                    _playerTransform.Translate(new Vector3(-this._moveSpeed * Time.deltaTime, 0, 0));
                    break;
                case MoveDir.Right:
                    _playerTransform.Translate(new Vector3(this._moveSpeed * Time.deltaTime, 0, 0));
                    break;
                case MoveDir.LeftUp:
                    _playerTransform.Translate(new Vector3(-this._moveSpeed * Time.deltaTime * Mathf.Sin(45f), this._moveSpeed * Time.deltaTime * Mathf.Sin(45f), 0));
                    break;
                case MoveDir.RightUp:
                    _playerTransform.Translate(new Vector3(this._moveSpeed * Time.deltaTime * Mathf.Sin(45f), this._moveSpeed * Time.deltaTime * Mathf.Sin(45f), 0));
                    break;
                case MoveDir.LeftDown:
                    _playerTransform.Translate(new Vector3(-this._moveSpeed * Time.deltaTime * Mathf.Sin(45f), -this._moveSpeed * Time.deltaTime * Mathf.Sin(45f), 0));
                    break;
                case MoveDir.RightDown:
                    _playerTransform.Translate(new Vector3(this._moveSpeed * Time.deltaTime * Mathf.Sin(45f), -this._moveSpeed * Time.deltaTime * Mathf.Sin(45f), 0));
                    break;
                case MoveDir.Stop:
                    break;
            }

            // if (Input.GetMouseButtonDown(0))
            // {
            //     this.RemoveListener();
            // }
        }

        private void AddListener()
        {
            Game.EventSystem.RegisterEvent(EventIdType.MoveDirChange, new EventProxy(MoveDirChange));
        }

        private void MoveDirChange(List<object> obj)
        {
            MoveDir dir = (MoveDir)obj[0];

            this._moveDir = dir;
        }

        public override void Dispose()
        {
            base.Dispose();

            this.RemoveListener();
        }

        private void RemoveListener()
        {
            Game.EventSystem.UnRegisterEvent(EventIdType.MoveDirChange, new EventProxy(MoveDirChange));
        }


    }
}
