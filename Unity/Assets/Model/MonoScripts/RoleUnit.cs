using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class RoleUnit : MonoBehaviour
    {
        [SerializeField]
        private int roleId;


        void Start()
        {
             UnitComponent unitComponent = Game.Scene.GetComponent<UnitComponent>();

            //RoleConfig roleConfig = Game.Scene.GetComponent<ConfigComponent>().Get<RoleConfig>(this.roleId);

            RoleConfig roleConfig = Game.Scene.GetComponent<ConfigComponent>().Get(typeof(RoleConfig), this.roleId) as RoleConfig;

            if (roleConfig == null)
             {
                 Log.Error("不存在角色id :" + this.roleId);

                 return;
             }

             Unit unit = ComponentFactory.CreateWithId<Unit, GameObject>(this.roleId, this.gameObject);

             unitComponent.Add(unit);

             if (this.roleId == -1)
                 UnitComponent.Instance.MyUnit = unit; 
        }
        //
        // private void OnControllerColliderHit(ControllerColliderHit hit)
        // {
        //     Log.Error("OnControllerColliderHit");
        // }
        //
        // private void OnCollisionEnter(Collision collision)
        // {
        //     Log.Error("OnControllerColliderHit");
        // }
        //
        // private void OnCollisionEnter2D(Collision2D collision)
        // {
        //     Log.Error("OnControllerColliderHit");
        // }
        //
        // private void OnCollisionExit(Collision collision)
        // {
        //     Log.Error("OnControllerColliderHit");
        // }
        //
        // private void OnCollisionExit2D(Collision2D collision)
        // {
        //     Log.Error("OnControllerColliderHit");
        // }
        //
        // private void OnCollisionStay(Collision collision)
        // {
        //     Log.Error("OnControllerColliderHit");
        // }
        //
        // private void OnCollisionStay2D(Collision2D collision)
        // {
        //     Log.Error("OnControllerColliderHit");
        // }
        //
        // private void OnTriggerEnter(Collider other)
        // {
        //     Log.Error("OnControllerColliderHit");
        // }
        //
        // private void OnTriggerEnter2D(Collider2D collision)
        // {
        //     Log.Error("OnControllerColliderHit");
        // }
        //
        // private void OnTriggerExit(Collider other)
        // {
        //     Log.Error("OnControllerColliderHit");
        // }
        //
        // private void OnTriggerExit2D(Collider2D collision)
        // {
        //     Log.Error("OnControllerColliderHit");
        // }
        //
        // private void OnTriggerStay(Collider other)
        // {
        //     Log.Error("OnControllerColliderHit");
        // }
        //
        // private void OnTriggerStay2D(Collider2D collision)
        // {
        //     Log.Error("OnControllerColliderHit");
        // }
        //


    }
}
