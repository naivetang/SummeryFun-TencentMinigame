using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class MapFllowAwakeSystem : AwakeSystem<MapFllowComponent, Transform, Transform>
    {
        public override void Awake(MapFllowComponent self, Transform a, Transform b)
        {
            self.Awake(a,b);
        }
    }

    [ObjectSystem]
    public class MapFllowLateUpdateSystem : LateUpdateSystem<MapFllowComponent>
    {
        public override void LateUpdate(MapFllowComponent self)
        {
            self.LateUpdate();
        }
    }

    [HideInHierarchy]
    public class MapFllowComponent : Component
    {

        private Transform player;

        private Transform map;

        private Camera camera;

        private float minMapY = -4000;

        private float maxMapY = 0;

        private float speed = 60;

        public void Awake(Transform player, Transform map)
        {
            this.player = player;

            this.map = map;

            this.camera = Game.Scene.GetComponent<UIComponent>().Camera;
        }
        public void LateUpdate()
        {
            if (this.player == null || this.map == null)
                return;

            Vector3 v3 =  this.camera.WorldToViewportPoint(this.player.position);

            if (Mathf.Abs(v3.y - 0.5f) < 0.005f)
            {
                return;
            }

            // player在屏幕下半屏，map上移动
            if (v3.y < 0.5)
            {
                if (this.map.position.y > this.maxMapY)
                    return;
                
                this.map.transform.position += new Vector3(0,this.speed * Time.deltaTime);
            }
            // player在屏幕上半屏，map下移动
            else
            {
                if (this.map.position.y < this.minMapY)
                    return;

                
//                Vector3.Lerp()
                
                
                this.map.transform.position -= new Vector3(0,this.speed * Time.deltaTime);
            }
            
            
            //Log.Info(v3.ToString());

        }




    }
}
