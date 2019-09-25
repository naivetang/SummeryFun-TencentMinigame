using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class UnitCameraFollowAwakeSystem: AwakeSystem<UnitCameraFollowComponent>
    {
        public override void Awake(UnitCameraFollowComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class UnitCameraFollowLateSystem : LateUpdateSystem<UnitCameraFollowComponent>
    {
        

        public override void LateUpdate(UnitCameraFollowComponent self)
        {
            self.LateUpdate();
        }
    }

    public class UnitCameraFollowComponent : Component
    {
        private Camera uiCamera;

        private GameObject player;

        private float minY = 0;

        private float maxY = 4000;
        public void Awake()
        {
            this.uiCamera = Game.Scene.GetComponent<UIComponent>().Camera;

            this.player = this.Parent.GameObject;
        }


        public void LateUpdate()
        {
            Vector3 pos = this.uiCamera.transform.position;

            pos.y = this.player.transform.position.y;

            pos.y = Mathf.Clamp(pos.y, this.minY, this.maxY);
            
            
            this.uiCamera.transform.position = pos;
        }
    }
}
