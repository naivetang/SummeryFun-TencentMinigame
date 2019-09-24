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
        
        
        
    }
}
