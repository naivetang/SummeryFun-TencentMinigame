﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
            self.Update();
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

        private float _moveSpeed = 75f;

        private Transform _playerTransform;

        private Rigidbody2D rigidbody2D;

        public void Awake()
        {
            this._playerTransform = this.Parent.GameObject.transform;

            this.rigidbody2D = this._playerTransform.GetComponent<Rigidbody2D>();
            
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

        public void Update()
        {
            return;
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

        public void FixedUpdate()
        {
            
            Vector2 endpos = _playerTransform.transform.position;

            switch (_moveDir)
            {
                case MoveDir.Up:

                    endpos.y += this._moveSpeed * Time.deltaTime;
                    
                    break;
                case MoveDir.Down:
                    endpos.y -= this._moveSpeed * Time.deltaTime;
                    
                    break;
                case MoveDir.Left:


                    endpos.x -= this._moveSpeed * Time.deltaTime;
                    
                    break;
                case MoveDir.Right:

                    endpos.x += this._moveSpeed * Time.deltaTime;
                    
                    break;
                case MoveDir.LeftUp:

                    endpos += new Vector2(-this._moveSpeed * Time.deltaTime * Mathf.Sin(45f), this._moveSpeed * Time.deltaTime * Mathf.Sin(45f));

                    //_playerTransform.Translate(new Vector3(-this._moveSpeed * Time.deltaTime * Mathf.Sin(45f), this._moveSpeed * Time.deltaTime * Mathf.Sin(45f), 0));
                    break;
                case MoveDir.RightUp:

                    endpos += new Vector2(this._moveSpeed * Time.deltaTime * Mathf.Sin(45f), this._moveSpeed * Time.deltaTime * Mathf.Sin(45f));

                    //_playerTransform.Translate(new Vector3(this._moveSpeed * Time.deltaTime * Mathf.Sin(45f), this._moveSpeed * Time.deltaTime * Mathf.Sin(45f), 0));
                    break;
                case MoveDir.LeftDown:

                    endpos += new Vector2(-this._moveSpeed * Time.deltaTime * Mathf.Sin(45f), -this._moveSpeed * Time.deltaTime * Mathf.Sin(45f));

                    //_playerTransform.Translate(new Vector3(-this._moveSpeed * Time.deltaTime * Mathf.Sin(45f), -this._moveSpeed * Time.deltaTime * Mathf.Sin(45f), 0));
                    break;
                case MoveDir.RightDown:

                    endpos += new Vector2(this._moveSpeed * Time.deltaTime * Mathf.Sin(45f), -this._moveSpeed * Time.deltaTime * Mathf.Sin(45f));

                    //_playerTransform.Translate(new Vector3(this._moveSpeed * Time.deltaTime * Mathf.Sin(45f), -this._moveSpeed * Time.deltaTime * Mathf.Sin(45f), 0));
                    break;
                case MoveDir.Stop:
                    break;
            }

            rigidbody2D.MovePosition(endpos);

            // if (Input.GetMouseButtonDown(0))
            // {
            //     this.RemoveListener();
            // }
        }

        private EventProxy moveDirChangeProxt;
        
        private void AddListener()
        {
            moveDirChangeProxt = new EventProxy(MoveDirChange);
            Game.EventSystem.RegisterEvent(EventIdType.MoveDirChange, this.moveDirChangeProxt);
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
            Game.EventSystem.UnRegisterEvent(EventIdType.MoveDirChange, this.moveDirChangeProxt);
        }


    }
}
