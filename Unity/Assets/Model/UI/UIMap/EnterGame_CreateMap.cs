using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    [Event(EventIdType.EnterGame)]
    public class EnterGame_CreateMap : AEvent
    {
        
        
        
        public override void Run()
        {
            //UIFactory.Create<UIMapComponent>(ViewLayer.UIMainLayer, UIType.UIHall).Coroutine();

            //UIFactory.Create<UIStartComponent>(ViewLayer.UIFullScreenLayer, UIType.UIStart).Coroutine();

            //UIFactory.Create<UIGuideSceneComponent>(ViewLayer.UIFullScreenLayer, UIType.UIGuideScene).Coroutine();
            ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();

            if (!Define.LoadFromRes)
                resourcesComponent.LoadBundle(UIType.UIStart.StringToAB());

            UIFactory.Create<UIStartComponent>(ViewLayer.UIPopupLayer, UIType.UIStart).Coroutine();

            // 如果从AssetBund加载，先在登录界面预加载
            if(!Define.LoadFromRes)
                resourcesComponent.AsyncBeforeLoadBundle().Coroutine();
            
            
        }
    }


    [Event(EventIdType.CGFinish)]
    public class CGFinish_CreateHall : AEvent<GameObject>
    {

        public override void Run(GameObject a)
        {
            
            
             UIFactory.Create<UIMapComponent, GameObject, TaskQueryRsp>(ViewLayer.UIMainLayer, UIType.UIMap, a, null).Coroutine();

             // Transform player = Game.Scene.GetComponent<UnitComponent>().MyUnit.GameObject.transform;
             //
             // Transform map = b.GameObject.transform;
             //
             //
             // b.AddComponent<MapFllowComponent, Transform, Transform>(player, map);

            UIFactory.Create<UIMainComponent>(ViewLayer.UIFixedLayer, UIType.UIMain).Coroutine();
        }
    }
}
